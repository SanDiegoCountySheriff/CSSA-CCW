/* eslint-disable @typescript-eslint/no-unused-vars */
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import * as msal from '@azure/msal-browser';
import { useAuthStore } from '@shared-ui/stores/auth';

/**
 * Implementation of MSAL B2C Login through a popup or redirect
 */
function Auth() {
  let auth = null;
  let loginType = null;
  let token = null;
  let refreshTime = null;

  const authService = {};

  /**
   * Create the msal popup instance
   **/
  authService.setupAuth = (
    clientId: string,
    authority: string,
    knownAuthorities: Array<string>,
    type: string,
    refresh: number
  ) => {
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

    auth = new msal.PublicClientApplication(msalConfig);
    loginType = type;
    refreshTime = refresh;

    if (loginType !== 'Popup') {
      auth
        .handleRedirectPromise()
        .then(response => {
          if (response) {
            authService.handleResponse(response);
          }
        })
        .catch(error => {
          window.console.log(error);
        });
    }
  };

  /**
   * See here for more info on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  authService.selectAccount = () => {
    const currentAccounts = auth.getAllAccounts();

    if (currentAccounts.length >= 1) {
      auth.setActiveAccount(currentAccounts[0]);
      authService.isAuthenticated();
      authService.acquireToken();
      authService.setUser();
    }
  };

  /**
   * To see the full list of response object properties, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#response
   */
  authService.handleResponse = (response: msal.AuthenticationResult) => {
    if (response !== null) {
      auth.setActiveAccount(response.account);
      authService.selectAccount();
    } else {
      selectAccount();
    }
  };

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  authService.signIn = async () => {
    if (loginType === 'Popup') {
      const res = await auth
        .loginPopup({
          scopes: ['openid', 'profile', 'email', 'offline_access'],
        })
        .catch(error => {
          window.console.log(error);
        });

      handleResponse(res);
    } else {
      await auth.loginRedirect({
        scopes: ['openid', 'profile', 'email', 'offline_access'],
      });
    }
  };

  /**
   * You can pass a custom request object below. This will override the initial configuration. For more information, visit:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/request-response-object.md#request
   */
  authService.signOut = async () => {
    const logoutRequest: msal.EndSessionPopupRequest = {
      postLogoutRedirectUri: window.location.origin,
      mainWindowRedirectUri: window.location.origin,
    };

    useAuthStore().resetStore();

    loginType === 'Popup'
      ? await auth.logoutPopup(logoutRequest)
      : auth.logoutRedirect(logoutRequest);
  };

  /**
   * See here for more information on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */
  authService.acquireToken = async () => {
    const request: msal.PopupRequest | msal.RedirectRequest = {
      scopes: ['openid'],
      account: auth?.getActiveAccount(),
      forceRefresh: false,
    };

    try {
      window.console.log(auth);
      const response = await auth.acquireTokenSilent(request);

      window.console.log(`access_token acquired at: ${new Date().toString()}`);
      token = response.idToken;
      useAuthStore().setToken(response.idToken);
      useAuthStore().setSessionStarted(new Date().toString());

      return response.idToken;
    } catch (err) {
      window.console.log('Silent token failed. \n', err);
    }
  };

  authService.tokenInterval = () => {
    authService.acquireToken();
  };

  authService.setUser = () => {
    const account = auth?.getActiveAccount();

    if (!account) {
      return null;
    }

    useAuthStore().setUser(account.name);
    useAuthStore().setUserEmail(account.username);
    useAuthStore().setRoles(account.idTokenClaims.roles);
  };

  /**
   * Check if user is authenticated
   * @returns boolean
   */
  authService.isAuthenticated = () => {
    const account = auth?.getActiveAccount();

    if (!account) {
      return false;
    }

    const isAuthn =
      (new Date(account.idTokenClaims.exp * 1000) as number) > Date.now();

    useAuthStore().setIsAuthenticated(isAuthn);

    return isAuthn;
  };

  return authService;
}

export default new Auth();
