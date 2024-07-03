<template>
  <v-container fluid>
    <v-card
      :loading="isLoading || isGetAppointmentManagementTemplateLoading"
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
          :disabled="
            invalidTime ||
            isLoading ||
            isGetAppointmentManagementTemplateLoading
          "
          color="primary"
          class="mr-4"
        >
          <v-icon left>mdi-content-save</v-icon>
          {{ $t('Save') }}
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-form ref="form">
          <v-row>
            <v-col cols="3">
              <v-select
                v-model="appointmentManagement.daysOfTheWeek"
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
                v-model="appointmentManagement.firstAppointmentStartTime"
                @change="handleChangeAppointmentParameters"
                :error-messages="startTimeError"
                append-icon="mdi-clock-time-four-outline"
                label="First appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.lastAppointmentStartTime"
                @change="handleChangeAppointmentParameters"
                :error-messages="startTimeError"
                append-icon="mdi-clock-time-four-outline"
                label="Last appointment start time"
                type="time"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.numberOfSlotsPerAppointment"
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
                v-model="appointmentManagement.appointmentLength"
                @change="handleChangeAppointmentParameters"
                label="Appointment length"
                type="number"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.numberOfWeeksToCreate"
                label="Number of weeks to create"
                type="number"
                outlined
              />
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.breakLength"
                @change="handleChangeAppointmentParameters"
                label="Break length"
                type="number"
                outlined
              ></v-text-field>
            </v-col>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.breakStartTime"
                @change="handleChangeAppointmentParameters"
                append-icon="mdi-clock-time-four-outline"
                label="Break start time"
                type="time"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="3">
              <v-text-field
                v-model="appointmentManagement.startDate"
                @change="handleChangeAppointmentParameters"
                label="Start appointments after this date"
                append-icon="mdi-calendar"
                type="date"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-calendar
                :events="events"
                :first-interval="getFirstInterval"
                :interval-minutes="appointmentManagement.appointmentLength"
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
import { AppointmentManagement } from '@shared-utils/types/defaultTypes'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { computed, onMounted, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

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
const startTimeError = ref('')
const appointmentManagement = ref<AppointmentManagement>({
  daysOfTheWeek: ['Monday'],
  firstAppointmentStartTime: '08:00',
  lastAppointmentStartTime: '16:00',
  numberOfSlotsPerAppointment: 1,
  appointmentLength: 30,
  numberOfWeeksToCreate: 1,
  breakLength: 0,
  startDate: formatDate(new Date(), 0, 0).split(' ')[0],
  breakStartTime: null,
})

const { refetch, isLoading: isGetAppointmentManagementTemplateLoading } =
  useQuery({
    queryKey: ['getAppointmentManagementTemplate'],
    queryFn: async () => {
      return await appointmentsStore.getAppointmentManagementTemplate()
    },
    onSuccess: data => {
      data.startDate = formatDate(new Date(), 0, 0).split(' ')[0]
      appointmentManagement.value = data
      handleChangeAppointmentParameters()
    },
  })

const { isLoading, mutate: uploadAppointments } = useMutation({
  mutationKey: ['uploadAppointments'],
  mutationFn: async () => {
    appointmentManagement.value.startDate = new Date(
      appointmentManagement.value.startDate
    ).toISOString()

    return await appointmentsStore.createNewAppointments(
      appointmentManagement.value
    )
  },
  onSuccess: data => {
    refetch()
    emit(
      'on-upload-appointments',
      `${data.Item1} new appointment${
        parseInt(data.Item1) > 1 ? 's' : ''
      } created, ${data.Item2} holiday${
        parseInt(data.Item2) > 1 ? 's' : ''
      } skipped.`
    )
  },
})

onMounted(() => {
  handleChangeAppointmentParameters()
})

const invalidTime = computed(() => {
  return startTimeError.value.length > 0
})

const getFirstInterval = computed(() => {
  const startTime = parseInt(
    appointmentManagement.value.firstAppointmentStartTime.split(':')[0]
  )

  const firstInterval =
    startTime *
    Math.pow(2, Math.log2(60 / appointmentManagement.value.appointmentLength))

  return Math.round(firstInterval - 1)
})

const numberOfAppointmentsThatWillBeCreated = computed(() => {
  return (
    events.value.length *
    appointmentManagement.value.numberOfWeeksToCreate *
    appointmentManagement.value.numberOfSlotsPerAppointment
  )
})

const getIntervalCount = computed(() => {
  const startDate = new Date(
    `2000-01-01T${appointmentManagement.value.firstAppointmentStartTime}`
  )
  const endDate = new Date(
    `2000-01-01T${appointmentManagement.value.lastAppointmentStartTime}`
  )
  const diffInMs = endDate.getTime() - startDate.getTime()
  const diffInMinutes = Math.floor(diffInMs / (1000 * 60))

  return diffInMinutes / appointmentManagement.value.appointmentLength + 3
})

function handleChangeAppointmentParameters() {
  startTimeError.value = ''

  const selectedStart = new Date(
    `1970-01-01T${appointmentManagement.value.firstAppointmentStartTime}`
  )
  const selectedEnd = new Date(
    `1970-01-01T${appointmentManagement.value.lastAppointmentStartTime}`
  )

  if (selectedStart >= selectedEnd) {
    startTimeError.value = 'First appointment must be before last appointment'

    return
  }

  events.value = []
  const today = new Date()
  const firstDayOfWeek = new Date(
    today.setDate(today.getDate() - today.getDay())
  )

  appointmentManagement.value.daysOfTheWeek.forEach(day => {
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

    const startHour = parseInt(
      appointmentManagement.value.firstAppointmentStartTime.split(':')[0]
    )
    let startMinute = parseInt(
      appointmentManagement.value.firstAppointmentStartTime.split(':')[1]
    )
    const lastAppointmentHour = parseInt(
      appointmentManagement.value.lastAppointmentStartTime.split(':')[0]
    )
    const lastAppointmentMinute = parseInt(
      appointmentManagement.value.lastAppointmentStartTime.split(':')[1]
    )
    const appointmentLength = appointmentManagement.value.appointmentLength
    const startDateTime = new Date()
    const endDateTime = new Date()

    startDateTime.setHours(startHour, startMinute, 0)
    endDateTime.setHours(lastAppointmentHour, lastAppointmentMinute, 0)

    while (startDateTime <= endDateTime) {
      if (
        appointmentManagement.value.breakStartTime &&
        willAppointmentFallInBreakTime(
          startDateTime.toTimeString().split(' ')[0].substring(0, 5),
          appointmentManagement.value.breakStartTime,
          appointmentManagement.value.breakLength ??
            appointmentManagement.value.appointmentLength
        )
      ) {
        startDateTime.setMinutes(
          startDateTime.getMinutes() +
            Number(
              appointmentManagement.value.breakLength ??
                appointmentManagement.value.appointmentLength
            )
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
