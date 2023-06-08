<template>
  <v-container>
    <v-row>
      <v-col>
        <v-card
          height="60"
          outlined
        >
          <v-card-title>
            <v-row>
              <v-col>
                Order ID:
                {{ applicationStore.completeApplication.application.orderId }}
              </v-col>
              <v-col>
                Application Type:
                {{
                  capitalize(
                    applicationStore.completeApplication.application
                      .applicationType
                  )
                }}
              </v-col>
              <v-col>
                <v-progress-linear
                  color="primary"
                  height="20"
                  value="10"
                  striped
                >
                  <template #default>
                    {{
                      state.applicationStatuses.find(
                        s =>
                          s.value ===
                          applicationStore.completeApplication.application
                            .status
                      )?.text
                    }}
                  </template>
                </v-progress-linear>
              </v-col>
            </v-row>
          </v-card-title>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        md="4"
      >
        <v-card
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            Customer Information
          </v-card-title>

          <v-card-text>
            <v-banner
              icon="mdi-account"
              icon-color="primary"
            >
              Name:
              {{
                applicationStore.completeApplication.application.personalInfo
                  .firstName
              }}
              {{
                applicationStore.completeApplication.application.personalInfo
                  .lastName
              }}
            </v-banner>
            <v-banner
              icon="mdi-cake-variant"
              icon-color="primary"
            >
              Date Of Birth:
              {{
                new Date(
                  applicationStore.completeApplication.application.dob.birthDate
                ).toLocaleDateString()
              }}
            </v-banner>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
      >
        <v-card
          outlined
          class="fill-height"
        >
          <v-card-title class="justify-center"> Status </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <v-banner
                  class="text-center"
                  style="font-size: 18px"
                >
                  {{
                    state.applicationStatuses.find(
                      s =>
                        s.value ===
                        applicationStore.completeApplication.application.status
                    )?.text
                  }}
                </v-banner>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    1
                  "
                  @click="handleContinueApplication"
                >
                  Continue
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="
                    applicationStore.completeApplication.application.status ==
                    13
                  "
                  @click="handleWithdrawApplication()"
                >
                  Withdraw
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  @click="handleRenewApplication()"
                >
                  Renew
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  @click="handleModifyApplication()"
                >
                  Modify
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="4"
      >
        <v-card
          outlined
          class="fill-height"
        >
          <v-card-title class="justify-center">
            Appointment Information
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <v-banner
                  icon="mdi-calendar-range"
                  icon-color="primary"
                >
                  Appointment Date: <br />
                  {{
                    new Date(
                      applicationStore.completeApplication.application.appointmentDateTime
                    ).toLocaleString()
                  }}
                  <template #actions>
                    <v-btn
                      small
                      color="primary"
                      >Reschedule</v-btn
                    >
                  </template>
                </v-banner>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-card
          class="fill-height"
          outlined
        >
          <v-tabs
            v-model="tab"
            grow
          >
            <v-tab> Personal Info </v-tab>
            <v-tab> ID Info </v-tab>
            <v-tab> Address </v-tab>
            <v-tab> Appearance </v-tab>
            <v-tab> Employment & Weapons </v-tab>
            <v-tab> Qualifying Questions </v-tab>
            <v-tab> Uploaded Documents</v-tab>
          </v-tabs>
          <v-tabs-items v-model="tab">
            <v-tab-item>
              <PersonalInfoSection
                :color="'primary'"
                :personal-info="
                  applicationStore.completeApplication.application.personalInfo
                "
              />
              <SpouseInfoSection
                v-if="
                  applicationStore.completeApplication.application.personalInfo
                    .maritalStatus == 'Married'
                "
                :color="'primary'"
                :spouse-info="
                  applicationStore.completeApplication.application
                    .spouseInformation
                "
              />
              <DOBinfoSection
                :color="'primary'"
                :DOBInfo="applicationStore.completeApplication.application.dob"
              />
              <ContactInfoSection
                :color="'primary'"
                :contact-info="
                  applicationStore.completeApplication.application.contact
                "
              />
              <CitizenInfoSection
                :color="'primary'"
                :citizenship-info="
                  applicationStore.completeApplication.application.citizenship
                "
                :immigrant-info="
                  applicationStore.completeApplication.application
                    .immigrantInformation
                "
              />
            </v-tab-item>
            <v-tab-item>
              <IdInfoSection
                :color="'primary'"
                :id-info="
                  applicationStore.completeApplication.application.idInfo
                "
              />
            </v-tab-item>
            <v-tab-item>
              <AddressInfoSection
                :color="'primary'"
                :address-info="
                  applicationStore.completeApplication.application
                    .currentAddress
                "
                :title="'Current Address'"
              />
              <AddressInfoSection
                v-if="
                  applicationStore.completeApplication.application
                    .differentMailing
                "
                :color="'primary'"
                :address-info="
                  applicationStore.completeApplication.application
                    .mailingAddress
                "
                :title="'Mailing Address'"
              />
              <SpouseAddressInfoSection
                v-if="
                  applicationStore.completeApplication.application
                    .differentSpouseAddress
                "
                :title="'Spouse Address'"
                :color="'primary'"
                :spouse-address="
                  applicationStore.completeApplication.application
                    .spouseAddressInformation
                "
              />
              <PreviousAddressInfoSection
                :color="'primary'"
                :previous-address="
                  applicationStore.completeApplication.application
                    .previousAddresses
                "
              />
            </v-tab-item>
            <v-tab-item>
              <AppearanceInfoSection
                :color="'primary'"
                :appearance-info="
                  applicationStore.completeApplication.application
                    .physicalAppearance
                "
              />
            </v-tab-item>
            <v-tab-item>
              <EmploymentInfoSection
                :color="'primary'"
                :employment-info="
                  applicationStore.completeApplication.application.employment
                "
                :work-information="
                  applicationStore.completeApplication.application
                    .workInformation
                "
              />
              <WeaponsInfoSection
                :weapons="
                  applicationStore.completeApplication.application.weapons
                "
              />
            </v-tab-item>
            <v-tab-item>
              <QualifyingQuestionsInfoSection
                :color="'primary'"
                :qualifying-questions-info="
                  applicationStore.completeApplication.application
                    .qualifyingQuestions
                "
              />
            </v-tab-item>
            <v-tab-item>
              <FileUploadInfoSection
                :color="'primary'"
                :uploaded-documents="
                  applicationStore.completeApplication.application
                    .uploadedDocuments
                "
              />
            </v-tab-item>
          </v-tabs-items>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
  <!-- <div class="ml-5">
    <v-row class="mt-5">
      <v-col
        cols="12"
        lg="9"
      >
        <v-card>
          <ApplicationTable
            :headers="state.headers"
            :items="state.application"
            :is-loading="!!state.application"
          />
        </v-card>
      </v-col>
      <v-col
        cols="12"
        lg="3"
      >
        <v-card class="mr-5">
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="primary"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    1
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleContinueApplication"
                >
                  {{ $t('Continue Application') }}
                </v-btn>
              </template>
              <span>{{
                $t(' You can only continue incomplete applications.')
              }}</span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="primary"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleModifyApplication"
                >
                  {{ $t('Modify Application') }}
                </v-btn>
              </template>
              <span>
                {{
                  $t(` With modify make sure to change anything that need to be changed. Then make sure to
                    check the correct application type in step 7 `)
                }}
              </span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="primary"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleRenewApplication"
                >
                  {{ $t('Renew Application') }}
                </v-btn>
              </template>
              <span>
                {{
                  $t(` With a Renewal Application make sure to change anything that needs to be changed. Then
                  make sure to check the correct application type in step 7 `)
                }}
              </span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="error"
                  v-bind="attrs"
                  v-on="on"
                  @click="router.back()"
                >
                  {{ $t('Go Back') }}
                </v-btn>
              </template>
              <span>
                {{ $t('Go back to application list') }}
              </span>
            </v-tooltip>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div> -->
