import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { ConfigType } from '@core-admin/api/apiTypes';

export const useAdminAppConfigStore = defineStore('AdminAppConfig', () => {
  const adminAppConfig = ref({});
  const getAdminAppConfig = computed(() => adminAppConfig.value);

  function setAdminAppConfig(payload: ConfigType) {
    adminAppConfig.value = payload;
  }

  return { adminAppConfig, getAdminAppConfig, setAdminAppConfig };
});
