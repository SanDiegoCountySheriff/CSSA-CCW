<template>
  <v-container
    class="px-0 py-0"
    fluid
  >
    <v-row>
      <v-col
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          v-if="props.isLoading"
          class="fill-height"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          :loading="isAddHistoricalApplicationLoading || isLoading"
          v-else
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            {{ permitStore.getPermitDetail.application.personalInfo.lastName }},
            {{ permitStore.getPermitDetail.application.personalInfo.firstName }}
          </v-card-title>

          <v-card-subtitle class="py-1">
            <v-row>
              <v-col>
                <div class="text-no-wrap">
                  Date of Birth:
                  {{
                    new Date(
                      permitStore.getPermitDetail.application.dob.birthDate
                    ).toLocaleDateString('en-US', {
                      year: '2-digit',
                      month: '2-digit',
                      day: '2-digit',
                      timeZone: 'UTC',
                    })
                  }}
                </div>
              </v-col>
              <v-col>
                <div class="text-no-wrap">
                  CII Number:
                  {{ permitStore.getPermitDetail.application.ciiNumber }}
                </div>
              </v-col>
            </v-row>
          </v-card-subtitle>

          <v-divider></v-divider>
          <v-spacer></v-spacer>

          <v-card-text>
            <v-row>
              <v-col
                cols="12"
                lg="8"
                align-self="end"
              >
                <v-row>
                  <v-col>
                    <v-menu
                      bottom
                      :elevation="10"
                    >
                      <template #activator="{ on, attrs }">
                        <v-btn
                          block
                          :loading="state.loading"
                          :disabled="readonly"
                          v-bind="attrs"
                          v-on="on"
                          color="primary"
                          small
                        >
                          <v-icon left>mdi-printer</v-icon>
                          Print Documents
                        </v-btn>
                      </template>

                      <v-list>
                        <v-list-item @click="printPdf('printApplicationApi')">
                          <v-list-item-title>
                            Print Application
                          </v-list-item-title>
                        </v-list-item>

                        <v-list-item
                          v-if="isApplicationModification"
                          @click="printPdf('printModificationApi')"
                        >
                          <v-list-item-title>
                            Print Modification
                          </v-list-item-title>
                        </v-list-item>

                        <v-list-item
                          :style="{
                            color: isOfficialLicenseMissingInformation
                              ? 'gray'
                              : 'inherit',
                            cursor: isOfficialLicenseMissingInformation
                              ? 'default'
                              : 'pointer',
                          }"
                          @click.prevent="
                            !isOfficialLicenseMissingInformation &&
                              printPdf('printOfficialLicenseApi')
                          "
                        >
                          <v-tooltip
                            v-if="isOfficialLicenseMissingInformation"
                            bottom
                          >
                            <template #activator="{ on }">
                              <span v-on="on">Print Official License</span>
                            </template>
                            <span>{{ tooltipText }}</span>
                          </v-tooltip>
                          <span v-else>Print Official License</span>
                        </v-list-item>

                        <v-list-item
                          :style="{
                            color: isUnofficialLicenseMissingInformation
                              ? 'gray'
                              : 'inherit',
                            cursor: isUnofficialLicenseMissingInformation
                              ? 'default'
                              : 'pointer',
                          }"
                          @click.prevent="
                            !isUnofficialLicenseMissingInformation &&
                              printPdf('printUnofficialLicenseApi')
                          "
                        >
                          <v-tooltip
                            v-if="isUnofficialLicenseMissingInformation"
                            bottom
                          >
                            <template #activator="{ on }">
                              <span v-on="on">Print Unofficial License</span>
                            </template>
                            <span>{{ tooltipText }}</span>
                          </v-tooltip>
                          <span v-else>Print Unofficial License</span>
                        </v-list-item>
                        <v-list-item @click="printPdf('printLiveScanApi')">
                          <v-list-item-title>
                            Print LiveScan Document
                          </v-list-item-title>
                        </v-list-item>
                        <v-list-item
                          @click="printPdf('printRevocationLetterApi')"
                        >
                          <v-list-item-title>
                            Print Revocation
                          </v-list-item-title>
                        </v-list-item>
                      </v-list>
                    </v-menu>
                  </v-col>
                </v-row>
              </v-col>

              <v-col
                cols="12"
                lg="4"
              >
                <v-img
                  v-if="userPhoto"
                  :src="userPhoto"
                  alt="user_photo"
                  contain
                />
                <v-img
                  v-else
                  src="../../../../../../../apps/admin/public/img/icons/no-photo.png"
                  alt="user_photo_not_found"
                  contain
                />
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        v-if="isModify"
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          v-if="props.isLoading"
          class="fill-height"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            <v-icon
              v-if="!isModificationComplete"
              color="primary"
              class="mr-2"
            >
              mdi-shield-alert
            </v-icon>

            <v-icon
              v-else
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
            Modification
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-row>
              <v-col>
                <v-btn
                  v-if="modificationReadyForApproval"
                  :disabled="readonly"
                  @click="handleApproveModification"
                  color="primary"
                  block
                  small
                >
                  <v-icon left>mdi-check-bold</v-icon>
                  Approve Modification
                </v-btn>

                <v-alert
                  v-if="modificationMissingChecklistItems"
                  :disabled="readonly"
                  color="primary"
                  type="info"
                  dense
                  outlined
                >
                  <span
                    :class="
                      themeStore.getThemeConfig.isDark ? 'white--text' : ''
                    "
                  >
                    Approve Checklist Items Next
                  </span>
                </v-alert>

                <FinishModificationDialog
                  :disabled="!isModificationPaymentComplete"
                  v-if="
                    permitStore.getPermitDetail.application.status ===
                    ApplicationStatus['Modification Approved']
                  "
                  @handle-finish-modification="handleFinishModification"
                />
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                <v-btn
                  @click="emit('on-check-name')"
                  :disabled="!isModifyingName || readonly"
                  :color="
                    permitStore.getPermitDetail.application.modifiedNameComplete
                      ? 'success'
                      : 'primary'
                  "
                  small
                  block
                >
                  <v-icon
                    v-if="
                      !permitStore.getPermitDetail.application
                        .modifiedNameComplete
                    "
                    left
                  >
                    mdi-rename
                  </v-icon>

                  <v-icon
                    v-else
                    left
                  >
                    mdi-check
                  </v-icon>
                  Check Name
                </v-btn>
              </v-col>

              <v-col>
                <v-btn
                  @click="emit('on-check-address')"
                  :disabled="!isModifyingAddress || readonly"
                  :color="
                    permitStore.getPermitDetail.application
                      .modifiedAddressComplete
                      ? 'success'
                      : 'primary'
                  "
                  small
                  block
                >
                  <v-icon
                    v-if="
                      !permitStore.getPermitDetail.application
                        .modifiedAddressComplete
                    "
                    left
                  >
                    mdi-map-marker
                  </v-icon>

                  <v-icon
                    v-else
                    left
                  >
                    mdi-check
                  </v-icon>
                  Check Address
                </v-btn>
              </v-col>
            </v-row>

            <v-row>
              <v-col>
                <v-btn
                  @click="emit('on-check-weapons')"
                  :disabled="!isModifyingWeapon || readonly"
                  :color="
                    permitStore.getPermitDetail.application
                      .modifiedWeaponComplete
                      ? 'success'
                      : 'primary'
                  "
                  small
                  block
                >
                  <v-icon
                    v-if="
                      !permitStore.getPermitDetail.application
                        .modifiedWeaponComplete
                    "
                    left
                  >
                    mdi-invoice-list
                  </v-icon>

                  <v-icon
                    v-else
                    left
                  >
                    mdi-check
                  </v-icon>
                  Check Weapons
                </v-btn>
              </v-col>

              <v-col>
                <v-btn
                  @click="emit('on-check-documents')"
                  :disabled="readonly"
                  :color="
                    permitStore.getPermitDetail.application.status ===
                    ApplicationStatus['Modification Approved']
                      ? 'success'
                      : 'primary'
                  "
                  small
                  block
                >
                  <v-icon left>
                    {{
                      permitStore.getPermitDetail.application.status ===
                      ApplicationStatus['Modification Approved']
                        ? 'mdi-check-bold'
                        : 'mdi-file-document-check'
                    }}
                  </v-icon>
                  Check Documents
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        v-else-if="isRenew"
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          :loading="isAppointmentLoading"
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            <v-icon
              v-if="
                permitStore.getPermitDetail.application.status !==
                ApplicationStatus['Renewal Approved']
              "
              color="primary"
              class="mr-2"
            >
              mdi-shield-alert
            </v-icon>

            <v-icon
              v-else
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
            Renewal
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-row>
              <v-col>
                <v-btn
                  v-if="renewalReadyForApproval"
                  @click="handleApproveRenewal"
                  color="primary"
                  block
                  small
                >
                  <v-icon left>mdi-check-bold</v-icon>
                  Approve Renewal
                </v-btn>

                <v-alert
                  v-if="renewalMissingChecklistItems"
                  color="primary"
                  type="info"
                  dense
                  outlined
                >
                  <span
                    :class="
                      themeStore.getThemeConfig.isDark ? 'white--text' : ''
                    "
                  >
                    Approve Checklist Items Next
                  </span>
                </v-alert>

                <FinishRenewalDialog
                  :disabled="!isRenewalPaymentComplete"
                  v-if="
                    permitStore.getPermitDetail.application.status ===
                    ApplicationStatus['Renewal Approved']
                  "
                  @handle-finish-renewal="handleFinishRenewal"
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-btn
                  @click="emit('on-check-documents')"
                  :color="
                    permitStore.getPermitDetail.application.status ===
                    ApplicationStatus['Renewal Approved']
                      ? 'success'
                      : 'primary'
                  "
                  block
                  small
                >
                  <v-icon left>
                    {{
                      permitStore.getPermitDetail.application.status ===
                      ApplicationStatus['Renewal Approved']
                        ? 'mdi-check-bold'
                        : 'mdi-file-document-check'
                    }}
                  </v-icon>
                  Check Documents
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  @click="emit('on-check-questions')"
                  :color="
                    permitStore.getPermitDetail.application.status ===
                    ApplicationStatus['Renewal Approved']
                      ? 'success'
                      : 'primary'
                  "
                  block
                  small
                >
                  <v-icon left>
                    {{
                      permitStore.getPermitDetail.application.status ===
                      ApplicationStatus['Renewal Approved']
                        ? 'mdi-check-bold'
                        : 'mdi-file-document-check'
                    }}
                  </v-icon>
                  Check Survey Details
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        v-else
        cols="4"
        class="pt-0 pr-0"
      >
        <v-card
          v-if="props.isLoading"
          class="fill-height"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else-if="
            permitStore.getPermitDetail.application.status ===
            ApplicationStatus['Permit Delivered']
          "
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            <v-icon
              color="primary"
              class="mr-2"
            >
              mdi-clock-alert-outline
            </v-icon>
            Expiration Date
          </v-card-title>

          <v-card-title class="justify-center">
            {{
              permitStore.getPermitDetail.application.license.expirationDate
                ? new Date(
                    permitStore.getPermitDetail.application.license.expirationDate
                  ).toLocaleDateString('en-US', {
                    month: 'long',
                    day: 'numeric',
                    year: 'numeric',
                  })
                : 'Invalid Expiration Date, Please Update.'
            }}
          </v-card-title>

          <v-spacer />

          <v-card-text>
            <v-row>
              <v-col>
                <ExpirationDateDialog
                  @handle-save-expiration-date="handleSaveExpirationDate"
                />
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>

        <v-card
          v-else-if="isAppointmentComplete"
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center">
            <v-icon
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
            Next Steps
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-row>
              <v-col>
                <v-btn
                  v-if="showStart90DayCountdownButton"
                  :disabled="readonly"
                  @click="handleStart90DayCountdown"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-timer</v-icon>
                  Start 90 Days
                </v-btn>

                <v-btn
                  v-else-if="showPause90DayCountdownButton"
                  :disabled="readonly"
                  @click="pause90DayCountdown"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-pause</v-icon>
                  Pause 90 Days
                </v-btn>

                <v-btn
                  v-else-if="showReactivate90DayCountdownButton"
                  :disabled="readonly"
                  @click="reactivate90DayCountdown"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-play</v-icon>
                  Reactivate 90 Days
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>

        <v-card
          v-else
          :loading="isAppointmentLoading"
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title class="justify-center mb-0 pb-0">
            <v-icon
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
            Appointment
          </v-card-title>

          <v-card-title
            v-if="permitStore.getPermitDetail.application.appointmentDateTime"
            class="justify-center"
          >
            {{ appointmentTime }} on {{ appointmentDate }}
          </v-card-title>

          <v-card-title
            v-else
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Not Scheduled
          </v-card-title>

          <v-spacer></v-spacer>

          <v-card-text class="text-center">
            <v-row>
              <v-col>
                <v-btn
                  v-if="
                    permitStore.getPermitDetail.application
                      .appointmentStatus !== 3
                  "
                  :disabled="readonly"
                  @click="handleCheckIn"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-check-bold</v-icon>
                  Check In
                </v-btn>
                <v-btn
                  v-else
                  :disabled="readonly"
                  @click="handleSetAppointmentScheduled"
                  small
                  block
                  color="primary"
                >
                  <v-icon left>mdi-undo</v-icon>
                  Undo Check In
                </v-btn>
              </v-col>
              <v-col>
                <v-btn
                  v-if="
                    permitStore.getPermitDetail.application
                      .appointmentStatus !== 4
                  "
                  :disabled="readonly"
                  @click="handleNoShow"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-close-thick</v-icon>
                  No Show
                </v-btn>
                <v-btn
                  v-else
                  :disabled="readonly"
                  @click="handleSetAppointmentScheduled"
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-undo</v-icon>
                  Undo No Show
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <DateTimePicker @on-save-reschedule="handleSaveReschedule" />
              </v-col>
              <v-col>
                <Schedule />
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col
        cols="4"
        class="pt-0"
      >
        <v-card
          v-if="props.isLoading"
          class="fill-height"
          outlined
        >
          <v-skeleton-loader type="list-item,divider,list-item" />
        </v-card>

        <v-card
          v-else
          class="d-flex flex-column fill-height"
          outlined
          :loading="props.isLoading"
        >
          <v-card-title
            v-if="
              !permitStore.getPermitDetail.application.isComplete &&
              !permitStore.getPermitDetail.isMatchUpdated === false
            "
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Missing Requirement
          </v-card-title>

          <v-card-title
            v-else-if="permitStore.getPermitDetail.isMatchUpdated === false"
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Waiting for Customer Verification
          </v-card-title>

          <v-card-title
            v-else-if="
              permitStore.getPermitDetail.application.flaggedForCustomerReview
            "
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Flagged for Applicant Review
          </v-card-title>

          <v-card-title
            v-else-if="
              permitStore.getPermitDetail.application.flaggedForLicensingReview
            "
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Review Survey Details
          </v-card-title>

          <v-card-title
            v-else-if="waitingForPayment"
            class="justify-center"
          >
            <v-icon
              color="error"
              class="mr-2"
            >
              mdi-alert
            </v-icon>
            Waiting for Customer Payment
          </v-card-title>

          <v-card-title
            v-else
            class="justify-center"
          >
            <v-icon
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
            Requirements Fulfilled
          </v-card-title>

          <v-card-text
            v-if="
              permitStore.getPermitDetail.application
                .startOfNinetyDayCountdown &&
              !permitStore.getPermitDetail.application.ninetyDayCountdownPaused
            "
            class="text-center"
          >
            {{ daysLeft }} day{{ daysLeft > 1 ? 's' : '' }} left to complete
            application before it expires
          </v-card-text>

          <v-card-text
            v-if="
              permitStore.getPermitDetail.application.ninetyDayCountdownPaused
            "
            class="text-center"
          >
            90 day countdown paused on
            {{
              permitStore.getPermitDetail.application
                .ninetyDayCountdownPausedDate
                ? new Date(
                    permitStore.getPermitDetail.application.ninetyDayCountdownPausedDate
                  ).toLocaleDateString()
                : ''
            }}
          </v-card-text>

          <v-card-text
            v-if="permitStore.getPermitDetail.application.assignedTo"
            class="text-center"
          >
            Assigned to:
            {{ permitStore.getPermitDetail.application.assignedTo }}
          </v-card-text>

          <v-spacer></v-spacer>

          <v-card-text>
            <v-row>
              <v-col
                v-if="
                  permitStore.getPermitDetail.application.status ===
                    ApplicationStatus.Submitted &&
                  !permitStore.getPermitDetail.application
                    .readyForInitialPayment &&
                  !isInitialPaymentComplete &&
                  !isRenew &&
                  !isModify
                "
                cols="12"
              >
                <ReadyForPaymentDialog
                  @on-ready-for-payment="handleReadyForInitialPayment"
                />
              </v-col>

              <v-col
                v-else-if="
                  !permitStore.getPermitDetail.application
                    .readyForRenewalPayment &&
                  !isRenewalPaymentComplete &&
                  isRenew
                "
                cols="12"
              >
                <ReadyForPaymentDialog
                  @on-ready-for-payment="handleReadyForRenewalPayment"
                />
              </v-col>

              <v-col
                v-else-if="
                  !permitStore.getPermitDetail.application
                    .readyForModificationPayment &&
                  !isModificationPaymentComplete &&
                  isModify
                "
                cols="12"
              >
                <ReadyForPaymentDialog
                  @on-ready-for-payment="handleReadyForModificationPayment"
                />
              </v-col>
            </v-row>

            <v-row>
              <v-col cols="12">
                <v-menu offset-y>
                  <template #activator="{ on }">
                    <v-btn
                      :disabled="readonly"
                      color="primary"
                      v-on="on"
                      small
                      block
                    >
                      <v-icon left>mdi-clipboard-account</v-icon>
                      {{ 'Assign User' }}
                    </v-btn>
                  </template>
                  <v-list>
                    <v-list-item
                      v-for="(adminUser, index) in adminUserStore.allAdminUsers"
                      :key="index"
                      @click="handleAssignApplication(adminUser.name)"
                    >
                      <v-list-item-title>
                        {{ adminUser.name }}
                      </v-list-item-title>
                    </v-list-item>
                  </v-list>
                </v-menu>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-dialog
      v-model="ninetyDayDialog"
      persistent
      max-width="300"
    >
      <v-card>
        <v-card-title>90 Day Countdown Begins</v-card-title>
        <v-card-text>
          <v-row>
            <v-col> This will begin a 90 day countdown, are you sure? </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-radio-group v-model="ninetyDayStartDateSelection">
                <v-radio
                  label="Start now"
                  value="startNow"
                />
                <v-radio
                  label="Start on submission date"
                  value="startSubmissionDate"
                />
              </v-radio-group>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-btn
            color="error"
            text
            @click="handle90DayCountdownDeny"
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            color="primary"
            text
            @click="handle90DayCountdownConfirm"
          >
            Start 90 Days
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import DateTimePicker from '@core-admin/components/appointment/DateTimePicker.vue'
import ExpirationDateDialog from '@core-admin/components/dialogs/ExpirationDateDialog.vue'
import FinishModificationDialog from '@core-admin/components/dialogs/FinishModificationDialog.vue'
import FinishRenewalDialog from '@core-admin/components/dialogs/FinishRenewalDialog.vue'
import ReadyForPaymentDialog from '@core-admin/components/dialogs/ReadyForPaymentDialog.vue'
import Schedule from '@core-admin/components/appointment/Schedule.vue'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import {
  ApplicationStatus,
  AppointmentStatus,
  AppointmentWindowCreateRequestModel,
  PaymentType,
} from '@shared-utils/types/defaultTypes'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, inject, reactive, ref } from 'vue'
import {
  getOriginalApplicationTypeModification,
  getOriginalApplicationTypeRenwal,
} from '@shared-ui/composables/getOriginalApplicationType'

