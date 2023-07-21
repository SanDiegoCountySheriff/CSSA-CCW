import axios from 'axios'
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

  async function chargeCard(
    token: string,
    amount: number,
    applicationId: string,
    paymentType: string
  ) {
    const res = await axios
      .put(
        `http://localhost:5180/payment/v1/payment/chargeCard?token=${token}&amount=${amount}&applicationId=${applicationId}&paymentType=${paymentType}`
      )
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  async function refundPayment(
    transactionId: string,
    amount: number,
    applicationId: string
  ) {
    const res = await axios
      .put(
        `http://localhost:5180/payment/v1/payment/refundPayment?transactionId=${transactionId}&amount=${amount}&applicationId=${applicationId}`
      )
      .catch(err => {
        console.warn(err)

        return Promise.reject()
      })

    return res?.data
  }

  return {
    state,
    getPaymentType,
    setPaymentType,
    chargeCard,
    refundPayment,
  }
})
