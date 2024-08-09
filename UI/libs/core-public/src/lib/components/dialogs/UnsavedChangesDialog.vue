<template>
  <v-dialog
    v-model="viewModel"
    max-width="600"
    persistent
  >
    <v-card>
      <v-card-title>You have unsaved changes</v-card-title>

      <v-alert
        text
        class="ma-3"
        outlined
        type="warning"
      >
        If you leave or close the browser your changes will be lost.
      </v-alert>

      <v-card-actions>
        <v-btn
          @click="handleContinueWithoutSaving"
          color="error"
          text
        >
          Continue without saving
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleSaveAndContinue"
          color="success"
          text
        >
          Save Changes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface UnsavedChangesDialogProps {
  value: boolean
}

const props = defineProps<UnsavedChangesDialogProps>()
const emit = defineEmits([
  'input',
  'continue-without-saving',
  'save-and-continue',
])

const viewModel = computed({
  get() {
    return props.value
  },
  set(newVal) {
    emit('input', newVal)
  },
})

function handleContinueWithoutSaving() {
  emit('continue-without-saving')
}

function handleSaveAndContinue() {
  emit('save-and-continue')
}
</script>
