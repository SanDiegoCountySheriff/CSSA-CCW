<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-title class="justify-center">Current Active CCWs</v-card-title>
    <v-card-title class="justify-center">
      {{ currentActiveLicenses }}
    </v-card-title>

    <v-card-text>
      Standard: <span class="float-right">{{ activeStandardLicenses }}</span>
      <br />
      <v-divider /> Judicial:
      <span class="float-right">{{ activeJudicialLicenses }}</span> <br />
      <v-divider /> Reserve:
      <span class="float-right">{{ activeReserveLicenses }}</span>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()

const currentActiveLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.status === ApplicationStatus.Approved
  }).length
})

const activeStandardLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus.Approved &&
      p.applicationType === 'standard'
    )
  }).length
})

const activeJudicialLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus.Approved &&
      p.applicationType === 'judicial'
    )
  }).length
})

const activeReserveLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus.Approved && p.applicationType === 'reserve'
    )
  }).length
})
</script>
