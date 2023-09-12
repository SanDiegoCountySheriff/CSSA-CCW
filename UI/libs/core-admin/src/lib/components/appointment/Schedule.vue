<template>
  <div>
    <v-dialog
      v-model="state.dialog"
      fullscreen
    >
      <template #activator="{ attrs }">
        <v-btn
          color="primary"
          v-bind="attrs"
          @click="openDialog"
          small
          block
        >
          <v-icon left> mdi-calendar-multiple-check </v-icon>
          Reschedule
        </v-btn>
      </template>

      <v-card>
        <v-toolbar
          dark
          color="primary"
        >
          <v-btn
            icon
            dark
            @click="state.dialog = false"
          >
            <v-icon>mdi-close</v-icon>
          </v-btn>
          <v-toolbar-title>Schedule Appointment</v-toolbar-title>
        </v-toolbar>

        <v-container>
          <v-toolbar
            flat
            color="primary"
          >
            <v-btn
              outlined
              class="mr-4"
              color="white"
              @click="setToday"
            >
              {{ $t('Next Available') }}
            </v-btn>

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
              v-if="$refs.calendar"
              :style="{
                color: 'white',
              }"
              class="ml-5"
            >
              {{ getCalendarTitle }}
            </v-toolbar-title>
            <v-spacer />
            <v-menu>
              <template #activator="{ on, attrs }">
                <v-btn
                  outlined
                  color="white"
                  v-bind="attrs"
                  v-on="on"
                >
                  {{ $t(state.type) }}
                  <v-icon right> mdi-menu-down </v-icon>
                </v-btn>
              </template>
              <v-list>
                <v-list-item @click="state.type = 'day'">
                  <v-list-item-title>{{ $t('Day') }}</v-list-item-title>
                </v-list-item>
                <v-list-item @click="state.type = 'week'">
                  <v-list-item-title>{{ $t('Week') }}</v-list-item-title>
                </v-list-item>
                <v-list-item @click="state.type = 'month'">
                  <v-list-item-title>{{ $t('Month') }}</v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-toolbar>

          <template
            v-if="
              (isLoading && isError) ||
              (state.appointmentsLoaded && state.appointments.length > 0)
            "
          >
            <v-calendar
              ref="calendar"
              v-model="state.focus"
              color="primary"
              first-time="8"
              first-interval="8"
              interval-width="80"
              interval-count="16"
              :start="state.appointments[0].start"
              :type="state.type"
              :events="state.appointments"
              :event-overlap-mode="'column'"
              event-color="primary"
              @click:date="viewDay($event)"
              @click:event="selectEvent($event)"
            >
              <template #event="{ event }">
                <div class="ml-2">
                  {{
                    `${event.start.split(' ')[1]} - ${event.end.split(' ')[1]}`
                  }}
                </div>
              </template>
            </v-calendar>
            <v-menu
              v-model="state.selectedOpen"
              :activator="state.selectedElement"
              min-width="250px"
              min-height="150px"
              max-height="250px"
              max-width="450px"
            >
              <v-card flat>
                <v-card-title>
                  {{ $t('Confirm Appointment Selection') }}
                </v-card-title>
                <v-card-text class="button-card">
                  <v-btn
                    text
                    class="m-3"
                    color="error"
                    @click="state.selectedOpen = false"
                  >
                    {{ $t('Cancel') }}
                  </v-btn>
                  <v-btn
                    text
                    color="primary"
                    @click="handleConfirm"
                    class="m-3"
                  >
                    {{ $t('Confirm') }}
                  </v-btn>
                </v-card-text>
              </v-card>
            </v-menu>
          </template>

          <template
            class="text-center"
            v-else
          >
            <v-progress-linear
              indeterminate
              :height="20"
            >
              Loading Appointments
            </v-progress-linear>
          </template>
        </v-container>
      </v-card>
    </v-dialog>

    <v-snackbar
      color="error"
      v-model="state.snackbar"
      :timeout="5000"
      class="font-weight-bold"
    >
      {{
        $t(
          'Appointment is no longer available. Please select another appointment.'
        )
      }}
    </v-snackbar>
    <v-snackbar
      color="primary"
      v-model="state.snackbarOk"
      :timeout="5000"
      class="font-weight-bold"
    >
      {{ $t(`Appointment is confirmed for: `) }}
      {{ new Date(state.selectedEvent.start).toLocaleString() }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive, ref } from 'vue'

