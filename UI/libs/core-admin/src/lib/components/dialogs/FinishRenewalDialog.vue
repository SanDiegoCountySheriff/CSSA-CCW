<template>
  <v-dialog
    v-model="dialog"
    max-width="650"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="disabled"
        v-on="on"
        v-bind="attrs"
        color="primary"
        block
        small
      >
        Finish Renewal
      </v-btn>
    </template>

    <v-card>
      <v-card-title>Is the renewal complete?</v-card-title>

      <v-card-text>
        In order to complete the renewal you must print the application and
        renewed license, and mail the renewed license to the customer.
      </v-card-text>

      <v-card-text>
        <v-row>
          <v-col>
            <v-checkbox
              v-model="printedLicense"
              label="I have printed the application and renewed license."
            ></v-checkbox>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-checkbox
              v-model="mailedLicense"
              label="I have mailed the customer's renewed license."
            ></v-checkbox>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
          text
        >
          Cancel
        </v-btn>

        <v-spacer />

        <v-btn
          @click="emit('handle-finish-renewal')"
          :disabled="!printedLicense || !mailedLicense"
          color="primary"
          text
        >
          Finish Renewal
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

const emit = defineEmits(['handle-finish-renewal'])

const dialog = ref(false)
const printedLicense = ref(false)
const mailedLicense = ref(false)
</script>
