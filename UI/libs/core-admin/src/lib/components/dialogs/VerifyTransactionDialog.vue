<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        color="primary"
        class="ml-3"
        small
      >
        <v-icon left>mdi-cash-check</v-icon>
        Verify
      </v-btn>
    </template>

    <v-form v-model="valid">
      <v-card>
        <v-card-title>
          Are you sure you want to verify this transaction?
        </v-card-title>

        <v-card-text>
          If you have verified the transaction on the vendor's website please
          enter the transaction ID below.
        </v-card-text>

        <v-card-text>
          <v-text-field
            v-model="transactionId"
            label="Transaction ID"
            color="primary"
            outlined
            :rules="[v => !!v || 'Transaction ID is required']"
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
            :disabled="!valid"
            color="primary"
            text
          >
            Confirm
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-form>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits(['confirm'])

const dialog = ref(false)
const transactionId = ref('')
const valid = ref(false)

function handleConfirm() {
  emit('confirm', transactionId.value)
  dialog.value = false
}
</script>
