<template>
  <v-dialog
    v-if="item.paymentStatus === 1"
    v-model="dialog"
    max-width="600px"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="loading"
        v-on="on"
        v-bind="attrs"
        color="primary"
        small
      >
        <v-icon left>mdi-delete</v-icon>
        Delete
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Delete Transaction {{ item.transactionId }} </v-card-title>

      <v-card-text>
        Are you sure you wish to delete transaction
        {{ item.transactionId }} for ${{ item.amount }}? This cannot be undone.
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleDeleteTransaction()"
          color="primary"
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { PaymentHistoryType } from '@shared-utils/types/defaultTypes'
import { ref } from 'vue'

interface DeleteDialogProps {
  item: PaymentHistoryType
  index: number
  loading: boolean
}

const props = defineProps<DeleteDialogProps>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleDeleteTransaction() {
  emit('confirm', props.index)
  dialog.value = false
}
</script>
