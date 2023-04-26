import PublicApp from './PublicApp.vue'
import Vue from 'vue'
import wb from './registerServiceWorker'
import { i18n, pinia, router, vuetify } from '@cssa-ccw/core-public'
import '@core-public/plugins/query'

Vue.config.productionTip = false
Vue.prototype.$workbox = wb

new Vue({
  pinia,
  router,
  vuetify,
  i18n,
  name: 'Public',
  render: h => h(PublicApp),
}).$mount('#app')
