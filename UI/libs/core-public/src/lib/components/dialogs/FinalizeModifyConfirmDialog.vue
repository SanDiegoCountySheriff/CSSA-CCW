<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <v-card>
      <v-card-title>Confirm Modification Details</v-card-title>

      <v-card-text> </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          color="error"
          text
        >
          Close
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleConfirm"
          color="primary"
          text
        >
          Accept
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  value: boolean
}

const props = defineProps<Props>()

const emit = defineEmits(['input', 'confirm'])

const dialog = computed({
  get: () => props.value,
  set: newVal => emit('input', newVal),
})

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
