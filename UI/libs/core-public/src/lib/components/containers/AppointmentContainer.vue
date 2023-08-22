<template>
  <v-container
    v-if="state.updatingAppointment"
    class="text-center"
  >
    <v-progress-circular indeterminate></v-progress-circular>
  </v-container>

  <v-container v-else>
    <v-subheader v-if="props.showHeader && !state.updatingAppointment">
      <h2>
        {{ $t('Schedule Appointment') }}
      </h2>
    </v-subheader>

    <v-toolbar
      color="primary"
      flat
    >
      <v-btn
        outlined
        :small="$vuetify.breakpoint.mdAndDown"
        color="white"
        @click="selectNextAvailable"
      >
        {{ $t('Next available') }}
      </v-btn>

      <template v-if="$vuetify.breakpoint.mdAndUp">
        <v-btn
          fab
          text
          small
          color="white"
          @click="$refs.calendar.prev()"
        >
          <v-icon> mdi-chevron-left </v-icon>
        </v-btn>

        <v-btn
          fab
          text
          small
          color="white"
          @click="$refs.calendar.next()"
        >
          <v-icon> mdi-chevron-right </v-icon>
        </v-btn>
      </template>

      <v-toolbar-title
        v-if="state.calendarLoading"
        :style="{
          color: 'white',
        }"
        class="ml-5"
      >
        {{ $refs.calendar.title }}
      </v-toolbar-title>
    </v-toolbar>

    <v-calendar
      ref="calendar"
      v-model="state.focus"
      color="primary"
      :start="props.events[0].start"
      :type="getCalendarType"
      :events="props.events"
      :first-interval="getFirstInterval"
      :interval-minutes="appointmentLength"
      :interval-count="numberOfAppointments"
      event-color="primary"
      @click:event="selectEvent($event)"
    >
      <template #event="{ event }">
        <div class="ml-2">
          {{ `${event.start.split(' ')[1]} - ${event.end.split(' ')[1]}` }}
        </div>
      </template>
    </v-calendar>

    <v-menu
      v-model="state.selectedOpen"
      :activator="state.selectedElement"
    >
      <v-card flat>
        <v-card-title>
          {{ $t('Confirm Appointment Selection') }}
        </v-card-title>
        <v-card-actions>
          <v-btn
            color="success"
            @click="handleConfirm"
          >
            {{ $t('Confirm') }}
          </v-btn>
          <v-btn
            color="error"
            @click="state.selectedOpen = false"
          >
            {{ $t('Cancel') }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-menu>

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
      color="success"
      v-model="state.snackbarOk"
      :timeout="5000"
      class="font-weight-bold"
    >
      {{ $t(`Appointment is confirmed for: `) }}
      {{ state.selectedEvent.start }} - {{ state.selectedEvent.end }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@core-public/stores/paymentStore'
import {
  ApplicationStatus,
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, getCurrentInstance, onMounted, reactive, ref } from 'vue'

interface IProps {
  toggleAppointment: CallableFunction
  events: Array<AppointmentType>
  showHeader: boolean
  rescheduling: boolean
}

const props = withDefaults(defineProps<IProps>(), {
  showHeader: true,
  rescheduling: false,
})

const app = getCurrentInstance()
const applicationStore = useCompleteApplicationStore()
const appointmentStore = useAppointmentsStore()
const paymentStore = usePaymentStore()
const paymentType = paymentStore.getPaymentType
const calendar = ref<any>(null)
const state = reactive({
  focus: '',
  selectedEvent: {} as AppointmentType,
  selectedOpen: false,
  selectedElement: null,
  selectedDay: '',
  snackbar: false,
  snackbarOk: false,
  calendarLoading: false,
  updatingAppointment: false,
})

const getCalendarType = computed(() => {
  if (app?.proxy.$vuetify.breakpoint.mdAndUp) {
    return 'month'
  }

  return 'day'
})

const appointmentMutation = useMutation({
  mutationFn: () => {
    const body: AppointmentType = {
      userId: applicationStore.completeApplication.userId,
      applicationId: applicationStore.completeApplication.id,
      date: '',
      end: new Date(state.selectedEvent.end).toISOString(),
      isManuallyCreated: false,
      id: state.selectedEvent.id,
      name: `${applicationStore.completeApplication.application.personalInfo.firstName} ${applicationStore.completeApplication.application.personalInfo.lastName} `,
      payment: paymentType,
      permit: applicationStore.completeApplication.application.orderId,
      start: new Date(state.selectedEvent.start).toISOString(),
      appointmentCreatedDate: new Date().toISOString(),
      status: AppointmentStatus.Scheduled,
      time: '',
    }

    if (props.rescheduling) {
      return appointmentStore.rescheduleAppointment(body).then(response => {
        appointmentStore.currentAppointment = response
        applicationStore.completeApplication.application.appointmentDateTime =
          response.start
        applicationStore.completeApplication.application.appointmentId =
          response.id
      })
    }

    return appointmentStore.setAppointmentPublic(body).then(response => {
      appointmentStore.currentAppointment = response
      applicationStore.completeApplication.application.appointmentDateTime =
        response.start
      applicationStore.completeApplication.application.appointmentId =
        response.id
    })
  },
  onSuccess: () => {
    state.snackbarOk = true
    applicationStore.completeApplication.application.appointmentStatus =
      AppointmentStatus.Scheduled

    if (
      applicationStore.completeApplication.application.status ===
      ApplicationStatus['Appointment No Show']
    ) {
      applicationStore.completeApplication.application.status =
        ApplicationStatus.Submitted
    }

    state.updatingAppointment = false
    props.toggleAppointment()
  },
  onError: () => {
    state.snackbar = true
  },
})

function selectEvent(event) {
  if (!state.updatingAppointment) {
    state.selectedEvent = event.event
    state.selectedElement = event.nativeEvent.target
    state.selectedOpen = true
  }
}

function handleConfirm() {
  state.updatingAppointment = true
  appointmentMutation.mutate()
}

function selectNextAvailable() {
  state.focus = new Date(props.events[0].start).toLocaleDateString()
}

onMounted(() => {
  state.calendarLoading = true
})

const appointmentLength = computed(() => {
  const startTime = new Date(props.events[0].start)
  const endTime = new Date(props.events[0].end)
  const difference = endTime.getTime() - startTime.getTime()
  const resultInMinutes = Math.round(difference / 60000)

  return resultInMinutes
})

const numberOfAppointments = computed(() => {
  const groupedEvents = props.events.reduce((result, obj) => {
    if (!result[obj.start]) {
      result[obj.start] = []
    }

    result[obj.start].push(obj)

    return result
  }, {})

  return Object.keys(groupedEvents).length + 2
})

const getFirstInterval = computed(() => {
  const startTime = parseInt(props.events[0].start.split(' ')[1].split(':')[0])

  const firstInterval =
    startTime * Math.pow(2, Math.log2(60 / appointmentLength.value))

  return Math.round(firstInterval - 1)
})
</script>
