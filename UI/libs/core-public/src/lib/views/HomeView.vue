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
          min-height="600px"
        >
          <v-card>
            <v-card-title class="text-h3 justify-center">
              Let's find out more about you
            </v-card-title>

            <v-card-title class="text-h5">
              {{ brandStore.getBrand.agencyName }} CCW records have moved to
              this new program. Help us find your record by answering these
              questions.
            </v-card-title>

            <v-card-text>
              <ul>
                <li>
                  Have you ever attempted to get a Concealed Carry Weapon's
                  license with {{ brandStore.getBrand.agencyName }}?
                  <ul>
                    <li>
                      yes - did you obtain the license
                      <ul>
                        <li>yes - match</li>
                        <li>
                          no - do you have an upcoming appointment scheduled?
                          <ul>
                            <li>yes - match</li>
                            <li>
                              no - have you started the application process?
                              <ul>
                                <li>yes - match</li>
                                <li>no - new (same 'new' admonishment)</li>
                              </ul>
                            </li>
                          </ul>
                        </li>
                      </ul>
                    </li>
                    <li>
                      no - If you have previously scheduled an appointment it
                      will be released and you will lose your spot. If you have
                      previously applied with (SDSHERIFF) your application will
                      be withdrawn.
                      <ul>
                        <li>Acknowledge - new</li>
                        <li>Back - start over</li>
                      </ul>
                    </li>
                  </ul>
                </li>
              </ul>
            </v-card-text>

            <!-- <v-card-title class="text-h5 justify-center">
              Let us know which applies to you
            </v-card-title>

            <v-container
              class="px-10"
              fluid
            >
              <v-sheet
                outlined
                color="primary"
                rounded
                elevation="3"
                class="mb-5"
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
                    Help me find my appointment
                  </v-card-title>

                  <v-card-text>
                    I have an upcoming appointment with
                    <b>{{ brandStore.getBrand.agencyName }}</b>
                    Please help me find it.
                  </v-card-text>
                </v-card>
              </v-sheet>

              <v-sheet
                outlined
                color="primary"
                rounded
                elevation="3"
                class="mb-5"
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
                    Help me find my license
                  </v-card-title>

                  <v-card-text>
                    I have filled out a CCW application in another system for
                    <b>{{ brandStore.getBrand.agencyName }}</b
                    >, or hold a CCW license with
                    <b>{{ brandStore.getBrand.agencyName }}</b> and would like
                    to link my existing application or license.
                  </v-card-text>
                </v-card>
              </v-sheet>

              <v-sheet
                outlined
                color="primary"
                rounded
                elevation="3"
                class="mb-5"
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
                    I have never filled out a CCW application
                  </v-card-title>

                  <v-card-text>
                    I am applying for a CCW license with
                    <b>{{ brandStore.getBrand.agencyName }}</b> for the first
                    time. I have <b>never</b> filled out an application in any
                    previous system for
                    <b>{{ brandStore.getBrand.agencyName }}</b>
                    .
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
            </v-card-actions> -->
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
