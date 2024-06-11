import { defineStore } from 'pinia'
import { ref } from 'vue'
import {
  AppConfigType,
  QuestionsConfig,
} from '@shared-utils/types/defaultTypes'

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
    isPaymentServiceAvailable: false,
    environmentName: '',
    refreshTime: 0,
    payBeforeSubmit: false,
    applicationInsightsConnectionString: '',
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
      eighteen: 2661,
      nineteen: 2661,
      twenty: 2661,
      twentyone: 2661,
    } as QuestionsConfig,
  })

  function setAppConfig(payload: AppConfigType) {
    appConfig.value = payload
  }

  return { appConfig, setAppConfig }
})
