<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :disabled="props.disabled"
        color="primary"
        block
      >
        Withdraw Renewal
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Withdraw Renewal</v-card-title>

      <v-card-text>
        Are you sure you want to withdraw your renewal? If you have paid for
        your renewal the refund may take some time to process.
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          text
          color="primary"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleConfirm"
          text
          color="primary"
        >
          Withdraw
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface dialogProps {
  disabled: boolean
}

const props = defineProps<dialogProps>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
