<template>
  <div>
    <v-container>
      <v-row>
        <v-col class="mt-md-3 ml-lg-15">
          <v-btn
            block
            :disabled="
              applicationStore.completeApplication.application.paymentStatus !==
              0
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
            :disabled="
              applicationStore.completeApplication.application.paymentStatus !==
              0
            "
            color="primary"
            @click="handleOnlinePayment"
          >
            {{ $t('Pay Online ') }}
          </v-btn>
        </v-col>
      </v-row>
      <v-row
        v-if="state.showInfo"
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

    <script
      type="application/javascript"
      src="https://js.globalpay.com/v1/globalpayments.js"
    ></script>
    <v-dialog
      v-model="state.paymentDialog"
      max-width="600"
    >
      <v-card :loading="state.paymentCardLoading">
        <v-card-title>Payment</v-card-title>
        <v-card-text
          class="pb-16"
          align="center"
        >
          <form
            id="payment-form"
            action="/"
            method="get"
          >
            <!-- Target for the credit card form -->
            <div id="credit-card"></div>
          </form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { nextTick, reactive, ref } from 'vue'

interface IPaymentButtonContainerProps {
  cashPayment: CallableFunction
}

const emit = defineEmits(['on-submit-online-payment'])
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const cardForm = ref()
const props = defineProps<IPaymentButtonContainerProps>()
const state = reactive({
  showInfo: false,
  paymentDialog: false,
  paymentCardLoading: false,
  created: false,
})

function handleCashPayment() {
  props.cashPayment()
  state.showInfo = true
}

async function handleOnlinePayment() {
  state.paymentDialog = true
  await nextTick()

  if (!state.created) {
    // eslint-disable-next-line no-undef, @typescript-eslint/ban-ts-comment
    // @ts-ignore
    // eslint-disable-next-line no-undef
    GlobalPayments.configure({
      publicApiKey: 'pkapi_cert_ggnOQ05ygBPxmxOrBX',
    })
    // eslint-disable-next-line no-undef, @typescript-eslint/ban-ts-comment
    // @ts-ignore
    // eslint-disable-next-line no-undef
    cardForm.value = GlobalPayments.creditCard.form('#credit-card')
    state.created = true
  }

  cardForm.value.on('click', () => {
    state.paymentCardLoading = true
  })

  cardForm.value.on('token-success', async resp => {
    await paymentStore.chargeCard(
      resp.paymentReference,
      applicationStore.completeApplication.application.cost.new.standard,
      applicationStore.completeApplication.id,
      'Initial Payment'
    )
    state.paymentCardLoading = false
    state.paymentDialog = false
    emit('on-submit-online-payment')
  })

  cardForm.value.on('token-error', resp => {
    // show error to the consumer
  })
}
</script>
