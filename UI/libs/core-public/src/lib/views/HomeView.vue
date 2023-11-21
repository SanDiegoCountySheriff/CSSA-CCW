<template>
  <v-container v-if="isFetching">
    <Loader />
  </v-container>

  <v-container
    v-else
    :fill-height="doesAgencyHomePageImageExist"
  >
    <v-row
      no-gutters
      justify="center"
    >
      <v-col
        cols="12"
        :lg="doesAgencyHomePageImageExist ? 5 : 12"
      >
        <v-img
          :class="
            $vuetify.breakpoint.lgAndUp && doesAgencyHomePageImageExist
              ? ''
              : 'mx-auto'
          "
          alt="Application logo"
          :src="brandStore.getDocuments.agencyLandingPageImage"
          max-width="500"
        />
      </v-col>

      <v-col
        cols="6"
        lg="2"
        class="text-center"
      >
        <v-btn
          v-if="authStore.getAuthState.isAuthenticated && data?.length > 0"
          @click="viewApplication"
          :color="$vuetify.theme.dark ? 'white' : 'primary'"
          text
          :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
          :x-large="$vuetify.breakpoint.lgAndUp"
          :small="$vuetify.breakpoint.smAndDown"
        >
          <v-container>
            <v-row>
              <v-col>
                <v-icon x-large> mdi-card-account-details-outline </v-icon>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                {{ $t('View Application') }}
              </v-col>
            </v-row>
          </v-container>
        </v-btn>

        <v-btn
          v-else-if="
            authStore.getAuthState.isAuthenticated && data?.length === 0
          "
          @click="redirectToAcknowledgements"
          :color="$vuetify.theme.dark ? 'white' : 'primary'"
          text
          :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
          :x-large="$vuetify.breakpoint.lgAndUp"
          :small="$vuetify.breakpoint.smAndDown"
        >
          <v-container>
            <v-row>
              <v-col>
                <v-icon x-large> mdi-file-star-outline </v-icon>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                {{ $t('Create Application') }}
              </v-col>
            </v-row>
          </v-container>
        </v-btn>

        <v-btn
          v-else
          @click="handleLogIn"
          :color="$vuetify.theme.dark ? 'white' : 'primary'"
          text
          :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
          :x-large="$vuetify.breakpoint.lgAndUp"
          :small="$vuetify.breakpoint.smAndDown"
        >
          <v-container>
            <v-row>
              <v-col>
                <v-icon x-large>mdi-login </v-icon>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                {{ $t('Login or Sign-up') }}
              </v-col>
            </v-row>
          </v-container>
        </v-btn>
      </v-col>

      <v-col
        cols="6"
        lg="2"
        class="text-center"
      >
        <v-btn
          @click="redirectToMoreInformation"
          :color="$vuetify.theme.dark ? 'white' : 'primary'"
          :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
          :x-large="$vuetify.breakpoint.lgAndUp"
          :small="$vuetify.breakpoint.smAndDown"
          text
        >
          <v-container>
            <v-row>
              <v-col>
                <v-icon x-large> mdi-information-box-outline </v-icon>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                {{ $t('Information') }}
              </v-col>
            </v-row>
          </v-container>
        </v-btn>
      </v-col>

      <v-col
        cols="6"
        lg="2"
        class="text-center"
      >
        <PriceInfoDialog />
      </v-col>

      <v-col
        cols="6"
        lg="1"
        class="text-center"
      >
        <ContactDialog />
      </v-col>
    </v-row>

    <v-row
      justify="center"
      v-if="doesAgencyHomePageImageExist"
    >
      <v-col cols="10">
        <v-carousel
          :show-arrows="false"
          cycle
          hide-delimiters
        >
          <v-carousel-item
            v-for="(item, i) in items"
            :key="i"
            :src="item.src"
          ></v-carousel-item>
        </v-carousel>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import ContactDialog from '@core-public/components/dialogs/ContactDialog.vue'
import Loader from '@core-public/views/Loader.vue'
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import PriceInfoDialog from '@core-public/components/dialogs/PriceInfoDialog.vue'
import Routes from '@core-public/router/routes'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useQuery } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { computed, inject, ref } from 'vue'

const brandStore = useBrandStore()
const authStore = useAuthStore()
const router = useRouter()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)
const completeApplicationStore = useCompleteApplicationStore()
const canGetAllUserApplications = computed(() => {
  return authStore.getAuthState.isAuthenticated
})

const items = computed(() => [
  {
    src: brandStore.getDocuments.agencyHomePageImage,
  },
])

const doesAgencyHomePageImageExist = computed(() => {
  return Boolean(brandStore.getDocuments.agencyHomePageImage)
})

const { data, isFetching } = useQuery(
  ['getApplicationsByUser'],
  completeApplicationStore.getAllUserApplicationsApi,
  {
    refetchOnMount: 'always',
    enabled: canGetAllUserApplications,
  }
)

function handleLogIn() {
  msalInstance.value.logIn()
}

function redirectToAcknowledgements() {
  router.push({
    name: 'Application',
    params: { informationOnly: 'false' },
  })
}

function redirectToMoreInformation() {
  router.push({
    name: 'Application',
    params: { informationOnly: 'true' },
  })
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
