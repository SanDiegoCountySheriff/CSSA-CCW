<template>
  <div>
    <v-row>
      <v-col>
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
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <v-card>
          <v-card-title>My Assigned Applications</v-card-title>

          <v-card-text>
            <template v-for="(permit, index) in assignedApplications">
              <router-link
                :key="index"
                :to="{
                  name: 'PermitDetail',
                  params: { orderId: permit.orderID },
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
                      {{
                        permit.isComplete ? 'Ready for review' : 'Incomplete'
                      }}
                    </v-chip>

                    <v-chip
                      medium
                      label
                      color="primary"
                      class="ml-4"
                    >
                      {{ permit.orderID }}
                    </v-chip>
                  </v-col>
                </v-row>
              </router-link>
            </template>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import SearchBar from '@core-admin/components/search/SearchBar.vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, inject, ref } from 'vue'

const store = useBrandStore()
const authStore = useAuthStore()
const permitsStore = usePermitsStore()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)

const assignedApplications = computed(() => {
  return permitsStore.permits.filter(p => {
    return p.assignedTo === authStore.auth.userName
  })
})

async function handleLogIn() {
  msalInstance.value.logIn()
}
</script>
