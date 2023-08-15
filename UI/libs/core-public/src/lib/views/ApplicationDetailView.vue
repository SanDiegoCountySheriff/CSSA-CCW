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

              <v-col class="text-center">
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
                      ApplicationStatus[
                        applicationStore.completeApplication.application.status
                      ]
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

          <v-divider></v-divider>

          <v-card-title>
            <v-icon
              color="primary"
              class="mr-2"
            >
              mdi-account
            </v-icon>
            Name:
            {{
              applicationStore.completeApplication.application.personalInfo
                .firstName
            }}
            {{
              applicationStore.completeApplication.application.personalInfo
                .lastName
            }}
          </v-card-title>

          <v-card-title>
            <v-icon
              color="primary"
              class="mr-2"
            >
              mdi-cake-variant
            </v-icon>
            Date Of Birth:
            {{
              new Date(
                applicationStore.completeApplication.application.dob.birthDate
              ).toLocaleDateString()
            }}
          </v-card-title>
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
            <template
              v-if="
                applicationStore.completeApplication.application
                  .flaggedForCustomerReview
              "
            >
              <v-btn
                color="error"
                medium
                @click="showReviewDialog"
              >
                <v-icon left> mdi-alert-circle-outline </v-icon>
                Additional Information Required
              </v-btn>

              <v-dialog
                v-model="reviewDialog"
                max-width="800"
              >
                <v-card>
                  <v-card-title
                    class="headline"
                    style="background-color: #bdbdbd"
                  >
                    <v-icon
                      large
                      class="mr-3"
                    >
                      mdi-information-outline
                    </v-icon>
                    {{ flaggedQuestionHeader }}
                  </v-card-title>
                  <v-card-text>
                    <div
                      class="text-h6 font-weight-bold dark-grey--text mt-5 mb-5"
                    >
                      Incorrect information has been discovered in one or more
                      of your qualifying questions. Please review the revised
                      information
                    </div>
                    <v-textarea
                      v-if="flaggedQuestionText"
                      class="mt-7"
                      outlined
                      rows="6"
                      auto-grow
                      :value="flaggedQuestionText"
                      readonly
                      style="font-size: 18px"
                    ></v-textarea>
                  </v-card-text>
                  <v-card-actions>
                    <v-btn
                      elevation="2"
                      color="error"
                      @click="cancelChanges"
                    >
                      Cancel
                    </v-btn>
                    <v-spacer></v-spacer>
                    <v-btn
                      color="primary"
                      @click="acceptChanges"
                      class="white--text"
                    >
                      Accept
                    </v-btn>
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </template>
            <template
              v-else-if="
                applicationStore.completeApplication.application
                  .flaggedForLicensingReview
              "
            >
              <div>Status: Under Review</div>
            </template>
            <template v-else>
              Status:
              {{
                ApplicationStatus[
                  applicationStore.completeApplication.application.status
                ]
              }}
            </template>
          </v-card-title>

          <v-divider></v-divider>

          <v-card-text>
            <v-row>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="!canApplicationBeContinued"
                  @click="handleContinueApplication"
                >
                  Continue
                </v-btn>
              </v-col>

              <v-col>
                <v-btn
                  v-if="
                    applicationStore.completeApplication.application.status !==
                      ApplicationStatus.Withdrawn &&
                    applicationStore.completeApplication.application.status !==
                      ApplicationStatus.Incomplete
                  "
                  @click="handleShowWithdrawDialog"
                  color="primary"
                  block
                >
                  Withdraw
                </v-btn>

                <v-btn
                  v-else-if="
                    applicationStore.completeApplication.application.status ===
                      ApplicationStatus.Withdrawn ||
                    applicationStore.completeApplication.application.status ===
                      ApplicationStatus.Incomplete
                  "
                  color="primary"
                  block
                  @click="handleSubmit"
                >
                  Submit
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
                    ApplicationStatus['Contingently Approved']
                  "
                  @click="handleRenewApplication"
                >
                  Renew
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  color="primary"
                  block
                  :disabled="!canApplicationBeModified"
                  @click="handleModifyApplication"
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
          :loading="isLoading"
          outlined
          class="fill-height"
        >
          <v-card-title
            v-if="
              applicationStore.completeApplication.application
                .appointmentDateTime
            "
            class="justify-center"
          >
            Appointment Date:
            {{
              new Date(
                applicationStore.completeApplication.application.appointmentDateTime
              ).toLocaleString([], {
                year: 'numeric',
                month: 'numeric',
                day: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
              })
            }}
          </v-card-title>

          <v-card-title
            v-else
            class="justify-center"
          >
            Appointment Date: Not Scheduled
          </v-card-title>

          <v-divider></v-divider>

          <v-card-text>
            <v-row>
              <v-col>
                <v-btn
                  v-if="canRescheduleAppointment"
                  @click="handleShowAppointmentDialog"
                  block
                  color="primary"
                >
                  Reschedule
                </v-btn>

                <v-btn
                  v-else-if="canScheduleAppointment"
                  @click="handleShowAppointmentDialogSchedule"
                  block
                  color="primary"
                >
                  Schedule
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  block
                  color="primary"
                  @click="handleCancelAppointment"
                  :disabled="!canCancelAppointment"
                >
                  Cancel
                </v-btn>
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
          min-height="50vh"
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
                :birth-info="
                  applicationStore.completeApplication.application.dob
                "
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
                v-if="
                  applicationStore.completeApplication.application
                    .previousAddresses.length > 0
                "
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

    <v-dialog
      fullscreen
      v-model="state.appointmentDialog"
    >
      <v-card>
        <v-toolbar
          dark
          color="primary"
        >
          <v-btn
            icon
            dark
            @click="state.appointmentDialog = false"
          >
            <v-icon>mdi-close</v-icon>
          </v-btn>
          <v-toolbar-title>Schedule Appointment</v-toolbar-title>
        </v-toolbar>
        <AppointmentContainer
          v-if="
            (isLoading && isError) ||
            (state.appointmentsLoaded && state.appointments.length > 0)
          "
          :events="state.appointments"
          :toggle-appointment="toggleAppointmentComplete"
          :show-header="false"
          :rescheduling="state.rescheduling"
        />
        <div
          class="text-center"
          v-else
        >
          <v-progress-linear
            indeterminate
            :height="20"
          >
            Loading Appointments
          </v-progress-linear>
        </div>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="state.withdrawDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>Withdraw your application?</v-card-title>

        <v-card-text>
          Are you sure you wish to withdraw your application?<br />
          Your appointment will be canceled and the time slot may no longer be
          available.<br />
          If you wish to resubmit you will be required to select a new
          appointment date.
        </v-card-text>

        <v-card-actions>
          <v-btn
            @click="state.withdrawDialog = false"
            color="primary"
            text
          >
            Cancel
          </v-btn>

          <v-btn
            @click="handleWithdrawApplication"
            color="primary"
            text
          >
            Yes, withdraw
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="state.invalidSubmissionDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>Action Needed Before Submission</v-card-title>

        <v-card-text>
          Please schedule an appointment in order to submit your application
        </v-card-text>

        <v-card-actions>
          <v-btn
            @click="state.invalidSubmissionDialog = false"
            color="primary"
            text
          >
            Close
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="state.confirmSubmissionDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>Confirm Submission</v-card-title>

        <v-card-text>
          Are you sure you wish to submit your application? This will begin the
          application process
        </v-card-text>

        <v-card-actions>
          <v-btn
            @click="state.confirmSubmissionDialog = false"
            color="primary"
            text
          >
            Close
          </v-btn>

          <v-btn
            @click="handleConfirmSubmit"
            color="primary"
            text
          >
            Submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog
      v-model="state.cancelAppointmentDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>Cancel Appointment</v-card-title>

        <v-card-text>
          Are you sure you wish to cancel your appointment? <br />If you cancel
          your appointment the time slot will not be reserved for you. You may
          reschedule for the next available appointment.
        </v-card-text>

        <v-card-actions>
          <v-btn
            @click="state.cancelAppointmentDialog = false"
            color="primary"
            text
          >
            Close
          </v-btn>
          <v-btn
            @click="handleConfirmCancelAppointment"
            color="primary"
            text
          >
            Cancel Appointment
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue'
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue'
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue'
import { AppointmentType } from '@shared-utils/types/defaultTypes'
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue'
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue'
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue'
import FileUploadInfoSection from '@shared-ui/components/info-sections/FileUploadInfoSection.vue'
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue'
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue'
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue'
import QualifyingQuestionsInfoSection from '@shared-ui/components/info-sections/QualifyingQuestionsInfoSection.vue'
import Routes from '@core-public/router/routes'
import SpouseAddressInfoSection from '@shared-ui/components/info-sections/SpouseAddressInfoSection.vue'
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue'
import WeaponsInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { i18n } from '@shared-ui/plugins'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import {
  ApplicationStatus,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive, ref } from 'vue'

