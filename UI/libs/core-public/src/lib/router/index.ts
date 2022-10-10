import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '@core-public/views/Home.vue';
import FormView from '@core-public/views/FormView.vue';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/form',
    name: 'form',
    component: FormView,
  },
  {
    path: '/form-2',
    name: 'form-2',
    component: FormView,
  },
];

export const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});
