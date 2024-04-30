<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        color="success"
        icon
      >
        <v-icon>mdi-check-bold</v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Refund Request </v-card-title>

      <v-card-text>
        Are you sure you wish to refund ${{ refundRequest.refundAmount }} from
        application {{ refundRequest.orderId }} due to
        {{ refundRequest.reason }}?
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
import { ref } from 'vue'

interface RefundRequestProps {
  refundRequest: RefundRequest
}

defineProps<RefundRequestProps>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
