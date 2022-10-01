import Vue from 'vue';
import PublicApp from './PublicApp.vue';
import { i18n, router, pinia, vuetify } from '@cssa-ccw/core-public';
import wb from './registerServiceWorker';

Vue.config.productionTip = false;
Vue.prototype.$workbox = wb;

new Vue({
  pinia,
  router,
  vuetify,
  i18n,
  render: h => h(PublicApp),
}).$mount('#app');
