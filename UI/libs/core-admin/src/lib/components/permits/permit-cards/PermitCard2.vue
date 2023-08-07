<template>
  <v-container class="px-0 py-0">
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
                    formatDate(
                      permitStore.getPermitDetail.application.dob.birthDate
                    )
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
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-camera'"
                      :default-selection="'portrait'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-fingerprint'"
                      :default-selection="'thumbprint'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                </v-row>
                <v-row>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <FileUploadDialog
                      :icon="'mdi-file-upload'"
                      :get-file-from-dialog="onFileChanged"
                    />
                  </v-col>
                  <v-col
                    cols="6"
                    sm="6"
                  >
                    <v-menu
                      bottom
                      :elevation="10"
                    >
                      <template #activator="{ on, attrs }">
                        <v-btn
                          block
                          :loading="state.loading"
                          v-bind="attrs"
                          v-on="on"
                          color="primary"
                          small
                        >
                          <v-icon>mdi-printer</v-icon>
                        </v-btn>
                      </template>
                      <v-list>
                        <v-list-item @click="printPdf('printApplicationApi')">
                          <v-list-item-title>
                            Print Application
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
          :loading="props.isLoading"
        >
          <v-card-title
            v-if="!permitStore.getPermitDetail.application.isComplete"
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
              permitStore.getPermitDetail.application.startOfNinetyDayCountdown
            "
            class="text-center"
          >
            {{ daysLeft }} day{{ daysLeft > 1 ? 's' : '' }} left to complete
            application before it expires
          </v-card-text>
          <v-card-text class="text-center">
            Assigned to:
            {{ permitStore.getPermitDetail.application.assignedTo }}
          </v-card-text>

          <v-spacer></v-spacer>

          <v-card-text>
            <template v-if="authStore.auth.isAdmin">
              <v-autocomplete
                v-model="permitStore.getPermitDetail.application.assignedTo"
                :items="adminUserStore.allAdminUsers"
                @input="handleAssignApplication"
                label="Assign Application"
                item-text="name"
                item-value="name"
                clearable
                outlined
                dense
                color="primary"
              ></v-autocomplete>
            </template>

            <v-row>
              <v-col
                cols="12"
                xl="6"
              >
                <v-btn
                  @click="handleStart90DayCountdown"
                  :disabled="
                    permitStore.getPermitDetail.application
                      .startOfNinetyDayCountdown !== null &&
                    permitStore.getPermitDetail.application
                      .startOfNinetyDayCountdown !== undefined
                  "
                  color="primary"
                  small
                  block
                >
                  <v-icon left>mdi-timer</v-icon>
                  Start 90 Days
                </v-btn>
              </v-col>
              <v-col
                cols="12"
                xl="6"
              >
                <v-btn
                  color="primary"
                  :href="`mailto:${permitStore.getPermitDetail.application.userEmail}`"
                  target="_blank"
                  small
                  block
                >
                  <v-icon left>mdi-email-outline</v-icon>
                  Send Request
                </v-btn>
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
          :loading="isAppointmentLoading"
          class="d-flex flex-column fill-height"
          outlined
        >
          <v-card-title
            v-if="permitStore.getPermitDetail.application.appointmentDateTime"
            class="justify-center"
          >
            <v-icon
              color="success"
              class="mr-2"
            >
              mdi-shield-check
            </v-icon>
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
    </v-row>

    <v-snackbar
      v-model="state.snackbar"
      :multi-line="state.multiLine"
    >
      {{ state.text }}

      <template #action="{ attrs }">
        <v-btn
          :color="$vuetify.theme.dark ? '' : 'red'"
          text
          v-bind="attrs"
          @click="state.snackbar = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>

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
          <v-spacer></v-spacer>
          <v-btn
            color="error"
            text
            @click="handle90DayCountdownDeny"
          >
            No
          </v-btn>
          <v-btn
            color="primary"
            text
            @click="handle90DayCountdownConfirm"
          >
            Yes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import DateTimePicker from '@core-admin/components/appointment/DateTimePicker.vue'
