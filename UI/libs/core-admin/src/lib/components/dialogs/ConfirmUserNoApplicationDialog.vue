<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :disabled="disabled"
        color="primary"
        outlined
      >
        No Application Found
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Kick back to applicant</v-card-title>

      <v-card-text>
        Are you sure you wish to mark this applicant as 'No Applications Found'?
        This will send it back to the applicant to apply again, or create a new
        application.
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
import { ref } from 'vue'

interface Props {
  disabled: boolean
}

const props = defineProps<Props>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
}
</script>
