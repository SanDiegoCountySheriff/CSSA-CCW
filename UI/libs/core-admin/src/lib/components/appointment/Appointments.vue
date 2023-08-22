<template>
  <v-container fluid>
    <v-data-table
      :headers="state.headers"
      :items="appointments"
      :search="state.search"
      :loading="loading && !isError"
      :loading-text="$t('Loading appointment schedules...')"
      :items-per-page="15"
      show-select
      v-model="state.selected"
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
          <v-container>
            <v-row justify="end">
              <v-col md="8">
                <v-menu offset-y>
                  <template #activator="{ on }">
                    <v-btn
                      class="mr-2"
                      color="primary"
                      dark
                      v-on="on"
                    >
                      {{ 'Assign User' }}
                    </v-btn>
                  </template>
                  <v-list>
                    <v-list-item
                      v-for="(adminUser, index) in adminUserStore.allAdminUsers"
                      :key="index"
                      @click="handleAdminUserSelect(adminUser.name)"
                    >
                      <v-list-item-title>
                        {{ adminUser.name }}
                      </v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
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
              <v-col>
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

    <v-dialog
      v-model="state.assignDialog"
      persistent
      max-width="500"
    >
      <v-card>
        <v-card-title>Assign User</v-card-title>
        <v-card-text>
          Are you sure you want to assign
          {{ state.selected.length }} applications to:
          {{ state.selectedAdminUser }}
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="error"
            @click="state.assignDialog = false"
          >
            No
          </v-btn>
          <v-btn
            rounded
            color="primary"
            @click="handleAssignMultipleApplications"
          >
            Yes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

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
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const appointmentsStore = useAppointmentsStore()
const adminUserStore = useAdminUserStore()
const permitStore = usePermitsStore()
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
  selected: [] as AppointmentType[],
  selectedAdminUser: '',
  assignDialog: false,
  headers: [
    {
      text: 'STATUS',
      align: 'start',
      value: 'status',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'ORDER ID', value: 'permit' },
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

const { mutate: updateMultiplePermitDetailsApi } = useMutation({
  mutationFn: (orderIds: string[]) =>
    permitStore.updateMultiplePermitDetailsApi(
      orderIds,
      state.selectedAdminUser
    ),
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

function handleAdminUserSelect(adminUser) {
  state.selectedAdminUser = adminUser
  state.assignDialog = true
}

async function handleAssignMultipleApplications() {
  const orderIds = state.selected.map(element => element.permit)

  if (state.selectedAdminUser) {
    updateMultiplePermitDetailsApi(orderIds)
  }

  state.assignDialog = false
}
</script>
