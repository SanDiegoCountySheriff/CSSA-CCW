import Home from '@core-admin/views/HomeView.vue';
import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/brand',
    name: 'Brand',
    component: () =>
      import(/* webpackChunkName: "brand" */ '@core-admin/views/BrandView.vue'),
  },
  {
    path: '/appointments',
    name: 'Appointments',
    component: () =>
      import(
        /* webpackChunkName: "appointment" */ '@core-admin/views/AppointmentView.vue'
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
