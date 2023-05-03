<!-- eslint-disable vue/multiline-html-element-content-newline -->
<!-- eslint-disable vue-a11y/form-has-label -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <v-container fluid>
    <v-data-table
      :headers="state.headers"
      :items="data"
      :search="state.search"
      :loading="isLoading && !isError"
      :loading-text="$t('Loading appointment schedules...')"
      :single-expand="state.singleExpand"
      :expanded.sync="state.expanded"
      :items-per-page="14"
      :footer-props="{
        showCurrentPage: true,
        showFirstLastPage: true,
        firstIcon: 'mdi-skip-backward',
        lastIcon: 'mdi-skip-forward',
        prevIcon: 'mdi-skip-previous',
        nextIcon: 'mdi-skip-next',
      }"
      item-key="id"
      item-class="rowClass"
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title
            class="text-no-wrap pr-4"
            style="text-overflow: clip"
            >{{ $t('Appointments') }}</v-toolbar-title
          >
          <v-spacer></v-spacer>
          <v-container>
            <v-row justify="end">
              <v-col align="right">
                <v-btn
                  :to="Routes.APPOINTMENT_MANAGEMENT_ROUTE_PATH"
                  color="primary"
                  >Appointment Management</v-btn
                >
              </v-col>
              <v-col md="6">
                <v-text-field
                  v-model="state.search"
                  prepend-icon="mdi-filter"
                  label="Filter"
                  placeholder="Start typing to filter"
                  single-line
                  hide-details
                >
                </v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-toolbar>
      </template>
      <template #item.status="props">
        <v-chip
          v-if="props.item.status.length !== 0"
          :color="$vuetify.theme.dark ? '' : getColor(props.item.status)"
          :text-color="
            $vuetify.theme.dark ? '' : getTextColor(props.item.status)
          "
          class="ma-0 font-weight-regular"
          small
        >
          {{ props.item.status === 'true' ? 'Scheduled' : 'Not Scheduled' }}
        </v-chip>
        <v-icon
          :color="$vuetify.theme.dark ? '' : 'error'"
          medium
          v-else
        >
          mdi-alert-octagon
        </v-icon>
      </template>
      <template #item.name="props">
        <v-avatar
          color="primary"
          size="30"
          class="mr-1"
        >
          <span class="white--text .text-xs-caption"> {{ $t('JS') }}</span>
        </v-avatar>
        {{ props.item.name }}
      </template>
      <template #item.permit="props">
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
      <template #item.payment="props">
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
      <template #item.deletion="props">
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <AppointmentDeleteDialog
              :appointment="props.item"
              :refetch="appointmentRefetch"
              v-bind="attrs"
              v-on="on"
            />
          </template>
          {{ $t('Remove applicant from appointment') }}
        </v-tooltip>
      </template>
      <template #expanded-item="{ item }">
        <td colspan="2">
          {{ $t(`More info about ${item.name}`) }}
        </td>
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
      sortable: false,
      value: 'status',
    },
    { text: 'APPLICANT NAME', value: 'name' },
    { text: 'ORDER ID', value: 'permit' },
    { text: 'PAYMENT', value: 'payment' },
    { text: 'DATE', value: 'date' },
    { text: 'TIME', value: 'time' },
    { text: '', value: 'deletion' },
  ],
  multiLine: false,
  snackbar: false,
  text: `Invalid file type provided.`,
})

function getColor(name) {
  if (name === 'New' || name.match(/^\d/)) return '#eff8ff'

  if (name === 'cash' || name === 'card') return '#ecfdf3'

  if (name === 'Set') return '#fffaeb'

  return '#f2f4f7'
}

function getTextColor(name) {
  if (name === 'New' || name.match(/^\d/)) return '#2e90fa'

  if (name === 'cash' || name === 'card') return '#12B76A'

  if (name === 'Set') return '#F79009'

  return '#667085'
}
</script>
