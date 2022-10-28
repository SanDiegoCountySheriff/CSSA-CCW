import AdminApp from './AdminApp.vue';
import Vue from 'vue';
import wb from './registerServiceWorker';
import { i18n, pinia, router, vuetify } from '@cssa-ccw/core-admin';
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
