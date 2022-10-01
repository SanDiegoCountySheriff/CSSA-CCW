import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAdminAppConfigStore = defineStore('AdminAppConfig', () => {
  const adminAppConfig = ref({});
  const getAdminAppConfig = computed(() => adminAppConfig.value);

  return { adminAppConfig, getAdminAppConfig };
});
