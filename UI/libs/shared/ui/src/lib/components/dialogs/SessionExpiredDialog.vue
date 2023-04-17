<template>
  <v-row justify="center">
    <v-dialog
      v-model="dialog"
      persistent
      width="600"
    >
      <v-card>
        <v-card-title class="text-h5 primary white--text">
          {{ $t('Session Timed Out') }}
        </v-card-title>
        <v-card-text class="mt-2">
          {{
            $t(
              'Your session has expired, Please refresh to continue your session'
            )
          }}
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="secondary"
            class="black--text"
            elevation="2"
            raised
            @click="authStore.getAuthState.tokenExpired = false"
          >
            {{ $t('Close') }}
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            elevation="2"
            raised
            @click="handleSessionTimeout"
          >
            {{ $t('Refresh') }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>
<script setup lang="ts">
import { computed } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'

const authStore = useAuthStore()
const dialog = computed(
  () =>
    authStore.getAuthState.isAuthenticated &&
    authStore.getAuthState.tokenExpired
)

function handleSessionTimeout() {
  authStore.getAuthState.tokenExpired = false
  window.location.reload()
}
</script>
