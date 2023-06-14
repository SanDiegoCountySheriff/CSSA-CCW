<template>
  <v-container fluid>
    <v-data-table
      :headers="state.headers"
      :items="appointments"
      :search="state.search"
      :loading="loading && !isError"
      :loading-text="$t('Loading appointment schedules...')"
      :items-per-page="15"
      :footer-props="{
        showCurrentPage: true,
        showFirstLastPage: true,
        firstIcon: 'mdi-skip-backward',
        lastIcon: 'mdi-skip-forward',
        prevIcon: 'mdi-skip-previous',
        nextIcon: 'mdi-skip-next',
      }"
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title
            class="text-no-wrap pr-4"
            style="text-overflow: clip"
          >
            {{ $t('Appointments') }}
          </v-toolbar-title>
          <v-spacer></v-spacer>
          <v-container>
            <v-row justify="end">
              <v-col align="right">
                <v-btn
                  @click="handleToggleTodaysAppointments"
                  color="primary"
                  class="mr-2"
                >
                  {{ state.showingTodaysAppointments ? 'All' : "Today's" }}
                  Appointments
                </v-btn>
                <v-btn
                  :to="Routes.APPOINTMENT_MANAGEMENT_ROUTE_PATH"
                  color="primary"
                >
                  Appointment Management
                </v-btn>
              </v-col>
              <v-col md="6">
                <v-text-field
                  v-model="state.search"
                  prepend-icon="mdi-magnify"
                  label="Search"
                  placeholder="Start typing to search"
                  single-line
                  hide-details
                >
                </v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-toolbar>
      </template>

      <template #[`item.status`]="{ item }">
        <v-chip
          color="primary"
          class="ma-0 font-weight-regular"
          small
        >
          {{ AppointmentStatus[item.status] }}
        </v-chip>
      </template>

      <template #[`item.permit`]="props">
        <router-link
          :to="{
            name: 'PermitDetail',
            params: { orderId: props.item.permit },
          }"
          style="text-decoration: underline; color: inherit"
        >
          {{ props.item.permit }}
        </router-link>
      </template>

      <template #[`item.payment`]="props">
        <v-chip
          v-if="props.item.payment.length !== 0"
          color="primary"
          small
        >
          {{ props.item.payment }}
        </v-chip>
        <v-icon
          color="error"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>

      <template #[`item.actions`]="props">
        <v-row>
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-if="props.item.status !== 3"
                @click="handleCheckIn(props.item)"
                v-bind="attrs"
                v-on="on"
                color="success"
                class="mr-2"
                icon
              >
                <v-icon> mdi-check-bold </v-icon>
              </v-btn>
              <v-btn
                v-else
                @click="handleSetScheduled(props.item)"
                v-bind="attrs"
                v-on="on"
                color="success"
                class="mr-2"
                icon
              >
                <v-icon> mdi-undo </v-icon>
              </v-btn>
            </template>
            <span v-if="props.item.status !== 3">Check In</span>
            <span v-else>Undo Check In</span>
          </v-tooltip>

          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-if="props.item.status !== 4"
                @click="handleNoShow(props.item)"
                v-bind="attrs"
                v-on="on"
                color="error"
                class="mr-2"
                icon
              >
                <v-icon> mdi-close-thick </v-icon>
              </v-btn>
              <v-btn
                v-else
                @click="handleSetScheduled(props.item)"
                v-bind="attrs"
                v-on="on"
                color="error"
                class="mr-2"
                icon
              >
                <v-icon>mdi-undo</v-icon>
              </v-btn>
            </template>
            <span v-if="props.item.status !== 4">No Show</span>
            <span v-else>Undo No Show</span>
          </v-tooltip>

          <AppointmentDeleteDialog
            :appointment="props.item"
            :refetch="appointmentRefetch"
          />
        </v-row>
      </template>
    </v-data-table>
    <v-snackbar
      v-model="state.snackbar"
      :multi-line="state.multiLine"
    >
      {{ state.text }}

      <template #action="{ attrs }">
        <v-btn
          color="warning"
          text
          v-bind="attrs"
          @click="state.snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import AppointmentDeleteDialog from '../dialogs/AppointmentDeleteDialog.vue'
import Routes from '@core-admin/router/routes'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const appointmentsStore = useAppointmentsStore()
const {
  isLoading,
  isError,
  data,
  refetch: appointmentRefetch,
} = useQuery(['appointments'], appointmentsStore.getAppointmentsApi, {
  refetchOnMount: 'always',
})

const { mutate: checkInAppointment, isLoading: isCheckInLoading } = useMutation(
  {
    mutationFn: (appointmentId: string) =>
      appointmentsStore.putCheckInAppointment(appointmentId),
  }
)

const { mutate: noShowAppointment, isLoading: isNoShowLoading } = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentsStore.putNoShowAppointment(appointmentId),
})

const {
  mutate: setAppointmentScheduled,
  isLoading: isAppointmentScheduledLoading,
} = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentsStore.putSetAppointmentScheduled(appointmentId),
})

const loading = computed(() => {
  return (
    isLoading.value ||
    isNoShowLoading.value ||
    isCheckInLoading.value ||
    isAppointmentScheduledLoading.value
  )
})

const state = reactive({
  isSelecting: false,
  search: '',
  singleExpand: true,
  expanded: [],
  snack: false,
  snackColor: '',
  snackText: '',
  pagination: {},
  headers: [
    {
      text: 'STATUS',
      align: 'start',
      value: 'status',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'ORDER ID', value: 'permit' },
    { text: 'PAYMENT', value: 'payment' },
    { text: 'DATE', value: 'date' },
    { text: 'TIME', value: 'time' },
    { text: 'ACTIONS', value: 'actions' },
  ],
  multiLine: false,
  snackbar: false,
  text: `Invalid file type provided.`,
  showingTodaysAppointments: false,
  filteredData: data.value?.filter(d => {
    return d.date === new Date().toDateString()
  }),
})

const appointments = computed(() => {
  if (state.showingTodaysAppointments) {
    return state.filteredData
  }

  return data.value
})

function handleCheckIn(appointment: AppointmentType) {
  appointment.status = AppointmentStatus['Checked In']
  checkInAppointment(appointment.id)
}

function handleNoShow(appointment: AppointmentType) {
  appointment.status = AppointmentStatus['No Show']
  noShowAppointment(appointment.id)
}

function handleSetScheduled(appointment: AppointmentType) {
  appointment.status = AppointmentStatus.Scheduled
  setAppointmentScheduled(appointment.id)
}

function handleToggleTodaysAppointments() {
  state.showingTodaysAppointments = !state.showingTodaysAppointments
}
</script>
