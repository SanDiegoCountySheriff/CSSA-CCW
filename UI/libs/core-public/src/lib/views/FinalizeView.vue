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
            :loading="isUpdateLoading"
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
    completeApplicationStore.completeApplication.paymentHistory[0].paymentStatus
  ) {
    case 1:
      return 'In Person'
    case 2:
      return 'Credit Card'
    default:
      return 'None'
  }
})

const { mutate: updatePaymentHistory } = useMutation({
  mutationFn: ({
    transactionId,
    successful,
    amount,
    transactionDateTime,
    hmac,
  }: {
    transactionId: string
    successful: boolean
    amount: number
    transactionDateTime: string
    hmac: string
  }) => {
    return paymentStore.updatePaymentHistory(
      transactionId,
      successful,
      amount,
      transactionDateTime,
      hmac
    )
  },
})

const isInitialPaymentComplete = computed(() => {
  return (
    completeApplicationStore.completeApplication.paymentHistory.some(ph => {
      return (
        ph.paymentType === 'CCW Application Initial Payment' &&
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

provide('isInitialPaymentComplete', isInitialPaymentComplete)

const {
  mutate: getAppointmentMutation,
  isLoading,
  isError,
} = useMutation({
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  mutationFn: () => {
    const appRes = appointmentsStore.getAvailableAppointments()

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
  // verifySession()
  const transactionId = route.query.transactionId
  const successful = route.query.successful
  const amount = route.query.amount
  const hmac = route.query.hmac
  let transactionDateTime = route.query.transactionDateTime

  if (typeof transactionDateTime === 'string') {
    transactionDateTime = transactionDateTime.replace(':', '%3A')
    transactionDateTime = transactionDateTime.replace(':', '%3A')
  }

  if (
    typeof transactionId === 'string' &&
    typeof successful === 'string' &&
    typeof amount === 'string' &&
    typeof transactionDateTime === 'string' &&
    typeof hmac === 'string'
  ) {
    updatePaymentHistory({
      transactionId,
      successful: Boolean(successful),
      amount: Number(amount),
      transactionDateTime,
      hmac,
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

// function verifySession(): boolean {
//   const session = route.query.hmac

//   window.console.log('session', session)
//   let hmac: string

//   const sessionId = getSession()

//   window.console.log('sessionId', sessionId)
//   const parameters = getParameters()

//   hmac = generateHmac(sessionId, parameters)
//   window.console.log('hmac', hmac)

//   window.console.log('verify', hmac === session)

//   return true
// }

// function getSession(): string {
//   const name = 'session='
//   const decodedCookie = decodeURIComponent(document.cookie)
//   const cookieArray = decodedCookie.split(';')
//   let cookieValue = ''

//   for (let cookie of cookieArray) {
//     cookie = cookie.trim()

//     if (cookie.indexOf(name) === 0) {
//       cookieValue = cookie.substring(name.length, cookie.length)
//     }
//   }

//   if (cookieValue) {
//     return cookieValue
//   }

//   return ''
// }

// function getParameters(): string {
//   const transactionId = route.query.transactionId
//   const successful = route.query.successful
//   const amount = route.query.amount
//   let transactionDateTime = route.query.transactionDateTime

//   if (typeof transactionDateTime === 'string') {
//     transactionDateTime = transactionDateTime.replace(':', '%3A')
//     transactionDateTime = transactionDateTime.replace(':', '%3A')
//   }

//   window.console.log(transactionDateTime)

//   const params = `transactionId=${transactionId}&successful=${successful}&amount=${amount}&transactionDateTime=${transactionDateTime}`

//   window.console.log(params)

//   return params
// }

// function generateHmac(session: string, parameters: string): string {
//   const hmac = createHmac('sha256', session)

//   hmac.update(parameters)

//   return hmac.digest('hex')
// }
</script>
