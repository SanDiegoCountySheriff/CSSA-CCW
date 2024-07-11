<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-on="on"
        v-bind="attrs"
        :color="buttonColor"
        :height="buttonHeight"
        :x-large="buttonXLarge"
        :small="buttonSmall"
        text
      >
        <v-container>
          <v-row>
            <v-col>
              <v-icon x-large> {{ icon }} </v-icon>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              {{ buttonText }}
            </v-col>
          </v-row>
        </v-container>
      </v-btn>
    </template>
    <v-card>
      <v-card-title>{{ title }}</v-card-title>
      <v-card-text>
        {{ description }}
        <v-date-picker
          v-model="selectedDate"
          class="mt-3 mb-3 rounded-lg"
          color="primary"
          full-width
        ></v-date-picker>
      </v-card-text>
      <v-card-actions>
        <v-btn
          text
          color="error"
          @click="dialog = false"
        >
          Close
        </v-btn>
        <v-spacer></v-spacer>
        <v-btn
          text
          color="primary"
          @click="handleGenerate"
        >
          Generate Report
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface ReportsDialogProps {
  buttonText: string
  title: string
  description: string
  icon: string
  buttonColor: string
  buttonHeight: string
  buttonXLarge: boolean
  buttonSmall: boolean
}

defineProps<ReportsDialogProps>()
const emit = defineEmits(['generate'])

const dialog = ref(false)
const selectedDate = ref('')

function handleGenerate() {
  emit('generate', selectedDate.value)
  dialog.value = false
}
</script>
