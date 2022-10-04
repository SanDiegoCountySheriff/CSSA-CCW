import { ConfigType } from '@core-public/api/apiTypes';
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const usePublicAppConfigStore = defineStore('PublicAppConfig', () => {
  const publicAppConfig = ref<ConfigType>({});
  const getPublicAppConfig = computed(() => publicAppConfig.value);

  function setPublicAppConfig(payload: ConfigType) {
    publicAppConfig.value = payload;
  }

  return { publicAppConfig, getPublicAppConfig, setPublicAppConfig };
});
