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
            isMakePaymentLoading ||
            isWithdrawRenewLoading
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
          class="d-flex flex-column fill-height"
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
          class="d-flex flex-column fill-height"
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
                @click="handleInfoClick"
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

          <v-spacer />

          <v-card-text
            v-if="applicationStore.completeApplication.isMatchUpdated !== false"
          >
            <v-row>
              <v-col
                v-if="canApplicationBeContinued"
                cols="12"
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
                v-if="
                  showModifyWithdrawButton ||
                  showWithdrawRenewButton ||
                  showInitialWithdrawButton ||
                  applicationStore.completeApplication.application.status ===
                    ApplicationStatus.Withdrawn
                "
                cols="12"
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

                <WithdrawRenewDialog
                  v-if="showWithdrawRenewButton"
                  :disabled="
                    isRefundRequestLoading ||
                    isUpdateApplicationLoading ||
                    fileUploadLoading ||
                    isMakePaymentLoading
                  "
                  @confirm="handleConfirmWithdrawRenewal"
                />

                <v-btn
                  v-if="showInitialWithdrawButton && canWithdrawApplication"
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
                v-if="canApplicationBeUpdated || canApplicationBeModified"
                cols="12"
              >
                <v-btn
                  v-if="
                    canApplicationBeUpdated &&
                    applicationStore.completeApplication.application
                      .appointmentDateTime &&
                    new Date() <=
                      new Date(
                        applicationStore.completeApplication.application.appointmentDateTime
                      )
                  "
                  color="primary"
                  block
                  :disabled="
                    !canApplicationBeUpdated ||
                    isGetApplicationsLoading ||
                    isMakePaymentLoading
                  "
                  @click="handleUpdateApplication"
                >
                  Update Application Information
                </v-btn>

                <ConfirmDialog
                  v-else-if="canApplicationBeModified"
                  :icon="'mdi-swap-horizontal'"
                  title="Are you sure you want to modify your permit?"
                  text="Modifying allows you to change your address, name, and weapons listed on your permit for a small fee.  If you are able to renew you can make any necessary changes during the renewal process instead."
                  button-text="Click here to modify"
                  @confirm="handleModifyApplication"
                />
              </v-col>
            </v-row>
          </v-card-text>

          <v-card-text v-else>
            <v-btn
              @click="handleUpdateApplication"
              color="primary"
              block
            >
              Verify Information
            </v-btn>
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
              ApplicationStatus['Permit Delivered'] &&
            !applicationStore.completeApplication.application
              .readyForIssuancePayment
          "
          :loading="isUpdateApplicationLoading"
          outlined
          class="d-flex flex-column fill-height"
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
                <PaymentConfirmationDialog
                  :disabled="isMakePaymentLoading"
                  payment-type="Initial"
                  @confirm="handlePayment"
                />
              </v-col>

              <v-col></v-col>
            </v-row>
          </v-card-text>
        </v-card>

        <v-card
          v-else-if="
            applicationStore.completeApplication.application
              .readyForRenewalPayment
          "
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            Ready for Renewal Payment
          </v-card-title>

          <v-divider></v-divider>

          <v-card-title>
            <v-row>
              <v-col></v-col>

              <v-col>
                <PaymentConfirmationDialog
                  :disabled="isMakePaymentLoading"
                  payment-type="Renewal"
                  @confirm="handlePayment"
                />
              </v-col>

              <v-col></v-col>
            </v-row>
          </v-card-title>
        </v-card>

        <v-card
          v-else-if="
            applicationStore.completeApplication.application
              .readyForModificationPayment
          "
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            Ready for Modification Payment
          </v-card-title>

          <v-divider></v-divider>

          <v-card-title>
            <v-row>
              <v-col></v-col>

              <v-col>
                <PaymentConfirmationDialog
                  :disabled="isMakePaymentLoading"
                  payment-type="Modification"
                  @confirm="handlePayment"
                />
              </v-col>

              <v-col></v-col>
            </v-row>
          </v-card-title>
        </v-card>

        <v-card
          v-else-if="
            applicationStore.completeApplication.application
              .readyForIssuancePayment
          "
          class="fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            Ready for Issuance Payment
          </v-card-title>

          <v-divider></v-divider>

          <v-card-title>
            <v-row>
              <v-col></v-col>

              <v-col>
                <PaymentConfirmationDialog
                  :disabled="isMakePaymentLoading"
                  payment-type="Issuance"
                  @confirm="handlePayment"
                />
              </v-col>

              <v-col></v-col>
            </v-row>
          </v-card-title>
        </v-card>

        <v-card
          v-else-if="
            (applicationStore.completeApplication.application.status ===
              ApplicationStatus['Permit Delivered'] ||
              isRenew ||
              isModification) &&
            !isLicenseExpired
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

          <v-card-title
            v-if="!isRenewalActive && !isModification && !isRenew"
            class="justify-center"
          >
            You can begin your renewal in
            {{ numberOfDaysUntilRenewalIsActive }} days
          </v-card-title>

          <v-card-title
            v-if="
              isRenewalActive &&
              applicationStore.completeApplication.isMatchUpdated === false
            "
            class="justify-center"
          >
            You can begin your renewal after verifying your information
          </v-card-title>

          <v-spacer />

          <v-card-text>
            <v-row>
              <v-col
                v-if="
                  isRenewalActive &&
                  applicationStore.completeApplication.isMatchUpdated !== false
                "
                cols="12"
              >
                <v-btn
                  color="primary"
                  block
                  :disabled="!isRenewalActive || isMakePaymentLoading"
                  @click="handleShowRenewDialog"
                >
                  <v-icon left>mdi-autorenew</v-icon>
                  Click here to renew
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
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
                  Your permit has expired past the renewal period. You must fill
                  out a new application.
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
                v-if="
                  !isRenew &&
                  !applicationStore.completeApplication.application
                    .legacyQualifyingQuestions
                "
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
              <QualifyingQuestionsInfoSection />
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
              <SignatureInfoSection
                @on-signature-submit="handleSignatureSubmit"
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
          v-if="state.appointmentsLoaded && state.appointments.length > 0"
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
          <br />
          You will need to verify some of your information, upload new
          documents, accept the terms and agreements, and provide a new
          signature.
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
            :loading="
              isRenewLoading ||
              isGetApplicationsLoading ||
              isUpdateApplicationLoading ||
              isAddHistoricalApplicationLoading
            "
            :disabled="
              isRenewLoading ||
              isGetApplicationsLoading ||
              isUpdateApplicationLoading ||
              isAddHistoricalApplicationLoading
            "
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
        <v-card-title> Processing Payment </v-card-title>

        <v-card-text>
          Please do not close the browser or click the back button.
        </v-card-text>
      </v-card>
    </v-dialog>

    <v-snackbar
      v-model="state.snackbar"
      color="primary"
    >
      {{ $t('Appointment has been set for: ') }}
      {{
        applicationStore.completeApplication.application.appointmentDateTime
          ? new Date(
              applicationStore.completeApplication.application.appointmentDateTime
            ).toLocaleString([], {
              year: 'numeric',
              month: 'numeric',
              day: 'numeric',
              hour: '2-digit',
              minute: '2-digit',
            })
          : appointmentTime
      }}

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
      {{ $t('There was a problem processing the payment. Please try again.') }}
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
import ConfirmDialog from '@core-public/components/dialogs/ConfirmDialog.vue'
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import DOBinfoSection from '@shared-ui/components/info-sections/DOBinfoSection.vue'
import EmploymentInfoSection from '@shared-ui/components/info-sections/EmploymentInfoSection.vue'
import Endpoints from '@shared-ui/api/endpoints'
import FileUploadInfoSection from '@shared-ui/components/info-sections/FileUploadInfoSection.vue'
import IdInfoSection from '@shared-ui/components/info-sections/IdInfoSection.vue'
import PaymentConfirmationDialog from '@core-public/components/dialogs/PaymentConfirmationDialog.vue'
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
import WithdrawRenewDialog from '@core-public/components/dialogs/WithdrawRenewDialog.vue'
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
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query'
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
const queryClient = useQueryClient()

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
    // TODO: check if this works without this method
    applicationStore.getUserApplication(),
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

