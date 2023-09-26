import { defineStore } from 'pinia'
import { computed, reactive } from 'vue'
import { useRouter } from 'vue-router/composables'
import axios from 'axios'

export const usePaymentStore = defineStore('paymentStore', () => {
  const state = reactive({
    paymentType: '',
  })
  const router = useRouter()

  const getPaymentType = computed(() => state.paymentType)

  function setPaymentType(payload: string) {
    state.paymentType = payload
  }

  async function getPayment() {
    // await fetch('http://localhost:5180/payment/v1/payment/makePayment', {
    //   method: 'GET',
    //   mode: 'no-cors',
    // })
    await axios
      .get('http://localhost:5180/payment/v1/payment/makePayment')
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
