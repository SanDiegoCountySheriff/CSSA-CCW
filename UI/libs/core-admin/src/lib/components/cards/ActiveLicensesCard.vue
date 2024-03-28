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
      Standard:
      <span class="float-right">
        {{ permitsStore.summaryCount?.activeStandardStatus }}
      </span>

      <br />

      <v-divider />
      Judicial:
      <span class="float-right">
        {{ permitsStore.summaryCount?.activeJudicialStatus }}
      </span>

      <br />

      <v-divider />
      Reserve:
      <span class="float-right">
        {{ permitsStore.summaryCount?.activeReserveStatus }}
      </span>

      <br />

      <template v-if="brandStore.brand.employmentLicense">
        <v-divider />
        Employment:
        <span class="float-right">
          {{ permitsStore.summaryCount?.activeEmploymentStatus }}
        </span>
      </template>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()
const brandStore = useBrandStore()

const currentActiveLicenses = computed(() => {
  if (
    permitsStore.summaryCount?.activeJudicialStatus &&
    permitsStore.summaryCount?.activeReserveStatus &&
    permitsStore.summaryCount?.activeStandardStatus &&
    permitsStore.summaryCount?.activeEmploymentStatus
  )
    return (
      permitsStore.summaryCount?.activeJudicialStatus +
      permitsStore.summaryCount?.activeReserveStatus +
      permitsStore.summaryCount?.activeStandardStatus +
      permitsStore.summaryCount?.activeEmploymentStatus
    )

  return 0
})
</script>