interface IPermitCard2Props {
  isLoading: boolean
  userPhoto: string
}

const props = withDefaults(defineProps<IPermitCard2Props>(), {
  isLoading: false,
  userPhoto: '',
})

const readonly = inject('readonly')

const emit = defineEmits([
  'refetch',
  'on-check-name',
  'on-check-address',
  'on-check-weapons',
  'on-check-documents',
  'on-check-questions',
])

const state = reactive({
  isSelecting: false,
  loading: false,
  snack: false,
  snackColor: '',
  snackText: '',
  multiLine: false,
  text: `Invalid file type provided.`,
})

const ninetyDayStartDateSelection = ref(null)
const ninetyDayDialog = ref(false)
const permitStore = usePermitsStore()
const appointmentStore = useAppointmentsStore()
const adminUserStore = useAdminUserStore()
const themeStore = useThemeStore()
const changed = ref('')

const isInitialPaymentComplete = computed(() => {
  if (permitStore.permitDetail.paymentHistory) {
    return (
      permitStore.permitDetail.paymentHistory.some(ph => {
        return (
          (ph.paymentType === 0 ||
            ph.paymentType === 1 ||
            ph.paymentType === 2 ||
            ph.paymentType === 3 ||
            ph.paymentType === 8 ||
            ph.paymentType === 9 ||
            ph.paymentType === 10 ||
            ph.paymentType === 11) &&
          ph.successful === true
        )
      }) || permitStore.permitDetail.application.paymentStatus === 1
    )
  }

  return false
})