const applicationStore = useCompleteApplicationStore()
const appointmentStore = useAppointmentsStore()
const router = useRouter()
const tab = ref(null)
const reviewDialog = ref(false)
const flaggedQuestionText = ref('')
const flaggedQuestionHeader = ref('')

const state = reactive({
  cancelAppointmentDialog: false,
  invalidSubmissionDialog: false,
  confirmSubmissionDialog: false,
  rescheduling: false,
  withdrawDialog: false,
  appointmentDialog: false,
  appointments: [] as Array<AppointmentType>,
  appointmentsLoaded: false,
  application: [applicationStore.completeApplication],
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

const {
  mutate: getAppointmentMutation,
  isLoading,
  isError,
} = useMutation({
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  mutationFn: () => {
    const appRes = appointmentStore.getAvailableAppointments()

    appRes
      .then((data: Array<AppointmentType>) => {
        data.forEach(event => {
          let start = new Date(event.start)
          let end = new Date(event.end)

          let formatedStart = `${start.getFullYear()}-${
            start.getMonth() + 1
          }-${start.getDate()} ${start.getHours()}:${start.getMinutes()}`

          let formatedEnd = `${end.getFullYear()}-${
            end.getMonth() + 1
          }-${end.getDate()} ${end.getHours()}:${end.getMinutes()}`

          event.name = 'open'
          event.start = formatedStart
          event.end = formatedEnd
        })
        state.appointments = data
        state.appointmentsLoaded = true
      })
      .catch(() => {
        state.appointmentsLoaded = true
      })
  },
})

const canApplicationBeModified = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment Complete'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Background In Progress'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Contingently Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Approved &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Permit Delivered'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Cancelled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Withdrawn &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Flagged For Review']
  )
})

