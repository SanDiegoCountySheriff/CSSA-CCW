// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import * as msal from '@azure/msal-browser';
import { useAuthStore } from '@shared-ui/stores/auth';

/**
 * Implementation of MSAL B2C Login through a popup or redirect
 */
export default {
  auth: null,
  loginType: null,
  token: null,
  refreshTime: null,

  /**
   * Create the msal popup instance
   **/
  setupAuth(
    clientId: string,
    authority: string,
    knownAuthorities: Array<string>,
    loginType: string,
    refreshTime: number
  ) {
    /**
     * MSAL configuration for the Application
     */
    const msalConfig: msal.Configuration = {
      auth: {
        clientId,
        authority,
        knownAuthorities,
        redirectUri: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: false,
      },
      system: {
        loggerOptions: {
          loggerCallback: (level, message, containsPii) => {
            if (containsPii) {
              return;
            }

            switch (level) {
              case msal.LogLevel.Error:
                window.console.error(message);

                return;
              case msal.LogLevel.Info:
                window.console.info(message);

                return;
              case msal.LogLevel.Verbose:
                window.console.debug(message);

                return;
              case msal.LogLevel.Warning:
                window.console.warn(message);
            }
          },
        },
      },
    };

    this.auth = new msal.PublicClientApplication(msalConfig);
    this.loginType = loginType;
    this.refreshTime = refreshTime;

    if (this.loginType !== 'Popup') {
      this.auth
        .handleRedirectPromise()
        .then(response => {
          if (response) {
            this.handleResponse(response);
          }
        })
        .catch(error => {
          window.console.log(error);
        });
    }
  },

  /**
   * See here for more info on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  selectAccount() {
    const currentAccounts = this.auth.getAllAccounts();

    if (currentAccounts.length >= 1) {
      this.auth.setActiveAccount(currentAccounts[0]);
      this.isAuthenticated();
      this.acquireToken();
      this.setUser();
    }
  },

  /**
   * To see the full list of response object properties, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#response
   */
  handleResponse(response: msal.AuthenticationResult) {
    if (response !== null) {
      this.auth.setActiveAccount(response.account);
      this.selectAccount();
    } else {
      this.selectAccount();
    }
  },

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  async signIn() {
    if (this.loginType === 'Popup') {
      const res = await this.auth
        .loginPopup({
          scopes: ['openid', 'profile', 'email', 'offline_access'],
        })
        .catch(error => {
          window.console.log(error);
        });

      this.handleResponse(res);
    } else {
      await this.auth.loginRedirect({
        scopes: ['openid', 'profile', 'email', 'offline_access'],
      });
    }
  },

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  async signOut() {
    const logoutRequest: msal.EndSessionPopupRequest = {
      postLogoutRedirectUri: window.location.origin,
      mainWindowRedirectUri: window.location.origin,
    };

    useAuthStore().resetStore();

    this.loginType === 'Popup'
      ? await this.auth.logoutPopup(logoutRequest)
      : this.auth.logoutRedirect(logoutRequest);
  },

  /**
   * See here for more information on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  async acquireToken() {
    const request: msal.PopupRequest | msal.RedirectRequest = {
      scopes: ['openid'],
      account: this.auth?.getActiveAccount(),
      forceRefresh: false,
    };

    try {
      const response = await this.auth.acquireTokenSilent(request);

      window.console.log(`access_token acquired at: ${new Date().toString()}`);
      this.token = response.idToken;
      useAuthStore().setToken(response.idToken);
      useAuthStore().setSessionStarted(new Date().toString());

      return response.idToken;
    } catch (err) {
      window.console.log('Silent token failed. \n', err);
    }
  },

  tokenInterval() {
    this.acquireToken();
  },

  setUser() {
    const account = this.auth?.getActiveAccount();

    if (!account) {
      return null;
    }

    useAuthStore().setUser(account.name);
    useAuthStore().setUserEmail(account.username);
    useAuthStore().setRoles(account.idTokenClaims.roles);
  },

  /**
   * Check if user is authenticated
   * @returns boolean
   */
  isAuthenticated() {
    const account = this.auth?.getActiveAccount();

    if (!account) {
      return false;
    }

    const isAuthn =
      (new Date(account.idTokenClaims.exp * 1000) as number) > Date.now();

    useAuthStore().setIsAuthenticated(isAuthn);

    return isAuthn;
  },
};
