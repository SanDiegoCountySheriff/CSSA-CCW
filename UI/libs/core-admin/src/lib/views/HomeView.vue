<template>
  <v-card
    max-width="900"
    class="mt-3"
  >
    <v-img
      alt="Agency Landing Page Image"
      :src="store.getDocuments.agencyLogo"
      height="300"
      contain
    ></v-img>

    <v-card-title class="justify-center">
      {{ store.getBrand.agencyName }} CCW Application
    </v-card-title>

    <v-container class="text-center">
      <div v-if="!authStore.getAuthState.isAuthenticated">
        <v-btn
          outlinedd
          color="primary"
          x-large
          @click="handleLogIn"
        >
          <v-icon class="mr-2"> mdi-login </v-icon>
          {{ $t('Login') }}
        </v-btn>
      </div>
      <v-container v-else>
        <SearchBar />
      </v-container>
    </v-container>
  </v-card>
</template>

<script setup lang="ts">
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import SearchBar from '@core-admin/components/search/SearchBar.vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { inject, ref } from 'vue'

const store = useBrandStore()
const authStore = useAuthStore()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)

async function handleLogIn() {
  msalInstance.value.logIn()
}
</script>
