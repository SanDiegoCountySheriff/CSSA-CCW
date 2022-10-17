import Vue from 'vue';
import { createPinia, PiniaVuePlugin } from 'pinia';
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate';

Vue.use(PiniaVuePlugin);

export const pinia = createPinia();
pinia.use(piniaPluginPersistedstate);
