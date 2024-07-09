<template>
  <v-container fluid>
    <v-card
      flat
      class="mb-3"
    >
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
            :items="applicationTypeItems"
            @change="setApplicationType"
            label="Application Type"
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
    </v-card>

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
          <v-menu
            offset-y
            max-height="500"
          >
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
        {{
          isAppointmentComplete(props.item)
            ? 'Complete'
            : AppointmentStatus[props.item.appointmentStatus]
        }}
      </template>

      <template #[`item.appointmentDateTime`]="props">
        {{
          isAppointmentComplete(props.item)
            ? 'Complete'
            : props.item.appointmentDateTime
        }}
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
          <AppointmentActionConfirmationDialog
            :undo-active="props.item.appointmentStatus === 3"
            check-in
            title="Check In"
            @confirm="handleCheckIn(props.item)"
            @undo="handleSetScheduled(props.item)"
          />
          <AppointmentActionConfirmationDialog
            :undo-active="props.item.appointmentStatus === 4"
            :check-in="false"
            title="No Show"
            @confirm="handleNoShow(props.item)"
            @undo="handleSetScheduled(props.item)"
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
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import AppointmentActionConfirmationDialog from '@core-admin/components/dialogs/AppointmentActionConfirmationDialog.vue'
import { PermitsType } from '@core-admin/types'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationType,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive, ref, watch } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const { getAllPermitsSummary, options } = usePermitsStore()
const brandStore = useBrandStore()

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
  { text: 'Modification Approved', value: 19 },
  { text: 'Renewal Approved', value: 20 },
  { text: 'Modification Denied', value: 21 },
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

const applicationTypeItems = brandStore.brand.employmentLicense
  ? [
      { text: 'Standard' },
      { text: 'Reserve' },
      { text: 'Judicial' },
      { text: 'Employment' },
      { text: 'Renew' },
      { text: 'Modify' },
    ]
  : [
      { text: 'Standard' },
      { text: 'Reserve' },
      { text: 'Judicial' },
      { text: 'Renew' },
      { text: 'Modify' },
    ]

function setApplicationType(item: Array<string>) {
  if (item.includes('Renew')) {
    options.applicationTypes.push(5)
    options.applicationTypes.push(6)
    options.applicationTypes.push(7)
    options.applicationTypes.push(8)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 5 && at !== 6 && at !== 7 && at !== 8
    })
  }

  if (item.includes('Modify')) {
    options.applicationTypes.push(9)
    options.applicationTypes.push(10)
    options.applicationTypes.push(11)
    options.applicationTypes.push(12)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 9 && at !== 10 && at !== 11 && at !== 12
    })
  }

  if (item.includes('Standard')) {
    options.applicationTypes.push(1)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 1
    })
  }

  if (item.includes('Reserve')) {
    options.applicationTypes.push(2)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 2
    })
  }

  if (item.includes('Judicial')) {
    options.applicationTypes.push(3)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 3
    })
  }

  if (item.includes('Employment')) {
    options.applicationTypes.push(4)
  } else {
    options.applicationTypes = options.applicationTypes.filter(at => {
      return at !== 4
    })
  }
}

const permitStore = usePermitsStore()
const adminUserStore = useAdminUserStore()
const appointmentsStore = useAppointmentsStore()
const menu = ref(false)
const date = ref('')
let changed: string

function isAppointmentComplete(permit: PermitsType) {
  return (
    permit.status === ApplicationStatus['Appointment Complete'] ||
    permit.status === ApplicationStatus['Background In Progress'] ||
    permit.status === ApplicationStatus['Contingently Denied'] ||
    permit.status === ApplicationStatus['Contingently Approved'] ||
    permit.status === ApplicationStatus.Approved ||
    permit.status === ApplicationStatus['Permit Delivered'] ||
    permit.status === ApplicationStatus.Suspended ||
    permit.status === ApplicationStatus.Revoked ||
    permit.status === ApplicationStatus.Canceled ||
    permit.status === ApplicationStatus.Denied ||
    permit.status === ApplicationStatus.Withdrawn ||
    permit.status === ApplicationStatus['Ready To Issue'] ||
    permit.status === ApplicationStatus['Modification Approved'] ||
    permit.status === ApplicationStatus['Renewal Approved'] ||
    (permit.status === ApplicationStatus['Waiting For Customer'] &&
      permit.appointmentDateTime !== null &&
      permit.appointmentDateTime < new Date().toISOString()) ||
    (permit.status === ApplicationStatus['Flagged For Review'] &&
      permit.appointmentDateTime !== null &&
      permit.appointmentDateTime < new Date().toISOString())
  )
}

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
    {
      text: 'Appointment Status',
      value: 'appointmentStatus',
    },
    {
      text: 'Appointment Date/Time',
      value: 'appointmentDateTime',
    },
    { text: 'Payment Status', value: 'paymentStatus', sortable: false },
    { text: 'Assigned User', value: 'assignedTo' },
    { text: 'Application Status', value: 'status' },
    { text: 'Actions', value: 'actions', sortable: false },
  ],
})

