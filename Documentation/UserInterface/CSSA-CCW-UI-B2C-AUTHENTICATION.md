## Overview

OpenID Connect (OIDC) is an authentication protocol that's built on OAuth 2.0. You can use it to securely sign a user into an application. This SPA sample uses MSAL.js and the OIDC PKCE flow. MSAL.js is a Microsoft provided library that simplifies adding authentication and authorization support to SPAs.

## Sign in flow
The sign-in flow involves the following steps:

1. Users go to the web app and select Sign-in.
2. The app initiates an authentication request and redirects users to Azure AD B2C.
3. Users sign up or sign in and reset the password. Alternatively, they can sign in with a social account.
4. After users sign in, Azure AD B2C returns an authorization code to the app.
5. The single-page application validates the ID token, reads the claims, and in turn allows users to call protected resources and APIs.

## App registration overview
To enable your app to sign in with Azure AD B2C and call a web API, you register two applications in the Azure AD B2C directory.

- The web application registration enables your app to sign in with Azure AD B2C. During the registration, you specify the redirect URI. The redirect URI is the endpoint to which users are redirected by Azure AD B2C after their authentication with Azure AD B2C is completed. The app registration process generates an application ID, also known as the client ID, which uniquely identifies your app.

The app architecture and registrations are illustrated in the following diagram:

![SPA-Auth](https://learn.microsoft.com/en-us/azure/active-directory-b2c/media/configure-authentication-sample-spa-app/spa-app-with-api-architecture.png)

## Call to a web API or .net API's
After the authentication is completed, users interact with the app, which invokes a protected API. The API uses bearer token authentication. The bearer token is the access token that the app obtained from Azure AD B2C. The app passes the token in the authorization header of the HTTPS request.


`Authorization: Bearer <token>`

## Sign out flow
The sign-out flow involves the following steps:

1. From the app, users sign out.
2. The app clears its session objects, and the authentication library clears its token cache.
3. The app takes users to the Azure AD B2C sign-out endpoint to terminate the Azure AD B2C session.
4. Users are redirected back to the app.
