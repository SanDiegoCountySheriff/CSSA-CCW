import { AppConfigType } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import axios from 'axios'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import {
  Configuration,
  PublicClientApplication,
  SilentRequest,
} from '@azure/msal-browser'

export class MsalBrowser {
  private static instance: MsalBrowser
  private app: PublicClientApplication
  private authStore = useAuthStore()

  private constructor(config: Configuration) {
    this.app = new PublicClientApplication(config)

    this.app.handleRedirectPromise().then(() => {
      const accounts = this.app.getAllAccounts()

      if (accounts.length > 0) {
        this.app.setActiveAccount(accounts[0])
        this.isAuthenticated()
        this.authStore.setToken(accounts[0].idToken)
        this.authStore.setSessionStarted(new Date().toString())
        this.authStore.setUser(accounts[0].name)
        this.authStore.setUserEmail(accounts[0].username)
        this.authStore.setRoles(accounts[0].idTokenClaims?.roles)
      }
    })
  }

  static async getInstance(): Promise<MsalBrowser> {
    if (!MsalBrowser.instance) {
      const authConfig = await MsalBrowser.fetchAuthConfig()

      MsalBrowser.instance = new MsalBrowser(authConfig)
    }

    return MsalBrowser.instance
  }

  private static async fetchAuthConfig(): Promise<Configuration> {
    const response = await axios.get(Endpoints.GET_CONFIG_ENDPOINT)

    const msalConfig: Configuration = {
      auth: {
        clientId: response.data.Authentication.ClientId,
        authority: response.data.Authentication.AuthorityUrl,
        knownAuthorities: response.data.Authentication.KnownAuthorities,
        redirectUri: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
      },
      cache: {
        cacheLocation: 'localStorage',
      },
      system: {
        loadFrameTimeout: 60000,
      },
    }

    const configStore = useAppConfigStore()

    const config: AppConfigType = {
      apiBaseUrl: response.data.Configuration.ServicesBaseUrl,
      adminApiBaseUrl: response.data.Configuration.AdminServicesBaseUrl,
      applicationApiBaseUrl:
        response.data.Configuration.ApplicationServicesBaseUrl,
      documentApiBaseUrl: response.data.Configuration.DocumentServicesBaseUrl,
      paymentApiBaseUrl: response.data.Configuration.PaymentServicesBaseUrl,
      scheduleApiBaseUrl: response.data.Configuration.ScheduleServicesBaseUrl,
      userProfileApiBaseUrl:
        response.data.Configuration.UserProfileServicesBaseUrl,
      apiSubscription: response.data.Configuration.Subscription,
      authorityUrl: response.data.Authentication.AuthorityUrl,
      knownAuthorities: response.data.Authentication.KnownAuthorities,
      clientId: response.data.Authentication.ClientId,
      defaultCounty: response.data.Configuration.DefaultCounty,
      displayDebugger: response.data.Configuration.DisplayDebugger === 'True',
      environmentName:
        response.data.Configuration?.Environment.toUpperCase() || 'DEV',
      refreshTime: response.data.Authentication.RefreshTimeInMinutes || 30,
      questions: response.data.QuestionsConfig || [],
    }

    configStore.setAppConfig(config)

    return msalConfig
  }

  logIn() {
    this.app.loginRedirect()
  }

  logOut() {
    this.authStore.resetStore()
    this.app.logoutRedirect()
  }

  async acquireToken() {
    const account = this.app.getActiveAccount()

    if (!account) {
      return
    }

    const silentRequest: SilentRequest = {
      scopes: ['openid'],
      account,
      forceRefresh: false,
    }

    const token = await this.app.acquireTokenSilent(silentRequest)

    return token.idToken
  }

  isAuthenticated() {
    const account = this.app.getActiveAccount()

    if (!account) {
      return false
    }

    if (account.idTokenClaims?.exp) {
      const isAuthenticated =
        (new Date(account.idTokenClaims.exp * 1000) as unknown as number) >
        Date.now()

      this.authStore.setIsAuthenticated(isAuthenticated)

      return isAuthenticated
    }

    return false
  }
}

export async function getMsalInstance(): Promise<MsalBrowser> {
  return await MsalBrowser.getInstance()
}
