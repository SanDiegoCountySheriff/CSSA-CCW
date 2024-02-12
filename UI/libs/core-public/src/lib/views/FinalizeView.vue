<template>
  <div>
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
      <v-row class="mt-3 mb-3">
        <v-col>
          <FinalizeContainer />
        </v-col>
      </v-row>
      <v-row class="mt-3 mb-3">
        <v-col>
          <PaymentContainer
            v-if="
              completeApplicationStore.completeApplication.application
                .applicationType
            "
            :payment-complete="isInitialPaymentComplete"
            :hide-online-payment="false"
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

      <template v-if="!state.appointmentsLoaded && !state.appointmentComplete">
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
            v-if="
              (isLoading && isError) ||
              (state.appointmentsLoaded &&
                state.appointments.length > 0 &&
                !state.appointmentComplete)
            "
            elevation="2"
          >
            <AppointmentContainer
              :show-header="true"
              :events="state.appointments"
              @toggle-appointment="toggleAppointmentComplete"
              :rescheduling="false"
            />
          </v-card>

          <template v-else>
            <v-card
              v-if="
                completeApplicationStore.completeApplication.application
                  .appointmentDateTime
              "
            >
              <v-alert
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
      <v-row class="float-right">
        <v-col>
          <v-btn
            class="mr-10 mb-10"
            color="error"
            to="/"
          >
            {{ $t('Cancel') }}
          </v-btn>
          <v-btn
            class="mb-10"
            :disabled="!state.appointmentComplete || !isInitialPaymentComplete"
            :loading="isUpdateLoading || isUpdatePaymentHistoryLoading"
            color="primary"
            @click="handleSubmit"
          >
            {{ $t('Submit Application') }}
          </v-btn>
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
</template>

<script lang="ts" setup>
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue'
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue'
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue'
import Routes from '@core-public/router/routes'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import {
  ApplicationStatus,
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
const completeApplicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const appointmentsStore = useAppointmentsStore()
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

const isInitialPaymentComplete = computed(() => {
  return (
    completeApplicationStore.completeApplication.paymentHistory.some(ph => {
      return (
        (ph.paymentType === 0 ||
          ph.paymentType === 1 ||
          ph.paymentType === 2) &&
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
  },
  onError: () => {
    state.snackbar = true
  },
})

async function handleSubmit() {
  completeApplicationStore.completeApplication.application.isComplete = true
  completeApplicationStore.completeApplication.application.status =
    ApplicationStatus.Submitted
  completeApplicationStore.completeApplication.application.submittedToLicensingDateTime =
    new Date().toISOString()
  updateMutation()
}

function toggleAppointmentComplete() {
  state.appointmentComplete = !state.appointmentComplete
  completeApplicationStore.updateApplication().then(() => {
    state.appointmentsLoaded = false
  })
}
</script>
