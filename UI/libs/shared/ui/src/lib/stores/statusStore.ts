import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

export type StatusType = {
  isOnline: boolean
}

export const useStatusStore = defineStore('StatusStore', () => {
  const status = ref<StatusType>({
    isOnline: false,
  })
  const getConnectionStatus = computed(() => status.value)

  function setConnectionStatus(payload: boolean) {
    status.value.isOnline = payload
  }

  return {
    status,
    getConnectionStatus,
    setConnectionStatus,
  }
})
