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
        color="warning"
        small
        block
      >
        <v-icon left>mdi-currency-usd</v-icon>
        Ready For Payment
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Ready for payment?</v-card-title>

      <v-card-text>
        Would you like to mark this application as ready for payment?
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
          @click="onReadyForPayment"
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
import { inject, ref } from 'vue'

const readonly = inject('readonly')

const emit = defineEmits(['on-ready-for-payment'])

const dialog = ref(false)

function onReadyForPayment() {
  emit('on-ready-for-payment')
  dialog.value = false
}
</script>
