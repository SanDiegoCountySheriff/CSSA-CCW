<template>
  <v-container>
    <v-subheader v-if="props.showHeader">
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
        color="white"
        @click="selectNextAvailable"
      >
        {{ $t('Next available') }}
      </v-btn>

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

      <v-toolbar-title
        v-if="state.calendarLoading"
        :style="{
          color: 'white',
        }"
        class="ml-5"
      >
        {{ $refs.calendar.title }}
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

    <v-calendar
      ref="calendar"
      v-model="state.focus"
      color="primary"
      :start="props.events[0].start"
      :type="state.type"
      :events="props.events"
      event-color="primary"
      @click:date="viewDay($event)"
      @click:event="selectEvent($event)"
    >
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
import { onMounted, reactive, ref } from 'vue'

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

const applicationStore = useCompleteApplicationStore()
const appointmentStore = useAppointmentsStore()
const paymentStore = usePaymentStore()
const paymentType = paymentStore.getPaymentType
const calendar = ref<any>(null)
const state = reactive({
  focus: '',
  type: 'month',
  selectedEvent: {} as AppointmentType,
  selectedOpen: false,
  selectedElement: null,
  selectedDay: '',
  snackbar: false,
  snackbarOk: false,
  calendarLoading: false,
  updatingAppointment: false,
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

function viewDay({ date }) {
  state.focus = date
  state.type = 'day'
}

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
</script>
