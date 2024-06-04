<template>
  <v-container
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
              {{ brandStore.getBrand.agencyName }} CCW Pro
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
            v-if="
              authStore.getAuthState.isAuthenticated &&
              completeApplicationStore.completeApplication.id
            "
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
              authStore.getAuthState.isAuthenticated &&
              !completeApplicationStore.completeApplication.id &&
              !userStore.getUserState.isPendingReview
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
                <v-col>
                  {{ $t('Start Here') }}
                </v-col>
              </v-row>
            </v-container>
          </v-btn>
          <v-btn
            v-else-if="userStore.getUserState.isPendingReview"
            @click="showStatus = true"
            :color="$vuetify.theme.dark ? 'white' : 'primary'"
            text
            :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
            :x-large="$vuetify.breakpoint.lgAndUp"
            :small="$vuetify.breakpoint.smAndDown"
          >
            <v-container>
              <v-row>
                <v-col>
                  <v-icon x-large> mdi-account-clock </v-icon>
                </v-col>
              </v-row>

              <v-row>
                <v-col>
                  {{ $t('Pending Review') }}
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
          v-model="showStatus"
          max-width="600px"
        >
          <v-card>
            <v-card-title class="text-h4 justify-center">
              We have received your request
            </v-card-title>

            <v-card-title class="text-h5 justify-center">
              Application linking request is currently under review
              <v-card-subtitle class="text-5 justify-center">
                This process can take some time, please check back soon!
              </v-card-subtitle>
            </v-card-title>

            <v-card-actions>
              <v-spacer />

              <v-btn
                @click="showStatus = false"
                color="primary"
              >
                Dismiss
              </v-btn>

              <v-spacer />
            </v-card-actions>
          </v-card>
        </v-dialog>

        <v-dialog
          v-model="showDialog"
          max-width="900px"
        >
          <v-card>
            <v-card-title
              v-if="!isMobile"
              class="text-h3 justify-center"
            >
              Let's find out more about you
            </v-card-title>

            <v-card-title
              v-else
              class="text-h5"
            >
              Let's find out more about you
            </v-card-title>

            <v-card-title :class="isMobile ? 'text-h6' : 'text-h5'">
              <template v-if="step === 1">
                <v-row class="mb-10">
                  <v-col>
                    {{ brandStore.getBrand.agencyName }} CCW records have moved
                    to this new program. Help us find your record by answering
                    these questions. It is very important to read the questions
                    thoroughly before answering
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 2"
                      color="primary"
                      x-large
                      outlined
                    >
                      Get Started
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 2">
                <v-row class="mb-10">
                  <v-col>
                    Have you ever attempted to get a Concealed Carry Weapon's
                    license with {{ brandStore.getBrand.agencyName }}?
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 3"
                      color="primary"
                      x-large
                      outlined
                    >
                      Yes
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="step = 4"
                      color="primary"
                      x-large
                      outlined
                    >
                      No
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 3">
                <v-row class="mb-10">
                  <v-col>
                    Did you obtain the Concealed Carry Weapon's license with
                    {{ brandStore.getBrand.agencyName }}?
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 5"
                      color="primary"
                      x-large
                      outlined
                    >
                      Yes
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="step = 6"
                      color="primary"
                      x-large
                      outlined
                    >
                      No
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 6">
                <v-row class="mb-10">
                  <v-col>
                    Do you have an upcoming appointment with
                    {{ brandStore.getBrand.agencyName }}?
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 7"
                      color="primary"
                      x-large
                      outlined
                    >
                      Yes
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="step = 8"
                      color="primary"
                      x-large
                      outlined
                    >
                      No
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 8">
                <v-row class="mb-10">
                  <v-col>
                    Have you started the Concealed Carry Weapon's license
                    application with
                    {{ brandStore.getBrand.agencyName }}?
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 9"
                      color="primary"
                      x-large
                      outlined
                    >
                      Yes
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="step = 10"
                      color="primary"
                      x-large
                      outlined
                    >
                      No
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 5 || step === 7 || step === 9">
                <v-row>
                  <v-col>
                    According to these answers you have an existing CCW license,
                    or an existing CCW license application with
                    {{ brandStore.getBrand.agencyName }}. Acknowledging will
                    lead you to the next step.
                  </v-col>
                </v-row>

                <v-row class="mb-10">
                  <v-col>
                    On this next step you will be asked to enter information
                    about yourself and a picture of your state-issued ID. If you
                    have an existing CCW Permit you will be asked to upload
                    information about that permit as well. Please only do this
                    if you have an existing application or permit, otherwise
                    your application process will be greatly delayed.
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 1"
                      color="primary"
                      x-large
                      outlined
                    >
                      Go Back
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="handleExistingApplication"
                      color="primary"
                      x-large
                      outlined
                    >
                      Acknowledge
                    </v-btn>
                  </v-col>
                </v-row>
              </template>

              <template v-if="step === 10 || step === 4">
                <v-row>
                  <v-col>
                    According to these answers you are required to start a new
                    application. Only acknowledge this if you have never filled
                    out an application with
                    {{ brandStore.getBrand.agencyName }} before.
                  </v-col>
                </v-row>

                <v-row class="mb-10">
                  <v-col>
                    If you have previously scheduled an appointment it will be
                    released and you will lose your spot. If you have previously
                    applied with {{ brandStore.getBrand.agencyName }} your
                    application will be withdrawn.
                  </v-col>
                </v-row>

                <v-row class="text-center">
                  <v-col>
                    <v-btn
                      @click="step = 1"
                      color="primary"
                      x-large
                      outlined
                    >
                      Go Back
                    </v-btn>
                  </v-col>

                  <v-col>
                    <v-btn
                      @click="redirectToAcknowledgements"
                      color="primary"
                      x-large
                      outlined
                    >
                      Acknowledge
                    </v-btn>
                  </v-col>
                </v-row>
              </template>
            </v-card-title>

            <v-card-text></v-card-text>
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
import ContactDialog from '@core-public/components/dialogs/ContactDialog.vue'
import { MsalBrowser } from '@shared-ui/api/auth/authentication'
import PriceInfoDialog from '@core-public/components/dialogs/PriceInfoDialog.vue'
import Routes from '@core-public/router/routes'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { useUserStore } from '@shared-ui/stores/userStore'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, inject, onBeforeUnmount, onMounted, ref } from 'vue'

const brandStore = useBrandStore()
const authStore = useAuthStore()
const userStore = useUserStore()
const router = useRouter()
const msalInstance = ref(inject('msalInstance') as MsalBrowser)
const completeApplicationStore = useCompleteApplicationStore()
const innerHeight = ref(0)
const showDialog = ref(false)
const showStatus = ref(false)
const step = ref(1)
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

onMounted(() => {
  calculateInnerHeight()

  window.addEventListener('resize', calculateInnerHeight)
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', calculateInnerHeight)
})

const items = computed(() => [
  {
    src: brandStore.getDocuments.agencyHomePageImage,
  },
])

const doesAgencyHomePageImageExist = computed(() => {
  return Boolean(brandStore.getDocuments.agencyHomePageImage)
})

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

<style>
[class*='text'] {
  white-space: normal;
}

.v-card__title {
  word-break: normal;
}
</style>
