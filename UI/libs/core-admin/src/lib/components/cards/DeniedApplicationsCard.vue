<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-text>
      Suspended:
      <span class="float-right">{{ numberOfSuspendedApplications }}</span>
      <br />
      <v-divider /> Revoked:
      <span class="float-right">{{ numberOfRevokedApplications }}</span> <br />
      <v-divider /> Denied:
      <span class="float-right">{{ numberOfDeniedApplications }}</span>
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

const numberOfSuspendedApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.status === ApplicationStatus.Suspended
  }).length
})

const numberOfRevokedApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.status === ApplicationStatus.Revoked
  }).length
})

const numberOfDeniedApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.status === ApplicationStatus.Denied
  }).length
})
</script>
