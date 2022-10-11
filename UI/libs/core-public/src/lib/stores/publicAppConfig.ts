import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { PublicAppConfigType } from '@core-public/types';

export const usePublicAppConfigStore = defineStore('PublicAppConfig', () => {
  const publicAppConfig = ref<PublicAppConfigType>({
    apiBaseUrl: '',
    apiSubscription: '',
    authorityUrl: '',
    knownAuthorities: [],
    clientId: '',
    defaultCounty: '',
    displayDebugger: false,
    environmentName: '',
    loginType: '',
    refreshTime: 0,
  });
  const getPublicAppConfig = computed(() => publicAppConfig.value);

  function setPublicAppConfig(payload: PublicAppConfigType) {
    publicAppConfig.value = payload;
  }

  return { publicAppConfig, getPublicAppConfig, setPublicAppConfig };
});