const isRenewalPaymentComplete = computed(() => {
  return (
    permitStore.permitDetail.paymentHistory.some(ph => {
      return (
        (ph.paymentType === PaymentType['CCW Application Renewal Payment'] ||
          ph.paymentType ===
            PaymentType['CCW Application Renewal Employment Payment'] ||
          ph.paymentType ===
            PaymentType['CCW Application Renewal Judicial Payment'] ||
          ph.paymentType ===
            PaymentType['CCW Application Renewal Reserve Payment']) &&
        ph.successful === true
      )
    }) || permitStore.permitDetail.application.paymentStatus === 1
  )
})

const isModificationPaymentComplete = computed(() => {
  return (
    permitStore.permitDetail.paymentHistory.some(ph => {
      return (
        ((ph.paymentType ===
          PaymentType['CCW Application Modification Payment'] &&
          ph.modificationNumber ===
            permitStore.permitDetail.application.modificationNumber) ||
          (ph.paymentType ===
            PaymentType['CCW Application Modification Employment Payment'] &&
            ph.modificationNumber ===
              permitStore.permitDetail.application.modificationNumber) ||
          (ph.paymentType ===
            PaymentType['CCW Application Modification Judicial Payment'] &&
            ph.modificationNumber ===
              permitStore.permitDetail.application.modificationNumber) ||
          (ph.paymentType ===
            PaymentType['CCW Application Modification Reserve Payment'] &&
            ph.modificationNumber ===
              permitStore.permitDetail.application.modificationNumber)) &&
        ph.successful === true
      )
    }) || permitStore.permitDetail.application.paymentStatus === 1
  )
})

