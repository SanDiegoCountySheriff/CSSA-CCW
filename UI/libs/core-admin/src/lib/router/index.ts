import Home from '@core-admin/views/HomeView.vue';
import Routes from '@core-admin/router/routes';
import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: Routes.HOME_ROUTE_PATH,
    name: 'Home',
    component: Home,
  },
  {
    path: Routes.SETTINGS_ROUTE_PATH,
    name: 'Settings',
    component: () =>
      import(/* webpackChunkName: "brand" */ '@core-admin/views/BrandView.vue'),
  },
  {
    path: Routes.APPOINTMENTS_ROUTE_PATH,
    name: 'Appointments',
    component: () =>
      import(
        /* webpackChunkName: "appointment" */ '@core-admin/views/AppointmentView.vue'
      ),
  },
  {
    path: Routes.PERMITS_ROUTE_PATH,
    name: 'Applications',
    component: () =>
      import(
        /* webpackChunkName: "permits" */ '@core-admin/views/PermitsView.vue'
      ),
  },
  {
    path: `${Routes.PERMITS_ROUTE_PATH}/:orderId`,
    name: 'PermitDetail',
    component: () =>
      import(
        /* webpackChunkName: "permitdetail" */ '@core-admin/views/PermitDetailView.vue'
      ),
  },
  {
    // keep this at the very end
    path: '*',
    component: () =>
      import(
        /* webpackChunkName: "404" */ '@core-admin/views/NotFoundView.vue'
      ),
  },
];

export const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

router.beforeEach((to, from, next) => {
  import('@shared-ui/stores/auth').then(auth => {
    const store = auth.useAuthStore();

    if (!store.getAuthState.isAuthenticated && to.name !== 'Home') {
      next({ name: 'Home' });
    } else next();
  });
});