const canApplicationBeContinued = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Submitted &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment Complete'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Background In Progress'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Contingently Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Approved &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Permit Delivered'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Cancelled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Withdrawn &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Flagged For Review'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment No Show']
  )
})

const canRescheduleAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment Complete'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.appointmentStatus === 2
  )
})

const canScheduleAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.appointmentStatus === 1
  )
})

const canCancelAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.appointmentStatus === 2
  )
})

const createMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })
  },
  onError: () => null,
})

const updateMutation = useMutation({
  mutationFn: applicationStore.updateApplication,
  onSuccess: () => {
    router.push({
      path: Routes.APPLICATION_DETAIL_ROUTE,
      query: {
        applicationId: state.application[0].id,
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
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })
  },
  onError: () => null,
})

function handleContinueApplication() {
  if (
    applicationStore.completeApplication.application.status ===
    ApplicationStatus.None
  ) {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })
  } else if (
    applicationStore.completeApplication.application.status ===
    ApplicationStatus.Incomplete
  ) {
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })
  } else {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })
  }
}

function handleModifyApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID()
  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.isComplete = false
  applicationStore.completeApplication.application.appointmentStatus =
    AppointmentStatus.Scheduled
  applicationStore.completeApplication.application.status =
    ApplicationStatus.Incomplete
  applicationStore.completeApplication.application.applicationType = `modify-${applicationStore.completeApplication.application.applicationType}`
  renewMutation.mutate()
}

function handleRenewApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID()
  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.isComplete = false
  applicationStore.completeApplication.application.appointmentStatus =
    AppointmentStatus.Scheduled
  applicationStore.completeApplication.application.status =
    ApplicationStatus.Incomplete
  applicationStore.completeApplication.application.applicationType = `renew-${applicationStore.completeApplication.application.applicationType}`
  createMutation.mutate()
}

