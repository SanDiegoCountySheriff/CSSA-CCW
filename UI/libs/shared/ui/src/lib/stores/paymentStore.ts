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
      .catch(err => {
        throw err
      })
  }

  async function refundPayment(payment: RefundRequest) {
    await axios
      .post('http://localhost:5180/payment/v1/payment/refundPayment', payment)
      .then(response => {
        window.console.log(response.data)
      })
      .catch(err => {
        throw err
      })
  }

  async function updatePaymentHistory(
    transactionId: string,
    successful: boolean,
    amount: number,
    paymentType: string,
    transactionDateTime: string,
    hmac: string,
    applicationId: string
  ) {
    await axios
      .post(
        `http://localhost:5180/payment/v1/payment/updatePaymentHistory?transactionId=${transactionId}&successful=${successful}&amount=${amount}&paymentType=${paymentType}&transactionDateTime=${transactionDateTime}&hmac=${hmac}&applicationId=${applicationId}`,
        null
      )
      .then(response => response)
      .catch(err => {
        throw err
      })
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
