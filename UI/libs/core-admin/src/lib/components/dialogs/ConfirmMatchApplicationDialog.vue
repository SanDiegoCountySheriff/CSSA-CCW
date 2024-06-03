<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :disabled="disabled"
        color="primary"
      >
        Match Applicant/Application
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Confirm Match Application </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <p class="font-weight-bold">Applicant:</p>
            <p>Name: {{ applicantName }}</p>
            <p>Email: {{ applicantEmail }}</p>
            <p>ID Number: {{ applicantIdNumber }}</p>
          </v-col>

          <v-col>
            <p class="font-weight-bold">Application:</p>
            <p>Name: {{ applicationName }}</p>
            <p>Email: {{ applicationEmail }}</p>
            <p>ID Number: {{ applicationIdNumber }}</p>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-text>
        <v-checkbox
          v-model="overrideEmail"
          label="Would you like to override the existing email address on the right with the new email on the left?"
        ></v-checkbox>

        <p v-if="overrideEmail">Email will be overwritten</p>

        <p v-else>Email will NOT be overwritten</p>
      </v-card-text>

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

const overrideEmail = ref(true)

interface Props {
  disabled: boolean
  applicantName?: string
  applicationName?: string
  applicantEmail?: string
  applicationEmail?: string
  applicantIdNumber?: string
  applicationIdNumber?: string
}

const props = defineProps<Props>()

const emit = defineEmits(['confirm'])

const dialog = ref(false)

function handleConfirm() {
  emit('confirm', overrideEmail.value)
  dialog.value = false
}
</script>
