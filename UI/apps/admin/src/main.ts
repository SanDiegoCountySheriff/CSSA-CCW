import Vue from 'vue';
import AdminApp from './AdminApp.vue';
import { i18n, router, pinia, vuetify } from '@cssa-ccw/core-admin';
import wb from './registerServiceWorker';
import '@core-admin/plugins/query';

Vue.config.productionTip = false;
Vue.prototype.$workbox = wb;

new Vue({
  pinia,
  router,
  vuetify,
  i18n,
  name: 'Admin',
  render: h => h(AdminApp),
}).$mount('#app');