const { isLoading: isGetApplicationsLoading, refetch: getApplications } =
  useQuery(
    ['getApplicationsByUser'],
    () => applicationStore.getUserApplication(),
    {
      enabled: !state.isApplicationValid,
    }
  )

const { refetch } = useQuery(
  ['getAppointments', true],
  () => appointmentStore.getAvailableAppointments(true),
  {
    enabled: false,
    onSuccess: (data: Array<AppointmentType>) => {
      const uniqueData = data.reduce(
        (result, currentObj) => {
          const key = `${currentObj.start}-${currentObj.end}`

          if (!result.set.has(key)) {
            currentObj.slots = 1
            result.set.add(key)
            result.array.push(currentObj)
          } else {
            const index = result.array.findIndex(
              obj => obj.start === currentObj.start
            )

            if (index !== -1) {
              const updatedObj = result.array[index]

              if (updatedObj.slots) {
                updatedObj.slots += 1
              }

              result.array[index] = updatedObj
            }
          }

          return result
        },
        { set: new Set(), array: [] } as {
          set: Set<string>
          array: Array<AppointmentType>
        }
      ).array

      uniqueData.forEach(event => {
        const start = new Date(event.start)
        const end = new Date(event.end)

        if (event.slots) {
          event.name = `${event.slots} slot${event.slots > 1 ? 's' : ''} left`
        }

        event.start = formatDate(start, start.getHours(), start.getMinutes())
        event.end = formatDate(end, end.getHours(), end.getMinutes())
      })

      state.appointments = uniqueData
      state.appointmentsLoaded = true
    },
  }
)

