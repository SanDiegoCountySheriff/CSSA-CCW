import Vue from 'vue';
import PublicApp from './PublicApp.vue';
import { router, vuetify, store, i18n } from '@cssa-ccw/core-public';
import wb from './registerServiceWorker';

Vue.config.productionTip = false;
Vue.prototype.$workbox = wb;

new Vue({
  store,
  router,
  vuetify,
  i18n,
  render: h => h(PublicApp),
}).$mount('#app');
