<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :disabled="disabled"
        color="success"
        block
      >
        Make Payment
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Are you ready to make your initial payment?</v-card-title>

      <v-card-text>
        Clicking confirm will redirect you to the payment provider. Please do
        not click the back button or close the browser.
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          text
          color="error"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleConfirm"
          text
          color="primary"
        >
          Confirm
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface InitialPaymentDialogProps {
  disabled: boolean
}

defineProps<InitialPaymentDialogProps>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
