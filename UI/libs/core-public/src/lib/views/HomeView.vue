<template>
  <div>
    <template v-if="isFetching">
      <Loader />
    </template>

    <template v-else>
      <v-container class="text-center">
        <img
          :class="{ dark: $vuetify.theme.dark }"
          alt="Application logo"
          :src="brandStore.getDocuments.agencyLandingPageImage"
        />
      </v-container>

      <v-container fluid>
        <v-row>
          <v-col
            cols="4"
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
              :loading="isLoading"
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

          <v-col cols="4">
            <GeneralInfoWrapper />
          </v-col>

          <v-col cols="4">
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
import { defaultPermitState } from '@shared-utils/lists/defaultConstants'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { inject, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

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

const { isLoading, mutate: createMutation } = useMutation({
  mutationFn: completeApplicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        applicationId: completeApplicationStore.completeApplication.id,
        isComplete:
          completeApplicationStore.completeApplication.application.isComplete.toString(),
      },
    })
  },
  onError: () => null,
})

function handleLogIn() {
  msalInstance.value.logIn()
}

function createNewApplication() {
  completeApplicationStore.setCompleteApplication(defaultPermitState)
  completeApplicationStore.completeApplication.application.cost = {
    new: {
      standard: brandStore.brand.cost.new.standard,
      judicial: brandStore.brand.cost.new.judicial,
      reserve: brandStore.brand.cost.new.reserve,
    },
    renew: {
      standard: brandStore.brand.cost.renew.standard,
      judicial: brandStore.brand.cost.renew.judicial,
      reserve: brandStore.brand.cost.renew.reserve,
    },
    issuance: brandStore.brand.cost.issuance,
    modify: brandStore.brand.cost.modify,
    creditFee: brandStore.brand.cost.creditFee,
    convenienceFee: brandStore.brand.cost.convenienceFee,
  }
  completeApplicationStore.completeApplication.application.userEmail =
    authStore.auth.userEmail
  completeApplicationStore.completeApplication.id = window.crypto.randomUUID()
  completeApplicationStore.completeApplication.application.currentStep = 0
  createMutation()
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