</template>

<script setup lang="ts">
import ApplicationTable from '@core-public/components/tables/ApplicationTable.vue'
import Routes from '@core-public/router/routes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { computed, getCurrentInstance, onMounted, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router/composables'
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue'
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue'
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue'
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue'
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue'
import WeaponsInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue'
import ApplicationTypeInfoSection from '@shared-ui/components/info-sections/ApplicationTypeInfoSection.vue'
import QualifyingQuestionsInfoSection from '@shared-ui/components/info-sections/QualifyingQuestionsInfoSection.vue'
import FileUploadInfoSection from '@shared-ui/components/info-sections/FileUploadInfoSection.vue'
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue'
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue'
import SpouseAddressInfoSection from '@shared-ui/components/info-sections/SpouseAddressInfoSection.vue'
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue'
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'

const applicationStore = useCompleteApplicationStore()
const router = useRouter()
const route = useRoute()
const app = getCurrentInstance()
const tab = ref(null)

const state = reactive({
  application: [applicationStore.completeApplication],
  applicationStatuses: [
    { value: 1, text: 'Started' },
    {
      value: 2,
      text: 'Submitted',
    },
    {
      value: 3,
      text: 'In Progress',
    },
    {
      value: 4,
      text: 'Canceled',
    },
    {
      value: 5,
      text: 'Returned',
    },
    {
      value: 6,
      text: 'Completed',
    },
    {
      value: 13,
      text: 'Withdrawn',
    },
  ],
  headers: [
    {
      text: 'ORDER ID',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    {
      text: 'COMPLETED',
      value: 'completed',
    },
    {
      text: 'CURRENT STATUS',
      value: 'status',
    },
    {
      text: 'APPOINTMENT STATUS',
      value: 'appointmentStatus',
    },
    {
      text: 'APPOINTMENT DATE',
      value: 'appointmentDate',
    },
    {
      text: 'CURRENT STEP',
      value: 'step',
    },
    {
      text: 'APPLICATION TYPE',
      value: 'type',
    },
  ],
})

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res)
      })
  }
})

const getTextColor = computed(() => {
  return app?.proxy.$vuetify.theme.dark ? 'white' : 'black'
})

const canApplicationBeContinued = computed(() => {
  return (
    applicationStore.completeApplication.application.status !== 2 &&
    applicationStore.completeApplication.application.status !== 4 &&
    applicationStore.completeApplication.application.status !== 5 &&
    applicationStore.completeApplication.application.status !== 6
  )
})

const createMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  },
  onError: () => null,
})

const renewMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  },
  onError: () => null,
})

function handleContinueApplication() {
  if (applicationStore.completeApplication.application.status === 0) {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  } else if (applicationStore.completeApplication.application.status === 1) {
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  } else {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  }
}

function handleModifyApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID()
  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.isComplete = false
  applicationStore.completeApplication.application.appointmentStatus = false
  applicationStore.completeApplication.application.status = 1
  applicationStore.completeApplication.application.applicationType = `modify-${applicationStore.completeApplication.application.applicationType}`
  renewMutation.mutate()
}

function handleRenewApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID()
  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.isComplete = false
  applicationStore.completeApplication.application.appointmentStatus = false
  applicationStore.completeApplication.application.status = 1
  applicationStore.completeApplication.application.applicationType = `renew-${applicationStore.completeApplication.application.applicationType}`
  createMutation.mutate()
}

function handleWithdrawApplication() {
  applicationStore.completeApplication.application.status = 13
  applicationStore.completeApplication.application.appointmentDateTime = ''
}
</script>
