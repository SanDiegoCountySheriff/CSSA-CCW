<template>
  <v-container fluid>
    <v-card
      :loading="
        isLoading ||
        isDeleteByDateLoading ||
        isDeleteByTimeSlotLoading ||
        isBookedAppointmentsLoading
      "
      flat
    >
      <v-card-title>
        {{
          $t(
            'Right click date to remove entire days or on individual appointments'
          )
        }}

        <v-switch
          v-model="showBookedAppointments"
          @change="handleChangeShowBookedAppointments"
          class="ml-5"
          color="primary"
          label="Show booked appointments"
        />

        <v-spacer></v-spacer>

        <v-text-field
          v-model="manuallySelectedDate"
          outlined
          dense
          type="date"
          label="Select date"
          append-icon="mdi-calendar"
          clearable
        >
        </v-text-field>
      </v-card-title>

      <v-card-text>
        There are currently {{ appointmentsLength }}
        {{ showBookedAppointments ? 'booked' : 'open' }} appointments in the
        database.
      </v-card-text>

      <v-toolbar
        flat
        color="primary"
      >
        <v-btn
          fab
          text
          small
          color="white"
          @click="handleCalendarPrevious"
        >
          <v-icon> mdi-chevron-left </v-icon>
        </v-btn>

        <v-btn
          fab
          text
          small
          color="white"
          @click="handleCalendarNext"
        >
          <v-icon> mdi-chevron-right </v-icon>
        </v-btn>

        <v-toolbar-title
          v-if="calendar"
          :style="{
            color: 'white',
          }"
          class="ml-5 text-no-wrap"
        >
          {{ calendar.title }}
        </v-toolbar-title>
      </v-toolbar>

      <v-calendar
        ref="calendar"
        v-model="focus"
        :start="getStart"
        :events="appointments"
        @contextmenu:date="handleOpenDayMenu($event)"
        @contextmenu:event="handleOpenEventMenu($event)"
        color="primary"
        event-color="primary"
      >
        <template #event="{ event }">
          <span class="ml-1">
            {{
              new Date(event.start).toLocaleTimeString('en-US', {
                hour: '2-digit',
                minute: '2-digit',
              })
            }}
          </span>
          <span class="float-right mr-1">{{ event.name }}</span>
        </template>
      </v-calendar>
    </v-card>

    <v-dialog
      v-model="dayDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>
          Delete All Appointments On {{ selectedDate }}
        </v-card-title>
        <v-card-text>
          Are you sure you want to delete all appointments on
          {{ selectedDate }}? <br />
          This cannot be undone.
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="error"
            text
            @click="dayDialog = false"
          >
            Cancel
          </v-btn>
          <v-spacer />
          <v-btn
            color="primary"
            text
            @click="handleDeleteAppointmentsOnDay"
          >
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="eventDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title> Delete Event </v-card-title>
        <v-card-text>
          Are you sure you want to delete all appointments on
          {{ selectedEvent?.start.split(' ')[0] }} at
          {{ selectedEvent?.start.split(' ')[1] }}? <br />
          This cannot be undone.
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="error"
            text
            @click="eventDialog = false"
          >
            Cancel
          </v-btn>
          <v-spacer />
          <v-btn
            color="primary"
            text
            @click="handleDeleteEvents"
          >
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { AppointmentType } from '@shared-utils/types/defaultTypes'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { computed, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const emit = defineEmits(['on-delete-appointments'])
const manuallySelectedDate = ref(null)
const focus = ref('')
const selectedDate = ref<Date>()
const selectedEvent = ref<AppointmentType>()
const calendar = ref()
const appointments = ref<Array<AppointmentType>>([])
const appointmentsLength = ref(0)
const appointmentStore = useAppointmentsStore()
const dayDialog = ref(false)
const eventDialog = ref(false)
const showBookedAppointments = ref(false)

const getStart = computed(() => {
  if (manuallySelectedDate.value) {
    return manuallySelectedDate.value
  }

  if (appointments.value[0]?.start) {
    return appointments.value[0].start
  }

  return new Date()
})

const {
  isFetching: isBookedAppointmentsLoading,
  refetch: getBookedAppointments,
} = useQuery(
  ['getBookedAppointments', true],
  () => appointmentStore.getBookedAppointments(false),
  {
    enabled: false,
    onSuccess: (data: Array<AppointmentType>) => {
      const currentOffset = new Date().getTimezoneOffset() / 60

      appointmentsLength.value = data.length

      const uniqueData = data.reduce(
        (result, currentObj) => {
          const key = `${currentObj.start}-${currentObj.end}`

          if (!result.set.has(key)) {
            currentObj.slots = 1
            result.set.add(key)
            result.array.push(currentObj)
          } else {
            const index = result.array.findIndex(
              obj => obj.start === currentObj.start
            )

            if (index !== -1) {
              const updatedObj = result.array[index]

              if (updatedObj.slots) {
                updatedObj.slots += 1
              }

              result.array[index] = updatedObj
            }
          }

          return result
        },
        { set: new Set(), array: [] } as {
          set: Set<string>
          array: Array<AppointmentType>
        }
      ).array

      uniqueData.forEach(event => {
        const start = new Date(event.start)

        if (currentOffset !== start.getTimezoneOffset() / 60) {
          const correctedOffset = currentOffset - start.getTimezoneOffset() / 60

          start.setTime(start.getTime() - correctedOffset * 60 * 60 * 1000)
        }

        const end = new Date(event.end)

        if (currentOffset !== end.getTimezoneOffset() / 60) {
          const correctedOffset = currentOffset - end.getTimezoneOffset() / 60

          end.setTime(end.getTime() - correctedOffset * 60 * 60 * 1000)
        }

        if (event.slots) {
          event.name = `${event.slots} slot${event.slots > 1 ? 's' : ''} booked`
        }

        event.start = formatDate(start, start.getHours(), start.getMinutes())
        event.end = formatDate(end, end.getHours(), end.getMinutes())
      })

      appointments.value = uniqueData
    },
  }
)

const { isLoading, refetch } = useQuery(
  ['getAppointments', true],
  () => appointmentStore.getAvailableAppointments(true),
  {
    onSuccess: (data: Array<AppointmentType>) => {
      const currentOffset = new Date().getTimezoneOffset() / 60

      appointmentsLength.value = data.length

      const uniqueData = data.reduce(
        (result, currentObj) => {
          const key = `${currentObj.start}-${currentObj.end}`

          if (!result.set.has(key)) {
            currentObj.slots = 1
            result.set.add(key)
            result.array.push(currentObj)
          } else {
            const index = result.array.findIndex(
              obj => obj.start === currentObj.start
            )

            if (index !== -1) {
              const updatedObj = result.array[index]

              if (updatedObj.slots) {
                updatedObj.slots += 1
              }

              result.array[index] = updatedObj
            }
          }

          return result
        },
        { set: new Set(), array: [] } as {
          set: Set<string>
          array: Array<AppointmentType>
        }
      ).array

      uniqueData.forEach(event => {
        const start = new Date(event.start)

        if (currentOffset !== start.getTimezoneOffset() / 60) {
          const correctedOffset = currentOffset - start.getTimezoneOffset() / 60

          start.setTime(start.getTime() - correctedOffset * 60 * 60 * 1000)
        }

        const end = new Date(event.end)

        if (currentOffset !== end.getTimezoneOffset() / 60) {
          const correctedOffset = currentOffset - end.getTimezoneOffset() / 60

          end.setTime(end.getTime() - correctedOffset * 60 * 60 * 1000)
        }

        if (event.slots) {
          event.name = `${event.slots} slot${event.slots > 1 ? 's' : ''} left`
        }

        event.start = formatDate(start, start.getHours(), start.getMinutes())
        event.end = formatDate(end, end.getHours(), end.getMinutes())
      })

      appointments.value = uniqueData
    },
  }
)

const { isLoading: isDeleteByDateLoading, mutate: deleteAppointmentsByDate } =
  useMutation({
    mutationFn: () =>
      appointmentStore.deleteAppointmentsByDate(
        toUTCDateTimeString(selectedDate.value)
      ),
    onSuccess: data => {
      emit(
        'on-delete-appointments',
        `${data} appointment${parseInt(data) > 1 ? 's' : ''} deleted.`
      )
      refetch()
    },
  })

const {
  isLoading: isDeleteByTimeSlotLoading,
  mutate: deleteAppointmentsByTimeSlot,
} = useMutation({
  mutationFn: () =>
    appointmentStore.deleteAppointmentsByTimeSlot(
      toISOString(selectedDate.value)
    ),
  onSuccess: data => {
    emit(
      'on-delete-appointments',
      `${data} appointment${parseInt(data) > 1 ? 's' : ''} deleted.`
    )
    refetch()
  },
})

function handleOpenDayMenu({ nativeEvent, date }) {
  nativeEvent.preventDefault()

  if (!showBookedAppointments) {
    selectedDate.value = date
    dayDialog.value = true
  }
}

function handleOpenEventMenu({ nativeEvent, event }) {
  nativeEvent.preventDefault()

  if (!showBookedAppointments) {
    selectedDate.value = event.start
    selectedEvent.value = event
    eventDialog.value = true
  }
}

function handleDeleteAppointmentsOnDay() {
  deleteAppointmentsByDate()
  dayDialog.value = false
}

function handleDeleteEvents() {
  deleteAppointmentsByTimeSlot()
  eventDialog.value = false
}

function handleChangeShowBookedAppointments() {
  appointments.value = []

  if (showBookedAppointments.value) {
    getBookedAppointments()
  } else {
    refetch()
  }
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

function handleCalendarNext() {
  if (calendar.value) {
    calendar.value.next()
  }
}

function handleCalendarPrevious() {
  if (calendar.value) {
    calendar.value.prev()
  }
}

function toISOString(localDate: Date | string | undefined): string {
  if (!localDate) {
    return ''
  }

  const dateObj = new Date(localDate)

  return `${dateObj.toISOString().slice(0, -5)}Z`
}

function toUTCDateTimeString(localDate: Date | string | undefined): string {
  if (!localDate) {
    return ''
  }

  const startLocal = new Date(localDate)
  const timeZoneOffset = startLocal.getTimezoneOffset()
  const startUTC = new Date(
    startLocal.getTime() + timeZoneOffset * 60 * 1000
  ).toISOString()

  return startUTC
}
</script>

<style lang="scss" scoped>
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
