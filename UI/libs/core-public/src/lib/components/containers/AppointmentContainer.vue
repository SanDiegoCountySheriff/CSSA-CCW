<template>
  <v-row class="calendar-container">
    <v-col>
      <v-sheet height="64">
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
            {{ $t('Today') }}
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
            v-if="$refs.calendar"
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
      </v-sheet>
      <v-sheet height="675">
        <v-calendar
          ref="calendar"
          v-model="state.focus"
          :color="$vuetify.theme.dark ? 'accent' : 'primary'"
          first-time="8"
          first-interval="8"
          interval-width="80"
          interval-count="16"
          :type="state.type"
          :events="props.events"
          :event-color="$vuetify.theme.dark ? 'accent' : 'primary'"
          @click:date="viewDay($event)"
          @click:event="selectEvent($event)"
        >
        </v-calendar>
        <v-menu
          v-model="state.selectedOpen"
          :activator="state.selectedElement"
          min-width="250px"
          min-height="150px"
          max-height="250px"
          max-width="450px"
        >
          <v-card
            flat
            min-width="250px"
            min-height="150px"
            max-height="250px"
            max-width="450px"
          >
            <v-card-title>
              {{ $t('Confirm Appointment Selection') }}
            </v-card-title>
            <v-card-text class="button-card">
              <v-btn
                color="primary"
                @click="handleConfirm"
                class="m-3"
              >
                {{ $t('Confirm') }}
              </v-btn>
              <v-btn
                class="m-3"
                color="error"
                @click="state.selectedOpen = false"
              >
                {{ $t('Cancel') }}
              </v-btn>
            </v-card-text>
          </v-card>
        </v-menu>
      </v-sheet>
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
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { AppointmentType } from '@shared-utils/types/defaultTypes';
import { reactive } from 'vue';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { usePaymentStore } from '@core-public/stores/paymentStore';

interface IProps {
  toggleAppointment: CallableFunction;
  events: Array<AppointmentType>;
  reschedule: boolean;
}

const props = defineProps<IProps>();
const applicationStore = useCompleteApplicationStore();
const appointmentStore = useAppointmentsStore();
const paymentStore = usePaymentStore();
const paymentType = paymentStore.getPaymentType;

const state = reactive({
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
});

const appointmentMutation = useMutation({
  mutationFn: () => {
    const body: AppointmentType = {
      applicationId: applicationStore.completeApplication.id,
      date: '',
      end: new Date(state.selectedEvent.end).toISOString(),
      isManuallyCreated: false,
      id: state.selectedEvent.id,
      name: `${applicationStore.completeApplication.application.personalInfo.firstName} ${applicationStore.completeApplication.application.personalInfo.lastName} `,
      payment: paymentType.paymentType,
      permit: applicationStore.completeApplication.application.orderId,
      start: new Date(state.selectedEvent.start).toISOString(),
      // TODO: once the backend is change have this just send a boolean
      status: true.toString(),
      time: '',
    };

    return appointmentStore.sendAppointmentCheck(body).then(() => {
      appointmentStore.currentAppointment = body;
      applicationStore.completeApplication.application.appointmentDateTime =
        body.start;
    });
  },
  onSuccess: () => {
    state.isLoading = false;
    state.setAppointment = true;
    state.snackbarOk = true;
    applicationStore.completeApplication.application.appointmentStatus = true;
    props.toggleAppointment();
  },
  onError: () => {
    state.snackbar = true;
    state.checkAppointment = false;
    state.isLoading = false;
  },
});

function viewDay({ date }) {
  state.focus = date;
  state.type = 'day';
}

function setToday() {
  state.focus = 'date';
  state.type = 'day';
}

function selectEvent(event) {
  state.selectedEvent = event.event;
  state.selectedElement = event.nativeEvent.target;
  state.selectedOpen = true;
}

function handleConfirm() {
  if (!props.reschedule) {
    state.isLoading = true;
    state.checkAppointment = true;

    appointmentMutation.mutate();
  } else {
    let appointment = appointmentStore.currentAppointment;

    appointment.applicationId = null;
    //TODO: also change this once backend is changed
    appointment.status = false.toString();
    appointmentStore.sendAppointmentCheck(appointment).then(() => {
      appointmentMutation.mutate();
    });
  }
}
</script>

<style lang="scss" scoped>
.calendar-container {
  height: 800px;
  overflow: scroll;
  margin: 2em 0;
}
.button-card {
  height: 100%;
  display: flex;
  justify-content: space-around;
  align-items: center;
}
</style>