const isRenew = computed(() => {
  const applicationType =
    permitStore.getPermitDetail.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const isModify = computed(() => {
  const applicationType =
    permitStore.getPermitDetail.application.applicationType

  return (
    applicationType === ApplicationType['Modify Standard'] ||
    applicationType === ApplicationType['Modify Reserve'] ||
    applicationType === ApplicationType['Modify Judicial'] ||
    applicationType === ApplicationType['Modify Employment']
  )
})

const isAppointmentComplete = computed(() => {
  return (
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Appointment Complete'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Background In Progress'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Contingently Denied'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Contingently Approved'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Approved ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Permit Delivered'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Suspended ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Revoked ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Canceled ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Denied ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus.Withdrawn ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Ready To Issue'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Modification Approved'] ||
    permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Renewal Approved'] ||
    (permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Waiting For Customer'] &&
      permitStore.getPermitDetail.application.appointmentDateTime !== null &&
      permitStore.getPermitDetail.application.appointmentDateTime <
        new Date().toISOString()) ||
    (permitStore.getPermitDetail.application.status ===
      ApplicationStatus['Flagged For Review'] &&
      permitStore.getPermitDetail.application.appointmentDateTime !== null &&
      permitStore.getPermitDetail.application.appointmentDateTime <
        new Date().toISOString())
  )
})

const { mutate: updatePermitDetails, isLoading } = useMutation({
  mutationFn: () =>
    permitStore.updatePermitDetailApi(`Updated ${changed.value}`),
})

const { mutate: deleteSlotByApplicationId } = useMutation({
  mutationFn: (applicationId: string) =>
    appointmentStore.deleteSlotByApplicationId(applicationId),
})

const {
  mutateAsync: addHistoricalApplication,
  isLoading: isAddHistoricalApplicationLoading,
} = useMutation({
  mutationFn: (application: CompleteApplication) =>
    permitStore.addHistoricalApplication(application),
})

const { mutate: reopenSlotByApplicationId } = useMutation({
  mutationFn: (applicationId: string) =>
    appointmentStore.putReopenSlotByApplicationId(applicationId),
})

const { mutateAsync: createManualAppointment } = useMutation({
  mutationFn: async (appointment: AppointmentWindowCreateRequestModel) => {
    return await appointmentStore.putCreateManualAppointment(appointment)
  },
})

const { mutate: checkInAppointment, isLoading: isCheckInLoading } = useMutation(
  {
    mutationFn: (appointmentId: string) =>
      appointmentStore.putCheckInAppointment(appointmentId).then(() => {
        changed.value = 'checked in appointment'
        updatePermitDetails()
      }),
  }
)

const {
  mutate: setAppointmentScheduled,
  isLoading: isAppointmentScheduledLoading,
} = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentStore.putSetAppointmentScheduled(appointmentId).then(() => {
      changed.value = 'appointment time'
      updatePermitDetails()
    }),
})

