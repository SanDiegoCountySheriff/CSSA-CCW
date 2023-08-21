<template>
  <v-card
    v-if="authStore.getAuthState.isAuthenticated"
    :loading="isLoading"
    height="100%"
  >
    <v-card-text class="text-center">
      New Appointments In Last
      <v-btn
        @click="numberOfDays++"
        icon
        x-small
      >
        <v-icon>mdi-menu-up</v-icon>
      </v-btn>
      {{ numberOfDays }}
      <v-btn
        @click="numberOfDays--"
        :disabled="numberOfDays === 0"
        icon
        x-small
      >
        <v-icon>mdi-menu-down</v-icon>
      </v-btn>
      Day{{ numberOfDays > 1 ? 's' : '' }}
    </v-card-text>

    <v-card-title class="justify-center">
      {{ data }}
    </v-card-title>
  </v-card>
</template>

<script setup lang="ts">
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useQuery } from '@tanstack/vue-query'
import { ref, watch } from 'vue'

const numberOfDays = ref(7)
const authStore = useAuthStore()
const appointmentsStore = useAppointmentsStore()
// eslint-disable-next-line no-undef
let timeout: NodeJS.Timeout

const { data, isLoading, refetch } = useQuery(
  ['getNumberOfNewAppointments'],
  () =>
    appointmentsStore.getNumberOfNewAppointmentsByNumberOfDays(
      numberOfDays.value
    )
)

watch(numberOfDays, () => {
  clearTimeout(timeout)

  timeout = setTimeout(() => {
    refetch()
  }, 2000)
})
</script>
