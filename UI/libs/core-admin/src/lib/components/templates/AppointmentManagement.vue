<template>
  <v-container fluid>
    <v-card
      :loading="isLoading"
      flat
    >
      <v-toolbar
        flat
        color="primary"
      >
        <v-btn
          outlined
          color="white"
          @click="handleRefresh"
        >
          {{ $t('Refresh') }}
        </v-btn>

        <v-btn
          fab
          text
          small
          color="white"
          @click="calendar.prev()"
        >
          <v-icon> mdi-chevron-left </v-icon>
        </v-btn>
        <v-btn
          fab
          text
          small
          color="white"
          @click="calendar.next()"
        >
          <v-icon> mdi-chevron-right </v-icon>
        </v-btn>
        <v-toolbar-title
          v-if="calendar"
          :style="{
            color: 'white',
          }"
          class="ml-5"
        >
          {{ calendar.title }}
        </v-toolbar-title>
      </v-toolbar>
      <v-calendar
        v-model="focus"
        ref="calendar"
        :start="appointments[0].start"
        :events="appointments"
        @contextmenu:date="handleOpenDayMenu($event)"
        @contextmenu:event="handleOpenEventMenu($event)"
        color="primary"
        event-color="primary"
      >
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
          <v-spacer />
          <v-btn
            color="primary"
            text
            @click="dayDialog = false"
          >
            Cancel
          </v-btn>
          <v-btn
            color="error"
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
          <v-spacer />
          <v-btn
            color="primary"
            text
            @click="eventDialog = false"
          >
            Cancel
          </v-btn>
          <v-btn
            color="error"
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
import { ref } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useQuery } from '@tanstack/vue-query'

const selectedDate = ref(null)
const selectedEvent = ref(null)
const calendar = ref()
const focus = ref()
const appointments = ref<Array<AppointmentType>>([])
const appointmentStore = useAppointmentsStore()
const dayDialog = ref(false)
const eventDialog = ref(false)

const { isLoading, refetch } = useQuery(
  ['getAppointments'],
  appointmentStore.getAvailableAppointments,
  {
    onSuccess: (data: Array<AppointmentType>) => {
      data.forEach(event => {
        let start = new Date(event.start)
        let end = new Date(event.end)

        event.name = 'Appt'
        event.start = formatDate(start, start.getHours(), start.getMinutes())
        event.end = formatDate(end, end.getHours(), end.getMinutes())
      })
      appointments.value = data
    },
  }
)

function handleRefresh() {
  refetch()
}

function handleOpenDayMenu({ nativeEvent, date }) {
  nativeEvent.preventDefault()
  selectedDate.value = date
  dayDialog.value = true
}

function handleOpenEventMenu({ nativeEvent, event }) {
  nativeEvent.preventDefault()
  selectedEvent.value = event
  eventDialog.value = true
}

function handleDeleteAppointmentsOnDay() {
  // do the work
  dayDialog.value = false
}

function handleDeleteEvents() {
  // do the work
  eventDialog.value = false
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
