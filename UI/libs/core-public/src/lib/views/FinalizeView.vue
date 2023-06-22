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
      <v-row class="mt-1">
        <v-col>
          <FinalizeContainer />
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <PaymentContainer
            v-if="
              completeApplicationStore.completeApplication.application
                .applicationType
            "
            :toggle-payment="togglePaymentComplete"
          />
        </v-col>
      </v-row>

      <template
        v-if="
          completeApplicationStore.completeApplication.application
            .paymentStatus !== 0
        "
      >
        <v-card class="mt-4">
          <v-alert
            color="primary"
            outlined
            type="info"
            class="font-weight-bold"
          >
            <!-- TODO: update with different options once online is implemented -->
            {{ $t('Payment method selected: Pay in person ') }}
          </v-alert>
        </v-card>
      </template>
      <v-container
        v-if="!state.appointmentsLoaded && !state.appointmentComplete"
      >
        <v-skeleton-loader
          fluid
          class="fill-height"
          type="list-item, divider, list-item-three-line,
       actions"
        >
        </v-skeleton-loader>
      </v-container>
      <v-container
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
      </v-container>

      <v-row>
        <v-col>
          <v-card
            elevation="2"
            class="mt-3"
          >
            <AppointmentContainer
              v-if="
                (isLoading && isError) ||
                (state.appointmentsLoaded &&
                  state.appointments.length > 0 &&
                  !state.appointmentComplete)
              "
              :events="state.appointments"
              :toggle-appointment="toggleAppointmentComplete"
              :reschedule="false"
            />

            <v-container v-else>
              <v-card>
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
            </v-container>
          </v-card>
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
            :disabled="!state.appointmentComplete || !state.paymentComplete"
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
import {
  AppointmentStatus,
  AppointmentType,
} from '@shared-utils/types/defaultTypes'
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue'
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue'
import Routes from '@core-public/router/routes'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { onMounted, reactive } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'
import { useRoute, useRouter } from 'vue-router/composables'

const state = reactive({
  snackbar: false,
  paymentComplete: false,
  appointmentComplete: false,
  appointments: [] as Array<AppointmentType>,
  applicationLoaded: false,
  appointmentsLoaded: false,
  isLoading: true,
  isError: false,
})
const completeApplicationStore = useCompleteApplicationStore()
const appointmentsStore = useAppointmentsStore()
const route = useRoute()
const router = useRouter()

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

onMounted(() => {
  if (!completeApplicationStore.completeApplication.application.orderId) {
    state.isLoading = true
    completeApplicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId,
        route.query.isComplete
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

        if (
          completeApplicationStore.completeApplication.application
            .paymentStatus > 0
        ) {
          state.paymentComplete = true
        }
      })
      .catch(() => {
        state.isError = true
      })
  } else {
    state.isLoading = false
  }

  if (completeApplicationStore.completeApplication.application.paymentStatus) {
    state.paymentComplete = true
  }

  if (
    completeApplicationStore.completeApplication.application
      .appointmentStatus === AppointmentStatus.Scheduled
  ) {
    state.appointmentComplete = true
  }

  getAppointmentMutation()
})

const updateMutation = useMutation({
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
  completeApplicationStore.completeApplication.application.status = 2
  updateMutation.mutate()
}

function togglePaymentComplete() {
  completeApplicationStore.updateApplication().then(() => {
    state.paymentComplete = !state.paymentComplete
  })
}

function toggleAppointmentComplete() {
  state.appointmentComplete = !state.appointmentComplete
  completeApplicationStore.updateApplication().then(() => {
    state.appointmentsLoaded = false
  })
}
</script>
