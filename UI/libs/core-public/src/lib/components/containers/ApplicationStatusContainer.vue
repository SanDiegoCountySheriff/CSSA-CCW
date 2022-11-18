<template>
  <div>
    <v-container v-if="isLoading">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item"
      />
    </v-container>
    <v-container v-else>
      <v-sheet class="p-5 rounded">
        <v-row>
          <v-col
            cols="12"
            lg="6"
          >
            <v-card class="ml-5 text-left">
              <v-card-title class="ml-5">
                {{ $t('Application Status') }}
              </v-card-title>

              <v-card-text class="ml-5">
                <v-chip
                  :color="state.statusColor"
                  label
                  large
                  class="font-weight-bold"
                >
                  {{ $t('Status: ') }}
                  {{ state.status }}
                </v-chip>
              </v-card-text>

              <v-card-text class="ml-5">
                <v-chip
                  color="accent"
                  label
                  large
                  class="font-weight-bold"
                >
                  {{ $t('Order Number: ') }}
                  {{ applicationStore.completeApplication.application.orderId }}
                </v-chip>
              </v-card-text>

              <v-card-text class="ml-5">
                <v-chip
                  color="accent"
                  label
                  large
                  class="font-weight-bold"
                >
                  {{ $t('Application Type: ') }}
                  {{
                    applicationStore.completeApplication.application
                      .applicationType
                  }}
                </v-chip>
              </v-card-text>
              <v-card-title>
                {{ $t('Uploaded Documents') }}
              </v-card-title>
              <v-card-text>
                <v-row>
                  <v-col
                    cols="12"
                    lg="4"
                    v-for="(doc, index) in applicationStore.completeApplication
                      .application.uploadedDocuments"
                    :key="index"
                  >
                    <v-chip color="info">
                      {{ $t(doc.name) }}
                    </v-chip>
                  </v-col>
                </v-row>
              </v-card-text>
            </v-card>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-card class="mr-5">
              <v-card-title>
                {{ $t('Appointment Status') }}
              </v-card-title>
              <v-card-text>
                <v-chip
                  color="warning"
                  label
                  large
                  class="font-weight-bold"
                >
                  {{ $t('Scheduled Appointment') }} -
                  {{
                    new Date(
                      appointmentsStore.currentAppointment.start
                    ).toLocaleString()
                  }}
                </v-chip>
              </v-card-text>
              <v-card-title>
                <v-btn
                  color="success"
                  @click="state.showCalendar = !state.showCalendar"
                >
                  {{ $t('Reschedule') }}
                </v-btn>
              </v-card-title>
            </v-card>
          </v-col>
        </v-row>
      </v-sheet>
    </v-container>

    <v-container v-if="state.showCalendar">
      <v-skeleton-loader
        v-if="state.appointmentsLoading"
        fluid
        class="fill-height"
        type="list-item"
      />

      <AppointmentContainer
        v-else
        :events="state.appointments"
        :toggle-appointment="() => {}"
        :reschedule="true"
      />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue';
import { AppointmentType } from '@shared-utils/types/defaultTypes';
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { onMounted, reactive } from 'vue';
import { useMutation, useQuery } from '@tanstack/vue-query';

const applicationStore = useCompleteApplicationStore();
const appointmentsStore = useAppointmentsStore();

const state = reactive({
  status: '',
  statusColor: 'accent',
  showCalendar: false,
  appointments: [],
  appointmentsLoading: true,
});

const { isLoading } = useQuery(['getAppointment'], () => {
  const appointment = appointmentsStore.getSingleAppointment(
    applicationStore.completeApplication.id
  );

  appointment.then(data => {
    appointmentsStore.setCurrentAppointment(data);
  });
});

const allAppointmentsMutation = useMutation({
  mutationFn: () => {
    return appointmentsStore.getAvailableAppointments().then(data => {
      state.appointments = data;
    });
  },
  onSuccess: () => {
    formatAppointments();
    state.appointmentsLoading = false;
  },
  onError: () => {
    window.console.warn('failed');
  },
});

function formatAppointments() {
  let appointments = [] as Array<AppointmentType>;

  state.appointments.forEach((event: AppointmentType) => {
    let newEvent = event;
    let start = new Date(event.start);
    let end = new Date(event.end);

    let formatedStart = `${start.getFullYear()}-${
      start.getMonth() + 1
    }-${start.getDate()} ${start.getHours()}:${start.getMinutes()}`;

    let formatedEnd = `${end.getFullYear()}-${
      end.getMonth() + 1
    }-${end.getDate()} ${end.getHours()}:${end.getMinutes()}`;

    newEvent.start = formatedStart;
    newEvent.end = formatedEnd;
    appointments.push(newEvent);
  });

  state.appointments = appointments;
}

onMounted(() => {
  switch (applicationStore.completeApplication.application.status) {
    case 0:
      state.status = 'None';
      state.statusColor = 'warning';
      break;
    case 1:
      state.status = 'Started';
      state.statusColor = 'accent';
      break;
    case 2:
      state.status = 'Submitted';
      state.statusColor = 'accent';
      break;
    case 3:
      state.status = 'In progress';
      state.statusColor = 'accent';
      break;
    case 4:
      state.status = 'Cancelled';
      state.statusColor = 'error';
      break;
    case 5:
      state.status = 'Returned';
      state.statusColor = 'warning';
      break;
    case 6:
      state.status = 'Complete';
      state.statusColor = 'success';
      break;
    default:
      break;
  }

  allAppointmentsMutation.mutate();
});
</script>
