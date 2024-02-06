import { RefundRequest } from '@shared-utils/types/defaultTypes'
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
    orderId: string,
    paymentType: string
  ) {
    await axios
      .get(
        `http://localhost:5180/payment/v1/payment/makePayment?applicationId=${applicationId}&amount=${amount}&orderId=${orderId}&paymentType=${paymentType}`
      )
      .then(response => {
        window.location.href = response.data
      })
      .catch(err => window.console.log(err))
  }

  async function refundPayment(payment: RefundRequest) {
    await axios
      .post('http://localhost:5180/payment/v1/payment/refundPayment', payment)
      .then(response => {
        window.console.log(response.data)
      })
      .catch(err => window.console.log(err))
  }

  async function updatePaymentHistory(
    transactionId: string,
    successful: boolean,
    amount: number,
    paymentType: number,
    transactionDateTime: string,
    hmac: string
  ) {
    await axios.post(
      `http://localhost:5180/payment/v1/payment/updatePaymentHistory?transactionId=${transactionId}&successful=${successful}&amount=${amount}&transactionDateTime=${transactionDateTime}&hmac=${hmac}`,
      null
    )
  }

  return {
    state,
    getPaymentType,
    setPaymentType,
    getPayment,
    refundPayment,
    updatePaymentHistory,
  }
})
