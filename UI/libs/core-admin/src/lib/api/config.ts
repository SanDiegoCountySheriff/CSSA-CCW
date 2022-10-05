import axios from 'axios';
import interceptors from '@core-admin/api/interceptors';
import { useAdminAppConfigStore } from '@core-admin/stores/adminAppConfig';

const initialize = async () => {
  const res = await axios.get('/config.json');
  const configStore = useAdminAppConfigStore();

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
  };

  configStore.setAdminAppConfig(config);
  const { clientId, authority, knownAuthorities, loginType } = config;

  import('@shared-ui/api/auth/authentication').then(auth => {
    auth.default.setupAuth(clientId, authority, knownAuthorities, loginType);
    interceptors();
  });

  return res.data;
};
export default initialize;
