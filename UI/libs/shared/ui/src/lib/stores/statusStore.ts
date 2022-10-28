import { StatusType } from '@shared-utils/types/defaultTypes';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useStatusStore = defineStore('StatusStore', () => {
  const status = ref<StatusType>({
    isOnline: false,
  });
  const getConnectionStatus = computed(() => status.value);

  function setConnectionStatus(payload: boolean) {
    status.value.isOnline = payload;
  }

  return {
    status,
    getConnectionStatus,
    setConnectionStatus,
  };
});
