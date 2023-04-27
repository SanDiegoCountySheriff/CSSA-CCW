<template>
  <v-container fluid>
    <v-card flat>
      <v-card-title>
        {{ $t('Appointment Management') }}
        <v-spacer></v-spacer>
        <v-tooltip
          color="primary"
          bottom
        >
          <template #activator="{ on, attrs }">
            <v-btn
              v-bind="attrs"
              v-on="on"
              color="primary"
              class="mr-4"
            >
              {{ $t('Save') }}
            </v-btn>
          </template>
          <span
            >This will create
            {{ numberOfAppointmentsThatWillBeCreated }} appointments in the
            database</span
          >
        </v-tooltip>
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
                label="Number of slots per appointment"
                type="number"
                outlined
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedAppointmentLength"
                @change="handleChangeAppointmentParameters"
                label="Appointment length"
                type="number"
                outlined
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="selectedNumberOfWeeks"
                label="Number of weeks to create"
                type="number"
                outlined
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-calendar
                :events="events"
                :first-interval="getFirstInterval"
                :interval-minutes="getIntervalMinutes"
                :interval-count="getIntervalCount"
                color="primary"
                type="week"
              >
                <template #day-label-header="{}">{{ '' }}</template>
              </v-calendar>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Routes from '@core-admin/router/routes'
import { computed, onMounted, ref } from 'vue'

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
const selectedNumberOfWeeks = ref(0)

onMounted(() => {
  handleChangeAppointmentParameters()
})

const getFirstInterval = computed(() => {
  const startTime = parseInt(selectedStartTime.value.split(':')[0])

  const firstInterval =
    startTime * Math.pow(2, Math.log2(60 / selectedAppointmentLength.value))

  return Math.round(firstInterval - 1)
})

const getIntervalMinutes = computed(() => {
  return selectedAppointmentLength.value
})

const numberOfAppointmentsThatWillBeCreated = computed(() => {
  return events.value.length * selectedNumberOfWeeks.value
})

const getIntervalCount = computed(() => {
  const startDate = new Date(`2000-01-01T${selectedStartTime.value}`)
  const endDate = new Date(`2000-01-01T${selectedEndTime.value}`)
  const diffInMs = endDate.getTime() - startDate.getTime()
  const diffInMinutes = Math.floor(diffInMs / (1000 * 60))

  return diffInMinutes / selectedAppointmentLength.value + 3
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

<style lang="scss">
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

.v-calendar-daily__scroll-area {
  overflow-y: hidden !important;
}

.v-calendar-daily__head {
  margin-right: 0px !important;
}

.theme--dark.v-calendar-daily .v-calendar-daily__intervals-body {
  border-bottom: #9e9e9e 1px solid;
}

.theme--light.v-calendar-daily .v-calendar-daily__intervals-body {
  border-bottom: #e0e0e0 1px solid;
}

.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
