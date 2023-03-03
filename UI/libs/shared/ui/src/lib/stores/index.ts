import Vue from 'vue';
import piniaPluginPersistedState from 'pinia-plugin-persistedstate';
import { PiniaVuePlugin, createPinia } from 'pinia';

Vue.use(PiniaVuePlugin);

export const pinia = createPinia();
pinia.use(piniaPluginPersistedState);
