<template>
  <v-container>
    <v-row>
      <v-col class="mt-md-3 ml-lg-15">
        <v-btn
          block
          :disabled="
            applicationStore.completeApplication.application.paymentStatus !== 0
          "
          color="primary"
          @click="handleCashPayment"
        >
          {{ $t('Pay in person') }}
        </v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col class="mt-md-3 ml-lg-15">
        <v-btn
          block
          color="primary"
          @click="onlinePayment"
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
import { ref } from 'vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'

const emit = defineEmits(['cash-payment'])
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const showInfo = ref(false)

function handleCashPayment() {
  emit('cash-payment')
  showInfo.value = true
}

async function onlinePayment() {
  await paymentStore.getPayment()
}
</script>
