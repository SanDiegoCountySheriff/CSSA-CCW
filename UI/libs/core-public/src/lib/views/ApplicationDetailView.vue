<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <v-card
          :loading="
            isGetApplicationsLoading ||
            isUpdateApplicationLoading ||
            isRefundRequestLoading ||
            isAddHistoricalApplicationLoading ||
            isMakePaymentLoading
          "
          outlined
        >
          <v-card-title>
            <v-row>
              <v-col
                md="4"
                cols="12"
                :class="
                  $vuetify.breakpoint.name === 'md' ||
                  $vuetify.breakpoint.name === 'lg' ||
                  $vuetify.breakpoint.name === 'xl'
                    ? 'text-left'
                    : ''
                "
              >
                Order ID:
                {{ applicationStore.completeApplication.application.orderId }}
              </v-col>

              <v-col
                md="4"
                cols="12"
                :class="
                  $vuetify.breakpoint.name === 'md' ||
                  $vuetify.breakpoint.name === 'lg' ||
                  $vuetify.breakpoint.name === 'xl'
                    ? 'text-center'
                    : ''
                "
              >
                Application Type:
                {{
                  ApplicationType[
                    applicationStore.completeApplication.application
                      .applicationType
                  ].toString()
                }}
              </v-col>
              <v-col
                md="4"
                cols="12"
                :class="
                  $vuetify.breakpoint.name === 'md' ||
                  $vuetify.breakpoint.name === 'lg' ||
                  $vuetify.breakpoint.name === 'xl'
                    ? 'text-right'
                    : ''
                "
              >
                Status:
                {{ getApplicationStatusText }}
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
                applicationStore.completeApplication.application.dob.birthDate +
                  'T12:00:00Z'
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
                :disabled="isGetApplicationsLoading"
              >
                <v-icon left> mdi-alert-circle-outline </v-icon>
                Additional Information Required
              </v-btn>

              <v-dialog
                v-model="reviewDialog"
                max-width="800"
              >
                <v-card outlined>
                  <v-card-title>
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
                      text
                      color="error"
                      @click="cancelChanges"
                    >
                      Cancel
                    </v-btn>
                    <v-spacer></v-spacer>
                    <v-btn
                      text
                      color="primary"
                      @click="acceptChanges"
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
              {{ getApplicationStatusText }}
            </template>
          </v-card-title>

          <v-divider></v-divider>

          <v-card-text>
            <v-row>
              <v-col
                cols="12"
                xl="6"
              >
                <v-btn
                  color="primary"
                  block
                  :disabled="
                    !canApplicationBeContinued ||
                    isGetApplicationsLoading ||
                    isRenewLoading ||
                    isMakePaymentLoading
                  "
                  @click="handleContinueApplication"
                >
                  Continue
                </v-btn>
              </v-col>

              <v-col
                cols="12"
                xl="6"
              >
                <WithdrawModifyDialog
                  v-if="showModifyWithdrawButton"
                  :disabled="
                    isRefundRequestLoading ||
                    isUpdateApplicationLoading ||
                    fileUploadLoading ||
                    isMakePaymentLoading
                  "
                  @confirm="handleConfirmWithdrawModification"
                />

                <v-btn
                  v-if="showInitialWithdrawButton"
                  @click="handleShowWithdrawDialog"
                  :disabled="
                    isGetApplicationsLoading ||
                    !canWithdrawApplication ||
                    isMakePaymentLoading
                  "
                  color="primary"
                  block
                >
                  Withdraw
                </v-btn>

                <v-btn
                  v-else-if="
                    applicationStore.completeApplication.application.status ===
                    ApplicationStatus.Withdrawn
                  "
                  color="primary"
                  block
                  @click="handleSubmit"
                  :disabled="isGetApplicationsLoading || isMakePaymentLoading"
                >
                  Submit
                </v-btn>
              </v-col>
            </v-row>

            <v-row>
              <v-col
                cols="12"
                xl="6"
              >
                <v-btn
                  color="primary"
                  block
                  :disabled="isRenewalActive || isMakePaymentLoading"
                  @click="handleShowRenewDialog"
                >
                  Renew
                </v-btn>
              </v-col>

              <v-col
                cols="12"
                xl="6"
              >
                <v-btn
                  v-if="canApplicationBeUpdated"
                  color="primary"
                  block
                  :disabled="
                    !canApplicationBeUpdated ||
                    isGetApplicationsLoading ||
                    isMakePaymentLoading ||
                    (applicationStore.completeApplication.application
                      .appointmentDateTime &&
                      new Date() >=
                        new Date(
                          applicationStore.completeApplication.application.appointmentDateTime
                        ))
                  "
                  @click="handleUpdateApplication"
                >
                  Update
                </v-btn>

                <v-btn
                  v-if="canApplicationBeModified"
                  color="primary"
                  block
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
          v-if="
            !isRenew &&
            !isModification &&
            applicationStore.completeApplication.application.status !==
              ApplicationStatus['Permit Delivered']
          "
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
                  :disabled="isGetApplicationsLoading || isMakePaymentLoading"
                  block
                  color="primary"
                >
                  Reschedule
                </v-btn>

                <v-btn
                  v-else-if="canScheduleAppointment"
                  @click="handleShowAppointmentDialogSchedule"
                  :disabled="isGetApplicationsLoading || isMakePaymentLoading"
                  block
                  color="primary"
                >
                  Schedule
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  v-if="canCancelAppointment && !isGetApplicationsLoading"
                  @click="handleCancelAppointment"
                  :disabled="isMakePaymentLoading"
                  color="primary"
                  block
                >
                  Cancel
                </v-btn>
              </v-col>
            </v-row>

            <v-row
              v-if="
                applicationStore.completeApplication.application
                  .readyForInitialPayment
              "
            >
              <v-col>
                <InitialPaymentConfirmationDialog
                  :disabled="isMakePaymentLoading"
                  @confirm="handleInitialPayment"
                />
              </v-col>

              <v-col></v-col>
            </v-row>
          </v-card-text>
        </v-card>
        <v-card
          v-else-if="
            applicationStore.completeApplication.application.status ===
              ApplicationStatus['Permit Delivered'] ||
            isRenew ||
            isModification
          "
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            Permit Expiration Date
          </v-card-title>

          <v-divider></v-divider>

          <v-card-title class="justify-center">
            <v-icon
              color="primary"
              class="mr-2"
            >
              mdi-clock-alert-outline
            </v-icon>
            {{
              applicationStore.completeApplication.application.license
                .expirationDate
                ? new Date(
                    applicationStore.completeApplication.application.license.expirationDate
                  ).toLocaleDateString('en-US', {
                    month: 'long',
                    day: 'numeric',
                    year: 'numeric',
                  })
                : ''
            }}
          </v-card-title>

          <v-card-title> </v-card-title>
        </v-card>
        <v-card
          v-else-if="isLicenseExpired"
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center"> Permit Expired </v-card-title>

          <v-divider></v-divider>

          <v-card-title>
            <v-row>
              <v-col>
                <v-alert
                  type="warning"
                  color="warning"
                  dark
                  outlined
                  dense
                  elevation="2"
                >
                  Please contact {{ brandStore.brand.agencyName }} Licensing
                  Staff
                </v-alert>
              </v-col>
            </v-row>
          </v-card-title>

          <v-card-title> </v-card-title>
        </v-card>
      </v-col>
    </v-row>

    <v-row>
      <v-col>
        <v-card
          :loading="fileUploadLoading || isGetApplicationsLoading"
          class="fill-height"
          min-height="50vh"
          outlined
        >
          <v-tabs
            :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
            v-model="tab"
            grow
          >
            <v-tabs-slider color="primary"></v-tabs-slider>
            <v-tab> Personal Info </v-tab>
            <v-tab> ID Info </v-tab>
            <v-tab> Address </v-tab>
            <v-tab> Appearance </v-tab>
            <v-tab> Employment & Weapons </v-tab>
            <v-tab> Qualifying Questions </v-tab>
            <v-tab> Uploaded Documents</v-tab>
            <v-tab> Signature</v-tab>
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
              <CharacterReferenceInfoSection
                v-if="!isRenew"
                :color="'primary'"
                :character-references="
                  applicationStore.completeApplication.application
                    .characterReferences
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
                @on-file-submit="handleFileSubmit"
                :enable-button="canUploadFiles"
                :enable-eight-hour-safety-course-button="
                  enableEightHourSafetyCourseButton
                "
              />
            </v-tab-item>
            <v-tab-item>
              <SignatureInfoSection />
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
          :show-header="false"
          :rescheduling="state.rescheduling"
          @toggle-appointment="toggleAppointmentComplete"
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
            color="error"
            text
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
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
      v-model="state.renewDialog"
      max-width="600"
    >
      <v-card>
        <v-card-title>Begin Renewal?</v-card-title>

        <v-card-text>
          Are you sure you wish to begin the renewal process?<br />
          You will need to update some of your information, and go through the
          payment process again.<br />
          Your application will be changed to a renewal. This action cannot be
          undone.
        </v-card-text>

        <v-card-actions>
          <v-btn
            @click="state.renewDialog = false"
            color="error"
            text
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            @click="handleRenewApplication"
            color="primary"
            text
            :loading="isRenewLoading"
            :disabled="isRenewLoading"
          >
            Begin Renewal
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
            Cancel
          </v-btn>

          <v-spacer />

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
            color="error"
            text
          >
            Close
          </v-btn>
          <v-spacer></v-spacer>
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

    <v-dialog
      v-model="isUpdatePaymentHistoryLoading"
      max-width="600"
      persistent
    >
      <v-card loading>
        <v-card-title> Processing Initial Payment </v-card-title>

        <v-card-text>
          Please do not close the browser or click the back button.
        </v-card-text>
      </v-card>
    </v-dialog>

    <v-snackbar
      v-model="state.snackbar"
      color="primary"
    >
      {{ $t(`Appointment is confirmed for: `) }}
      {{ appointmentTime }}

      <template #action="{ attrs }">
        <v-btn
          text
          v-bind="attrs"
          @click="state.snackbar = false"
        >
          Ok
        </v-btn>
      </template>
    </v-snackbar>

    <v-snackbar
      v-model="paymentSnackbar"
      :timeout="-1"
      color="primary"
      persistent
    >
      {{ $t('There was a problem processing the payment, please try again.') }}
      <v-btn
        @click="paymentSnackbar = !paymentSnackbar"
        icon
      >
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import AddressInfoSection from '@shared-ui/components/info-sections/AddressInfoSection.vue'
import AppearanceInfoSection from '@shared-ui/components/info-sections/AppearanceInfoSection.vue'
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue'
import CharacterReferenceInfoSection from '@shared-ui/components/info-sections/CharacterReferenceInfoSection.vue'
import CitizenInfoSection from '@shared-ui/components/info-sections/CitizenInfoSection.vue'
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue'
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue'
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadInfoSection from '@shared-ui/components/info-sections/FileUploadInfoSection.vue'
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue'
import InitialPaymentConfirmationDialog from '@core-public/components/dialogs/InitialPaymentConfirmationDialog.vue'
import PersonalInfoSection from '@shared-ui/components/info-sections/PersonalInfoSection.vue'
import PreviousAddressInfoSection from '@shared-ui/components/info-sections/PreviousAddressInfoSection.vue'
import QualifyingQuestionsInfoSection from '@shared-ui/components/info-sections/QualifyingQuestionsInfoSection.vue'
import Routes from '@core-public/router/routes'
import SignatureInfoSection from '@shared-ui/components/info-sections/SignatureInfoSection.vue'
import SpouseAddressInfoSection from '@shared-ui/components/info-sections/SpouseAddressInfoSection.vue'
import SpouseInfoSection from '@shared-ui/components/info-sections/SpouseInfoSection.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import WeaponsInfoSection from '@shared-ui/components/info-sections/WeaponsInfoSection.vue'
import WithdrawModifyDialog from '@core-public/components/dialogs/WithdrawModifyDialog.vue'
import axios from 'axios'
import { getOriginalApplicationTypeModification } from '@shared-ui/composables/getOriginalApplicationType'
import { i18n } from '@shared-ui/plugins'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentStatus,
  QualifyingQuestionStandard,
} from '@shared-utils/types/defaultTypes'
import {
  AppointmentType,
  RefundRequest,
} from '@shared-utils/types/defaultTypes'
import {
  CompleteApplication,
  PaymentType,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'
import { useRoute, useRouter } from 'vue-router/composables'

interface IFileSubmission {
  file: File
  fileType: string
}

const applicationStore = useCompleteApplicationStore()
const appointmentStore = useAppointmentsStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const themeStore = useThemeStore()
const router = useRouter()
const route = useRoute()
const tab = ref(null)
const reviewDialog = ref(false)
const flaggedQuestionText = ref('')
const flaggedQuestionHeader = ref('')
const fileUploadLoading = ref(false)
const appointmentTime = ref('')
const isRenewLoading = ref(false)
const paymentSnackbar = ref(false)

const state = reactive({
  snackbar: false,
  isApplicationValid: false,
  cancelAppointmentDialog: false,
  invalidSubmissionDialog: false,
  confirmSubmissionDialog: false,
  rescheduling: false,
  withdrawDialog: false,
  renewDialog: false,
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
  mutate: updatePaymentHistory,
  isLoading: isUpdatePaymentHistoryLoading,
} = useMutation({
  mutationFn: ({
    transactionId,
    successful,
    amount,
    paymentType,
    transactionDateTime,
    hmac,
    applicationId,
  }: {
    transactionId: string
    successful: boolean
    amount: number
    paymentType: string
    transactionDateTime: string
    hmac: string
    applicationId: string
  }) => {
    return paymentStore.updatePaymentHistory(
      transactionId,
      successful,
      amount,
      paymentType,
      transactionDateTime,
      hmac,
      applicationId
    )
  },
  onSuccess: () =>
    applicationStore
      .getCompleteApplicationFromApi(
        applicationStore.completeApplication.id,
        Boolean(route.query.isComplete)
      )
      .then(res => {
        applicationStore.setCompleteApplication(res)
      }),
})

onMounted(() => {
  state.isApplicationValid = Boolean(applicationStore.completeApplication.id)

  const transactionId = route.query.transactionId
  const successful = route.query.successful
  const amount = route.query.amount
  const hmac = route.query.hmac
  const paymentType = route.query.paymentType
  const applicationId = route.query.applicationId
  let transactionDateTime = route.query.transactionDateTime

  if (typeof transactionDateTime === 'string') {
    transactionDateTime = transactionDateTime.replace(':', '%3A')
    transactionDateTime = transactionDateTime.replace(':', '%3A')
  }

  if (
    typeof transactionId === 'string' &&
    typeof successful === 'string' &&
    typeof amount === 'string' &&
    typeof paymentType === 'string' &&
    typeof transactionDateTime === 'string' &&
    typeof hmac === 'string' &&
    typeof applicationId === 'string'
  ) {
    updatePaymentHistory({
      transactionId,
      successful: Boolean(successful),
      amount: Number(amount),
      paymentType,
      transactionDateTime,
      hmac,
      applicationId,
    })
  }
})

const { isLoading: isGetApplicationsLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !state.isApplicationValid,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
    },
  }
)

