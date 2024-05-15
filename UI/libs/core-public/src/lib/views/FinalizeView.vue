<template>
  <div>
    <div
      v-show="isSubmitting || appointmentsStore.schedulingAppointment"
      class="text-center mt-16"
    >
      <v-row>
        <v-col>
          <v-progress-circular
            :size="$vuetify.breakpoint.smAndUp ? '300' : '200'"
            color="primary"
            indeterminate
          >
            <v-row>
              <v-col>
                <v-avatar :size="$vuetify.breakpoint.smAndUp ? '200' : '100'">
                  <v-img
                    :src="brandStore.getDocuments.agencyLogo"
                    alt="Image"
                    contain
                  />
                </v-avatar>
              </v-col>
            </v-row>
          </v-progress-circular>
        </v-col>
      </v-row>

      <v-row>
        <v-col> Submitting your CCW Application </v-col>
      </v-row>
    </div>

    <div v-show="!isSubmitting && !appointmentsStore.schedulingAppointment">
      <v-container
        fluid
        v-if="isLoading && !isError && !state.isLoading && !state.isError"
      >
        <v-skeleton-loader
          fluid
          class="fill-height"
          type="list-item, divider, list-item-three-line,
       actions"
        >
        </v-skeleton-loader>
      </v-container>

      <v-container v-else>
        <v-row
          v-if="isRenew"
          class="mb-5 mt-5 text-center"
        >
          <v-col>
            <v-btn
              :loading="isUpdateLoading || isUpdatePaymentHistoryLoading"
              color="primary"
              @click="handleSubmit"
            >
              {{ $t('Submit Renewal') }}
            </v-btn>
          </v-col>
        </v-row>

        <v-row
          v-if="isRenew"
          justify="center"
          class="mt-3"
        >
          <v-alert
            type="info"
            color="primary"
            outlined
          >
            <span
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              Review your information before submitting your renewal
              application. You will be notified to pay at a later date.
            </span>
          </v-alert>
        </v-row>

        <v-row class="mt-3 mb-3">
          <v-col>
            <FinalizeContainer />
          </v-col>
        </v-row>

        <v-row
          v-if="!isRenew"
          justify="center"
          class="mt-3"
        >
          <v-alert
            type="info"
            color="primary"
            outlined
          >
            <span
              :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
            >
              You must choose an appointment in order to submit your
              application. You may reschedule or cancel your appointment at a
              later time.
            </span>
          </v-alert>
        </v-row>

        <template v-if="appConfigStore.appConfig.payBeforeSubmit">
          <v-row class="mt-3 mb-3">
            <v-col>
              <PaymentContainer
                v-if="
                  completeApplicationStore.completeApplication.application
                    .applicationType
                "
                :payment-complete="isInitialPaymentComplete"
                :hide-online-payment="
                  !appConfigStore.appConfig.isPaymentServiceAvailable
                "
              />
            </v-col>
          </v-row>

          <template v-if="wasInitialPaymentUnsuccessful">
            <v-card class="mt-3 mb-3">
              <v-alert
                color="error"
                outlined
                type="error"
                class="font-weight-bold mt-3"
              >
                {{ $t(`Payment method was unsuccessful, please try again`) }}
              </v-alert>
            </v-card>
          </template>

          <template v-if="isInitialPaymentComplete">
            <v-card class="mt-3 mb-3">
              <v-alert
                color="primary"
                outlined
                type="info"
                class="font-weight-bold mt-3"
              >
                <!-- TODO: update with different options once online is implemented -->
                {{ $t(`Payment method selected: ${paymentStatus} `) }}
              </v-alert>
            </v-card>
          </template>
        </template>

        <template
          v-if="!state.appointmentsLoaded && !state.appointmentComplete"
        >
          <v-skeleton-loader
            fluid
            class="fill-height"
            type="list-item, divider, list-item-three-line,
       actions"
          >
          </v-skeleton-loader>
        </template>
        <template
          v-if="
            (isLoading && isError) ||
            (state.appointmentsLoaded && state.appointments.length === 0)
          "
        >
          <v-card>
            <v-alert
              outlined
              type="warning"
            >
              {{
                $t(' No available appointments found. Please try again later.')
              }}
            </v-alert>
          </v-card>
        </template>

        <v-row class="mt-3 mb-3">
          <v-col>
            <v-card
              :loading="isUpdateLoading"
              v-if="
                (isLoading && isError) ||
                (state.appointmentsLoaded &&
                  state.appointments.length > 0 &&
                  !state.appointmentComplete)
              "
              elevation="2"
            >
              <AppointmentContainer
                v-if="!isRenew"
                :show-header="true"
                :events="state.appointments"
                @toggle-appointment="toggleAppointmentComplete"
                :rescheduling="false"
              />
            </v-card>

            <template v-else>
              <v-card
                :loading="isUpdateLoading"
                v-if="
                  completeApplicationStore.completeApplication.application
                    .appointmentDateTime
                "
              >
                <v-alert
                  v-if="!isRenew"
                  color="primary"
                  outlined
                  type="info"
                  class="font-weight-bold"
                >
                  {{ $t('Appointment has been set for ') }}
                  {{
                    new Date(
                      completeApplicationStore.completeApplication.application.appointmentDateTime
                    ).toLocaleString()
                  }}
                </v-alert>
              </v-card>
            </template>
          </v-col>
        </v-row>
      </v-container>

      <v-snackbar
        :value="state.snackbar"
        :timeout="3000"
        bottom
        color="error"
        outlined
      >
        {{ $t('Section update unsuccessful please try again.') }}
      </v-snackbar>
    </div>
  </div>
