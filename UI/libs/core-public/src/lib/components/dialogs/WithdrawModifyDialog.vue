<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        color="primary"
        block
      >
        Withdraw Modification
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Withdraw Modify</v-card-title>

      <v-card-text>Are you sure?</v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          text
          color="primary"
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="$emit('confirm')"
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

defineEmits(['confirm'])

const dialog = ref(false)
</script>