function formatDate(date: Date, hour: number, minute: number): string {
  const year = date.getFullYear()
  const month = date.getMonth() + 1
  const day = date.getDate()
  let formattedHour = hour.toString().padStart(2, '0')
  let formattedMinute = minute.toString().padStart(2, '0')

  if (formattedMinute === '60') {
    formattedHour = (hour + 1).toString().padStart(2, '0')
    formattedMinute = '00'
  }

  return `${year}-${month.toString().padStart(2, '0')}-${day
    .toString()
    .padStart(2, '0')} ${formattedHour}:${formattedMinute}`
}

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

const numberOfDaysUntilRenewalIsActive = computed(() => {
  if (applicationStore.completeApplication.application.license.expirationDate) {
    const expirationDate = new Date(
      applicationStore.completeApplication.application.license.expirationDate
    )

    const today = new Date()

    const timeDifference = today.getTime() - expirationDate.getTime()

    const daysDifference = Math.floor(timeDifference / (1000 * 60 * 60 * 24))

    const gracePeriod = brandStore.brand.daysBeforeActiveRenewal

    return Math.abs(daysDifference + gracePeriod)
  }

  return 0
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
      ApplicationType['Modify Standard'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Renew Standard'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Renew Judicial'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Renew Reserve'] &&
    applicationStore.completeApplication.application.applicationType !==
      ApplicationType['Renew Employment']
  )
})

const showModifyWithdrawButton = computed(() => {
  return (
    (applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Modify Employment'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Modify Judicial'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Modify Reserve'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Modify Standard']) &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Modification Denied']
  )
})

