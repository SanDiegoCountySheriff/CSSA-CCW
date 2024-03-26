<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    height="100%"
  >
    <v-card-text>
      Suspended:
      <span class="float-right">{{
        permitsStore.summaryCount?.suspendedStatus
      }}</span>
      <br />
      <v-divider /> Revoked:
      <span class="float-right">{{
        permitsStore.summaryCount?.revokedStatus
      }}</span>
      <br />
      <v-divider /> Denied:
      <span class="float-right">{{
        permitsStore.summaryCount?.deniedStatus
      }}</span>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()
</script>
