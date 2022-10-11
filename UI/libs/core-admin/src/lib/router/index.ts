import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '@core-admin/views/Home.vue';

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
      import(/* webpackChunkName: "brand" */ '@core-admin/views/Brand.vue'),
  },
];

export const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});
