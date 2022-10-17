import { ref, computed } from 'vue';
import { defineStore } from 'pinia';
import { AppConfigType } from '@shared-utils/types/defaultTypes';

export const useAppConfigStore = defineStore('PublicAppConfig', () => {
  const appConfig = ref<AppConfigType>({
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
  const getAppConfig = computed(() => appConfig.value);

  function setAppConfig(payload: AppConfigType) {
    appConfig.value = payload;
  }

  return { appConfig, getAppConfig, setAppConfig };
});