</template>

<script lang="ts" setup>
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue'
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue'
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue'
import Routes from '@core-public/router/routes'
import { ref } from 'vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, provide, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router/composables'

const state = reactive({
  snackbar: false,
  appointmentComplete: false,
  appointments: [] as Array<AppointmentType>,
  applicationLoaded: false,
  appointmentsLoaded: false,
  isLoading: true,
  isError: false,
})
const isSubmitting = ref(false)
const completeApplicationStore = useCompleteApplicationStore()
const brandStore = useBrandStore()
const appConfigStore = useAppConfigStore()
const paymentStore = usePaymentStore()
const appointmentsStore = useAppointmentsStore()
const themeStore = useThemeStore()
const route = useRoute()
const router = useRouter()
const paymentStatus = computed(() => {
  switch (
    completeApplicationStore.completeApplication.application.paymentStatus
  ) {
    case 1:
      return 'In Person'
    case 2:
      return 'Credit Card'
    default:
      return 'None'
  }
})

const isRenew = computed(() => {
  const applicationType =
    completeApplicationStore.completeApplication.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const isInitialPaymentComplete = computed(() => {
  return (
    completeApplicationStore.completeApplication.paymentHistory.some(ph => {
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
    }) ||
    completeApplicationStore.completeApplication.application.paymentStatus === 1
  )
})

const wasInitialPaymentUnsuccessful = computed(() => {
  return completeApplicationStore.completeApplication.paymentHistory.some(
    ph => {
      return ph.successful === false
    }
  )
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
    completeApplicationStore
      .getCompleteApplicationFromApi(
        completeApplicationStore.completeApplication.id,
        Boolean(route.query.isComplete)
      )
      .then(res => {
        completeApplicationStore.setCompleteApplication(res)
      }),
})

provide('isInitialPaymentComplete', isInitialPaymentComplete)
provide('isUpdatePaymentHistoryLoading', isUpdatePaymentHistoryLoading)

const {
  mutate: getAppointmentMutation,
  isLoading,
  isError,
} = useMutation({
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  mutationFn: () => {
    const appRes = appointmentsStore.getAvailableAppointments(false)

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

          let formatedStart = `${start.getFullYear()}-${
            start.getMonth() + 1
          }-${start.getDate()} ${start.getHours()}:${start
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

          let formatedEnd = `${end.getFullYear()}-${
            end.getMonth() + 1
          }-${end.getDate()} ${end.getHours()}:${end
            .getMinutes()
            .toString()
            .padStart(2, '0')}`

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

onMounted(() => {
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

  if (!completeApplicationStore.completeApplication.application.orderId) {
    state.isLoading = true
    completeApplicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId as string,
        Boolean(route.query.isComplete)
      )
      .then(res => {
        completeApplicationStore.setCompleteApplication(res)
        state.isLoading = false

        if (
          completeApplicationStore.completeApplication.application
            .appointmentStatus === AppointmentStatus.Scheduled
        ) {
          state.appointmentComplete = true
        }
      })
      .catch(() => {
        state.isError = true
      })
  } else {
    state.isLoading = false
  }

  if (
    completeApplicationStore.completeApplication.application
      .appointmentStatus === AppointmentStatus.Scheduled
  ) {
    state.appointmentComplete = true
  }

  getAppointmentMutation()
})

const { isLoading: isUpdateLoading, mutate: updateMutation } = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication()
  },
  onSuccess: () => {
    router.push(Routes.RECEIPT_PATH)
    isSubmitting.value = false
  },
  onError: () => {
    state.snackbar = true
    isSubmitting.value = false
  },
})

async function handleSubmit() {
  isSubmitting.value = true
  completeApplicationStore.completeApplication.application.currentStep = 1
  completeApplicationStore.completeApplication.application.isComplete = true
  completeApplicationStore.completeApplication.application.status =
    ApplicationStatus.Submitted
  completeApplicationStore.completeApplication.application.submittedToLicensingDateTime =
    new Date().toISOString()

  if (completeApplicationStore.completeApplication.isMatchUpdated === false) {
    completeApplicationStore.completeApplication.isMatchUpdated = true
  }

  updateMutation()
}

function toggleAppointmentComplete() {
  isSubmitting.value = true
  state.appointmentComplete = !state.appointmentComplete
  state.appointmentsLoaded = false
  handleSubmit()
}
</script>
