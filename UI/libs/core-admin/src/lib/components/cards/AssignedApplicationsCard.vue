<template>
  <v-card v-if="authStore.getAuthState.isAuthenticated">
    <v-card-title>My Assigned Applications</v-card-title>

    <v-card-text>
      <template v-for="(permit, index) in assignedApplications">
        <router-link
          :key="index"
          :to="{
            name: 'PermitDetail',
            params: { orderId: permit.orderId },
          }"
          tag="a"
          target="_self"
          style="text-decoration: none; color: inherit"
          replace
        >
          <v-row>
            <v-col>
              {{ permit.name }}
              <v-chip
                medium
                label
                :color="
                  $vuetify.theme.dark
                    ? ''
                    : permit.isComplete
                    ? 'primary'
                    : 'error'
                "
                class="ml-4 white--text"
              >
                {{ permit.isComplete ? 'Ready for review' : 'Incomplete' }}
              </v-chip>

              <v-chip
                medium
                label
                color="primary"
                class="ml-4"
              >
                {{ permit.orderId }}
              </v-chip>
            </v-col>
          </v-row>
        </router-link>
      </template>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const authStore = useAuthStore()
const permitsStore = usePermitsStore()

const assignedApplications = computed(() => {
  if (permitsStore.permits) {
    return permitsStore.permits.filter(p => {
      return p.assignedTo === authStore.auth.userName
    })
  }

  return []
})
</script>
