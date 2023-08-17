<template>
  <v-card v-if="authStore.getAuthState.isAuthenticated">
    <v-card-title>Pending Standard Applications</v-card-title>

    <v-card-title>
      {{ numberOfPendingStandardApplications }}
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()

const numberOfPendingStandardApplications = computed(() => {
  return permitsStore.permits?.filter(p => {
    return p.applicationType === 'standard'
  }).length
})
</script>
