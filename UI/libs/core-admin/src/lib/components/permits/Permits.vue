<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <v-select
          v-model="options.statuses"
          label="Application Status"
          :items="applicationStatusItems"
          item-text="text"
          item-value="value"
          color="primary"
          multiple
          outlined
          clearable
          hide-details
          small-chips
        />
      </v-col>

      <v-col>
        <v-select
          v-model="options.appointmentStatuses"
          :items="appointmentStatusItems"
          label="Appointment Status"
          item-value="value"
          item-text="text"
          color="primary"
          hide-details
          small-chips
          clearable
          outlined
          multiple
        />
      </v-col>

      <v-col>
        <v-select
          v-model="options.applicationTypes"
          :items="applicationTypeItems"
          label="Application Type"
          item-value="value"
          item-text="text"
          color="primary"
          hide-details
          small-chips
          clearable
          outlined
          multiple
        />
      </v-col>

      <v-col>
        <v-text-field
          v-model="options.search"
          label="Search"
          placeholder="Start typing to search"
          outlined
          hide-details
          clearable
        >
        </v-text-field>
      </v-col>
    </v-row>

    <v-data-table
      v-model="state.selected"
      :headers="state.headers"
      :items="data.items"
      :server-items-length="data.total"
      :options.sync="options.options"
      :loading="isLoading || isFetching || appointmentLoading"
      :loading-text="$t('Loading permit applications...')"
      :items-per-page="10"
      :footer-props="{
        'items-per-page-options': [10, 25, 50, 100],
      }"
      item-key="orderId"
      show-select
    >
      <template #top>
        <v-toolbar flat>
          <v-menu offset-y>
            <template #activator="{ on }">
              <v-btn
                v-on="on"
                color="primary"
                class="mr-2"
                dark
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
            {{ options.showingTodaysAppointments ? 'All' : "Today's" }}
            Appointments
          </v-btn>

          <v-menu
            ref="menuComponent"
            v-model="menu"
            :close-on-content-click="false"
            :return-value.sync="date"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template #activator="{ on, attrs }">
              <v-btn
                color="primary"
                v-bind="attrs"
                v-on="on"
              >
                Select a date {{ options.selectedDate }}
              </v-btn>
            </template>

            <v-date-picker
              v-model="options.selectedDate"
              no-title
              scrollable
            >
              <v-btn
                @click="clearDate"
                text
                color="primary"
              >
                Clear
              </v-btn>

              <v-spacer />

              <v-btn
                @click="menu = false"
                text
                color="primary"
              >
                Cancel
              </v-btn>

              <v-btn
                @click="menu = false"
                text
                color="primary"
              >
                OK
              </v-btn>
            </v-date-picker>
          </v-menu>
        </v-toolbar>
      </template>

      <template #[`item.orderId`]="props">
        <router-link
          :to="{
            name: 'PermitDetail',
            params: { orderId: props.item.orderId },
          }"
          style="text-decoration: underline; color: inherit"
        >
          {{ props.item.orderId }}
        </router-link>
      </template>

      <template #[`item.name`]="props">
        <div v-if="props.item.initials.length !== 0">
          <v-avatar
            color="primary"
            size="30"
            class="mr-1"
          >
            <span class="white--text .text-xs-caption">
              {{ props.item.initials }}</span
            >
          </v-avatar>
          {{ props.item.name }}
        </div>
        <v-icon
          :color="$vuetify.theme.dark ? '' : 'error'"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>

      <template #[`item.applicationType`]="props">
        {{ ApplicationType[props.item.applicationType] }}
      </template>

      <template #[`item.appointmentStatus`]="props">
        {{ AppointmentStatus[props.item.appointmentStatus] }}
      </template>

      <template #[`item.paymentStatus`]="{ item }">
        {{ item.paid ? 'Paid' : 'Unpaid' }}
      </template>

      <template #[`item.status`]="props">
        <v-btn
          :to="{
            name: 'PermitDetail',
            params: { orderId: props.item.orderId },
          }"
          color="primary"
          elevation="3"
          x-small
        >
          {{ ApplicationStatus[props.item.status] }}
        </v-btn>
      </template>

      <template #[`item.actions`]="props">
        <v-row>
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-if="props.item.appointmentStatus !== 3"
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
            <span v-if="props.item.appointmentStatus !== 3">Check In</span>
            <span v-else>Undo Check In</span>
          </v-tooltip>

          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-if="props.item.appointmentStatus !== 4"
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
            <span v-if="props.item.appointmentStatus !== 4">No Show</span>
            <span v-else>Undo No Show</span>
          </v-tooltip>
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
          <v-btn
            text
            color="error"
            @click="state.assignDialog = false"
          >
            Cancel
          </v-btn>

          <v-spacer></v-spacer>

          <v-btn
            text
            color="primary"
            @click="handleAssignMultipleApplications"
          >
            Assign
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { PermitsType } from '@core-admin/types'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  ApplicationTableOptionsType,
} from '@shared-utils/types/defaultTypes'
import {
  ApplicationType,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive, ref, watch } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const { getAllPermitsSummary } = usePermitsStore()

