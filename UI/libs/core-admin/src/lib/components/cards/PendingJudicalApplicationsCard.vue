<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-text class="text-center">Pending Judicial Applications</v-card-text>

    <v-card-title class="justify-center">
      {{ numberOfPendingJudicialApplications }}
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { ApplicationStatus, ApplicationType } from '@shared-utils/types/defaultTypes'
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()

const numberOfPendingJudicialApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status !== ApplicationStatus.Approved &&
      p.status !== ApplicationStatus.Incomplete &&
      p.applicationType === ApplicationType.Judicial
    )
  }).length
})
</script>
