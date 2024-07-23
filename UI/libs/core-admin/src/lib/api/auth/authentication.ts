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
  private publicClientApplication: PublicClientApplication
  private authStore = useAuthStore()
  private configStore = useAppConfigStore()

  private constructor(config: Configuration) {
    this.publicClientApplication = new PublicClientApplication(config)

    this.authStore.auth.handlingRedirectPromise = true
    this.publicClientApplication.handleRedirectPromise().then(() => {
      const accounts = this.publicClientApplication.getAllAccounts()

      if (accounts.length > 0) {
        this.publicClientApplication.setActiveAccount(accounts[0])
        this.isAuthenticated()
        this.authStore.setToken(accounts[0].idToken)
        this.authStore.setSessionStarted(new Date().toString())
        this.authStore.setUser(accounts[0].name)
        this.authStore.setUserEmail(accounts[0].username)
        this.authStore.setRoles(accounts[0].idTokenClaims?.roles)
      }

      this.authStore.auth.handlingRedirectPromise = false
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
    const response = await axios.get(`${Endpoints.GET_CONFIG_ENDPOINT}.json`)

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
      questions: response.data.Questions || {},
      isPaymentServiceAvailable:
        response.data.Configuration.IsPaymentServiceAvailable === 'True',
      payBeforeSubmit: response.data.Configuration.PayBeforeSubmit === 'True',
      applicationInsightsConnectionString:
        response.data.Configuration.ApplicationInsightsConnectionString,
      scope: response.data.Authentication.Scope,
    }

    configStore.setAppConfig(config)

    return msalConfig
  }

  logIn() {
    this.publicClientApplication.loginRedirect()
  }

  logOut() {
    this.authStore.resetStore()
    this.publicClientApplication.logoutRedirect()
  }

  async acquireToken() {
    const silentRequest: SilentRequest = {
      scopes: ['openid', this.configStore.appConfig.scope],
      forceRefresh: false,
    }

    const token = await this.publicClientApplication.acquireTokenSilent(
      silentRequest
    )

    return token.accessToken
  }

  isAuthenticated() {
    const account = this.publicClientApplication.getActiveAccount()

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