const { mutate: noShowAppointment, isLoading: isNoShowLoading } = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentStore.putNoShowAppointment(appointmentId).then(() => {
      changed.value = 'no show appointment'
      updatePermitDetails()
    }),
})

async function handleApproveModification() {
  const historicalApplication: CompleteApplication = {
    ...permitStore.getPermitDetail,
  }

  const app = permitStore.getPermitDetail.application

  await addHistoricalApplication(historicalApplication)

  app.status = ApplicationStatus['Modification Approved']

  changed.value = 'Application Status - Modification Approved'

  if (app.personalInfo.modifiedFirstName) {
    app.personalInfo.firstName = app.personalInfo.modifiedFirstName
    app.personalInfo.modifiedFirstName = ''
  }

  if (app.personalInfo.modifiedMiddleName) {
    app.personalInfo.middleName = app.personalInfo.modifiedMiddleName
    app.personalInfo.modifiedMiddleName = ''
  }

  if (app.personalInfo.modifiedLastName) {
    app.personalInfo.lastName = app.personalInfo.modifiedLastName
    app.personalInfo.modifiedLastName = ''
  }

  app.modifiedNameComplete = null

  if (app.modifiedAddress.streetAddress) {
    app.currentAddress.streetAddress = app.modifiedAddress.streetAddress
    app.modifiedAddress.streetAddress = ''
  }

  if (app.modifiedAddress.city) {
    app.currentAddress.city = app.modifiedAddress.city
    app.modifiedAddress.city = ''
  }

  if (app.modifiedAddress.state) {
    app.currentAddress.state = app.modifiedAddress.state
    app.modifiedAddress.state = ''
  }

  if (app.modifiedAddress.zip) {
    app.currentAddress.zip = app.modifiedAddress.zip
    app.modifiedAddress.zip = ''
  }

  if (app.modifiedAddress.county) {
    app.currentAddress.county = app.modifiedAddress.county
    app.modifiedAddress.county = ''
  }

  if (app.modifiedAddress.country) {
    app.currentAddress.country = app.modifiedAddress.country
    app.modifiedAddress.country = ''
  }

  app.modifiedAddressComplete = null

  for (const weapon of app.modifyAddWeapons) {
    weapon.added = undefined
    weapon.deleted = undefined
    app.weapons.push(weapon)
  }

  app.modifyAddWeapons = []

  for (const weapon of app.modifyDeleteWeapons) {
    app.weapons = app.weapons.filter(w => {
      return w.serialNumber !== weapon.serialNumber
    })
  }

  app.modifyDeleteWeapons = []
  app.modifiedWeaponComplete = null
  app.currentStep = 1
  app.modificationNumber += 1

  updatePermitDetails()
}

