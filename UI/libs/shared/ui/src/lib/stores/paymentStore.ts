import Endpoints from '@shared-ui/api/endpoints'
import { RefundRequest } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { defineStore } from 'pinia'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { computed, reactive, ref } from 'vue'

export const usePaymentStore = defineStore('paymentStore', () => {
  const state = reactive({
    paymentType: '',
  })
  const appConfigStore = useAppConfigStore()

  const getPaymentType = computed(() => state.paymentType)
  const isOnlinePaymentAvailable = ref(
    appConfigStore.appConfig.isPaymentServiceAvailable
  )

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
        `${Endpoints.GET_PAYMENT_ENDPOINT}?applicationId=${applicationId}&amount=${amount}&orderId=${orderId}&paymentType=${paymentType}`
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
      .post(Endpoints.REFUND_PAYMENT_ENDPOINT, payment)
      .then(response => {
        window.console.log(response.data)
      })
      .catch(err => {
        throw err
      })
  }

  async function requestRefund(refundRequest: RefundRequest) {
    await axios.post(Endpoints.REQUEST_REFUND_ENDPOINT, refundRequest)
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
        `${Endpoints.UPDATE_PAYMENT_HISTORY_ENDPOINT}?transactionId=${transactionId}&successful=${successful}&amount=${amount}&paymentType=${paymentType}&transactionDateTime=${transactionDateTime}&hmac=${hmac}&applicationId=${applicationId}`,
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
    isOnlinePaymentAvailable,
    setPaymentType,
    getPayment,
    refundPayment,
    updatePaymentHistory,
    requestRefund,
  }
})
