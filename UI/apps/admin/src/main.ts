import AdminApp from './AdminApp.vue'
import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import Vue from 'vue'
import { getMsalInstance } from '@shared-ui/api/auth/authentication'
import interceptors from '@core-admin/api/interceptors'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import wb from './registerServiceWorker'
import { PiniaVuePlugin, createPinia } from 'pinia'
import { i18n, router, vuetify } from '@cssa-ccw/core-admin'
import '@core-admin/plugins/query'

Vue.use(PiniaVuePlugin)
const pinia = createPinia()

useAppConfigStore(pinia)
const configStore = useAppConfigStore()

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
  const appInsights = new ApplicationInsights({
    config: {
      connectionString:
        configStore.appConfig.applicationInsightsConnectionString,
    },
  })

  appInsights.loadAppInsights()
  appInsights.trackPageView()
})
