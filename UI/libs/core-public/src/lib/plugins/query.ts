import Vue from 'vue'
import { VueQueryPlugin, VueQueryPluginOptions } from '@tanstack/vue-query'

const vueQueryPluginOptions: VueQueryPluginOptions = {
  queryClientConfig: {
    defaultOptions: {
      queries: {
        retry: 1,
        retryDelay: 2000,
        staleTime: 3600 * 1000,
        refetchOnWindowFocus: false,
        refetchOnMount: false,
      },
    },
  },
}

Vue.use(VueQueryPlugin, vueQueryPluginOptions)
