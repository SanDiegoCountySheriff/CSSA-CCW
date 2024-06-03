<template>
  <div>
    <v-card flat>
      <v-card-text>
        <v-data-table
          :items="data.items"
          :server-items-length="data.total"
          :options.sync="permitStore.undoOptions.options"
          :loading="isLoading || isFetching || isUndoMatchLoading"
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
                v-model="permitStore.undoOptions.search"
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
            <ConfirmationDialog
              @confirm="handleUndo(item)"
              button-text="Undo Match"
              title="Confirm Undo Match"
              text="Are you sure you want to undo this applicant/application match?"
              icon="mdi-undo"
            />
          </template>
        </v-data-table>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import ConfirmationDialog from '@core-admin/components/dialogs/ConfirmationDialog.vue'
import { PermitsType } from '@core-admin/types'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { watch } from 'vue'
import {
  ApplicationType,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { useMutation, useQuery } from '@tanstack/vue-query'

const permitStore = usePermitsStore()

const headers = [
  {
    text: 'Order ID',
    value: 'orderId',
  },
  { text: 'Applicant Name', value: 'name' },
  { text: 'Application Type', value: 'applicationType' },
  { text: 'Appointment Status', value: 'appointmentStatus' },
  { text: 'Appointment Date/Time', value: 'appointmentDateTime' },
  { text: 'Payment Status', value: 'paymentStatus', sortable: false },
  { text: 'Assigned User', value: 'assignedTo' },
  { text: 'Application Status', value: 'status' },
  { text: 'Actions', value: 'actions', sortable: false },
]

const { isLoading, isFetching, data, refetch } = useQuery(
  ['permits'],
  async ({ signal }) => {
    let response: {
      items: Array<PermitsType>
      total: number
    } = { items: [], total: 0 }

    if (permitStore.undoOptions) {
      response = await permitStore.getUndoPermitsSummary(signal)

      const totalPages = Math.ceil(
        response.total / permitStore.undoOptions.options.itemsPerPage
      )

      let isBeyondLastPage =
        permitStore.undoOptions.options.page > totalPages + 1

      while (isBeyondLastPage && permitStore.undoOptions.options.page > 1) {
        permitStore.undoOptions.options.page -= 1
        isBeyondLastPage = permitStore.undoOptions.options.page > totalPages
      }

      return response
    }

    return response
  },
  {
    refetchOnMount: 'always',
    enabled: Boolean(permitStore.undoOptions),
    initialData: { items: [], total: 0 },
  }
)

const { isLoading: isUndoMatchLoading, mutate: undoMatchApplication } =
  useMutation({
    mutationFn: (applicationId: string) =>
      permitStore.undoMatchApplication(applicationId),
    onSuccess: () => {
      refetch()
    },
  })

function handleUndo(item) {
  undoMatchApplication(item.item.id)
}

watch(
  permitStore.undoOptions,
  newVal => {
    if (newVal) {
      refetch()
    }
  },
  { deep: true }
)
</script>