const {
  mutate: addApplicationHistory,
  isLoading: isAddApplicationHistoryLoading,
} = useMutation({
  mutationFn: (applicationId: string) =>
    permitStore.addApplicationHistory(changed, applicationId),
})

const {
  mutate: setAppointmentScheduled,
  isLoading: isAppointmentScheduledLoading,
} = useMutation({
  mutationFn: ({
    appointmentId,
    applicationId,
  }: {
    appointmentId: string
    applicationId: string
  }) =>
    appointmentsStore.putSetAppointmentScheduled(appointmentId).then(() => {
      changed = 'Undo no show or check in appointment'
      addApplicationHistory(applicationId)
    }),
})

const { mutate: checkInAppointment, isLoading: isCheckInLoading } = useMutation(
  {
    mutationFn: ({
      appointmentId,
      applicationId,
    }: {
      appointmentId: string
      applicationId: string
    }) =>
      appointmentsStore.putCheckInAppointment(appointmentId).then(() => {
        changed = 'Update check in appointment'
        addApplicationHistory(applicationId)
      }),
  }
)

const { mutate: noShowAppointment, isLoading: isNoShowLoading } = useMutation({
  mutationFn: ({
    appointmentId,
    applicationId,
  }: {
    appointmentId: string
    applicationId: string
  }) =>
    appointmentsStore.putNoShowAppointment(appointmentId).then(() => {
      changed = 'Update no show appointment'
      addApplicationHistory(applicationId)
    }),
})

const appointmentLoading = computed(() => {
  return (
    isAppointmentScheduledLoading.value ||
    isCheckInLoading.value ||
    isNoShowLoading.value ||
    isAddApplicationHistoryLoading.value
  )
})

const { isLoading, isFetching, data, refetch } = useQuery(
  ['permits'],
  async ({ signal }) => {
    let response: {
      items: Array<PermitsType>
      total: number
    } = { items: [], total: 0 }

    if (options) {
      response = await getAllPermitsSummary(signal)

      const totalPages = Math.ceil(
        response.total / options.options.itemsPerPage
      )

      let isBeyondLastPage = options.options.page > totalPages + 1

      while (isBeyondLastPage && options.options.page > 1) {
        options.options.page -= 1
        isBeyondLastPage = options.options.page > totalPages
      }

      return response
    }

    return response
  },
  { enabled: Boolean(options), initialData: { items: [], total: 0 } }
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

  for (let application of state.selected) {
    application.assignedTo = state.selectedAdminUser
  }

  if (state.selectedAdminUser) {
    updateMultiplePermitDetailsApi(orderIds)
  }

  state.assignDialog = false
}

function handleToggleTodaysAppointments() {
  options.showingTodaysAppointments = !options.showingTodaysAppointments
}

function clearDate() {
  options.selectedDate = ''
  menu.value = false
}

function handleSetScheduled(application) {
  application.appointmentStatus = AppointmentStatus.Scheduled
  setAppointmentScheduled({
    appointmentId: application.appointmentId,
    applicationId: application.id,
  })
}

function handleCheckIn(application) {
  application.appointmentStatus = AppointmentStatus['Checked In']
  checkInAppointment({
    appointmentId: application.appointmentId,
    applicationId: application.id,
  })
}

function handleNoShow(application) {
  application.appointmentStatus = AppointmentStatus['No Show']
  noShowAppointment({
    appointmentId: application.appointmentId,
    applicationId: application.id,
  })
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
