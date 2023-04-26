import Home from '@core-admin/views/HomeView.vue'
import Routes from '@core-admin/router/routes'
import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: Routes.HOME_ROUTE_PATH,
    name: 'Home',
    component: Home,
  },
  {
    path: Routes.SETTINGS_ROUTE_PATH,
    name: 'Settings',
    component: () => import('@core-admin/views/BrandView.vue'),
  },
  {
    path: Routes.RECEIPT_ROUTE_PATH,
    name: 'Receipt',
    component: () => import('@core-admin/views/ReceiptView.vue'),
  },
  {
    path: Routes.APPOINTMENTS_ROUTE_PATH,
    name: 'Appointments',
    component: () => import('@core-admin/views/AppointmentView.vue'),
  },
  {
    path: Routes.APPOINTMENT_MANAGEMENT_ROUTE_PATH,
    name: 'AppointmentManagement',
    component: () => import('@core-admin/views/AppointmentManagementView.vue'),
  },
  {
    path: Routes.PERMITS_ROUTE_PATH,
    name: 'Applications',
    component: () => import('@core-admin/views/PermitsView.vue'),
  },
  {
    path: `${Routes.PERMITS_ROUTE_PATH}/:orderId`,
    name: 'PermitDetail',
    component: () => import('@core-admin/views/PermitDetailView.vue'),
  },
  {
    // keep this at the very end
    path: '*',
    component: () => import('@core-admin/views/NotFoundView.vue'),
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

    if (!store.getAuthState.isAuthenticated && to.name !== 'Home') {
      store.resetStore()
      next({ name: 'Home' })
    } else next()
  })
})
