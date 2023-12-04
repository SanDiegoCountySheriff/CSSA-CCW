<template>
  <v-container fluid>
    <v-row>
      <v-col cols="3">
        <v-row>
          <v-col>
            <PendingStandardApplicationsCard />
          </v-col>
          <v-col>
            <NewAppointmentsCard />
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <PendingJudicalApplicationsCard />
          </v-col>
          <v-col>
            <NextAvailableAppointmentCard />
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <PendingReserveApplicationsCard />
          </v-col>
          <v-col>
            <DeniedApplicationsCard />
          </v-col>
        </v-row>
      </v-col>
      <v-col cols="6">
        <v-row>
          <v-col>
            <v-card height="100%">
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
      </v-col>
      <v-col cols="3">
        <v-row>
          <v-col>
            <ActiveLicensesCard />
          </v-col>
        </v-row>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="3"> </v-col>
      <v-col cols="6">
        <AssignedApplicationsCard />
      </v-col>
      <v-col cols="3"> </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import ActiveLicensesCard from '@core-admin/components/cards/ActiveLicensesCard.vue'
import AssignedApplicationsCard from '@core-admin/components/cards/AssignedApplicationsCard.vue'
import DeniedApplicationsCard from '@core-admin/components/cards/DeniedApplicationsCard.vue'
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import NewAppointmentsCard from '@core-admin/components/cards/NewAppointmentsCard.vue'
import NextAvailableAppointmentCard from '@core-admin/components/cards/NextAvailableAppointmentCard.vue'
import PendingJudicalApplicationsCard from '@core-admin/components/cards/PendingJudicalApplicationsCard.vue'
import PendingReserveApplicationsCard from '@core-admin/components/cards/PendingReserveApplicationsCard.vue'
import PendingStandardApplicationsCard from '@core-admin/components/cards/PendingStandardApplicationsCard.vue'
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
