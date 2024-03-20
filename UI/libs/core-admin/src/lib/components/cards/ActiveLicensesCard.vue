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
      <v-divider />
      Judicial: <span class="float-right">{{ activeJudicialLicenses }}</span>
      <br />
      <v-divider />
      Reserve: <span class="float-right">{{ activeReserveLicenses }}</span>
      <br />
      <template v-if="brandStore.brand.employmentLicense">
        <v-divider />
        Employment:
        <span class="float-right">{{ activeEmploymentLicenses }}</span>
      </template>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  ApplicationStatus,
  ApplicationType,
} from '@shared-utils/types/defaultTypes'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()
const brandStore = useBrandStore()

const currentActiveLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.status === ApplicationStatus['Permit Delivered']
  }).length
})

const activeStandardLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus['Permit Delivered'] &&
      p.applicationType === ApplicationType.Standard
    )
  }).length
})

const activeJudicialLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus['Permit Delivered'] &&
      p.applicationType === ApplicationType.Judicial
    )
  }).length
})

const activeReserveLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus['Permit Delivered'] &&
      p.applicationType === ApplicationType.Reserve
    )
  }).length
})

const activeEmploymentLicenses = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status === ApplicationStatus['Permit Delivered'] &&
      p.applicationType === ApplicationType.Employment
    )
  }).length
})
</script>
