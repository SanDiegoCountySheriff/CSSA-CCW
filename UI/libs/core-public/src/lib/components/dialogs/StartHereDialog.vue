<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template #activator="{ attrs, on }">
      <v-btn
        v-on="on"
        v-bind="attrs"
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
    </template>

    <v-sheet
      rounded
      outlined
      color="primary"
      elevation="20"
      max-width="600"
    >
      <v-card
        :loading="isLoading"
        class="pb-4 px-4"
      >
        <v-card-title v-if="!isMobile">
          Let's find out more about you
          <v-spacer />
          <v-btn
            :disabled="isLoading"
            @click="handleStartOver"
            color="primary"
            outlined
          >
            <span
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Start Over
            </span>
          </v-btn>
        </v-card-title>

        <v-card-subtitle v-else>
          More about you
          <v-btn
            :disabled="isLoading"
            @click="handleStartOver"
            color="primary"
            class="ml-3"
            outlined
            small
          >
            <span
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Start Over
            </span>
          </v-btn>
        </v-card-subtitle>

        <v-card-text
          v-if="step === 1"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          <v-form
            ref="form"
            v-model="valid"
          >
            <v-row>
              <v-col>
                {{ brandStore.getBrand.agencyName }} CCW records have moved to
                CCW Pro.
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                To help determine your next step, please enter your state ID
                number and date of birth.
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                <v-text-field
                  v-model="idNumber"
                  :rules="[v => !!v || 'State ID Number is required.']"
                  :dense="isMobile"
                  label="State ID Number"
                  outlined
                />
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                <v-menu
                  v-model="menu"
                  :close-on-content-click="false"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
                >
                  <template #activator="{ on, attrs }">
                    <v-text-field
                      v-model="dateOfBirth"
                      :label="$t('Date of Birth')"
                      :rules="[v => !!v || $t('Date of birth is required')]"
                      :dense="isMobile"
                      outlined
                      prepend-inner-icon="mdi-calendar"
                      v-bind="attrs"
                      v-on="on"
                      readonly
                    ></v-text-field>
                  </template>

                  <v-date-picker
                    v-model="dateOfBirth"
                    @input="menu = false"
                    color="primary"
                    no-title
                    scrollable
                  >
                  </v-date-picker>
                </v-menu>
              </v-col>
            </v-row>

            <v-row class="text-center">
              <v-col>
                <v-btn
                  :disabled="!valid || isLoading"
                  @click="handleMatchUserInformation"
                  color="primary"
                  large
                >
                  Submit
                </v-btn>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-text
          v-if="step === 2"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          We found a match. Please enter more information on the following page
          to assist
          {{ brandStore.getBrand.agencyName }} licensing staff with finding your
          record.
        </v-card-text>

        <v-card-text v-if="step === 2">
          <v-row class="text-center">
            <v-col>
              <v-btn
                :disabled="!valid"
                @click="handleExistingApplication"
                color="primary"
                large
              >
                Continue
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 3"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          Do you have an upcoming CCW appointment with
          {{ brandStore.getBrand.agencyName }}?
        </v-card-text>

        <v-card-text v-if="step === 3">
          <v-row class="text-center">
            <v-col>
              <v-btn
                @click="step = 4"
                color="primary"
                large
              >
                Yes
              </v-btn>
            </v-col>

            <v-col>
              <v-btn
                @click="step = 5"
                color="primary"
                large
              >
                No
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 5"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          Do you need to renew or modify an existing CCW license with
          {{ brandStore.getBrand.agencyName }}?
        </v-card-text>

        <v-card-text v-if="step === 5">
          <v-row class="text-center">
            <v-col>
              <v-btn
                @click="step = 4"
                color="primary"
                large
              >
                Yes
              </v-btn>
            </v-col>

            <v-col>
              <v-btn
                @click="step = 7"
                color="primary"
                large
              >
                No
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 7"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          Have you completed a CCW appointment with
          {{ brandStore.getBrand.agencyName }}?
        </v-card-text>

        <v-card-text v-if="step === 7">
          <v-row class="text-center">
            <v-col>
              <v-btn
                @click="step = 4"
                color="primary"
                large
              >
                Yes
              </v-btn>
            </v-col>

            <v-col>
              <v-btn
                @click="step = 8"
                color="primary"
                large
              >
                No
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 8"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          Are you filling out a CCW application for the very first time with
          {{ brandStore.getBrand.agencyName }}?
        </v-card-text>

        <v-card-text v-if="step === 8">
          <v-row class="text-center">
            <v-col>
              <v-btn
                @click="step = 9"
                color="primary"
                large
              >
                Yes
              </v-btn>
            </v-col>

            <v-col>
              <v-btn
                @click="step = 10"
                color="primary"
                large
              >
                No
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 10"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          We can't determine your next step. Please start over and read the
          questions thoroughly.
        </v-card-text>

        <v-card-text v-if="step === 10">
          <v-row class="text-center">
            <v-col>
              <v-btn
                @click="handleStartOver"
                color="primary"
                large
              >
                Start Over
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 9"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          According to these answers, you have never filled out an application
          on this site, or any other site for
          {{ brandStore.getBrand.agencyName }}. If you continue you will be
          asked to fill out a new application.

          <v-alert
            type="warning"
            elevation="4"
            class="mt-3"
            border="left"
            outlined
          >
            <span
              :class="
                themeStore.getThemeConfig.isDark
                  ? 'text-h6 white--text'
                  : 'text-h6 black--text'
              "
            >
              If you have previously filled out an application on any other site
              or system and scheduled an appointment, you may lose your time
              slot and delay your application process.
            </span>
          </v-alert>

          <v-row>
            <v-col>
              <v-checkbox
                v-model="accepted"
                color="primary"
                label="I accept the above conditions"
              />
            </v-col>
          </v-row>

          <v-row class="text-center">
            <v-col>
              <v-btn
                :disabled="!accepted"
                @click="redirectToAcknowledgements"
                color="primary"
                large
              >
                Continue
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>

        <v-card-text
          v-if="step === 4"
          :class="
            themeStore.getThemeConfig.isDark
              ? 'text-h6 white--text'
              : 'text-h6 black--text'
          "
        >
          Please enter more information on the following page to assist
          {{ brandStore.getBrand.agencyName }} licensing staff with locating
          your record.
        </v-card-text>

        <v-card-text v-if="step === 4">
          <v-row class="text-center">
            <v-col>
              <v-btn
                :disabled="!valid"
                @click="handleExistingApplication"
                color="primary"
                large
              >
                Continue
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-sheet>
  </v-dialog>
</template>

<script setup lang="ts">
import Routes from '@core-public/router/routes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, ref } from 'vue'

const dialog = ref(false)
const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const themeStore = useThemeStore()
const brandStore = useBrandStore()
const step = ref(1)
const accepted = ref(false)
const valid = ref(false)
const idNumber = ref('')
const dateOfBirth = ref('')
const menu = ref(false)
const userInformationGood = ref(false)
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

const { isLoading, mutate } = useMutation({
  mutationFn: () =>
    applicationStore
      .matchUserInformation(idNumber.value, dateOfBirth.value)
      .then(res => {
        userInformationGood.value = res

        if (userInformationGood.value) {
          step.value = 2
        } else {
          step.value = 3
        }
      }),
})

function redirectToAcknowledgements() {
  router.push({
    name: 'Application',
    params: { informationOnly: 'false' },
  })
}

function handleExistingApplication() {
  router.push({
    path: Routes.EXISTING_APPLICATION_PATH,
  })
}

function handleMatchUserInformation() {
  mutate()
}

function handleStartOver() {
  idNumber.value = ''
  dateOfBirth.value = ''
  userInformationGood.value = false
  step.value = 1
}
</script>
