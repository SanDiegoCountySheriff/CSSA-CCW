<template>
  <v-container>
    <v-row>
      <v-col class="mt-md-3 ml-lg-15">
        <v-btn
          :disabled="isInitialPaymentComplete"
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
          :disabled="isInitialPaymentComplete"
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
  </v-container>
</template>

<script setup lang="ts">
import { PaymentType } from '@shared-utils/types/defaultTypes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { inject, ref } from 'vue'

interface IPaymentButtonContainerProps {
  hideOnlinePayment: boolean
}

const props = defineProps<IPaymentButtonContainerProps>()

const emit = defineEmits(['cash-payment'])
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const showInfo = ref(false)
const isInitialPaymentComplete = inject('isInitialPaymentComplete')

function handleCashPayment() {
  emit('cash-payment')
  showInfo.value = true
}

async function onlinePayment() {
  await paymentStore.getPayment(
    applicationStore.completeApplication.id,
    brandStore.brand.cost.new.standard,
    applicationStore.completeApplication.application.orderId,
    PaymentType['CCW Application Initial Payment'].toString()
  )
}
</script>
