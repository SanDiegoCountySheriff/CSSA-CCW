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
                  icon="mdi-list-status"
                  icon-color="primary"
                >
                  Status:
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
                >
                  Continue
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  block
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
                >
                  Renew
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  block
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
          <v-tabs v-model="tab">
            <v-tab> Personal Info </v-tab>
            <v-tab> Info Page 2 </v-tab>
            <v-tab> Info Page 3 </v-tab>
          </v-tabs>
          <v-tabs-items v-model="tab">
            <v-tab-item>
              <PersonalInfoSection
                :color="'primary'"
                :personal-info="
                  applicationStore.completeApplication.application.personalInfo
                "
              />
            </v-tab-item>
            <v-tab-item> Page 2 </v-tab-item>
            <v-tab-item> Page 3 </v-tab-item>
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
  if (applicationStore.completeApplication.application.currentStep === 0) {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete,
      },
    })
  } else if (
    applicationStore.completeApplication.application.applicationType ===
      'standard' ||
    applicationStore.completeApplication.application.applicationType ===
      'judicial' ||
    applicationStore.completeApplication.application.applicationType ===
      'reserve'
  ) {
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
</script>
