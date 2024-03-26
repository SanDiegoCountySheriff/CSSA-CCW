<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-text class="text-center">Pending Standard Applications</v-card-text>

    <v-card-title class="justify-center">
      {{ permitsStore.summaryCount?.standardType }}
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()
</script>
