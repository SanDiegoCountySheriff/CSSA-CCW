<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container class="px-0 py-0">
    <v-card
      class="pt-2 fill-height"
      outlined
    >
      <v-container>
        <v-row>
          <v-col
            cols="12"
            lg="4"
          >
            <div class="font-weight-bold">
              Application #{{ permitStore.getPermitDetail.application.orderId }}
            </div>
            <span class="body-2"> Submitted on {{ submittedDate }}</span>
          </v-col>

          <v-col
            cols="12"
            lg="4"
          >
            <v-select
              v-model="permitStore.getPermitDetail.application.applicationType"
              :items="items"
              @change="$event => updateApplicationType($event)"
              item-text="name"
              item-value="value"
              label="Application Type"
              dense
              outlined
            >
            </v-select>
          </v-col>

          <v-col
            cols="12"
            lg="4"
          >
            <v-select
              v-model="permitStore.getPermitDetail.application.status"
              :items="appStatus"
              @change="$event => updateApplicationStatus($event)"
              label="Application Status"
              item-text="value"
              item-value="id"
              dense
              outlined
            ></v-select>
          </v-col>
        </v-row>
      </v-container>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { computed, reactive } from 'vue'

const permitStore = usePermitsStore()

const items = [
  { name: 'Standard', value: 'standard' },
  { name: 'Reserve', value: 'reserve' },
  { name: 'Judicial', value: 'judicial' },
  { name: 'Renew Standard', value: 'renew-standard' },
  { name: 'Renew Reserve', value: 'renew-reserve' },
  { name: 'Renew Judicial', value: 'renew-judicial' },
  { name: 'Modify Standard', value: 'modify-standard' },
  { name: 'Modify Reserve', value: 'modify-reserve' },
  { name: 'Modify Judicial', value: 'modify-judicial' },
  { name: 'Duplicate Standard', value: 'duplicate-standard' },
  { name: 'Duplicate Reserve', value: 'duplicate-reserve' },
  { name: 'Duplicate Judicial', value: 'duplicate-judicial' },
]

const state = reactive({
  update: '',
})

const appStatus = [
  {
    id: 0,
    value: 'None',
  },
  {
    id: 1,
    value: 'Incomplete',
  },
  {
    id: 2,
    value: 'Submitted',
  },
  {
    id: 3,
    value: 'Ready for Appointment',
  },
  {
    id: 4,
    value: 'Appointment Complete',
  },
  {
    id: 5,
    value: 'Background in Progress',
  },
  {
    id: 6,
    value: 'Contingently Approved',
  },
  {
    id: 7,
    value: 'Approved',
  },
  {
    id: 8,
    value: 'Permit Delivered',
  },
  {
    id: 9,
    value: 'Suspend',
  },
  {
    id: 10,
    value: 'Revoke',
  },
  {
    id: 11,
    value: 'Cancelled',
  },
  {
    id: 12,
    value: 'Denied',
  },
  {
    id: 13,
    value: 'Withdrawn',
  },
]

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(state.update),
  {
    enabled: false,
  }
)

const submittedDate = computed(() => {
  if (permitStore.getPermitDetail.application.submittedToLicensingDateTime) {
    return new Date(
      permitStore.getPermitDetail.application.submittedToLicensingDateTime
    ).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    })
  }

  return ''
})

function updateApplicationStatus(update: string) {
  state.update = `Changed application status to ${update}`

  updatePermitDetails()
}

function updateApplicationType(update: string) {
  state.update = `Changed application type to ${update}`

  updatePermitDetails()
}
</script>
