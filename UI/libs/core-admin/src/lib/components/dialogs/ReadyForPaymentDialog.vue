<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="readonly"
        v-bind="attrs"
        v-on="on"
        color="primary"
        small
        block
      >
        <v-icon left>mdi-currency-usd</v-icon>
        Ready For Payment
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Ready for initial payment?</v-card-title>

      <v-card-text>
        Would you like to mark this application as ready to accept initial
        payment?
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
          @click="onReadyForInitialPayment"
          color="primary"
          text
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface ReadyForPaymentDialogProps {
  readonly: boolean
}

const props = withDefaults(defineProps<ReadyForPaymentDialogProps>(), {
  readonly: false,
})

const emit = defineEmits(['on-ready-for-initial-payment'])

const dialog = ref(false)

function onReadyForInitialPayment() {
  emit('on-ready-for-initial-payment')
  dialog.value = false
}
</script>
