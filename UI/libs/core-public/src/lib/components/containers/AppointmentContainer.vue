<template>
  <v-container
    v-if="state.updatingAppointment"
    class="text-center"
  >
    <v-progress-circular indeterminate></v-progress-circular>
  </v-container>

  <v-container v-else>
    <v-subheader
      v-if="props.showHeader && !state.updatingAppointment"
      class="d-flex justify-center align-center"
    >
      <h2>
        {{ $t('Schedule Appointment') }}
      </h2>
    </v-subheader>

    <v-subheader class="d-flex justify-center align-center">
      <h3>Click individual appointment slots to schedule.</h3>
    </v-subheader>

    <v-toolbar
      color="primary"
      flat
    >
      <template v-if="getCalendarType === 'month'">
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
      </template>

      <template v-if="getCalendarType === 'day'">
        <v-btn
          v-if="getCalendarType === 'day'"
          outlined
          :small="$vuetify.breakpoint.mdAndDown"
          color="white"
          @click="selectPreviousAvailable"
        >
          <v-icon> mdi-chevron-left </v-icon>
        </v-btn>

        <v-spacer />
      </template>

      <v-toolbar-title
        v-if="state.calendarLoading"
        :style="{
          color: 'white',
        }"
        :class="getCalendarType === 'month' ? 'ml-5' : ''"
      >
        {{ getCalendarTitle }}
      </v-toolbar-title>

      <v-spacer />

      <v-btn
        v-if="getCalendarType === 'day'"
        outlined
        :small="$vuetify.breakpoint.mdAndDown"
        color="white"
        @click="selectNextAvailable"
      >
        <v-icon> mdi-chevron-right </v-icon>
      </v-btn>
    </v-toolbar>

    <v-calendar
      ref="calendar"
      v-model="state.focus"
      color="primary"
      :start="start"
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
      :position-x="x"
      :position-y="y"
      absolute
    >
      <v-card
        outlined
        flat
      >
        <v-card-title v-if="!props.rescheduling">
          {{
            $t(
              'Would you like to select this appointment and submit your application?'
            )
          }}
        </v-card-title>

        <v-card-title v-else>
          {{ $t('Would you like to select this appointment?') }}
        </v-card-title>

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

    <v-snackbar
      v-model="state.snackbar"
      color="error"
    >
      {{
        $t(
          'Appointment is no longer available. Please select another appointment.'
        )
      }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import {
  ApplicationStatus,
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, getCurrentInstance, onMounted, reactive, ref } from 'vue'

interface IProps {
  events: Array<AppointmentType>
  showHeader: boolean
  rescheduling: boolean
}

const props = withDefaults(defineProps<IProps>(), {
  showHeader: true,
  rescheduling: false,
})

const emit = defineEmits(['toggle-appointment'])

const app = getCurrentInstance()
const applicationStore = useCompleteApplicationStore()
const appointmentStore = useAppointmentsStore()
const paymentStore = usePaymentStore()
const paymentType = paymentStore.getPaymentType
const calendar = ref<any>(null)
const start = ref(props.events[0].start)
const x = ref(0)
const y = ref(0)
const state = reactive({
  focus: '',
  selectedEvent: {} as AppointmentType,
  selectedOpen: false,
  selectedElement: null,
  selectedDay: '',
  snackbar: false,
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
      end: new Date(state.selectedEvent.end.replace(/-/g, '/')).toISOString(),
      isManuallyCreated: false,
      id: state.selectedEvent.id,
      name: `${applicationStore.completeApplication.application.personalInfo.firstName} ${applicationStore.completeApplication.application.personalInfo.lastName} `,
      payment: paymentType,
      permit: applicationStore.completeApplication.application.orderId,
      start: new Date(
        state.selectedEvent.start.replace(/-/g, '/')
      ).toISOString(),
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
    appointmentStore.schedulingAppointment = false
    state.selectedOpen = false
    emit('toggle-appointment', state.selectedEvent.start.split(' ')[1])
  },
  onError: () => {
    state.updatingAppointment = false
    appointmentStore.schedulingAppointment = false
    state.snackbar = true
  },
})

function selectEvent(event) {
  if (!state.updatingAppointment) {
    state.selectedEvent = event.event
    state.selectedElement = event.nativeEvent.target
    x.value = event.nativeEvent.clientX
    y.value = event.nativeEvent.clientY
    state.selectedOpen = true
  }
}

function handleConfirm() {
  state.updatingAppointment = true
  appointmentStore.schedulingAppointment = true
  appointmentMutation.mutate()
}

function selectNextAvailable() {
  for (let event of props.events) {
    const eventStart = new Date(event.start.replace(/-/g, '/'))
    const currentStart = new Date(start.value.replace(/-/g, '/'))

    if (
      new Date(
        eventStart.getFullYear(),
        eventStart.getMonth(),
        eventStart.getDate()
      ) >
      new Date(
        currentStart.getFullYear(),
        currentStart.getMonth(),
        currentStart.getDate()
      )
    ) {
      start.value = event.start
      break
    }
  }
}

function selectPreviousAvailable() {
  for (let event of props.events.slice().reverse()) {
    const eventStart = new Date(event.start.replace(/-/g, '/'))
    const currentStart = new Date(start.value.replace(/-/g, '/'))

    if (
      new Date(
        eventStart.getFullYear(),
        eventStart.getMonth(),
        eventStart.getDate()
      ) <
      new Date(
        currentStart.getFullYear(),
        currentStart.getMonth(),
        currentStart.getDate()
      )
    ) {
      start.value = event.start
      break
    }
  }
}

onMounted(() => {
  state.calendarLoading = true
  state.focus = new Date(props.events[0].start).toLocaleDateString()
})

const appointmentLength = computed(() => {
  const startTime = new Date(props.events[0].start.replace(/-/g, '/'))
  const endTime = new Date(props.events[0].end.replace(/-/g, '/'))
  const difference = endTime.getTime() - startTime.getTime()
  const resultInMinutes = Math.round(difference / 60000)

  return resultInMinutes
})

const numberOfAppointments = computed(() => {
  const startTime = parseInt(props.events[0].start.split(' ')[1].split(':')[0])
  const endTime = parseInt(
    props.events.slice(-1)[0].start.split(' ')[1].split(':')[0]
  )
  const totalMinutes = (endTime - startTime) * 60
  const intervals = Math.floor(totalMinutes / appointmentLength.value)

  return intervals + 3
})

const getFirstInterval = computed(() => {
  const startTime = parseInt(props.events[0].start.split(' ')[1].split(':')[0])
  const firstInterval =
    startTime * Math.pow(2, Math.log2(60 / appointmentLength.value))

  return Math.round(firstInterval - 1)
})

function handleCalendarNext() {
  calendar.value.next()
}

function handleCalendarPrevious() {
  calendar.value.prev()
}

const getCalendarTitle = computed(() => {
  return calendar.value?.title || ''
})
</script>