const showWithdrawRenewButton = computed(() => {
  return (
    (applicationStore.completeApplication.application.applicationType ===
      ApplicationType['Renew Employment'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Renew Judicial'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Renew Reserve'] ||
      applicationStore.completeApplication.application.applicationType ===
        ApplicationType['Renew Standard']) &&
    applicationStore.completeApplication.application.status ===
      ApplicationStatus.Submitted
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
      ApplicationStatus['Contingently Denied'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Approved &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Permit Delivered'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Ready To Issue'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Modification Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Renewal Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Canceled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Modification Denied'] &&
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
      ApplicationStatus['Contingently Denied'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Modification Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Renewal Approved'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Approved &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Permit Delivered'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Ready To Issue'] &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Suspended &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Revoked &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Canceled &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus.Denied &&
    applicationStore.completeApplication.application.status !==
      ApplicationStatus['Modification Denied'] &&
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
  const expirationDateString = application.license?.expirationDate
  const currentDateTime = new Date()

  if (!expirationDateString) return false

  const expirationDate = new Date(expirationDateString)

  expirationDate.setHours(23, 59, 59, 999)

  const isActive = application.status === ApplicationStatus['Permit Delivered']

  const expiredRenewalPeriod = brandStore.brand.expiredApplicationRenewalPeriod
  const daysBeforeRenewal = brandStore.brand.daysBeforeActiveRenewal

  const renewalActiveDate = new Date(
    expirationDate.getTime() - daysBeforeRenewal * 86400000
  )

  renewalActiveDate.setHours(0, 0, 0, 0)

  const renewalDisableDate = new Date(
    expirationDate.getTime() + expiredRenewalPeriod * 86400000
  )

  renewalDisableDate.setHours(23, 59, 59, 999)

  return (
    isActive &&
    currentDateTime >= renewalActiveDate &&
    currentDateTime <= renewalDisableDate
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

const updateWithoutRouteMutation = useMutation({
  mutationFn: (updateReason: string) =>
    applicationStore.updateApplication(updateReason),
  onSuccess: () => {
    fileUploadLoading.value = false
  },
  onError: () => null,
})

const { mutate: withdrawRenewal, isLoading: isWithdrawRenewLoading } =
  useMutation({
    mutationFn: applicationStore.withdrawRenewal,
    onSuccess: () => {
      getApplications()
    },
  })

const updateMutation = useMutation({
  mutationFn: (updateReason: string) =>
    applicationStore.updateApplication(updateReason),
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
  mutationFn: (updateReason: string) =>
    applicationStore.updateApplication(updateReason),
})

const { isLoading: isRefundRequestLoading, mutateAsync: requestRefund } =
  useMutation({
    mutationFn: (refundRequest: RefundRequest) =>
      paymentStore.requestRefund(refundRequest),
  })

const renewMutation = useMutation({
  mutationFn: (updateReason: string) =>
    applicationStore.updateApplication(updateReason),
  onSuccess: async () => {
    await queryClient.invalidateQueries(['getApplicationsByUser'])
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

const { mutate: makePayment, isLoading: isMakePaymentLoading } = useMutation({
  mutationFn: () => {
    let cost: number
    let paymentType: string
    let livescanAmount: number | null | undefined

    switch (applicationStore.completeApplication.application.applicationType) {
      case ApplicationType.Standard:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .standardLivescanFee ?? brandStore.brand.cost.standardLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .standard ?? brandStore.brand.cost.new.standard
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Judicial:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Judicial Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .judicialLivescanFee ?? brandStore.brand.cost.judicialLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .judicial ?? brandStore.brand.cost.new.judicial
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Reserve:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Reserve Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .reserveLivescanFee ?? brandStore.brand.cost.reserveLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new.reserve ??
            brandStore.brand.cost.new.reserve
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType.Employment:
        if (
          applicationStore.completeApplication.application
            .readyForInitialPayment
        ) {
          paymentType =
            PaymentType['CCW Application Initial Employment Payment'].toString()
          livescanAmount =
            applicationStore.completeApplication.application.cost
              .employmentLivescanFee ??
            brandStore.brand.cost.employmentLivescanFee
          cost =
            applicationStore.completeApplication.application.cost.new
              .employment ?? brandStore.brand.cost.new.employment
        } else {
          paymentType =
            PaymentType['CCW Application Issuance Payment'].toString()
          cost =
            applicationStore.completeApplication.application.cost.issuance ??
            brandStore.brand.cost.issuance
        }

        break

      case ApplicationType['Renew Standard']:
        paymentType = PaymentType['CCW Application Renewal Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .standard ?? brandStore.brand.cost.renew.standard
        break

      case ApplicationType['Renew Judicial']:
        paymentType =
          PaymentType['CCW Application Renewal Judicial Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .judicial ?? brandStore.brand.cost.renew.judicial
        break

      case ApplicationType['Renew Reserve']:
        paymentType =
          PaymentType['CCW Application Renewal Reserve Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew.reserve ??
          brandStore.brand.cost.renew.reserve
        break

      case ApplicationType['Renew Employment']:
        paymentType =
          PaymentType['CCW Application Renewal Employment Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.renew
            .employment ?? brandStore.brand.cost.renew.employment
        break

      case ApplicationType['Modify Standard']:
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Judicial']:
        paymentType =
          PaymentType[
            'CCW Application Modification Judicial Payment'
          ].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Reserve']:
        paymentType =
          PaymentType['CCW Application Modification Reserve Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      case ApplicationType['Modify Employment']:
        paymentType =
          PaymentType[
            'CCW Application Modification Employment Payment'
          ].toString()
        cost =
          applicationStore.completeApplication.application.cost.modify ??
          brandStore.brand.cost.modify
        break

      default:
        paymentType = PaymentType['CCW Application Initial Payment'].toString()
        cost =
          applicationStore.completeApplication.application.cost.new.standard ??
          brandStore.brand.cost.new.standard
    }

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      livescanAmount,
      applicationStore.completeApplication.application.orderId,
      paymentType
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})

function handlePayment() {
  makePayment()
}

async function handleConfirmWithdrawRenewal() {
  const transaction = applicationStore.completeApplication.paymentHistory.find(
    ph => {
      return (
        ph.paymentType === PaymentType['CCW Application Renewal Payment'] ||
        ph.paymentType ===
          PaymentType['CCW Application Renewal Judicial Payment'] ||
        ph.paymentType ===
          PaymentType['CCW Application Renewal Reserve Payment'] ||
        ph.paymentType ===
          PaymentType['CCW Application Renewal Employment Payment']
      )
    }
  )

  if (transaction) {
    const refundRequest: RefundRequest = {
      id: null,
      transactionId: transaction.transactionId,
      applicationId: applicationStore.completeApplication.id,
      refundAmount: Number(transaction.amount),
      reason: 'Withdraw Renewal',
      orderId: applicationStore.completeApplication.application.orderId,
    }

    await requestRefund(refundRequest)
  }

  withdrawRenewal()
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

  await updateApplication('Withdraw Modification')
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
  router.push({
    path: Routes.FORM_ROUTE_PATH,
    query: {
      applicationId: state.application[0].id,
      isComplete: state.application[0].application.isComplete.toString(),
    },
  })

  applicationStore.completeApplication.application.currentStep = 1
  applicationStore.completeApplication.application.isUpdatingApplication = true
  applicationStore.updateApplication('Next Step')
}

function handleModifyApplication() {
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    ''
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    ''
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    ''

  router.push({
    path: Routes.MODIFY_FORM_PATH,
    query: {
      applicationId: state.application[0].id,
      isComplete: state.application[0].application.isComplete.toString(),
    },
  })
}

async function handleRenewApplication() {
  const application = applicationStore.completeApplication.application

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

  const historicalApplication: CompleteApplication = {
    ...applicationStore.completeApplication,
  }

  await addHistoricalApplicationPublic(historicalApplication)

  isRenewLoading.value = true

  application.renewalNumber += 1

  applicationStore.completeApplication.application.cost = {
    new: {
      standard: brandStore.brand.cost.new.standard,
      judicial: brandStore.brand.cost.new.judicial,
      reserve: brandStore.brand.cost.new.reserve,
      employment: brandStore.brand.cost.new.employment,
    },
    renew: {
      standard: brandStore.brand.cost.renew.standard,
      judicial: brandStore.brand.cost.renew.judicial,
      reserve: brandStore.brand.cost.renew.reserve,
      employment: brandStore.brand.cost.renew.employment,
    },
    issuance: brandStore.brand.cost.issuance,
    modify: brandStore.brand.cost.modify,
    creditFee: brandStore.brand.cost.creditFee,
    convenienceFee: brandStore.brand.cost.convenienceFee,
    standardLivescanFee: brandStore.brand.cost.standardLivescanFee,
    judicialLivescanFee: brandStore.brand.cost.judicialLivescanFee,
    reserveLivescanFee: brandStore.brand.cost.reserveLivescanFee,
    employmentLivescanFee: brandStore.brand.cost.employmentLivescanFee,
  }

  applicationStore.completeApplication.application.isUpdatingApplication = false

  applicationStore.completeApplication.application.currentStep = 1

  applicationStore.completeApplication.application.status =
    ApplicationStatus.Incomplete

  applicationStore.completeApplication.application.paymentStatus = 0

  applicationStore.completeApplication.application.appointmentStatus = 1

  applicationStore.completeApplication.application.modificationNumber = 1

  resetDocuments()
  resetAgreements()
  renewMutation.mutate('Submit Renewal')
}

function handleInfoClick() {
  if (
    applicationStore.completeApplication.application.legacyQualifyingQuestions
  ) {
    showLegacyReviewDialog()
  } else {
    showReviewDialog()
  }
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
  updateMutation.mutate('Withdraw Application')
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

  updateMutation.mutate('Submit Application')
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
  updateMutation.mutate('Cancel Appointment')
  state.cancelAppointmentDialog = false
}

function handleShowAppointmentDialog() {
  state.rescheduling = true
  refetch()
  state.appointmentDialog = true
}

function handleShowAppointmentDialogSchedule() {
  refetch()
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

  const [hours, minutes] = appointmentTime.value.split(':').map(Number)

  const date = new Date()

  date.setHours(hours)
  date.setMinutes(minutes)

  appointmentTime.value = date.toLocaleTimeString('en-US', {
    hour: 'numeric',
    minute: 'numeric',
    hour12: true,
  })

  state.snackbar = true
  state.appointmentDialog = false
  state.rescheduling = false

  const updateReason = applicationStore.completeApplication.application
    .appointmentDateTime
    ? new Date(
        applicationStore.completeApplication.application.appointmentDateTime
      ).toLocaleString()
    : 'Reschedule Appointment'

  applicationStore.updateApplication(
    `Reschedule Appointment from ${updateReason}`
  )
}

function showReviewDialog() {
  const qualifyingQuestions =
    applicationStore.completeApplication.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  if (qualifyingQuestions) {
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
}

function showLegacyReviewDialog() {
  const qualifyingQuestions =
    applicationStore.completeApplication.application.legacyQualifyingQuestions

  flaggedQuestionText.value = ''

  if (qualifyingQuestions) {
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
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-ONE')}\n\n`

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
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-TWO')}\n\n`

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
      qualifyingQuestions.questionEight.temporaryTrafficViolations.length > 0
    ) {
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-EIGHT')}\n\n`

      for (const trafficViolation of qualifyingQuestions.questionEight
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
        key !== 'questionEight' &&
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      ) {
        const questionNumber = key.slice(8)

        flaggedQuestionText.value += `Question ${i18n.t(
          `LEGACY-QUESTION-${questionNumber.toUpperCase()}`
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
}

function acceptChanges() {
  applicationStore.completeApplication.application.flaggedForCustomerReview =
    false
  applicationStore.completeApplication.application.flaggedForLicensingReview =
    true

  updateMutation.mutate('Update Qualifying Questions Flag')

  reviewDialog.value = false
}

function cancelChanges() {
  reviewDialog.value = false
}

function handleFileSubmit(fileSubmission: IFileSubmission) {
  fileUploadLoading.value = true
  const form = new FormData()

  form.append('fileToUpload', fileSubmission.file)

  const documentType = fileSubmission.fileType
  const uploadedDocs =
    applicationStore.completeApplication.application.uploadedDocuments

  const sameTypeDocs = uploadedDocs.filter(
    doc => doc.documentType === documentType
  )

  let count = 0

  sameTypeDocs.forEach(doc => {
    const match = doc.name.match(/_(\d+)$/)

    if (match) {
      const num = parseInt(match[1], 10)

      if (num > count) {
        count = num
      }
    }
  })
  const nextCount = count + 1

  const documentName = `${documentType}_${nextCount}`

  axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${documentName}`,
      form
    )
    .catch(e => {
      window.console.warn(e)
      Promise.reject()
    })

  const uploadDoc: UploadedDocType = {
    documentType: fileSubmission.fileType,
    name: documentName,
    uploadedBy: applicationStore.completeApplication.application.userEmail,
    uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
  }

  applicationStore.completeApplication.application.uploadedDocuments.push(
    uploadDoc
  )

  updateWithoutRouteMutation.mutate(
    `Upload ${fileSubmission.fileType}, ${documentName}`
  )
}

function handleSignatureSubmit() {
  fileUploadLoading.value = true
  updateWithoutRouteMutation.mutate('Updated Signature')
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
