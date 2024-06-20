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
import { useBrandStore } from '@shared-ui/stores/brandStore'
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
  mutationFn: () => {
    let cost: number
    let paymentType: string
    let livescanAmount: number | null | undefined

    switch (applicationStore.completeApplication.application.applicationType) {
      case ApplicationType.Standard:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .standardLivescanFee ?? brandStore.brand.cost.standardLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .standard ?? brandStore.brand.cost.new.standard
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Judicial:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Judicial Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .judicialLivescanFee ?? brandStore.brand.cost.judicialLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .judicial ?? brandStore.brand.cost.new.judicial
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Reserve:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Reserve Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .reserveLivescanFee ?? brandStore.brand.cost.reserveLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new.reserve ??
            brandStore.brand.cost.new.reserve
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Employment:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Employment Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .employmentLivescanFee ??
            brandStore.brand.cost.employmentLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .employment ?? brandStore.brand.cost.new.employment
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType['Renew Standard']:
        paymentType = PaymentType['CCW Application Renewal Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .standard ?? brandStore.brand.cost.renew.standard
        break

      case ApplicationType['Renew Judicial']:
        paymentType =
          PaymentType['CCW Application Renewal Judicial Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .judicial ?? brandStore.brand.cost.renew.judicial
        break

      case ApplicationType['Renew Reserve']:
        paymentType =
          PaymentType['CCW Application Renewal Reserve Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew.reserve ??
          brandStore.brand.cost.renew.reserve
        break

      case ApplicationType['Renew Employment']:
        paymentType =
          PaymentType['CCW Application Renewal Employment Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .employment ?? brandStore.brand.cost.renew.employment
        break

      case ApplicationType['Modify Standard']:
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Judicial']:
        paymentType =
          PaymentType[
            'CCW Application Modification Judicial Payment'
          ].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Reserve']:
        paymentType =
          PaymentType['CCW Application Modification Reserve Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Employment']:
        paymentType =
          PaymentType[
            'CCW Application Modification Employment Payment'
          ].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      default:
        paymentType = PaymentType['CCW Application Initial Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.new.standard ??
          brandStore.brand.cost.new.standard
    }

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      livescanAmount,
      applicationStore.completeApplication.application.orderId,
      paymentType
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})
</script>
