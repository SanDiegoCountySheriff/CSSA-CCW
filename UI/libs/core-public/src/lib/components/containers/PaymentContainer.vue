<template>
  <v-card :loading="isUpdatePaymentHistoryLoading">
    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <PaymentWrapper
          v-if="state.payment.applicationCost"
          :payment="state.payment"
        />
      </v-col>
      <v-col
        cols="12"
        lg="4"
      >
        <PaymentButtonContainer
          @cash-payment="handleCashPayment"
          :hide-online-payment="props.hideOnlinePayment"
        />
      </v-col>
    </v-row>
  </v-card>
</template>

<script setup lang="ts">
import PaymentButtonContainer from '@core-public/components/containers/PaymentButtonContainer.vue'
import PaymentWrapper from '@core-public/components/wrappers/PaymentWrapper.vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { inject, onMounted, reactive } from 'vue'

interface IPaymentContainerProps {
  paymentComplete: boolean
  hideOnlinePayment: boolean
}

const props = defineProps<IPaymentContainerProps>()

const brandStore = useBrandStore()
const application = useCompleteApplicationStore()
const isUpdatePaymentHistoryLoading = inject('isUpdatePaymentHistoryLoading')
const paymentStore = usePaymentStore()

const state = reactive({
  payment: {
    applicationCost: 0,
    convenienceFee: 0,
    creditFee: 0,
    totalCost: 0,
  },
  isLoading: false,
  isError: false,
})

onMounted(() => {
  switch (application.completeApplication.application.applicationType) {
    case 'standard':
      state.payment.applicationCost = brandStore.brand.cost.new.standard
      break
    case 'judicial':
      state.payment.applicationCost = brandStore.brand.cost.new.judicial
      break
    case 'reserve':
      state.payment.applicationCost = brandStore.brand.cost.new.reserve
      break
    case 'employment':
      state.payment.applicationCost = brandStore.brand.cost.new.employment
      break
    case 'modify-standard':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'modify-judicial':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'modify-reserve':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'modify-employment':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'renew-standard':
      state.payment.applicationCost = brandStore.brand.cost.renew.standard
      break
    case 'renew-judicial':
      state.payment.applicationCost = brandStore.brand.cost.renew.judicial
      break
    case 'renew-reserve':
      state.payment.applicationCost = brandStore.brand.cost.renew.reserve
      break
    case 'renew-employment':
      state.payment.applicationCost = brandStore.brand.cost.renew.employment
      break
    case 'duplicate-standard':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'duplicate-judicial':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'duplicate-reserve':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case 'duplicate-employment':
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    default:
      return
  }

  state.payment.convenienceFee = brandStore.brand.cost.convenienceFee

  if (application.completeApplication.application.paymentStatus === 0) {
    state.payment.creditFee =
      state.payment.applicationCost + brandStore.brand.cost.creditFee
  }

  state.payment.totalCost =
    state.payment.applicationCost +
    state.payment.convenienceFee +
    state.payment.creditFee
})

function handleCashPayment() {
  state.payment.creditFee = 0
  state.payment.totalCost =
    state.payment.applicationCost + state.payment.convenienceFee
  paymentStore.setPaymentType('cash')
  application.completeApplication.application.paymentStatus = 1
}
</script>
