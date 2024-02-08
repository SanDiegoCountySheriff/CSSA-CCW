<template>
  <v-container>
    <v-row>
      <v-col class="mt-md-3 ml-lg-15">
        <v-btn
          :disabled="
            isInitialPaymentComplete ||
            isLoading ||
            isUpdatePaymentHistoryLoading
          "
          @click="handleCashPayment"
          color="primary"
          block
        >
          {{ $t('Pay in person') }}
        </v-btn>
      </v-col>
    </v-row>
    <v-row v-if="!hideOnlinePayment">
      <v-col class="mt-md-3 ml-lg-15">
        <v-btn
          :disabled="isInitialPaymentComplete || isUpdatePaymentHistoryLoading"
          :loading="isLoading"
          @click="onlinePayment"
          color="primary"
          block
        >
          {{ $t('Pay Online ') }}
        </v-btn>
      </v-col>
    </v-row>
    <v-row
      v-if="showInfo"
      class="mt-5 ml-md-15"
    >
      <v-alert
        color="primary"
        border="left"
        type="info"
        elevation="2"
        class="font-weight-bold"
      >
        {{
          $t(
            'Remember to bring the updated total to your appointment to pay for the application'
          )
        }}
      </v-alert>
    </v-row>

    <v-snackbar
      v-model="paymentSnackbar"
      :timeout="-1"
      color="primary"
      persistent
    >
      {{ $t('There was a problem processing the payment, please try again.') }}
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
import { PaymentType } from '@shared-utils/types/defaultTypes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { inject, ref } from 'vue'

interface IPaymentButtonContainerProps {
  hideOnlinePayment: boolean
}

defineProps<IPaymentButtonContainerProps>()

const emit = defineEmits(['cash-payment'])
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const showInfo = ref(false)
const isInitialPaymentComplete = inject('isInitialPaymentComplete')
const isUpdatePaymentHistoryLoading = inject('isUpdatePaymentHistoryLoading')
const paymentSnackbar = ref(false)

function handleCashPayment() {
  emit('cash-payment')
  showInfo.value = true
}

const { mutate: makePayment, isLoading } = useMutation({
  mutationFn: () =>
    paymentStore.getPayment(
      applicationStore.completeApplication.id,
      brandStore.brand.cost.new.standard,
      applicationStore.completeApplication.application.orderId,
      PaymentType['CCW Application Initial Payment'].toString()
    ),
  onError: () => {
    paymentSnackbar.value = true
  },
})

async function onlinePayment() {
  makePayment()
}
</script>
