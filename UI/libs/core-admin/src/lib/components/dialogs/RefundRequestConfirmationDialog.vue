<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        color="primary"
      >
        <v-icon left>mdi-credit-card-refund</v-icon>
        Refund
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Refund Request </v-card-title>

      <v-card-text>
        Are you sure you wish to refund ${{ refundAmount }} from the total ${{
          refundRequest.refundAmount
        }}
        from application {{ refundRequest.orderId }} due to
        {{ refundRequest.reason }}?
      </v-card-text>

      <v-card-text>
        This will leave a total charge of ${{
          refundRequest.refundAmount - refundAmount
        }}. This will be the only refund available on this refund request.

        {{ typeof refundAmount }}
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
          color="error"
          text
        >
          Cancel
        </v-btn>
        <v-spacer />
        <v-btn
          @click="handleConfirm"
          color="primary"
          text
        >
          Confirm
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { RefundRequest } from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'

interface RefundRequestProps {
  refundRequest: RefundRequest
  value: number
}

const props = defineProps<RefundRequestProps>()

const emit = defineEmits(['confirm', 'input'])

const dialog = ref(false)
const refundAmount = computed({
  get: () => props.value,
  set: newVal => emit('input', Number(newVal)),
})

const refundAmountRules = computed(() => {
  return [
    v => Boolean(v) || 'A refund amount is required',
    () =>
      Number(props.refundRequest.refundAmount) - Number(refundAmount.value) >=
        0 || 'Refund amount must be less than or equal to the total payment',
    v => v > 0 || 'Refund amount must be greater than 0',
    v =>
      /^\d*(?:\.\d{1,2})?$/.test(v) ||
      'Please only enter 2 digits after the decimal',
  ]
})

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
