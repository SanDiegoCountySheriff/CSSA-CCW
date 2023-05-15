<template>
  <v-dialog
    v-model="dialog"
    :width="600"
  >
    <template #activator="{ on }">
      <v-btn
        color="primary"
        v-on="on"
        small
        block
      >
        <v-icon left> mdi-calendar-blank </v-icon>
        Schedule Override
      </v-btn>
    </template>

    <v-card outlined>
      <v-card-title>Manual Schedule Override</v-card-title>
      <v-card-text>
        You may reschedule a customer for a custom time slot outside of the
        normal range of available appointment time slots.
        <v-form ref="form">
          <v-row>
            <v-col class="pt-8">
              <v-text-field
                v-model="selectedDate"
                append-icon="mdi-calendar"
                label="Appointment Date"
                type="date"
                clearable
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="selectedTime"
                append-icon="mdi-clock-time-four-outline"
                label="Appointment Time"
                type="time"
                clearable
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          Do you want to remove the old appointment from the database? Another
          customer will not be able to schedule themselves for the current
          customer's previous time slot if this option is selected.
          <v-row>
            <v-col>
              <v-checkbox
                v-model="removeOldAppointment"
                label="Yes, remove old appointment"
              ></v-checkbox>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn
          @click="handleCloseDialog"
          color="primary"
          text
        >
          Cancel
        </v-btn>
        <v-btn
          @click="handleSaveReschedule"
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
import { formatLocalDateAndTimeStringToUtcDateTime } from '@shared-utils/formatters/defaultFormatters'
import { ref } from 'vue'

const emit = defineEmits(['on-save-reschedule'])
const dialog = ref(false)
const removeOldAppointment = ref(null)
const selectedDate = ref()
const selectedTime = ref()

function handleCloseDialog() {
  selectedDate.value = null
  selectedTime.value = null
  removeOldAppointment.value = null
  dialog.value = false
}

function handleSaveReschedule() {
  const selectedDateAndTime = formatLocalDateAndTimeStringToUtcDateTime(
    selectedDate.value,
    selectedTime.value
  )

  emit('on-save-reschedule', {
    dateAndTime: selectedDateAndTime,
    removeOldAppointment: removeOldAppointment.value,
    change: 'manual reschedule',
  })
  selectedDate.value = null
  selectedTime.value = null
  removeOldAppointment.value = null
  dialog.value = false
}
</script>

<style>
::-webkit-calendar-picker-indicator {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: auto;
  height: auto;
  color: transparent;
  background: transparent;
}
input::-webkit-date-and-time-value {
  text-align: left;
}
</style>
