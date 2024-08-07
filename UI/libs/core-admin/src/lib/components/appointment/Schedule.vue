<template>
  <div>
    <v-dialog
      v-model="state.dialog"
      fullscreen
    >
      <template #activator="{ on, attrs }">
        <v-btn
          v-on="on"
          :disabled="readonly"
          @click="openDialog"
          color="primary"
          v-bind="attrs"
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

        <v-container fluid>
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
              v-if="state.calendarLoading"
              :style="{
                color: 'white',
              }"
              class="ml-5"
            >
              <strong>{{ getCalendarTitle }}</strong>
            </v-toolbar-title>
          </v-toolbar>

          <template v-if="state.appointments.length > 0">
            <v-calendar
              ref="calendar"
              v-model="state.focus"
              :start="state.appointments[0].start"
              :events="state.appointments"
              @click:date="viewDay($event)"
              @click:event="selectEvent($event)"
              event-color="primary"
              color="primary"
              :event-more="false"
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

            <v-menu
              v-model="state.selectedOpen"
              :activator="state.selectedElement"
              :position-x="x"
              :position-y="y"
              min-width="250px"
              min-height="150px"
              max-height="350px"
              max-width="500px"
            >
              <v-card
                flat
                outlined
              >
                <v-card-title>
                  {{ $t('Reschedule Appointment Confirmation:') }}
                </v-card-title>

                <v-card-title>
                  <strong>{{
                    new Date(state.selectedEvent.start).toLocaleDateString(
                      'en-US',
                      {
                        month: 'long',
                        day: 'numeric',
                        year: 'numeric',
                        hour: 'numeric',
                        minute: '2-digit',
                        hour12: true,
                      }
                    )
                  }}</strong>
                </v-card-title>

                <v-col
                  v-if="
                    permitStore.getPermitDetail.application.appointmentDateTime
                  "
                >
                  <v-alert
                    v-if="isMovingUp"
                    outlined
                    type="info"
                  >
                    Rescheduling to this date will move the applicant's
                    appointment up by
                    {{ appointmentDifference }} days.
                  </v-alert>
                  <v-alert
                    v-else-if="!isMovingUp"
                    outlined
                    type="error"
                  >
                    Rescheduling to this date will delay the applicant's current
                    appointment by
                    {{ appointmentDifference }} days.
                  </v-alert>
                </v-col>

                <v-card-actions>
                  <v-btn
                    text
                    color="error"
                    @click="state.selectedOpen = false"
                  >
                    {{ $t('Cancel') }}
                  </v-btn>

                  <v-spacer />

                  <v-btn
                    text
                    color="success"
                    @click="handleConfirm"
                  >
                    {{ $t('Confirm') }}
                  </v-btn>
                </v-card-actions>
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
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, inject, nextTick, onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const readonly = inject('readonly')

const permitStore = usePermitsStore()
const appointmentsStore = useAppointmentsStore()
const paymentType = 'cash'
const calendar = ref<any>(null)
const x = ref(0)
const y = ref(0)
let changed: string

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
  calendarLoading: false,
})

const { refetch } = useQuery(
  ['getAppointments', true],
  () => appointmentsStore.getAvailableAppointments(true),
  {
    enabled: false,
    onSuccess: (data: Array<AppointmentType>) => {
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
        const end = new Date(event.end)

        if (event.slots) {
          event.name = `${event.slots} slot${event.slots > 1 ? 's' : ''} left`
        }

        event.start = formatDate(start, start.getHours(), start.getMinutes())
        event.end = formatDate(end, end.getHours(), end.getMinutes())
      })

      state.appointments = uniqueData
      state.focus = new Date(state.appointments[0].start).toLocaleDateString()
    },
  }
)

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

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updatePermitDetailApi(`Updated ${changed}`),
})

const isMovingUp = computed(() => {
  const newAppointment = state.selectedEvent.start
  const currentAppointment = permitStore.getPermitDetail.application
    .appointmentDateTime
    ? permitStore.getPermitDetail.application.appointmentDateTime
    : ''

  return (
    new Date(newAppointment).getTime() <= new Date(currentAppointment).getTime()
  )
})

const appointmentDifference = computed(() => {
  const newAppointment = new Date(state.selectedEvent.start)
  const currentAppointment = new Date(
    permitStore.getPermitDetail.application.appointmentDateTime
      ? permitStore.getPermitDetail.application.appointmentDateTime
      : ''
  )

  newAppointment.setHours(0, 0, 0, 0)
  currentAppointment.setHours(0, 0, 0, 0)

  const timeDifference = Math.abs(
    newAppointment.getTime() - currentAppointment.getTime()
  )

  const daysDifference = Math.floor(timeDifference / (1000 * 60 * 60 * 24))

  return daysDifference
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

    return appointmentsStore.sendAppointmentCheck(body).then(res => {
      permitStore.getPermitDetail.application.appointmentDateTime = res.start
      permitStore.getPermitDetail.application.appointmentId = res.id
      permitStore.getPermitDetail.application.appointmentStatus =
        AppointmentStatus.Scheduled
      changed = 'scheduled appointment'

      updatePermitDetails()
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

onMounted(() => {
  state.calendarLoading = true
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
  x.value = event.nativeEvent.clientX
  y.value = event.nativeEvent.ClientY
  state.selectedOpen = true
}

function handleConfirm() {
  state.isLoading = true
  state.checkAppointment = true

  appointmentMutation.mutate()

  state.dialog = false
}

function openDialog() {
  nextTick(() => {
    refetch()
  })
}

function handleCalendarNext() {
  calendar.value.next()
}

function handleCalendarPrevious() {
  calendar.value.prev()
}

const getCalendarTitle = computed(() => {
  return calendar.value?.title
})
</script>
