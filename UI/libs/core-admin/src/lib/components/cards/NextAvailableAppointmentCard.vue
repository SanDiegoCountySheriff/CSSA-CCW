<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    :loading="isLoading"
    height="100%"
  >
    <v-card-text class="text-center"> Next Available Appointment </v-card-text>

    <v-card-title
      v-if="data"
      class="justify-center"
    >
      {{ new Date(data).toLocaleDateString() }}
    </v-card-title>

    <v-card-title
      v-else
      class="justify-center"
    >
      None!
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useQuery } from '@tanstack/vue-query'

const authStore = useAuthStore()
const appointmentsStore = useAppointmentsStore()
const isAuthenticated = computed(() => authStore.getAuthState.isAuthenticated)

const { data, isLoading } = useQuery(
  ['getNextAvailableAppointment'],
  () => appointmentsStore.getNextAvailableAppointment(),
  {
    enabled: isAuthenticated,
  }
)
</script>
