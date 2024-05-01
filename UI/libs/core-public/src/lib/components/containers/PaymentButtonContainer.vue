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
          @click="makePayment"
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
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { ApplicationType, PaymentType } from '@shared-utils/types/defaultTypes'
import { inject, ref } from 'vue'

interface IPaymentButtonContainerProps {
  hideOnlinePayment: boolean
}

defineProps<IPaymentButtonContainerProps>()

const emit = defineEmits(['cash-payment'])
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const showInfo = ref(false)
const isInitialPaymentComplete = inject('isInitialPaymentComplete')
const isUpdatePaymentHistoryLoading = inject('isUpdatePaymentHistoryLoading')
const paymentSnackbar = ref(false)

function handleCashPayment() {
  emit('cash-payment')
  showInfo.value = true
}

const { mutate: makePayment, isLoading } = useMutation({
  mutationFn: () => {
    let cost: number
    let paymentType: string

    switch (applicationStore.completeApplication.application.applicationType) {
      case ApplicationType.Standard:
        cost =
          applicationStore.completeApplication.application.cost.new.standard
        paymentType = PaymentType['CCW Application Initial Payment'].toString()
        break
      case ApplicationType.Judicial:
        cost =
          applicationStore.completeApplication.application.cost.new.judicial
        paymentType =
          PaymentType['CCW Application Initial Judicial Payment'].toString()
        break
      case ApplicationType.Reserve:
        cost = applicationStore.completeApplication.application.cost.new.reserve
        paymentType =
          PaymentType['CCW Application Initial Reserve Payment'].toString()
        break
      case ApplicationType['Renew Standard']:
        cost =
          applicationStore.completeApplication.application.cost.renew.standard
        paymentType = PaymentType['CCW Application Renewal Payment'].toString()
        break
      case ApplicationType['Renew Judicial']:
        cost =
          applicationStore.completeApplication.application.cost.renew.judicial
        paymentType =
          PaymentType['CCW Application Renewal Judicial Payment'].toString()
        break
      case ApplicationType['Renew Reserve']:
        cost =
          applicationStore.completeApplication.application.cost.renew.reserve
        paymentType =
          PaymentType['CCW Application Renewal Reserve Payment'].toString()
        break
      case ApplicationType['Renew Employment']:
        cost =
          applicationStore.completeApplication.application.cost.renew.employment
        paymentType =
          PaymentType['CCW Application Renewal Employment Payment'].toString()
        break
      default:
        cost =
          applicationStore.completeApplication.application.cost.new.standard
        paymentType = PaymentType['CCW Application Initial Payment'].toString()
    }

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      applicationStore.completeApplication.application.orderId,
      paymentType
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})
</script>
