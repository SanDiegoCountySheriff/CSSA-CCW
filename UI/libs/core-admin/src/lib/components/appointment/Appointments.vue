<template>
  <v-container fluid>
    <v-data-table
      :headers="state.headers"
      :items="data"
      :search="state.search"
      :loading="isLoading && !isError"
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
                  color="primary"
                  class="mr-2"
                >
                  Today's Appointments
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

      <template #[`item.status`]="props">
        <v-chip
          color="primary"
          class="ma-0 font-weight-regular"
          small
        >
          {{ props.item.status === 'true' ? 'Scheduled' : 'Not Scheduled' }}
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
                v-bind="attrs"
                v-on="on"
                color="success"
                class="mr-2"
                icon
              >
                <v-icon> mdi-check-bold </v-icon>
              </v-btn>
            </template>
            <span>Check In</span>
          </v-tooltip>

          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-btn
                v-bind="attrs"
                v-on="on"
                color="error"
                class="mr-2"
                icon
              >
                <v-icon> mdi-close-thick </v-icon>
              </v-btn>
            </template>
            <span>No Show</span>
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
import { reactive } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useQuery } from '@tanstack/vue-query'

const appointmentsStore = useAppointmentsStore()
const {
  isLoading,
  isError,
  data,
  refetch: appointmentRefetch,
} = useQuery(['appointments'], appointmentsStore.getAppointmentsApi)

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
})
</script>