const {
  mutate: getAppointmentMutation,
  isLoading,
  isError,
} = useMutation({
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  mutationFn: () => {
    const appRes = appointmentStore.getAvailableAppointments(false)

    appRes
      .then((data: Array<AppointmentType>) => {
        data = data.reduce(
          (result, currentObj) => {
            const key = `${currentObj.start}-${currentObj.end}`

            if (!result.set.has(key)) {
              result.set.add(key)
              result.array.push(currentObj)
            }

            return result
          },
          { set: new Set(), array: [] } as {
            set: Set<string>
            array: Array<AppointmentType>
          }
        ).array

        data.forEach(event => {
          let start = new Date(event.start)
          let end = new Date(event.end)

          let formattedStart = `${start.getFullYear()}-${
            start.getMonth() + 1
          }-${start.getDate()} ${start.getHours()}:${start
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

          let formattedEnd = `${end.getFullYear()}-${
            end.getMonth() + 1
          }-${end.getDate()} ${end.getHours()}:${end
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

          event.name = 'open'
          event.start = formattedStart
          event.end = formattedEnd
        })
        state.appointments = data
        state.appointmentsLoaded = true
      })
      .catch(() => {
        state.appointmentsLoaded = true
      })
  },
})

const enableEightHourSafetyCourseButton = computed(() => {
  return (
    applicationStore.completeApplication.application.status ===
    ApplicationStatus.Approved
  )
})

const canApplicationBeModified = computed(() => {
  return (
    applicationStore.completeApplication.application.status ===
    ApplicationStatus['Permit Delivered']
  )
})

const showInitialWithdrawButton = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Withdrawn &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Incomplete &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Permit Delivered'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Employment'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Judicial'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Reserve'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Standard']
  )
})

