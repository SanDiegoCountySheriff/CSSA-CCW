<template>
  <v-container v-if="isFetching">
    <Loader />
  </v-container>

  <v-container
    v-else
    :fill-height="doesAgencyHomePageImageExist"
    fluid
  >
    <v-card flat>
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
            :src="brandStore.getDocuments.agencyLogo"
            :max-height="maxHeight"
            contain
          />
        </v-col>

        <v-col
          cols="12"
          class="text-center"
        >
          <div :class="$vuetify.breakpoint.lgAndUp ? 'text-h3' : 'text-h5'">
            <template color="primary">
              {{ brandStore.getBrand.agencyName }} CCW System
            </template>
          </div>
        </v-col>
      </v-row>

      <v-row justify="center">
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
            @click="showDialog = true"
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
                <v-col v-if="userStore.getUserState.isPendingReview">
                  {{ $t('Under Review') }}
                </v-col>
                <v-col v-else-if="!userStore.getUserState.isPendingReview">
                  {{ $t('Start Here') }}
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

        <v-dialog
          v-model="showDialog"
          max-width="600px"
        >
          <v-card>
            <v-card-title class="text-h4 justify-center">
              Before You Begin
            </v-card-title>
            <v-card-title class="text-h5 justify-center">
              Let us know how we can assist you
            </v-card-title>
            <v-container
              class="px-10"
              fluid
            >
              <v-contianer class="px-5">
                <v-sheet
                  outlined
                  color="primary"
                  rounded
                  elevation="3"
                >
                  <v-card
                    hover
                    rounded
                    outlined
                    @click="redirectToAcknowledgements"
                  >
                    <v-card-title
                      :color="$vuetify.theme.dark ? 'white' : 'primary'"
                    >
                      <v-icon
                        x-large
                        class="mr-3"
                        :color="$vuetify.theme.dark ? 'white' : 'primary'"
                      >
                        mdi-account-plus
                      </v-icon>
                      New User
                    </v-card-title>

                    <v-card-text>
                      I am applying for a CCW license with
                      <b>{{ brandStore.getBrand.agencyName }}</b> for the first
                      time.
                    </v-card-text>
                  </v-card>
                </v-sheet>
              </v-contianer>

              <v-sheet
                outlined
                color="primary"
                rounded
                elevation="3"
              >
                <v-card
                  hover
                  rounded
                  outlined
                  @click="handleExistingApplication"
                >
                  <v-card-title
                    :color="$vuetify.theme.dark ? 'white' : 'primary'"
                  >
                    <v-icon
                      x-large
                      class="mr-3"
                      :color="$vuetify.theme.dark ? 'white' : 'primary'"
                    >
                      mdi-account-search
                    </v-icon>
                    Link Existing Application
                  </v-card-title>

                  <v-card-text>
                    I have previously applied for a CCW license with
                    <b>{{ brandStore.getBrand.agencyName }}</b> and would like
                    to link my existing application.
                  </v-card-text>
                </v-card>
              </v-sheet>
            </v-container>

            <v-card-actions class="d-flex flex-column align-center">
              <v-container
                class="px-0"
                fluid
              >
                <v-row justify="center">
                  <v-col
                    cols="12"
                    sm="8"
                    md="6"
                  >
                    <v-btn
                      color="primary"
                      @click="showDialog = false"
                      block
                    >
                      Cancel
                    </v-btn>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-actions>
          </v-card>
        </v-dialog>

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
          lg="2"
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
    </v-card>
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
import { useUserStore } from '@shared-ui/stores/userStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useQuery } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { computed, inject, onBeforeUnmount, onMounted, ref } from 'vue'

const brandStore = useBrandStore()
const authStore = useAuthStore()
const userStore = useUserStore()
const user = computed(() => userStore.userProfile)
const router = useRouter()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)
const completeApplicationStore = useCompleteApplicationStore()
const canGetAllUserApplications = computed(() => {
  return authStore.getAuthState.isAuthenticated
})
const innerHeight = ref(0)
const showDialog = ref(false)

onMounted(() => {
  calculateInnerHeight()

  window.addEventListener('resize', calculateInnerHeight)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', calculateInnerHeight)
})

function islinked() {
  if (user.value.isPendingReview) {
    window.console.log('user is under review')
  } else {
    window.console.log('user does not have active apps ')
  }
}

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
    name: 'moreinformation',
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

const maxHeight = computed(() => {
  const percentageOfViewportHeight = 0.35
  const heightInPixels = innerHeight.value * percentageOfViewportHeight
  const fixedHeight = Math.round(heightInPixels)

  return fixedHeight
})

function calculateInnerHeight() {
  innerHeight.value = window.innerHeight
}

function handleExistingApplication() {
  router.push({
    path: Routes.EXISTING_APPLICATION_PATH,
  })
}
</script>
