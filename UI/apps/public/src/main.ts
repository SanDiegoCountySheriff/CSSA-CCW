import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import PublicApp from './PublicApp.vue'
import Vue from 'vue'
import { getMsalInstance } from '@shared-ui/api/auth/authentication'
import interceptors from '@core-public/api/interceptors'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import wb from './registerServiceWorker'
import { PiniaVuePlugin, createPinia } from 'pinia'
import { i18n, router, vuetify } from '@cssa-ccw/core-public'
import '@core-public/plugins/query'

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
      render: h => h(PublicApp),
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
