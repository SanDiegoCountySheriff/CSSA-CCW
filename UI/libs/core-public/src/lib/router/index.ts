import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '@core-public/views/Home.vue';
import ApplicationView from '@core-public/views/ApplicationView.vue';
import FormView from '@core-public/views/FormView.vue';
import SecondFormView from '@core-public/views/SecondFormView.vue';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/application',
    name: 'Application',
    component: ApplicationView,
  },
  {
    path: '/form',
    name: 'form',
    component: FormView,
  },
  {
    path: '/form-2',
    name: 'form-2',
    component: SecondFormView,
  },
  {
    // keep this at the very end
    path: '*',
    component: () =>
      import(/* webpackChunkName: "404" */ '@core-public/views/NotFound.vue'),
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
    if (!store.isAuthenticated && to.name !== 'Home') {
      next({ name: 'Home' });
    } else next();
  });
});
