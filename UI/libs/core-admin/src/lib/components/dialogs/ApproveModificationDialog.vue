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
        small
        block
      >
        <v-icon left> mdi-check-bold </v-icon>
        Approve Modification
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Approve Modification</v-card-title>

      <v-alert type="warning">
        Are you sure you want to approve the modification? You will not be able
        to make any changes to the weapons once the modification has been
        approved.
      </v-alert>

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
          Approve Modification
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm')
  dialog.value = false
}
</script>
