import AdminApp from './AdminApp.vue'
import Vue from 'vue'
import { getMsalInstance } from '@core-admin/api/auth/authentication'
import interceptors from '@core-admin/api/interceptors'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import wb from './registerServiceWorker'
import { PiniaVuePlugin, createPinia } from 'pinia'
import { i18n, router, vuetify } from '@cssa-ccw/core-admin'
import '@core-admin/plugins/query'

Vue.use(PiniaVuePlugin)
const pinia = createPinia()

useAppConfigStore(pinia)

Vue.config.productionTip = false
Vue.prototype.$workbox = wb

getMsalInstance().then(response => {
  interceptors(response).then(() => {
    new Vue({
      pinia,
      router,
      vuetify,
      i18n,
      name: 'Public',
      render: h => h(AdminApp),
    }).$mount('#app')
  })
})
