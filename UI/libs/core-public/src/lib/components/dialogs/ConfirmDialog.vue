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
        block
      >
        <v-icon
          v-if="icon"
          left
        >
          {{ icon }}
        </v-icon>
        {{ buttonText }}
      </v-btn>
    </template>

    <v-card>
      <v-card-title>{{ title }}</v-card-title>

      <v-card-text>{{ text }}</v-card-text>

      <v-card-actions>
        <v-btn
          text
          color="error"
          @click="dialog = false"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          text
          color="primary"
          @click="handleConfirm"
        >
          Confirm
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface ConfirmDialogProps {
  buttonText: string
  title: string
  text: string
  icon?: string
}

defineProps<ConfirmDialogProps>()
const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