function handleApproveRenewal() {
  permitStore.getPermitDetail.application.status =
    ApplicationStatus['Renewal Approved']

  changed.value = 'Application Status - Renewal Approved'

  updatePermitDetails()
}

async function handleFinishModification() {
  const app = permitStore.getPermitDetail.application

  app.status = ApplicationStatus['Permit Delivered']

  changed.value = 'Modification - Permit Delivered'

  app.applicationType = getOriginalApplicationTypeModification(
    app.applicationType
  )

  updatePermitDetails()
}

function handleFinishRenewal() {
  const app = permitStore.getPermitDetail.application

  app.status = ApplicationStatus['Permit Delivered']

  app.applicationType = getOriginalApplicationTypeRenwal(app.applicationType)

  changed.value = 'Renewal - Permit Delivered'

  if (app.isRenewingWithLegacyQuestions) {
    permitStore.getPermitDetail.application.isRenewingWithLegacyQuestions =
      false
  }

  updatePermitDetails()
}

function handleSaveExpirationDate(expirationDate: string) {
  changed.value = 'Expiration Date'

  const [year, month, day] = expirationDate.split('-').map(Number)

  const date = new Date(year, month - 1, day)

  permitStore.getPermitDetail.application.license.expirationDate =
    date.toISOString()

  updatePermitDetails()
}

const waitingForPayment = computed(() => {
  return (
    permitStore.getPermitDetail.application.readyForInitialPayment === true ||
    permitStore.getPermitDetail.application.readyForRenewalPayment === true ||
    permitStore.getPermitDetail.application.readyForModificationPayment === true
  )
})

const isApplicationModification = computed(() => {
  return (
    permitStore.getPermitDetail.application.applicationType ===
      ApplicationType['Modify Standard'] ||
    permitStore.getPermitDetail.application.applicationType ===
      ApplicationType['Modify Reserve'] ||
    permitStore.getPermitDetail.application.applicationType ===
      ApplicationType['Modify Judicial'] ||
    permitStore.getPermitDetail.application.applicationType ===
      ApplicationType['Modify Employment']
  )
})

const modificationReadyForApproval = computed(() => {
  const checkListAddressComplete =
    permitStore.getPermitDetail.application.modifiedAddressComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.proofOfResidency
      ?.value === true

  const checkListNameComplete =
    permitStore.getPermitDetail.application.modifiedNameComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.proofOfID?.value ===
      true

  const checkListWeaponComplete =
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.firearms?.value ===
      true

  return (
    permitStore.getPermitDetail.application.modifiedNameComplete !== false &&
    permitStore.getPermitDetail.application.modifiedAddressComplete !== false &&
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== false &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved'] &&
    checkListAddressComplete &&
    checkListNameComplete &&
    checkListWeaponComplete
  )
})

const renewalReadyForApproval = computed(() => {
  const proofOfIDComplete =
    permitStore.getPermitDetail.application.backgroundCheck.proofOfID?.value ===
    true

  const proofOfResidencyComplete =
    permitStore.getPermitDetail.application.backgroundCheck.proofOfResidency
      ?.value === true

  const ncicWantsWarrantsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.ncicWantsWarrants
      ?.value === true

  const localsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.locals?.value ===
    true

  const dmvRecordComplete =
    permitStore.getPermitDetail.application.backgroundCheck.dmvRecord?.value ===
    true

  const crimeTracerComplete =
    permitStore.getPermitDetail.application.backgroundCheck.crimeTracer
      ?.value === true

  const trafficCourtPortalComplete =
    permitStore.getPermitDetail.application.backgroundCheck.trafficCourtPortal
      ?.value === true

  const liveScanComplete =
    permitStore.getPermitDetail.application.backgroundCheck.livescan?.value ===
    true

  const sr14Complete =
    permitStore.getPermitDetail.application.backgroundCheck.sR14?.value === true

  const firearmsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.firearms?.value ===
    true

  const safetyCertificateComplete =
    permitStore.getPermitDetail.application.backgroundCheck.safetyCertificate
      ?.value === true

  return (
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Renewal Approved'] &&
    proofOfIDComplete &&
    proofOfResidencyComplete &&
    ncicWantsWarrantsComplete &&
    localsComplete &&
    dmvRecordComplete &&
    crimeTracerComplete &&
    trafficCourtPortalComplete &&
    liveScanComplete &&
    sr14Complete &&
    firearmsComplete &&
    safetyCertificateComplete
  )
})