const showModifyWithdrawButton = computed(() => {
  return (
    applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Modify Employment'] ||
    applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Modify Judicial'] ||
    applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Modify Reserve'] ||
    applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Modify Standard']
  )
})

const canApplicationBeUpdated = computed(() => {
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
      ApplicationStatus.Canceled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Withdrawn &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Flagged For Review'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Incomplete &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Reserve'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Employment'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Judicial'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Modify Standard']
  )
})

const canApplicationBeContinued = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Submitted &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Ready For Appointment'] &&
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
      ApplicationStatus.Canceled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Withdrawn &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Flagged For Review'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment No Show'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Waiting For Customer']
  )
})

const canRescheduleAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment Complete'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    (applicationStore.completeApplication.application.appointmentStatus === 2 ||
      applicationStore.completeApplication.application.appointmentStatus === 4)
  )
})

const canScheduleAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.appointmentStatus ===
      AppointmentStatus['Not Scheduled'] &&
    applicationStore.completeApplication.application.status ===
      ApplicationStatus.Withdrawn
  )
})

const canCancelAppointment = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Appointment Complete'] &&
    applicationStore.completeApplication.application.appointmentStatus ===
      AppointmentStatus.Scheduled
  )
})

const canWithdrawApplication = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied
  )
})

