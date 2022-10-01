import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const usePublicAppConfigStore = defineStore('PublicAppConfig', () => {
  const publicAppConfig = ref({});
  const getPublicAppConfig = computed(() => publicAppConfig.value);

  return { publicAppConfig, getPublicAppConfig };
});
