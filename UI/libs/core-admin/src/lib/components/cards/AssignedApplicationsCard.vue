<template>
  <v-card v-if="authStore.getAuthState.isAuthenticated">
    <v-card-title>
      My Assigned Applications
      <v-spacer />
      <v-text-field
        v-model="search"
        label="Search"
        color="primary"
        outlined
        single-line
        hide-details
        dense
      ></v-text-field>
    </v-card-title>

    <v-card-text>
      <v-data-table
        :items="assignedApplications"
        :headers="headers"
        :search="search"
        :items-per-page="5"
      >
        <template #[`item.name`]="{ item }">
          <router-link
            :to="{
              name: 'PermitDetail',
              params: { orderId: item.orderId },
            }"
            tag="a"
            target="_self"
            style="text-decoration: none; color: inherit"
            replace
          >
            {{ item.name }}
          </router-link>
        </template>

        <template #[`item.status`]="{ item }">
          <router-link
            :to="{
              name: 'PermitDetail',
              params: { orderId: item.orderId },
            }"
            tag="a"
            target="_self"
            style="text-decoration: none; color: inherit"
            replace
          >
            {{ ApplicationStatus[item.status] }}
          </router-link>
        </template>

        <template #[`item.appointmentStatus`]="{ item }">
          <router-link
            :to="{
              name: 'PermitDetail',
              params: { orderId: item.orderId },
            }"
            tag="a"
            target="_self"
            style="text-decoration: none; color: inherit"
            replace
          >
            {{ AppointmentStatus[item.appointmentStatus] }}
          </router-link>
        </template>
      </v-data-table>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()
const search = ref('')
const headers = [
  {
    text: 'Name',
    value: 'name',
  },
  {
    text: 'Status',
    value: 'status',
  },
  { text: 'Appt', value: 'appointmentStatus' },
]

const assignedApplications = computed(() => {
  if (permitsStore.permits) {
    return permitsStore.permits.filter(p => {
      return p.assignedTo === authStore.auth.userName
    })
  }

  return []
})
</script>