function handleWithdrawApplication() {
  state.withdrawDialog = false
  applicationStore.completeApplication.application.currentStep = 10
  applicationStore.completeApplication.application.isComplete = false
  applicationStore.completeApplication.application.appointmentStatus =
    AppointmentStatus['Not Scheduled']
  appointmentStore.putRemoveApplicationFromAppointment(
    applicationStore.completeApplication.application.appointmentId
  )
  applicationStore.completeApplication.application.appointmentDateTime = null
  applicationStore.completeApplication.application.status =
    ApplicationStatus.Withdrawn
  applicationStore.completeApplication.application.appointmentId = null
  updateMutation.mutate()
}

function handleSubmit() {
  if (
    applicationStore.completeApplication.application.appointmentStatus === 1
  ) {
    state.invalidSubmissionDialog = true
  } else {
    state.confirmSubmissionDialog = true
  }
}

function handleConfirmSubmit() {
  applicationStore.completeApplication.application.isComplete = true
  applicationStore.completeApplication.application.status =
    ApplicationStatus.Submitted
  applicationStore.completeApplication.application.submittedToLicensingDateTime =
    new Date().toISOString()

  updateMutation.mutate()
  state.confirmSubmissionDialog = false
}

function handleCancelAppointment() {
  state.cancelAppointmentDialog = true
}

function handleConfirmCancelAppointment() {
  applicationStore.completeApplication.application.appointmentStatus =
    AppointmentStatus['Not Scheduled']
  applicationStore.completeApplication.application.appointmentDateTime = null
  appointmentStore.putRemoveApplicationFromAppointment(
    applicationStore.completeApplication.application.appointmentId
  )
  applicationStore.completeApplication.application.appointmentId = null
  applicationStore.completeApplication.application.status =
    ApplicationStatus.Incomplete
  applicationStore.completeApplication.application.startOfNinetyDayCountdown =
    null
  applicationStore.completeApplication.application.submittedToLicensingDateTime =
    null
  applicationStore.completeApplication.application.isComplete = false
  updateMutation.mutate()
  state.cancelAppointmentDialog = false
}

function handleShowAppointmentDialog() {
  state.rescheduling = true
  getAppointmentMutation()
  state.appointmentDialog = true
}

function handleShowAppointmentDialogSchedule() {
  getAppointmentMutation()
  state.appointmentDialog = true
}

function handleShowWithdrawDialog() {
  state.withdrawDialog = true
}

function toggleAppointmentComplete() {
  applicationStore.updateApplication()
  state.appointmentDialog = false
  state.rescheduling = false
}

function showReviewDialog() {
  const qualifyingQuestions =
    applicationStore.completeApplication.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  const questionOneAgencyTemp = qualifyingQuestions.questionOneAgencyTemp || ''
  const questionOneIssueDateTemp =
    qualifyingQuestions.questionOneIssueDateTemp || ''
  const questionOneNumberTemp = qualifyingQuestions.questionOneNumberTemp || ''

  if (
    questionOneAgencyTemp ||
    questionOneIssueDateTemp ||
    questionOneNumberTemp
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-ONE')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOneAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOneIssueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOneNumber || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOneAgencyTemp || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOneIssueDateTemp || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOneNumberTemp || 'N/A'
    }\n\n`
  }

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (
      key.endsWith('TempExplanation') &&
      value != null &&
      !key.startsWith('questionOne')
    ) {
      const questionNumber = key
        .replace('TempExplanation', '')
        .replace('question', '')

      const originalResponse =
        qualifyingQuestions[`question${questionNumber}Exp`]

      const revisedChanges = value

      flaggedQuestionText.value += `Question: ${i18n.t(
        `QUESTION-${questionNumber.toUpperCase()}`
      )}\n\n`
      flaggedQuestionText.value += `Original Response:  ${originalResponse}\n\n`
      flaggedQuestionText.value += `Revised Changes: ${revisedChanges}\n\n`
    }
  }

  if (flaggedQuestionText.value !== '') {
    reviewDialog.value = true
    flaggedQuestionHeader.value = 'Review Required'
  }
}

function acceptChanges() {
  applicationStore.completeApplication.application.flaggedForCustomerReview =
    false
  applicationStore.completeApplication.application.flaggedForLicensingReview =
    true

  updateMutation.mutate()

  reviewDialog.value = false
}

function cancelChanges() {
  reviewDialog.value = false
}
</script>
