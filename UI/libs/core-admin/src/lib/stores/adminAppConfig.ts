import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { AdminAppConfigType } from '@core-admin/types';

export const useAdminAppConfigStore = defineStore('AdminAppConfig', () => {
  const adminAppConfig = ref<AdminAppConfigType>({
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
  const getAdminAppConfig = computed(() => adminAppConfig.value);

  function setAdminAppConfig(payload: AdminAppConfigType) {
    adminAppConfig.value = payload;
  }

  return { adminAppConfig, getAdminAppConfig, setAdminAppConfig };
});
