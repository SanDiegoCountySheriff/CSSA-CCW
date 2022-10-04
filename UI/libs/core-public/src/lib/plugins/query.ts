import Vue from 'vue';
import { VueQueryPlugin, VueQueryPluginOptions } from '@tanstack/vue-query';

const vueQueryPluginOptions: VueQueryPluginOptions = {
  queryClientConfig: {
    defaultOptions: {
      queries: {
        staleTime: 3600 * 1000,
      },
    },
  },
};

Vue.use(VueQueryPlugin, vueQueryPluginOptions);
