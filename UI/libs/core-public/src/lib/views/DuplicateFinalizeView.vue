<template>
  <v-container>
    <v-card
      :loading="isMakePaymentLoading"
      max-width="900"
      class="mx-auto"
      flat
    >
      <v-card-title class="justify-center">
        Request a duplicate permit
      </v-card-title>

      <v-card-title>
        If you have lost or damaged your permit you may request a duplicate for
        a non-refundable fee of ${{ brandStore.brand.cost.duplicateFee }}.
      </v-card-title>

      <v-card-title>
        {{ brandStore.brand.agencyName }} Licensing staff will review the
        request and issue a replacement permit to you.
      </v-card-title>

      <v-card-title class="justify-center">
        <v-alert type="warning">
          You must click 'Finish' after completing your payment in order for the
          payment to be processed.
        </v-alert>
      </v-card-title>

      <v-card-text class="text-center">
        <v-btn
          @click="makePayment"
          color="success"
        >
          <v-icon left>mdi-currency-usd</v-icon>
          Pay Now
        </v-btn>
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="handleBackHome"
          :disabled="hasPaymentBeenMade"
          color="primary"
        >
          Back Home
        </v-btn>

        <v-spacer />

        <v-btn
          :disabled="!hasPaymentBeenMade"
          color="primary"
        >
          Submit
        </v-btn>
      </v-card-actions>
    </v-card>

    <v-dialog
      v-model="isUpdatePaymentHistoryLoading"
      max-width="600"
      persistent
    >
      <v-card loading>
        <v-card-title> Processing Payment </v-card-title>

        <v-card-text>
          Please do not close the browser or click the back button.
        </v-card-text>
      </v-card>
    </v-dialog>

    <v-snackbar
      v-model="paymentSnackbar"
      :timeout="-1"
      color="primary"
      persistent
    >
      {{ $t('There was a problem processing the payment. Please try again.') }}
      <v-btn
        @click="paymentSnackbar = !paymentSnackbar"
        icon
      >
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import Routes from '@core-public/router/routes'
import { router } from '@core-public/router'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { useRoute } from 'vue-router/composables'
import {
  PaymentHistoryType,
  PaymentType,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, ref } from 'vue'

const brandStore = useBrandStore()
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const paymentSnackbar = ref(false)
const route = useRoute()

const hasPaymentBeenMade = computed(() => {
  return applicationStore.completeApplication.paymentHistory.some(
    (ph: PaymentHistoryType) => {
      return (
        ph.paymentType === PaymentType['CCW Application Duplicate Payment'] &&
        ph.duplicateNumber ===
          applicationStore.completeApplication.application.duplicateNumber
      )
    }
  )
})

function handleBackHome() {
  router.push({
    path: Routes.APPLICATION_DETAIL_ROUTE,
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete:
        applicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}

const { mutate: makePayment, isLoading: isMakePaymentLoading } = useMutation({
  mutationFn: () => {
    const paymentType = PaymentType['CCW Application Duplicate Payment']
    const cost = brandStore.brand.cost.duplicateFee

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      null,
      applicationStore.completeApplication.application.orderId,
      paymentType.toString()
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})

const {
  mutate: updatePaymentHistory,
  isLoading: isUpdatePaymentHistoryLoading,
} = useMutation({
  mutationFn: ({
    transactionId,
    successful,
    amount,
    paymentType,
    transactionDateTime,
    hmac,
    applicationId,
  }: {
    transactionId: string
    successful: boolean
    amount: number
    paymentType: string
    transactionDateTime: string
    hmac: string
    applicationId: string
  }) => {
    return paymentStore.updatePaymentHistory(
      transactionId,
      successful,
      amount,
      paymentType,
      transactionDateTime,
      hmac,
      applicationId
    )
  },
  onSuccess: () =>
    applicationStore
      .getCompleteApplicationFromApi(
        applicationStore.completeApplication.id,
        Boolean(route.query.isComplete)
      )
      .then(res => {
        applicationStore.setCompleteApplication(res)
      }),
})

onMounted(() => {
  const transactionId = route.query.transactionId
  const successful = route.query.successful
  const amount = route.query.amount
  const hmac = route.query.hmac
  const paymentType = route.query.paymentType
  const applicationId = route.query.applicationId
  let transactionDateTime = route.query.transactionDateTime

  if (typeof transactionDateTime === 'string') {
    transactionDateTime = transactionDateTime.replace(':', '%3A')
    transactionDateTime = transactionDateTime.replace(':', '%3A')
  }

  if (
    typeof transactionId === 'string' &&
    typeof successful === 'string' &&
    typeof amount === 'string' &&
    typeof paymentType === 'string' &&
    typeof transactionDateTime === 'string' &&
    typeof hmac === 'string' &&
    typeof applicationId === 'string'
  ) {
    updatePaymentHistory({
      transactionId,
      successful: Boolean(successful),
      amount: Number(amount),
      paymentType,
      transactionDateTime,
      hmac,
      applicationId,
    })
  }
})
</script>
