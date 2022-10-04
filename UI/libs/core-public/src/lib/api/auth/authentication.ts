import * as msal from '@azure/msal-browser';

/**
 * Implementation of MSAL B2C Login through a popup
 */
export default {
  auth: null,
  loginType: null,

  /**
   * Create the msal popup instance
   **/
  setupAuth(
    clientId: string,
    authority: string,
    knownAuthorities: Array<string>,
    loginType: string
  ) {
    /**
     * MSAL configuration for the Application
     */
    const msalConfig: msal.Configuration = {
      auth: {
        clientId: clientId,
        authority: authority,
        knownAuthorities: knownAuthorities,
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
                console.error(message);
                return;
              case msal.LogLevel.Info:
                console.info(message);
                return;
              case msal.LogLevel.Verbose:
                console.debug(message);
                return;
              case msal.LogLevel.Warning:
                console.warn(message);
                return;
            }
          },
        },
      },
    };

    this.auth = new msal.PublicClientApplication(msalConfig);
    this.loginType = loginType;

    if (this.loginType !== 'Popup') {
      this.auth
        .handleRedirectPromise()
        .then(response => {
          if (response) {
            this.handleResponse(response);
          }
        })
        .catch(error => {
          console.log(error);
        });
    }
  },

  /**
   * See here for more info on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  selectAccount() {
    const currentAccounts = this.auth.getAllAccounts();

    if (currentAccounts.length < 1) {
      return;
    } else if (currentAccounts.length > 1) {
      this.auth.setActiveAccount(currentAccounts[0]);
    }
  },

  /**
   * To see the full list of response object properties, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#response
   */
  handleResponse(response: msal.AuthenticationResult) {
    if (response !== null) {
      this.auth.setActiveAccount(response.account);
    } else {
      selectAccount();
    }
  },

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  async signIn() {
    if (this.loginType === 'Popup') {
      await this.auth
        .loginPopup({
          scopes: [
            'openid',
            'https://fabrikamb2c.onmicrosoft.com/helloapi/demo.read',
          ],
        })
        .then(this.handleResponse)
        .catch(error => {
          console.log(error);
        });
    } else {
      await this.auth.loginRedirect({
        scopes: [
          'openid',
          'https://fabrikamb2c.onmicrosoft.com/helloapi/demo.read',
        ],
      });
    }
  },

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  async signOut() {
    const logoutRequest: msal.EndSessionPopupRequest = {
      postLogoutRedirectUri: msalConfig.auth.redirectUri,
      mainWindowRedirectUri: msalConfig.auth.redirectUri,
    };

    this.loginType === 'Popup'
      ? await this.auth.logoutPopup(logoutRequest)
      : this.authRedirect.logoutRedirect(logoutRequest);
  },

  /**
   * See here for more information on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  acquireToken(request: msal.PopupRequest) {
    request.account = this.auth.getActiveAccount();

    return this.auth
      .acquireTokenSilent(request)
      .then(response => {
        if (!response.accessToken || response.accessToken === '') {
          throw new msal.InteractionRequiredAuthError();
        }
        console.log('access_token acquired at: ' + new Date().toString());
        accessToken = response.accessToken;
        return accessToken;
      })
      .catch(error => {
        console.log(
          'Silent token acquisition fails. Acquiring token using popup. \n',
          error
        );
        if (error instanceof msal.InteractionRequiredAuthError) {
          // fallback to interaction when silent call fails
          return this.loginType === 'Popup'
            ? this.auth
                .acquireTokenPopup(request)
                .then(response => {
                  return response;
                })
                .catch(error => {
                  console.log(error);
                })
            : this.authRedirect.acquireTokenRedirect(request);
        } else {
          console.log(error);
        }
      });
  },

  /**
   * Check if user is authenticated
   * @returns boolean
   */
  isAuthenticated() {
    const account = this.auth.getActiveAccount();
    if (!account) {
      return false;
    }
    return new Date(account.idTokenClaims.exp * 1000) > Date.now();
  },
};
