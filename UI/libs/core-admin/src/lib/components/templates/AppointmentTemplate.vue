<template>
  <v-container fluid>
    <v-card
      :loading="isLoading"
      flat
    >
      <v-card-title>
        {{ $t('Appointment Template') }}

        <v-spacer></v-spacer>

        <div class="mr-5">
          Saving will create
          {{ numberOfAppointmentsThatWillBeCreated }} appointments in the
          database starting from the last available appointment.
        </div>

        <v-spacer></v-spacer>

        <v-btn
          @click="handleSaveAppointments"
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
            <v-col cols="3">
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
            <v-col cols="3">
              <v-text-field
                v-model="selectedStartTime"
                @change="handleChangeAppointmentParameters"
                append-icon="mdi-clock-time-four-outline"
                label="First appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="selectedEndTime"
                @change="handleChangeAppointmentParameters"
                append-icon="mdi-clock-time-four-outline"
                label="Last appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="selectedNumberOfSlots"
                @change="handleChangeAppointmentParameters"
                label="Number of slots per appointment"
                type="number"
                outlined
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="3">
              <v-text-field
                v-model="selectedAppointmentLength"
                @change="handleChangeAppointmentParameters"
                label="Appointment length"
                type="number"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="selectedNumberOfWeeks"
                label="Number of weeks to create"
                type="number"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="selectedBreakLength"
                @change="handleChangeAppointmentParameters"
                label="Break length"
                type="number"
                outlined
              ></v-text-field>
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="selectedBreakStartTime"
                @change="handleChangeAppointmentParameters"
                append-icon="mdi-clock-time-four-outline"
                label="Break start time"
                type="time"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-calendar
                :events="events"
                :first-interval="getFirstInterval"
                :interval-minutes="selectedAppointmentLength"
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
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useMutation } from '@tanstack/vue-query'
import { computed, onMounted, ref } from 'vue'

const emit = defineEmits(['on-upload-appointments'])
const appointmentsStore = useAppointmentsStore()
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
const selectedNumberOfWeeks = ref(1)
const selectedBreakLength = ref<number>()
const selectedBreakStartTime = ref()

const { isLoading, mutate: uploadAppointments } = useMutation({
  mutationKey: ['uploadAppointments'],
  mutationFn: async () =>
    await appointmentsStore.createNewAppointments({
      daysOfTheWeek: selectedDays.value,
      firstAppointmentStartTime: selectedStartTime.value,
      lastAppointmentStartTime: selectedEndTime.value,
      numberOfSlotsPerAppointment: selectedNumberOfSlots.value,
      appointmentLength: selectedAppointmentLength.value,
      numberOfWeeksToCreate: selectedNumberOfWeeks.value,
      breakLength: selectedBreakLength.value,
      breakStartTime: selectedBreakStartTime.value,
    }),
  onSuccess: data => {
    emit(
      'on-upload-appointments',
      `${data} new appointment${parseInt(data) > 1 ? 's' : ''} created.`
    )
  },
})

onMounted(() => {
  handleChangeAppointmentParameters()
})

const getFirstInterval = computed(() => {
  const startTime = parseInt(selectedStartTime.value.split(':')[0])

  const firstInterval =
    startTime * Math.pow(2, Math.log2(60 / selectedAppointmentLength.value))

  return Math.round(firstInterval - 1)
})

const numberOfAppointmentsThatWillBeCreated = computed(() => {
  return (
    events.value.length *
    selectedNumberOfWeeks.value *
    selectedNumberOfSlots.value
  )
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

    const startHour = parseInt(selectedStartTime.value.split(':')[0])
    let startMinute = parseInt(selectedStartTime.value.split(':')[1])
    const lastAppointmentHour = parseInt(selectedEndTime.value.split(':')[0])
    const lastAppointmentMinute = parseInt(selectedEndTime.value.split(':')[1])
    const appointmentLength = selectedAppointmentLength.value
    const startDateTime = new Date()
    const endDateTime = new Date()

    startDateTime.setHours(startHour, startMinute, 0)
    endDateTime.setHours(lastAppointmentHour, lastAppointmentMinute, 0)

    while (startDateTime <= endDateTime) {
      if (
        selectedBreakStartTime.value &&
        willAppointmentFallInBreakTime(
          startDateTime.toTimeString().split(' ')[0].substring(0, 5),
          selectedBreakStartTime.value,
          selectedBreakLength.value ?? selectedAppointmentLength.value
        )
      ) {
        startDateTime.setMinutes(
          startDateTime.getMinutes() +
            Number(selectedBreakLength.value ?? selectedAppointmentLength.value)
        )
        continue
      }

      startMinute = parseInt(startDateTime.toLocaleTimeString().split(':')[1])

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

  if (parseInt(formattedMinute) > 60) {
    formattedHour = (hour + 1).toString().padStart(2, '0')
    minute = parseInt(formattedMinute)
    formattedMinute = (minute - 60).toString()
  } else if (formattedMinute === '60') {
    formattedHour = (hour + 1).toString().padStart(2, '0')
    formattedMinute = '00'
  }

  return `${year}-${month.toString().padStart(2, '0')}-${day
    .toString()
    .padStart(2, '0')} ${formattedHour}:${formattedMinute}`
}

function handleSaveAppointments() {
  uploadAppointments()
}

function willAppointmentFallInBreakTime(
  appointmentStartTime: string,
  breakStartTime: string,
  breakLength: number
): boolean {
  const appointmentStart = new Date(
    1,
    1,
    1970,
    parseInt(appointmentStartTime.split(':')[0]),
    parseInt(appointmentStartTime.split(':')[1])
  )
  const breakStart = new Date(
    1,
    1,
    1970,
    parseInt(breakStartTime.split(':')[0]),
    parseInt(breakStartTime.split(':')[1])
  )
  const breakEnd = new Date(
    1,
    1,
    1970,
    parseInt(breakStartTime.split(':')[0]),
    parseInt(breakStartTime.split(':')[1])
  )

  breakEnd.setMinutes(breakEnd.getMinutes() + Number(breakLength))

  return appointmentStart >= breakStart && appointmentStart < breakEnd
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
