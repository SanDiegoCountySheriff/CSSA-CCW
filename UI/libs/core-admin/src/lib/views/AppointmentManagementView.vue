<template>
  <v-container fluid>
    <v-card>
      <v-card-title>
        {{ $t('Appointment Management') }}
        <v-spacer></v-spacer>
        <v-btn
          :to="Routes.APPOINTMENTS_ROUTE_PATH"
          color="primary"
        >
          {{ $t('Back') }}
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-form ref="form">
          <v-row>
            <v-col>
              <v-select
                v-model="selectedDays"
                :items="daysOfTheWeek"
                @change="handleChangeAppointmentParameters"
                label="Days of the week"
                color="primary"
                multiple
                outlined
              ></v-select>
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedStartTime"
                @change="handleChangeAppointmentParameters"
                label="First appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedEndTime"
                @change="handleChangeAppointmentParameters"
                label="Last appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedNumberOfSlots"
                @change="handleChangeAppointmentParameters"
                label="Number of slots"
                type="number"
                outlined
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="selectedAppointmentLength"
                @change="handleChangeAppointmentParameters"
                label="Appointment length"
                type="number"
                outlined
              />
            </v-col>
            <v-col>Number to create?</v-col>
            <v-col>Current open appointments</v-col>
            <v-col>Current booked appointments</v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-calendar
                :events="events"
                type="week"
              ></v-calendar>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary">Save</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Routes from '@core-admin/router/routes'
import { ref } from 'vue'

const form = ref()
const events = ref<Array<unknown>>([])
const daysOfTheWeek = ref([
  'Sunday',
  'Monday',
  'Tuesday',
  'Wednesday',
  'Thursday',
  'Friday',
  'Saturday',
])
const selectedDays = ref([])
const selectedStartTime = ref()
const selectedEndTime = ref()
const selectedNumberOfSlots = ref()
const selectedAppointmentLength = ref()

function handleChangeAppointmentParameters() {
  const today = new Date()
  const firstDayOfWeek = new Date(
    today.setDate(today.getDate() - today.getDay())
  )
  const lastDayOfWeek = new Date(
    today.setDate(today.getDate() - today.getDay() + 6)
  )

  window.console.log(lastDayOfWeek)
}
</script>
