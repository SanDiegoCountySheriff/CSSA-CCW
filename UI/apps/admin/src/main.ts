import Vue from 'vue';
import AdminApp from './AdminApp.vue';
import { router, vuetify, store, i18n } from '@cssa-ccw/core-admin';
import wb from './registerServiceWorker';

Vue.config.productionTip = false;
Vue.prototype.$workbox = wb;

new Vue({
  store,
  router,
  vuetify,
  i18n,
  render: h => h(AdminApp),
}).$mount('#app');