const modificationMissingChecklistItems = computed(() => {
  const checkListAddressComplete =
    permitStore.getPermitDetail.application.modifiedAddressComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.proofOfResidency
      ?.value === true

  const checkListNameComplete =
    permitStore.getPermitDetail.application.modifiedNameComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.proofOfID?.value ===
      true

  const checkListWeaponComplete =
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== false &&
    permitStore.getPermitDetail.application.backgroundCheck.firearms?.value ===
      true

  return (
    permitStore.getPermitDetail.application.modifiedNameComplete !== false &&
    permitStore.getPermitDetail.application.modifiedAddressComplete !== false &&
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== false &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved'] &&
    (!checkListAddressComplete ||
      !checkListNameComplete ||
      !checkListWeaponComplete)
  )
})

const renewalMissingChecklistItems = computed(() => {
  const proofOfIDComplete =
    permitStore.getPermitDetail.application.backgroundCheck.proofOfID?.value ===
    true

  const proofOfResidencyComplete =
    permitStore.getPermitDetail.application.backgroundCheck.proofOfResidency
      ?.value === true

  const ncicWantsWarrantsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.ncicWantsWarrants
      ?.value === true

  const localsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.locals?.value ===
    true

  const dmvRecordComplete =
    permitStore.getPermitDetail.application.backgroundCheck.dmvRecord?.value ===
    true

  const crimeTracerComplete =
    permitStore.getPermitDetail.application.backgroundCheck.crimeTracer
      ?.value === true

  const trafficCourtPortalComplete =
    permitStore.getPermitDetail.application.backgroundCheck.trafficCourtPortal
      ?.value === true

  const liveScanComplete =
    permitStore.getPermitDetail.application.backgroundCheck.livescan?.value ===
    true

  const sr14Complete =
    permitStore.getPermitDetail.application.backgroundCheck.sR14?.value === true

  const firearmsComplete =
    permitStore.getPermitDetail.application.backgroundCheck.firearms?.value ===
    true

  const safetyCertificateComplete =
    permitStore.getPermitDetail.application.backgroundCheck.safetyCertificate
      ?.value === true

  return (
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Renewal Approved'] &&
    (!proofOfIDComplete ||
      !proofOfResidencyComplete ||
      !ncicWantsWarrantsComplete ||
      !localsComplete ||
      !dmvRecordComplete ||
      !crimeTracerComplete ||
      !trafficCourtPortalComplete ||
      !liveScanComplete ||
      !sr14Complete ||
      !firearmsComplete ||
      !safetyCertificateComplete)
  )
})

const isModificationComplete = computed(() => {
  return (
    permitStore.getPermitDetail.application.modifiedNameComplete !== false &&
    permitStore.getPermitDetail.application.modifiedAddressComplete !== false &&
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== false
  )
})

const showStart90DayCountdownButton = computed(() => {
  return (
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown ===
      null ||
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown ===
      undefined
  )
})

const isModifyingName = computed(() => {
  return permitStore.getPermitDetail.application.modifiedNameComplete !== null
})

const isModifyingAddress = computed(() => {
  return (
    permitStore.getPermitDetail.application.modifiedAddressComplete !== null
  )
})

const isModifyingWeapon = computed(() => {
  return permitStore.getPermitDetail.application.modifiedWeaponComplete !== null
})

function handleStart90DayCountdown() {
  ninetyDayDialog.value = true
}

function handle90DayCountdownConfirm() {
  changed.value = 'Start 90 Day Countdown'

  if (ninetyDayStartDateSelection.value === 'startNow') {
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown =
      new Date(Date.now()).toISOString()
  } else if (ninetyDayStartDateSelection.value === 'startSubmissionDate') {
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown =
      permitStore.getPermitDetail.application.submittedToLicensingDateTime
  }

  ninetyDayDialog.value = false
  updatePermitDetails()
}

function handle90DayCountdownDeny() {
  ninetyDayDialog.value = false
}

const showPause90DayCountdownButton = computed(() => {
  return (
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown !==
      null &&
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown !==
      undefined &&
    permitStore.getPermitDetail.application.ninetyDayCountdownPaused === false
  )
})

function pause90DayCountdown() {
  changed.value = 'Pause 90 day countdown'

  permitStore.getPermitDetail.application.ninetyDayCountdownPaused = true
  permitStore.getPermitDetail.application.ninetyDayCountdownPausedDate =
    new Date(Date.now()).toISOString()

  updatePermitDetails()
}

const showReactivate90DayCountdownButton = computed(() => {
  return (
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown !==
      null &&
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown !==
      undefined &&
    permitStore.getPermitDetail.application.ninetyDayCountdownPaused === true
  )
})

function reactivate90DayCountdown() {
  changed.value = 'Reactivate 90 day countdown'

  permitStore.getPermitDetail.application.ninetyDayCountdownPaused = false

  const original90DayCountdownDate = new Date(
    permitStore.getPermitDetail.application.startOfNinetyDayCountdown as string
  )
  const paused90DayCountdownDate = new Date(
    permitStore.getPermitDetail.application
      .ninetyDayCountdownPausedDate as string
  )
  const differenceInDays = Math.floor(
    Math.abs(
      (original90DayCountdownDate.getTime() -
        paused90DayCountdownDate.getTime()) /
        (1000 * 60 * 60 * 24)
    )
  )

  original90DayCountdownDate.setDate(
    original90DayCountdownDate.getDate() + differenceInDays
  )
  permitStore.getPermitDetail.application.startOfNinetyDayCountdown =
    original90DayCountdownDate.toISOString()
  permitStore.getPermitDetail.application.ninetyDayCountdownPausedDate = null

  updatePermitDetails()
}

const isAppointmentLoading = computed(() => {
  return (
    isNoShowLoading.value ||
    isCheckInLoading.value ||
    isAppointmentScheduledLoading.value
  )
})