const canUploadFiles = computed(() => {
  return (
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Approved
  )
})

const getApplicationStatusText = computed(() => {
  if (
    applicationStore.completeApplication.application.status ===
      ApplicationStatus['Contingently Approved'] ||
    applicationStore.completeApplication.application.status ===
      ApplicationStatus['Contingently Denied']
  ) {
    return ApplicationStatus[ApplicationStatus['Background In Progress']]
  }

  if (
    applicationStore.completeApplication.application.status ===
    ApplicationStatus['Waiting For Customer']
  ) {
    return 'Pending Information'
  }

  return ApplicationStatus[
    applicationStore.completeApplication.application.status
  ]
})

const isRenewalActive = computed(() => {
  const application = applicationStore.completeApplication.application
  const license = application.license
  const expirationDate = license.expirationDate
    ? new Date(license.expirationDate).setHours(23, 59, 59, 999)
    : null
  const expiredApplicationRenewalPeriod =
    brandStore.brand.expiredApplicationRenewalPeriod
  const daysBeforeActiveRenewal = brandStore.brand.daysBeforeActiveRenewal

  return (
    application.status !== ApplicationStatus['Permit Delivered'] ||
    (expirationDate &&
      (new Date(expirationDate) <
        new Date(
          new Date(
            new Date().getTime() -
              expiredApplicationRenewalPeriod * 24 * 60 * 60 * 1000
          ).setHours(23, 59, 59, 999)
        ) ||
        new Date(expirationDate) >
          new Date(
            new Date().getTime() +
              (daysBeforeActiveRenewal + 1) * 24 * 60 * 60 * 1000
          ))) ||
    isGetApplicationsLoading
  )
})

