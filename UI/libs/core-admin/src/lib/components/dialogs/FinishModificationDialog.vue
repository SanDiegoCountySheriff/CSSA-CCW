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
        small
      >
        Finish Modification
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Is the modification complete?</v-card-title>

      <v-card-text>
        In order to complete the modification you must print and email the state
        modification form BOF-4052 and print a new license to send to the
        customer.
      </v-card-text>

      <v-card-text> Are these steps complete? </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
          text
        >
          No
        </v-btn>

        <v-spacer />

        <v-btn
          @click="emit('handle-finish-modification')"
          color="primary"
          text
        >
          Yes
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits(['handle-finish-modification'])

const dialog = ref(false)
</script>
