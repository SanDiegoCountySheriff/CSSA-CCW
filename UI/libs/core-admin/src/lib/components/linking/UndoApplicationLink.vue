<template>
  <div>
    <v-card flat>
      <v-card-text>
        <v-data-table
          :items="data.items"
          :server-items-length="data.total"
          :options.sync="options.options"
          :loading="isLoading || isFetching"
          :loading-text="$t('Loading permit applications...')"
          :headers="headers"
          :items-per-page="10"
          :footer-props="{
            'items-per-page-options': [10, 25, 50, 100],
          }"
        >
          <template #top>
            <v-toolbar flat>
              <v-toolbar-title> Undo Application Match </v-toolbar-title>

              <v-spacer />

              <v-text-field
                v-model="options.search"
                label="Search"
                color="primary"
                hide-details
                outlined
                clearable
              ></v-text-field>
            </v-toolbar>
          </template>

          <template #[`item.orderId`]="{ item }">
            <router-link
              :to="{
                name: 'PermitDetail',
                params: { orderId: item.orderId },
              }"
              style="text-decoration: underline; color: inherit"
            >
              {{ item.orderId }}
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

          <template #[`item.actions`]="item">
            <v-btn
              @click="handleUndo(item)"
              color="primary"
            >
              <v-icon left>mdi-undo</v-icon>
              Undo Match
            </v-btn>
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { PermitsType } from '@core-admin/types'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useMutation, useQuery } from '@tanstack/vue-query'
import {
  ApplicationStatus,
  ApplicationTableOptionsType,
} from '@shared-utils/types/defaultTypes'
import {
  ApplicationType,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { ref, watch } from 'vue'

const permitStore = usePermitsStore()

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
  statuses: [],
  search: '',
  paid: false,
  appointmentStatuses: [],
  applicationTypes: [],
  showingTodaysAppointments: false,
  selectedDate: '',
  applicationSearch: null,
  matchedApplications: true,
})

const headers = [
  {
    text: 'Order ID',
    value: 'orderId',
  },
  { text: 'Applicant Name', value: 'name' },
  { text: 'Application Type', value: 'applicationType' },
  { text: 'Appointment Status', value: 'appointmentStatus' },
  { text: 'Appointment Date/Time', value: 'appointmentDateTime' },
  { text: 'Payment Status', value: 'paymentStatus' },
  { text: 'Assigned User', value: 'assignedUser' },
  { text: 'Application Status', value: 'status' },
  { text: 'Actions', value: 'actions' },
]

const { isLoading, isFetching, data, refetch } = useQuery(
  ['permits'],
  async ({ signal }) => {
    let response: {
      items: Array<PermitsType>
      total: number
    } = { items: [], total: 0 }

    if (options.value) {
      response = await permitStore.getAllPermitsSummary(options.value, signal)

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
  {
    refetchOnMount: 'always',
    enabled: Boolean(options.value),
    initialData: { items: [], total: 0 },
  }
)

const { isLoading: isUndoMatchLoading, mutate: undoMatchApplication } =
  useMutation({
    mutationFn: ({
      userId,
      applicationId,
    }: {
      userId: string
      applicationId: string
    }) => permitStore.undoMatchApplication(userId, applicationId),
  })

function handleUndo(item) {
  window.console.log(item.item.id)
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
