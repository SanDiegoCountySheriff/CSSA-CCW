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
              v-if="$refs.calendar"
              :style="{
                color: 'white',
              }"
              class="ml-5"
            >
              {{ getCalendarTitle }}
            </v-toolbar-title>
            <!-- <v-spacer />
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
            </v-menu> -->
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

            <!-- <v-menu
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
            </v-menu> -->
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
import { useMutation, useQuery } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, inject, reactive, ref } from 'vue'

const readonly = inject('readonly')

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

const { refetch } = useQuery(
  ['getAppointments', true],
  () => appointmentsStore.getAvailableAppointments(true),
  {
    enabled: false,
    onSuccess: (data: Array<AppointmentType>) => {
      const currentOffset = new Date().getTimezoneOffset() / 60

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

      state.appointments = uniqueData
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
  refetch()
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
