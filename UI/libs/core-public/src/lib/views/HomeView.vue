<template>
  <div>
    <template v-if="isFetching">
      <v-container>
        <Loader />
      </v-container>
    </template>

    <template v-else>
      <v-container class="text-center">
        <v-img
          class="mx-auto"
          alt="Application logo"
          :src="brandStore.getDocuments.agencyLandingPageImage"
          max-width="500"
        />
      </v-container>

      <v-container fluid>
        <v-row>
          <v-col
            cols="12"
            lg="4"
            class="text-center"
          >
            <v-btn
              v-if="authStore.getAuthState.isAuthenticated && data?.length > 0"
              @click="viewApplication"
              color="primary"
              x-large
            >
              <v-icon class="mr-2"> mdi-card-account-details-outline </v-icon>
              {{ $t('View Application') }}
            </v-btn>

            <v-btn
              v-else-if="
                authStore.getAuthState.isAuthenticated && data?.length === 0
              "
              @click="createNewApplication"
              color="primary"
              x-large
            >
              <v-icon class="mr-2"> mdi-file-star-outline</v-icon>
              {{ $t('Create Application') }}
            </v-btn>

            <v-btn
              v-else
              @click="handleLogIn"
              color="primary"
              x-large
            >
              <v-icon class="mr-2"> mdi-login </v-icon>
              {{ $t('Login or Sign-up') }}
            </v-btn>
          </v-col>

          <v-col
            cols="12"
            lg="4"
            align="center"
          >
            <GeneralInfoWrapper />
          </v-col>

          <v-col
            cols="12"
            lg="4"
          >
            <PriceInfoWrapper />
          </v-col>
        </v-row>
      </v-container>
    </template>
  </div>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import GeneralInfoWrapper from '@core-public/components/wrappers/GeneralInfoWrapper.vue'
import Loader from '@core-public/views/Loader.vue'
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import PriceInfoWrapper from '@core-public/components/wrappers/PriceInfoWrapper.vue'
import Routes from '@core-public/router/routes'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useQuery } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { inject, ref } from 'vue'

const brandStore = useBrandStore()
const authStore = useAuthStore()
const router = useRouter()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)
const completeApplicationStore = useCompleteApplicationStore()

const { data, isFetching } = useQuery(
  ['getApplicationsByUser'],
  completeApplicationStore.getAllUserApplicationsApi,
  {
    refetchOnMount: 'always',
    enabled: authStore.getAuthState.isAuthenticated,
  }
)

function handleLogIn() {
  msalInstance.value.logIn()
}

function createNewApplication() {
  router.push({ path: Routes.APPLICATION_ROUTE_PATH })
}

function viewApplication() {
  completeApplicationStore.setCompleteApplication(
    data.value[0] as CompleteApplication
  )

  router.push({
    path: Routes.APPLICATION_DETAIL_ROUTE,
    query: {
      applicationId: completeApplicationStore.completeApplication.id,
      isComplete:
        completeApplicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}
</script>
