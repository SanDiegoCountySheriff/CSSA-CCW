import {
  AppConfigType,
  QuestionsConfig,
} from '@shared-utils/types/defaultTypes';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAppConfigStore = defineStore('ConfigStore', () => {
  const appConfig = ref<AppConfigType>({
    apiBaseUrl: '',
    adminApiBaseUrl: '',
    applicationApiBaseUrl: '',
    documentApiBaseUrl: '',
    paymentApiBaseUrl: '',
    scheduleApiBaseUrl: '',
    userProfileApiBaseUrl: '',
    apiSubscription: '',
    authorityUrl: '',
    knownAuthorities: [],
    clientId: '',
    defaultCounty: '',
    displayDebugger: false,
    environmentName: '',
    loginType: '',
    refreshTime: 0,
    questions: {
      one: 109,
      two: 545,
      three: 327,
      four: 440,
      five: 440,
      six: 440,
      seven: 440,
      eight: 1417,
      nine: 440,
      ten: 440,
      eleven: 770,
      twelve: 440,
      thirteen: 440,
      fourteen: 440,
      fifteen: 440,
      sixteen: 767,
      seventeen: 2661,
    } as QuestionsConfig,
  });
  const getAppConfig = computed(() => appConfig.value);

  function setAppConfig(payload: AppConfigType) {
    appConfig.value = payload;
  }

  return { appConfig, getAppConfig, setAppConfig };
});
