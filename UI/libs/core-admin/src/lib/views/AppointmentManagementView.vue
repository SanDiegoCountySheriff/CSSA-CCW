<template>
  <v-container fluid>
    <v-card flat>
      <v-card-title>
        {{ $t('Appointment Management') }}
        <v-spacer></v-spacer>
        <v-btn
          color="primary"
          class="mr-4"
        >
          {{ $t('Save') }}
        </v-btn>
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
                append-icon="mdi-clock-time-four-outline"
                label="First appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedEndTime"
                @change="handleChangeAppointmentParameters"
                append-icon="mdi-clock-time-four-outline"
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
                :first-interval="13"
                :interval-minutes="30"
                :interval-count="24"
                color="primary"
                type="week"
              ></v-calendar>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Routes from '@core-admin/router/routes'
import { onMounted, ref } from 'vue'

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
const selectedDays = ref(['Monday'])
const selectedStartTime = ref('08:00')
const selectedEndTime = ref('16:00')
const selectedNumberOfSlots = ref(1)
const selectedAppointmentLength = ref(30)

onMounted(() => {
  handleChangeAppointmentParameters()
})

function handleChangeAppointmentParameters() {
  events.value = []
  const today = new Date()
  const firstDayOfWeek = new Date(
    today.setDate(today.getDate() - today.getDay())
  )

  selectedDays.value.forEach(day => {
    const date = new Date(firstDayOfWeek)

    while (
      date.getDay() !==
      [
        'Sunday',
        'Monday',
        'Tuesday',
        'Wednesday',
        'Thursday',
        'Friday',
        'Saturday',
      ].indexOf(day)
    ) {
      date.setDate(date.getDate() + 1)
    }

    const startTime = parseInt(selectedStartTime.value.split(':')[0])
    const lastAppointmentTime = parseInt(selectedEndTime.value.split(':')[0])
    const appointmentLength = selectedAppointmentLength.value
    const startDateTime = new Date()
    const endDateTime = new Date()

    startDateTime.setHours(startTime, 0, 0)
    endDateTime.setHours(lastAppointmentTime, 0, 0)

    while (startDateTime <= endDateTime) {
      const startMinute = parseInt(
        startDateTime.toLocaleTimeString().split(':')[1]
      )

      const endTime = parseInt(
        startDateTime
          .toLocaleTimeString([], {
            hour12: false,
            hour: '2-digit',
            minute: '2-digit',
          })
          .split(':')[0]
      )
      const endMinute = Number(startMinute) + Number(appointmentLength)
      const event = {
        name: 'Appt',
        start: formatDate(date, endTime, startMinute),
        end: formatDate(date, endTime, endMinute),
        color: 'primary',
      }

      events.value.push(event)

      startDateTime.setMinutes(
        startDateTime.getMinutes() + Number(appointmentLength)
      )
    }
  })
}

function formatDate(date: Date, hour: number, minute: number): string {
  const year = date.getFullYear()
  const month = date.getMonth() + 1
  const day = date.getDate()
  let formattedHour = hour.toString().padStart(2, '0')
  let formattedMinute = minute.toString().padStart(2, '0')

  if (formattedMinute === '60') {
    formattedHour = (hour + 1).toString().padStart(2, '0')
    formattedMinute = '00'
  }

  return `${year}-${month.toString().padStart(2, '0')}-${day
    .toString()
    .padStart(2, '0')} ${formattedHour}:${formattedMinute}`
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
