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

      <v-card-text>
        <v-row>
          <v-col>
            <v-checkbox
              v-model="stateFormComplete"
              label="I have mailed the state form BOF-4502."
            ></v-checkbox>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-checkbox
              v-model="customerFormComplete"
              label="I have mailed the customer's updated license."
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
          @click="emit('handle-finish-modification')"
          :disabled="!stateFormComplete || !customerFormComplete"
          color="primary"
          text
        >
          Finish Modification
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

const emit = defineEmits(['handle-finish-modification'])

const dialog = ref(false)
const stateFormComplete = ref(false)
const customerFormComplete = ref(false)
</script>
