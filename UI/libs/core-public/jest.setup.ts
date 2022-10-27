import { PiniaVuePlugin } from 'pinia';
import Vue from 'vue';
import { VueQueryPlugin } from '@tanstack/vue-query';
import Vuetify from 'vuetify';

Vue.use(Vuetify);
Vue.use(PiniaVuePlugin);
Vue.use(VueQueryPlugin);