const isRenew = computed(() => {
  const applicationType =
    applicationStore.completeApplication.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const isModification = computed(() => {
  const applicationType =
    applicationStore.completeApplication.application.applicationType

  return (
    applicationType === ApplicationType['Modify Standard'] ||
    applicationType === ApplicationType['Modify Reserve'] ||
    applicationType === ApplicationType['Modify Judicial'] ||
    applicationType === ApplicationType['Modify Employment']
  )
})

const isLicenseExpired = computed(() => {
  const gracePeriod = brandStore.brand.expiredApplicationRenewalPeriod
  let expirationDate: Date
  let now: number

  if (applicationStore.completeApplication.application.license.expirationDate) {
    expirationDate = new Date(
      applicationStore.completeApplication.application.license.expirationDate
    )
    now = new Date().setHours(23, 59, 59, 999)
  } else {
    return false
  }

  return (
    now > expirationDate.getTime() + (gracePeriod + 1) * 24 * 60 * 60 * 1000
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

const updateWithoutRouteMutation = useMutation({
  mutationFn: applicationStore.updateApplication,
  onSuccess: () => {
    fileUploadLoading.value = false
  },
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

const {
  mutateAsync: addHistoricalApplicationPublic,
  isLoading: isAddHistoricalApplicationLoading,
} = useMutation({
  mutationFn: (application: CompleteApplication) =>
    applicationStore.addHistoricalApplicationPublic(application),
})

const {
  isLoading: isUpdateApplicationLoading,
  mutateAsync: updateApplication,
} = useMutation({
  mutationFn: applicationStore.updateApplication,
})

const { isLoading: isRefundRequestLoading, mutateAsync: requestRefund } =
  useMutation({
    mutationFn: (refundRequest: RefundRequest) =>
      paymentStore.requestRefund(refundRequest),
  })

const renewMutation = useMutation({
  mutationFn: applicationStore.updateApplication,
  onSuccess: () => {
    isRenewLoading.value = false
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
      },
    })
  },
  onError: () => null,
})

const { mutate: makeInitialPayment, isLoading: isMakePaymentLoading } =
  useMutation({
    mutationFn: () => {
      let cost: number
      let paymentType: string

      switch (
        applicationStore.completeApplication.application.applicationType
      ) {
        case ApplicationType.Standard:
          paymentType =
            PaymentType['CCW Application Initial Payment'].toString()
          cost = brandStore.brand.cost.new.standard
          break

        case ApplicationType.Judicial:
          paymentType =
            PaymentType['CCW Application Initial Judicial Payment'].toString()
          cost = brandStore.brand.cost.new.judicial
          break

        case ApplicationType.Reserve:
          paymentType =
            PaymentType['CCW Application Initial Reserve Payment'].toString()
          cost = brandStore.brand.cost.new.reserve
          break

        case ApplicationType.Employment:
          paymentType =
            PaymentType['CCW Application Initial Employment Payment'].toString()
          cost = brandStore.brand.cost.new.employment
          break

        default:
          paymentType =
            PaymentType['CCW Application Initial Payment'].toString()
          cost = brandStore.brand.cost.new.standard
      }

      return paymentStore.getPayment(
        applicationStore.completeApplication.id,
        cost,
        applicationStore.completeApplication.application.orderId,
        paymentType
      )
    },
    onError: () => {
      paymentSnackbar.value = true
    },
  })

function handleInitialPayment() {
  makeInitialPayment()
}

async function handleConfirmWithdrawModification() {
  const transaction = applicationStore.completeApplication.paymentHistory.find(
    ph => {
      return (
        ph.modificationNumber ===
        applicationStore.completeApplication.application.modificationNumber
      )
    }
  )

  if (transaction) {
    const refundRequest: RefundRequest = {
      id: null,
      transactionId: transaction.transactionId,
      applicationId: applicationStore.completeApplication.id,
      refundAmount: Number(transaction.amount),
      reason: 'Withdraw Modification',
      orderId: applicationStore.completeApplication.application.orderId,
    }

    await requestRefund(refundRequest)
  }

  applicationStore.completeApplication.application.modifiedAddress = {
    streetAddress: '',
    city: '',
    state: '',
    county: '',
    zip: '',
    country: '',
  }
  applicationStore.completeApplication.application.modifiedAddressComplete =
    null
  applicationStore.completeApplication.application.modifyAddWeapons = []
  applicationStore.completeApplication.application.modifyDeleteWeapons = []
  applicationStore.completeApplication.application.modifiedWeaponComplete = null
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    ''
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    ''
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    ''
  applicationStore.completeApplication.application.modifiedNameComplete = null
  applicationStore.completeApplication.application.status =
    ApplicationStatus['Permit Delivered']

  applicationStore.completeApplication.application.applicationType =
    getOriginalApplicationTypeModification(
      applicationStore.completeApplication.application.applicationType
    )

  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.modificationNumber += 1

  await updateApplication()
}

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

function handleUpdateApplication() {
  const appointmentDateTime =
    applicationStore.completeApplication.application.appointmentDateTime
  const appointmentDate = appointmentDateTime
    ? new Date(Date.parse(appointmentDateTime))
    : null
  const currentDate = new Date()

  if (
    appointmentDate &&
    currentDate < appointmentDate &&
    applicationStore.completeApplication.application.appointmentStatus ===
      AppointmentStatus.Scheduled
  ) {
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        applicationId: state.application[0].id,
        isComplete: state.application[0].application.isComplete.toString(),
      },
    })

    applicationStore.completeApplication.application.currentStep = 1
    applicationStore.completeApplication.application.isUpdatingApplication =
      true
    applicationStore.updateApplication()
  }
}

function handleModifyApplication() {
  router.push({
    path: Routes.MODIFY_FORM_PATH,
    query: {
      applicationId: state.application[0].id,
      isComplete: state.application[0].application.isComplete.toString(),
    },
  })
}

async function handleRenewApplication() {
  const historicalApplication: CompleteApplication = {
    ...applicationStore.getCompleteApplication,
  }

  await addHistoricalApplicationPublic(historicalApplication)

  isRenewLoading.value = true
  const application = applicationStore.completeApplication.application

  application.renewalNumber += 1

  if (!isRenew.value) {
    switch (application.applicationType) {
      case ApplicationType.Standard:
        applicationStore.completeApplication.application.applicationType =
          ApplicationType['Renew Standard']
        break
      case ApplicationType.Judicial:
        applicationStore.completeApplication.application.applicationType =
          ApplicationType['Renew Judicial']
        break
      case ApplicationType.Reserve:
        applicationStore.completeApplication.application.applicationType =
          ApplicationType['Renew Reserve']
        break
      case ApplicationType.Employment:
        applicationStore.completeApplication.application.applicationType =
          ApplicationType['Renew Employment']
        break
      default:
        break
    }
  }

  applicationStore.completeApplication.application.isUpdatingApplication = false

  applicationStore.completeApplication.application.currentStep = 1

  applicationStore.completeApplication.application.status =
    ApplicationStatus.Incomplete

  applicationStore.completeApplication.application.paymentStatus = 0

  resetDocuments()
  resetAgreements()
  renewMutation.mutate()
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

function handleShowRenewDialog() {
  state.renewDialog = true
}

function toggleAppointmentComplete(time: string) {
  appointmentTime.value = time
  state.snackbar = true
  state.appointmentDialog = false
  state.rescheduling = false
  applicationStore.updateApplication()
}

function showReviewDialog() {
  const qualifyingQuestions =
    applicationStore.completeApplication.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  const questionOneAgencyTempValue =
    qualifyingQuestions.questionOne.temporaryAgency || ''
  const questionOneIssueDateTempValue =
    qualifyingQuestions.questionOne.temporaryIssueDate || ''
  const questionOneNumberTempValue =
    qualifyingQuestions.questionOne.temporaryNumber || ''
  const questionOneTemporaryIssuingStateValue =
    qualifyingQuestions.questionOne.temporaryIssuingState || ''

  const questionTwoAgencyTempValue =
    qualifyingQuestions.questionTwo.temporaryAgency || ''
  const questionTwoDenialDateTempValue =
    qualifyingQuestions.questionTwo.temporaryDenialDate || ''
  const questionTwoDenialReasonTempValue =
    qualifyingQuestions.questionTwo.temporaryDenialReason || ''

  if (
    questionOneAgencyTempValue ||
    questionOneIssueDateTempValue ||
    questionOneNumberTempValue ||
    questionOneTemporaryIssuingStateValue
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-ONE')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOne.agency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issuing State: ${
      qualifyingQuestions.questionOne.issuingState || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOne.issueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOne.number || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionOne.temporaryAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issuing State: ${
      qualifyingQuestions.questionOne.temporaryIssuingState || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionOne.temporaryIssueDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionOne.temporaryNumber || 'N/A'
    }\n\n`
  }

  if (
    questionTwoAgencyTempValue ||
    questionTwoDenialDateTempValue ||
    questionTwoDenialReasonTempValue
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-TWO')}\n\n`

    flaggedQuestionText.value += `Original Response:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionTwo.agency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Denial Date: ${
      qualifyingQuestions.questionTwo.denialDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `Denial Reason Number: ${
      qualifyingQuestions.questionTwo.denialReason || 'N/A'
    }\n\n`

    flaggedQuestionText.value += `Revised Changes:\n`
    flaggedQuestionText.value += `Agency: ${
      qualifyingQuestions.questionTwo.temporaryAgency || 'N/A'
    }\n`
    flaggedQuestionText.value += `Issue Date: ${
      qualifyingQuestions.questionTwo.temporaryDenialDate || 'N/A'
    }\n`
    flaggedQuestionText.value += `License Number: ${
      qualifyingQuestions.questionTwo.temporaryDenialReason || 'N/A'
    }\n\n`
  }

  if (
    qualifyingQuestions.questionTwelve.temporaryTrafficViolations.length > 0
  ) {
    flaggedQuestionText.value += `${i18n.t('QUESTION-TWELVE')}\n\n`

    for (const trafficViolation of qualifyingQuestions.questionTwelve
      .temporaryTrafficViolations) {
      flaggedQuestionText.value += `Additional Citations Found: \n`
      flaggedQuestionText.value += `Date: ${trafficViolation.date}\n`
      flaggedQuestionText.value += `Agency: ${trafficViolation.agency}\n`
      flaggedQuestionText.value += `Violation: ${trafficViolation.violation}\n`
      flaggedQuestionText.value += `Citation Number: ${trafficViolation.citationNumber}\n\n`
    }
  }

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (
      key !== 'questionOne' &&
      key !== 'questionTwo' &&
      key !== 'questionTwelve' &&
      convertToQualifyingQuestionStandard(value).temporaryExplanation
    ) {
      const questionNumber = key.slice(8)

      flaggedQuestionText.value += `Question ${i18n.t(
        `QUESTION-${questionNumber.toUpperCase()}`
      )}\n\n`
      flaggedQuestionText.value += `Original Response: ${
        convertToQualifyingQuestionStandard(value).explanation
      }\n\n`
      flaggedQuestionText.value += `Revised Changes: ${
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      }\n\n`
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

function handleFileSubmit(fileSubmission: IFileSubmission) {
  fileUploadLoading.value = true
  const form = new FormData()

  form.append('fileToUpload', fileSubmission.file)

  axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${fileSubmission.fileType}`,
      form
    )
    .catch(e => {
      window.console.warn(e)
      Promise.reject()
    })

  const uploadDoc: UploadedDocType = {
    documentType: fileSubmission.fileType,
    name: fileSubmission.fileType,
    uploadedBy: applicationStore.completeApplication.application.userEmail,
    uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
  }

  applicationStore.completeApplication.application.uploadedDocuments.push(
    uploadDoc
  )

  updateWithoutRouteMutation.mutate()
}

function convertToQualifyingQuestionStandard(item) {
  return item as QualifyingQuestionStandard
}

function resetDocuments() {
  const uploadedDocuments =
    applicationStore.completeApplication.application.uploadedDocuments
  const documentTypesToReset = [
    'DriverLicense',
    'ProofResidency',
    'ProofResidency2',
    'Supporting',
    'NameChange',
    'Judicial',
    'Reserve',
    'Signature',
    'Employment',
  ]

  const filesToDelete = uploadedDocuments.filter(file => {
    return documentTypesToReset.includes(file.documentType)
  })

  filesToDelete.forEach(file => {
    if (
      file.documentType !== 'MilitaryDoc' &&
      file.documentType !== 'Citizenship'
    ) {
      const index = uploadedDocuments.indexOf(file)

      uploadedDocuments.splice(index, 1)
    }
  })

  applicationStore.completeApplication.application.uploadedDocuments =
    uploadedDocuments
}

function resetAgreements() {
  applicationStore.completeApplication.application.agreements.conditionsForIssuanceAgreed =
    false
  applicationStore.completeApplication.application.agreements.conditionsForIssuanceAgreedDate =
    null

  applicationStore.completeApplication.application.agreements.falseInfoAgreed =
    false
  applicationStore.completeApplication.application.agreements.falseInfoAgreedDate =
    null
}
</script>
, PaymentType, PaymentType
