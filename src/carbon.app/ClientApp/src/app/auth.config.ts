import {AuthConfig} from "angular-oauth2-oidc";

export const authConfig: AuthConfig = {

  waitForTokenInMsec: 1000,

  responseType: 'code',

  // URL of the SPA to redirect the user to after login
  redirectUri: window.location.origin + '/callback',
  silentRefreshRedirectUri: window.location.origin + '/silent-refresh',

  // The SPA's id. The SPA is registered with this id at the auth-server
  clientId: 'carbon.app',

  // set the scope for the permissions the client should request
  scope: 'openid profile carbon.read carbon.write',
};
