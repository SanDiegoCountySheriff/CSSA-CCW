import { defineStore } from 'pinia'
import { computed, reactive } from 'vue'

export const usePaymentStore = defineStore('paymentStore', () => {
  const state = reactive({
    paymentType: '',
  })

  const getPaymentType = computed(() => state)

  function setPaymentType(payload: string) {
    state.paymentType = payload
  }

  return {
    state,
    getPaymentType,
    setPaymentType,
  }
})