const isOfficialLicenseMissingInformation = computed(() => {
  if (permitStore.getPermitDetail.application.uploadedDocuments) {
    const uploadedDocuments =
      permitStore.getPermitDetail.application.uploadedDocuments
    const missingThumbprint = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('thumbprint') !== -1
    )
    const missingPortrait = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('portrait') !== -1
    )

    return missingThumbprint || missingPortrait
  }

  return true
})

const isUnofficialLicenseMissingInformation = computed(() => {
  if (permitStore.getPermitDetail.application.uploadedDocuments) {
    const uploadedDocuments =
      permitStore.getPermitDetail.application.uploadedDocuments
    const missingThumbprint = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('thumbprint') !== -1
    )
    const missingPortrait = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('portrait') !== -1
    )

    return missingThumbprint || missingPortrait
  }

  return true
})

const tooltipText = computed(() => {
  if (permitStore.getPermitDetail.application.uploadedDocuments) {
    const uploadedDocuments =
      permitStore.getPermitDetail.application.uploadedDocuments
    const missingThumbprint = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('thumbprint') !== -1
    )
    const missingPortrait = !uploadedDocuments.some(
      doc => doc.documentType.toLowerCase().indexOf('portrait') !== -1
    )

    let output = ''

    if (missingThumbprint && missingPortrait) {
      output = 'Please upload both the Thumbprint and Portrait documents.'
    } else if (missingThumbprint) {
      output = 'Please upload the Thumbprint document.'
    } else if (missingPortrait) {
      output = 'Please upload the Portrait document.'
    }

    return output
  }

  return ''
})

function handleCheckIn() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus['Checked In']
    checkInAppointment(permitStore.getPermitDetail.application.appointmentId)
  }
}

function handleAssignApplication(name: string) {
  permitStore.getPermitDetail.application.assignedTo = name
  changed.value = 'Assigned User to Application'
  updatePermitDetails()
}

function handleNoShow() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.status =
      ApplicationStatus['Appointment No Show']
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus['No Show']
    noShowAppointment(permitStore.getPermitDetail.application.appointmentId)
  }
}

function handleSetAppointmentScheduled() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.status =
      ApplicationStatus['Ready For Appointment']
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus.Scheduled
    setAppointmentScheduled(
      permitStore.getPermitDetail.application.appointmentId
    )
  }
}

function printPdf(type) {
  state.loading = true
  permitStore[type]().then(res => {
    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    let fileURL = URL.createObjectURL(res.data)

    window.open(fileURL)
    state.loading = false
  })
}

const appointmentDate = computed(() => {
  if (permitStore.getPermitDetail.application.appointmentDateTime) {
    return (
      new Date(
        permitStore.getPermitDetail?.application.appointmentDateTime
      )?.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      }) || ''
    )
  }

  return ''
})

const appointmentTime = computed(() => {
  if (permitStore.getPermitDetail.application.appointmentDateTime) {
    const date = new Date(
      permitStore.getPermitDetail?.application.appointmentDateTime
    )

    return date.toLocaleTimeString('en-US', {
      hour12: true,
      timeStyle: 'short',
    })
  }

  return ''
})

const daysLeft = computed(() => {
  if (permitStore.getPermitDetail?.application.startOfNinetyDayCountdown) {
    const date = new Date(
      permitStore.getPermitDetail?.application.startOfNinetyDayCountdown
    )
    const ninetyDays = date.setDate(date.getDate() + 90)
    const today = new Date()

    return Math.floor((ninetyDays - today.getTime()) / (1000 * 60 * 60 * 24))
  }

  return 0
})

async function handleSaveReschedule(reschedule) {
  const applicationHadPreviousAppointment =
    permitStore.getPermitDetail.application.appointmentId !== null

  changed.value = reschedule.change

  permitStore.getPermitDetail.application.appointmentDateTime =
    reschedule.dateAndTime

  const appointmentRequest: AppointmentWindowCreateRequestModel = {
    start: permitStore.getPermitDetail.application.appointmentDateTime || '',
    end: permitStore.getPermitDetail.application.appointmentDateTime || '',
    appointmentCreatedDate: new Date().toISOString(),
    applicationId: permitStore.getPermitDetail.id,
    userId: permitStore.getPermitDetail.userId,
    status: AppointmentStatus.Scheduled,
    name: `${permitStore.getPermitDetail.application.personalInfo.firstName} ${permitStore.getPermitDetail.application.personalInfo.lastName}`,
    permit: permitStore.getPermitDetail.application.orderId,
    payment:
      permitStore.getPermitDetail.application.paymentStatus === 1
        ? 'cash'
        : 'credit',
    isManuallyCreated: true,
  }

  const response = await createManualAppointment(appointmentRequest)

  permitStore.getPermitDetail.application.appointmentId = response.id
  permitStore.getPermitDetail.application.appointmentStatus =
    AppointmentStatus.Scheduled

  updatePermitDetails()

  if (reschedule.removeOldAppointment && applicationHadPreviousAppointment) {
    deleteSlotByApplicationId(permitStore.getPermitDetail.id)
  } else if (applicationHadPreviousAppointment) {
    reopenSlotByApplicationId(permitStore.getPermitDetail.id)
  }
}

function handleReadyForInitialPayment() {
  changed.value = 'Marked ready for initial payment'
  permitStore.getPermitDetail.application.readyForInitialPayment = true
  updatePermitDetails()
}

function handleReadyForRenewalPayment() {
  changed.value = 'Marked ready for renewal payment'
  permitStore.getPermitDetail.application.readyForRenewalPayment = true
  updatePermitDetails()
}

function handleReadyForModificationPayment() {
  changed.value = 'Marked ready for modification payment'
  permitStore.getPermitDetail.application.readyForModificationPayment = true
  updatePermitDetails()
}
</script>
