import axios from 'axios'
import { defineStore } from 'pinia'
import { computed, reactive } from 'vue'

export const usePaymentStore = defineStore('paymentStore', () => {
  const state = reactive({
    paymentType: '',
  })

  const getPaymentType = computed(() => state.paymentType)

  function setPaymentType(payload: string) {
    state.paymentType = payload
  }

  async function getPayment(
    applicationId: string,
    amount: number,
    orderId: string
  ) {
    await axios
      .get(
        `http://localhost:5180/payment/v1/payment/makePayment?applicationId=${applicationId}&amount=${amount}&orderId=${orderId}`
      )
      .then(response => {
        window.location.href = response.data
      })
      .catch(err => window.console.log(err))
  }

  return {
    state,
    getPaymentType,
    setPaymentType,
    getPayment,
  }
})
