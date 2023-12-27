<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="loading"
        small
        color="primary"
        v-bind="attrs"
        v-on="on"
      >
        <v-icon left>mdi-credit-card-refund</v-icon>
        Refund
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Refund</v-card-title>

      <v-card-text>
        How much would you like to refund from transaction
        {{ props.payment.transactionId }}? The total amount is ${{
          parseFloat(props.payment.amount).toFixed(2)
        }}
      </v-card-text>

      <v-card-text>
        <v-text-field
          v-model="refundAmount"
          :rules="refundAmountRules"
          prepend-icon="mdi-currency-usd"
          label="Amount"
          type="number"
          outlined
        />
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          color="primary"
          text
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="onRefund"
          color="primary"
          text
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { PaymentHistoryType } from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'

interface RefundDialogProps {
  payment: PaymentHistoryType
  loading: boolean
}

const props = defineProps<RefundDialogProps>()
const emit = defineEmits(['refund'])
const refundAmountRef = ref<string>(parseFloat(props.payment.amount).toFixed(2))

const refundAmount = computed({
  get() {
    return refundAmountRef.value
  },
  set(val) {
    refundAmountRef.value = val
  },
})

const refundAmountRules = computed(() => {
  return [
    v => Boolean(v) || 'A refund amount is required',
    v =>
      v <= props.payment.amount || 'Refund amount must be less than the total',
    v => v > 0 || 'Refund amount must be greater than 0',
    v =>
      /^\d*(?:\.\d{1,2})?$/.test(v) ||
      'Please only enter 2 digits after the decimal',
  ]
})

const dialog = ref(false)

function onRefund() {
  emit('refund', {
    transactionId: props.payment.transactionId,
    refundAmount: refundAmount.value,
  })
  dialog.value = false
}
</script>
