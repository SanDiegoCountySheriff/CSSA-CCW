import axios from 'axios';
import interceptors from '@core-public/api/interceptors';
import { usePublicAppConfigStore } from '@core-public/stores/publicAppConfig';

const initialize = async () => {
  const res = await axios.get('/config.json');
  const configStore = usePublicAppConfigStore();

  const config = {
    apiBaseUrl: res.data.Configuration.ServicesBaseUrl,
    apiSubscription: res.data.Configuration.Subscription,
    authority: res.data.Authentication.AuthorityUrl,
    knownAuthorities: res.data.Authentication.KnownAuthorities,
    clientId: res.data.Authentication.ClientId,
    defaultCounty: res.data.Configuration.DefaultCounty,
    displayDebugger: res.data.Configuration.DisplayDebugger === 'True',
    environmentName: res.data.Configuration?.Environment.toUpperCase() || 'DEV',
    loginType: res.data.Authentication.LoginType || 'Popup',
    refreshTime: res.data.Authentication.RefreshTimeInMinutes || 30,
  };

  configStore.setPublicAppConfig(config);
  const { clientId, authority, knownAuthorities, loginType, refreshTime } =
    config;

  import('@shared-ui/api/auth/authentication').then(auth => {
    auth.default.setupAuth(
      clientId,
      authority,
      knownAuthorities,
      loginType,
      refreshTime
    );
    // in case of refresh
    auth.default.selectAccount();
    interceptors();
  });

  return res.data;
};
export default initialize;
