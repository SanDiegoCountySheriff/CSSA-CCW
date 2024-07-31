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
import { ApplicationType } from '@shared-utils/types/defaultTypes'
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
    case ApplicationType.Standard:
      state.payment.applicationCost = brandStore.brand.cost.new.standard
      break
    case ApplicationType.Judicial:
      state.payment.applicationCost = brandStore.brand.cost.new.judicial
      break
    case ApplicationType.Reserve:
      state.payment.applicationCost = brandStore.brand.cost.new.reserve
      break
    case ApplicationType.Employment:
      state.payment.applicationCost = brandStore.brand.cost.new.employment
      break
    case ApplicationType['Modify Standard']:
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case ApplicationType['Modify Judicial']:
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case ApplicationType['Modify Reserve']:
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case ApplicationType['Modify Employment']:
      state.payment.applicationCost = brandStore.brand.cost.modify
      break
    case ApplicationType['Renew Standard']:
      state.payment.applicationCost = brandStore.brand.cost.renew.standard
      break
    case ApplicationType['Renew Judicial']:
      state.payment.applicationCost = brandStore.brand.cost.renew.judicial
      break
    case ApplicationType['Renew Reserve']:
      state.payment.applicationCost = brandStore.brand.cost.renew.reserve
      break
    case ApplicationType['Renew Employment']:
      state.payment.applicationCost = brandStore.brand.cost.renew.employment
      break
    case ApplicationType['Duplicate Standard']:
      state.payment.applicationCost = brandStore.brand.cost.duplicateFee
      break
    case ApplicationType['Duplicate Judicial']:
      state.payment.applicationCost = brandStore.brand.cost.duplicateFee
      break
    case ApplicationType['Duplicate Reserve']:
      state.payment.applicationCost = brandStore.brand.cost.duplicateFee
      break
    case ApplicationType['Duplicate Employment']:
      state.payment.applicationCost = brandStore.brand.cost.duplicateFee
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