import FileUploadDialog from '@core-admin/components/dialogs/FileUploadDialog.vue'
import Schedule from '@core-admin/components/appointment/Schedule.vue'
import { formatDate } from '@shared-utils/formatters/defaultFormatters'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  AppointmentStatus,
  AppointmentWindowCreateRequestModel,
} from '@shared-utils/types/defaultTypes'
import { computed, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

interface IPermitCard2Props {
  isLoading: boolean
  userPhoto: string
}

const props = withDefaults(defineProps<IPermitCard2Props>(), {
  isLoading: false,
  userPhoto: '',
})

const emit = defineEmits(['refetch'])

const state = reactive({
  isSelecting: false,
  loading: false,
  snack: false,
  snackColor: '',
  snackText: '',
  multiLine: false,
  snackbar: false,
  text: `Invalid file type provided.`,
})

const ninetyDayStartDateSelection = ref(null)
const ninetyDayDialog = ref(false)
const permitStore = usePermitsStore()
const documentsStore = useDocumentsStore()
const appointmentStore = useAppointmentsStore()
const adminUserStore = useAdminUserStore()
const authStore = useAuthStore()
const changed = ref('')

const allowedExtension = [
  '.png',
  '.jpeg',
  '.jpg',
  '.pjp',
  '.pjpeg',
  '.jfif',
  '.bmp',
]

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(`Updated ${changed.value}`),
  {
    enabled: false,
  }
)

const { mutate: deleteSlotByApplicationId } = useMutation({
  mutationFn: (applicationId: string) =>
    appointmentStore.deleteSlotByApplicationId(applicationId),
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
      appointmentStore.putCheckInAppointment(appointmentId),
  }
)

const {
  mutate: setAppointmentScheduled,
  isLoading: isAppointmentScheduledLoading,
} = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentStore.putSetAppointmentScheduled(appointmentId),
})

const { mutate: noShowAppointment, isLoading: isNoShowLoading } = useMutation({
  mutationFn: (appointmentId: string) =>
    appointmentStore.putNoShowAppointment(appointmentId),
})

const isAppointmentLoading = computed(() => {
  return (
    isNoShowLoading.value ||
    isCheckInLoading.value ||
    isAppointmentScheduledLoading.value
  )
})

const isOfficialLicenseMissingInformation = computed(() => {
  const uploadedDocuments =
    permitStore.getPermitDetail.application.uploadedDocuments
  const missingThumbprint = !uploadedDocuments.some(
    doc => doc.documentType.toLowerCase().indexOf('thumbprint') !== -1
  )
  const missingPortrait = !uploadedDocuments.some(
    doc => doc.documentType.toLowerCase().indexOf('portrait') !== -1
  )

  return missingThumbprint || missingPortrait
})

const isUnofficialLicenseMissingInformation = computed(() => {
  const uploadedDocuments =
    permitStore.getPermitDetail.application.uploadedDocuments
  const missingThumbprint = !uploadedDocuments.some(
    doc => doc.documentType.toLowerCase().indexOf('thumbprint') !== -1
  )
  const missingPortrait = !uploadedDocuments.some(
    doc => doc.documentType.toLowerCase().indexOf('portrait') !== -1
  )

  return missingThumbprint || missingPortrait
})

const tooltipText = computed(() => {
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
})

function handleCheckIn() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus['Checked In']
    checkInAppointment(permitStore.getPermitDetail.application.appointmentId)
  }
}

function handleAssignApplication() {
  changed.value = 'Assigned User to Application'
  updatePermitDetails()
}

function handleNoShow() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus['No Show']
    noShowAppointment(permitStore.getPermitDetail.application.appointmentId)
  }
}

function handleSetAppointmentScheduled() {
  if (permitStore.getPermitDetail.application.appointmentId) {
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus.Scheduled
    setAppointmentScheduled(
      permitStore.getPermitDetail.application.appointmentId
    )
  }
}

function onFileChanged(e: File, target: string) {
  if (allowedExtension.some(ext => e.name.toLowerCase().endsWith(ext))) {
    documentsStore
      .setUserApplicationFile(e, target)
      .then(() => {
        state.text = 'Successfully uploaded file.'
        state.snackbar = true
      })
      .catch(() => {
        state.text = 'An API error occurred.'
        state.snackbar = true
      })
  } else {
    state.text = 'Invalid file type provided.'
    state.snackbar = true
  }

  emit('refetch')
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
    const ninetyDays = date.setDate(date.getDate() + 91)
    const today = new Date()

    return Math.floor((ninetyDays - today.getTime()) / (1000 * 60 * 60 * 24))
  }

  return 0
})

function handleStart90DayCountdown() {
  ninetyDayDialog.value = true
}

function handle90DayCountdownConfirm() {
  changed.value = '90 Day Countdown'

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

async function handleSaveReschedule(reschedule) {
  const applicationHadPreviousAppointment =
    permitStore.getPermitDetail.application.appointmentId !== null

  changed.value = reschedule.change

  permitStore.getPermitDetail.application.appointmentDateTime =
    reschedule.dateAndTime

  const appointmentRequest: AppointmentWindowCreateRequestModel = {
    start: permitStore.getPermitDetail.application.appointmentDateTime || '',
    end: permitStore.getPermitDetail.application.appointmentDateTime || '',
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
</script>