const options = ref<ApplicationTableOptionsType>({
  options: {
    page: 1,
    itemsPerPage: 10,
    sortBy: [],
    sortDesc: [],
    groupBy: [],
    groupDesc: [],
    multiSort: false,
    mustSort: false,
  },
  statuses: [2],
  search: '',
  paid: false,
  appointmentStatuses: [],
  applicationTypes: [],
  showingTodaysAppointments: false,
  selectedDate: '',
})
const applicationStatusItems = [
  { text: 'Incomplete', value: 1 },
  { text: 'Submitted', value: 2 },
  { text: 'Ready For Appointment', value: 3 },
  { text: 'Appointment Complete', value: 4 },
  { text: 'Background In Progress', value: 5 },
  { text: 'Contingently Approved', value: 6 },
  { text: 'Approved', value: 7 },
  { text: 'Permit Delivered', value: 8 },
  { text: 'Suspended', value: 9 },
  { text: 'Revoked', value: 10 },
  { text: 'Canceled', value: 11 },
  { text: 'Denied', value: 12 },
  { text: 'Withdrawn', value: 13 },
  { text: 'Flagged For Review', value: 14 },
  { text: 'Appointment No Show', value: 15 },
  { text: 'Contingently Denied', value: 16 },
  { text: 'Ready To Issue', value: 17 },
  { text: 'Waiting For Customer', value: 18 },

  'Available',
  'Not Scheduled',
  'Scheduled',
  'Checked In',
  'No Show',
]

const appointmentStatusItems = [
  {
    text: 'Not Scheduled',
    value: 1,
  },
  { text: 'Scheduled', value: 2 },
  { text: 'Checked In', value: 3 },
  { text: 'No Show', value: 4 },
]

const applicationTypeItems = [
  { text: 'Standard', value: 1 },
  { text: 'Reserve', value: 2 },
  { text: 'Judicial', value: 3 },
  { text: 'Employment', value: 4 },
  { text: 'Renew Standard', value: 5 },
  { text: 'Renew Reserve', value: 6 },
  { text: 'Renew Judicial', value: 7 },
  { text: 'Renew Employment', value: 8 },
  { text: 'Modify Standard', value: 9 },
  { text: 'Modify Reserve', value: 10 },
  { text: 'Modify Judicial', value: 11 },
  { text: 'Modify Employment', value: 12 },
]

const state = reactive({
  selected: [] as PermitsType[],
  selectedAdminUser: '',
  assignDialog: false,
  headers: [
    {
      text: 'Order ID',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    { text: 'Applicant Name', value: 'name' },
    { text: 'Application Type', value: 'applicationType' },
    { text: 'Appointment Status', value: 'appointmentStatus' },
    { text: 'Appointment Date/Time', value: 'appointmentDateTime' },
    { text: 'Payment Status', value: 'paymentStatus' },
    { text: 'Assigned User', value: 'assignedTo' },
    { text: 'Application Status', value: 'status' },
    { text: 'Actions', value: 'actions' },
  ],
})
const permitStore = usePermitsStore()
const adminUserStore = useAdminUserStore()
const appointmentsStore = useAppointmentsStore()
const menu = ref(false)
const date = ref('')

const {
  mutate: setAppointmentScheduled,
  isLoading: isAppointmentScheduledLoading,
} = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentsStore.putSetAppointmentScheduled(appointmentId),
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

const appointmentLoading = computed(() => {
  return (
    isAppointmentScheduledLoading.value ||
    isCheckInLoading.value ||
    isNoShowLoading.value
  )
})

const { isLoading, isFetching, data, refetch } = useQuery(
  ['permits'],
  async ({ signal }) => {
    let response: {
      items: Array<PermitsType>
      total: number
    } = { items: [], total: 0 }

    if (options.value) {
      response = await getAllPermitsSummary(options.value, signal)

      const totalPages = Math.ceil(
        response.total / options.value.options.itemsPerPage
      )

      let isBeyondLastPage = options.value.options.page > totalPages + 1

      while (isBeyondLastPage && options.value.options.page > 1) {
        options.value.options.page -= 1
        isBeyondLastPage = options.value.options.page > totalPages
      }

      return response
    }

    return response
  },
  { enabled: Boolean(options.value), initialData: { items: [], total: 0 } }
)

const { mutate: updateMultiplePermitDetailsApi } = useMutation({
  mutationFn: (orderIds: string[]) =>
    permitStore.updateMultiplePermitDetailsApi(
      orderIds,
      state.selectedAdminUser
    ),
})

function handleAdminUserSelect(adminUser: string) {
  state.selectedAdminUser = adminUser
  state.assignDialog = true
}

async function handleAssignMultipleApplications() {
  const orderIds = state.selected.map(element => element.orderId)

  if (state.selectedAdminUser) {
    updateMultiplePermitDetailsApi(orderIds)
  }

  state.assignDialog = false
}

function handleToggleTodaysAppointments() {
  options.value.showingTodaysAppointments =
    !options.value.showingTodaysAppointments
}

function clearDate() {
  options.value.selectedDate = ''
  menu.value = false
}

function handleSetScheduled(application) {
  application.appointmentStatus = AppointmentStatus.Scheduled
  setAppointmentScheduled(application.appointmentId)
}

function handleCheckIn(application) {
  application.appointmentStatus = AppointmentStatus['Checked In']
  checkInAppointment(application.appointmentId)
}

function handleNoShow(application) {
  application.appointmentStatus = AppointmentStatus['No Show']
  noShowAppointment(application.appointmentId)
}

watch(
  options,
  newVal => {
    if (newVal) {
      refetch()
    }
  },
  { deep: true }
)
</script>