const permitStore = usePermitsStore()
const appointmentsStore = useAppointmentsStore()
const paymentType = 'cash'
const calendar = ref<any>(null)

const state = reactive({
  dialog: false,
  focus: '',
  type: 'month',
  selectedEvent: {} as AppointmentType,
  selectedOpen: false,
  selectedElement: null,
  selectedDay: '',
  isLoading: false,
  checkAppointment: true,
  setAppointment: false,
  snackbar: false,
  snackbarOk: false,
  appointments: [] as Array<AppointmentType>,
  appointmentsLoaded: false,
  reschedule: false,
})

const {
  mutate: getAppointmentMutation,
  isLoading,
  isError,
} = useMutation({
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  mutationFn: () => {
    const appRes = appointmentsStore.getAvailableAppointments()

    appRes
      .then((data: Array<AppointmentType>) => {
        data.forEach(event => {
          let start = new Date(event.start)
          let end = new Date(event.end)

          let formatedStart = `${start.getFullYear()}-${
            start.getMonth() + 1
          }-${start.getDate()} ${start.getHours()}:${start
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

          let formatedEnd = `${end.getFullYear()}-${
            end.getMonth() + 1
          }-${end.getDate()} ${end.getHours()}:${end
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

          event.name = 'open'
          event.start = formatedStart
          event.end = formatedEnd
        })
        state.appointments = data
        state.appointmentsLoaded = true
      })
      .catch(() => {
        state.appointmentsLoaded = true
      })
  },
  onSuccess: () => {
    state.dialog = true
  },
})

const appointmentMutation = useMutation({
  mutationFn: () => {
    const body: AppointmentType = {
      applicationId: permitStore.getPermitDetail.id,
      userId: permitStore.getPermitDetail.userId,
      date: '',
      end: new Date(state.selectedEvent.end).toISOString(),
      isManuallyCreated: false,
      id: state.selectedEvent.id,
      name: `${permitStore.getPermitDetail.application.personalInfo.firstName} ${permitStore.getPermitDetail.application.personalInfo.lastName} `,
      payment: paymentType,
      permit: permitStore.getPermitDetail.application.orderId,
      start: new Date(state.selectedEvent.start).toISOString(),
      appointmentCreatedDate: new Date().toISOString(),
      status: AppointmentStatus.Scheduled,
      time: '',
    }

    return appointmentsStore.sendAppointmentCheck(body).then(() => {
      appointmentsStore.currentAppointment = body
      permitStore.getPermitDetail.application.appointmentDateTime = body.start
    })
  },
  onSuccess: () => {
    state.isLoading = false
    state.setAppointment = true
    state.snackbarOk = true
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus.Scheduled
  },
  onError: () => {
    state.snackbar = true
    state.checkAppointment = false
    state.isLoading = false
  },
})

function viewDay({ date }) {
  state.focus = date
  state.type = 'day'
}

function setToday() {
  state.focus = 'date'
  state.type = 'day'
}

function selectEvent(event) {
  state.selectedEvent = event.event
  state.selectedElement = event.nativeEvent.target
  state.selectedOpen = true
}

function handleConfirm() {
  if (!state.reschedule) {
    state.isLoading = true
    state.checkAppointment = true

    appointmentMutation.mutate()
  } else {
    let appointment = appointmentsStore.currentAppointment

    appointment.applicationId = null
    appointment.status = AppointmentStatus.Scheduled
    appointmentsStore.sendAppointmentCheck(appointment).then(() => {
      appointmentMutation.mutate()
    })
  }

  state.dialog = false
}

function openDialog() {
  getAppointmentMutation()
}

function handleCalendarNext() {
  calendar.value.next()
}

function handleCalendarPrevious() {
  calendar.value.prev()
}

const getCalendarTitle = computed(() => {
  return calendar.value.title
})
</script>
