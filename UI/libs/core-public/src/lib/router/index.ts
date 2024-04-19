import ApplicationDetailView from '@core-public/views/ApplicationDetailView.vue'
import ApplicationView from '@core-public/views/ApplicationView.vue'
import FinalizeView from '@core-public/views/FinalizeView.vue'
import FormView from '@core-public/views/FormView.vue'
import HomeView from '@core-public/views/HomeView.vue'
import ModifyFinalizeView from '@core-public/views/ModifyFinalizeView.vue'
import ModifyFormView from '@core-public/views/ModifyFormView.vue'
import MoreInformationView from '@core-public/views/MoreInformationView.vue'
import PenalView from '@core-public/views/PenalView.vue'
import RecieptView from '@core-public/views/RecieptView.vue'
import RenewApplicationView from '@core-public/views/RenewApplicationView.vue'
import Routes from '@core-public/router/routes'
import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: Routes.HOME_ROUTE_PATH,
    name: 'Home',
    component: HomeView,
  },
  {
    path: Routes.APPLICATION_ROUTE_PATH,
    name: 'Application',
    component: ApplicationView,
    props: true,
  },
  {
    path: Routes.APPLICATION_DETAIL_ROUTE,
    name: 'ApplicationDetails',
    component: ApplicationDetailView,
  },
  {
    path: Routes.FINALIZE_ROUTE_PATH,
    name: 'finalize',
    component: FinalizeView,
  },
  {
    path: Routes.FORM_ROUTE_PATH,
    name: 'form',
    component: FormView,
  },
  {
    path: Routes.MORE_INFORMATION_ROUTE_PATH,
    name: 'moreinformation',
    component: MoreInformationView,
  },
  {
    path: Routes.PENAL_CODE_PATH,
    name: 'penalcode',
    component: PenalView,
  },
  {
    path: Routes.RENEW_APPLICATION_ROUTE_PATH,
    name: 'RenewApplication',
    component: RenewApplicationView,
  },
  {
    path: Routes.RECEIPT_PATH,
    name: 'Reciept',
    component: RecieptView,
  },
  {
    path: Routes.MODIFY_FORM_PATH,
    name: 'ModifyForm',
    component: ModifyFormView,
  },
  {
    path: Routes.MODIFY_FINALIZE_PATH,
    name: 'ModifyFinalize',
    component: ModifyFinalizeView,
  },
  {
    // keep this at the very end
    path: '*',
    component: () =>
      import(/* webpackChunkName: "404" */ '@core-public/views/NotFound.vue'),
  },
]

export const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
})

router.beforeEach((to, from, next) => {
  import('@shared-ui/stores/auth').then(auth => {
    const store = auth.useAuthStore()

    if (
      !store.getAuthState.isAuthenticated &&
      to.name !== 'Home' &&
      to.name !== 'moreinformation'
    ) {
      next({ name: 'Home' })
    } else {
      next()
    }
  })
})
