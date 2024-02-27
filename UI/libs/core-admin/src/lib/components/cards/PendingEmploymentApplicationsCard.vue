<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-text class="text-center">
      Pending Employment Applications
    </v-card-text>

    <v-card-title class="justify-center">
      {{ numberOfPendingEmploymentApplications }}
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()

const numberOfPendingEmploymentApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return (
      p.status !== ApplicationStatus.Approved &&
      p.status !== ApplicationStatus.Incomplete &&
      p.applicationType === 'employment'
    )
  }).length
})
</script>
