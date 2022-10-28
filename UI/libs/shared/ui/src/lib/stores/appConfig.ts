import { AppConfigType } from '@shared-utils/types/defaultTypes';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAppConfigStore = defineStore('AppStore', () => {
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
