<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        :disabled="readonly"
        v-on="on"
        v-bind="attrs"
        color="primary"
        small
        block
      >
        Update Expiration Date
      </v-btn>
    </template>

    <v-card>
      <v-card-title> Update Expiration Date </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-menu
              v-model="menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  v-model="expirationDate"
                  :label="$t('Expiration Date')"
                  :rules="[
                    validateDate,
                    v => !!v || $t('Expiration date is required'),
                  ]"
                  outlined
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                  readonly
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="expirationDate"
                color="primary"
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = !dialog"
          color="error"
          text
        >
          Close
        </v-btn>

        <v-spacer />

        <v-btn
          @click="handleSaveExpirationDate"
          color="primary"
          text
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'

interface ExpirationDateDialogProps {
  readonly: boolean
}

const props = withDefaults(defineProps<ExpirationDateDialogProps>(), {
  readonly: false,
})

const emit = defineEmits(['handle-save-expiration-date'])

const dialog = ref(false)
const menu = ref(false)
const expirationDate = ref('')

function validateDate(inputDate: string | null | undefined): boolean | string {
  const DATE_PATTERN = /^\d{4}-\d{2}-\d{2}$/

  if (!inputDate) {
    return true
  }

  if (!DATE_PATTERN.test(inputDate)) {
    return 'Date must be in the format YYYY-MM-DD'
  }

  return true
}

function handleSaveExpirationDate() {
  emit('handle-save-expiration-date', expirationDate.value)
  expirationDate.value = ''
  dialog.value = false
}
</script>
