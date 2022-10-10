import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '@core-admin/views/Home.vue';
import Brand from '@core-admin/views/Brand.vue';

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
    component: Brand,
  },
];

export const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});
