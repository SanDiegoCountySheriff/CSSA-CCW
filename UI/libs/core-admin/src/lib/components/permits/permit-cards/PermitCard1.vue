<template>
  <v-container
    class="px-0 py-0"
    fluid
  >
    <v-card
      class="pt-2 fill-height"
      outlined
    >
      <v-container fluid>
        <v-row>
          <v-col
            cols="12"
            lg="4"
          >
            <div class="font-weight-bold">
              Application #{{ permitStore.getPermitDetail.application.orderId }}
            </div>
            <span class="body-2"> Submitted on {{ submittedDate }}</span>
          </v-col>
          <v-col
            class="text-center"
            cols="12"
            lg="4"
          >
            <div class="font-weight-bold">
              Application Type:
              {{
                ApplicationType[
                  permitStore.getPermitDetail.application.applicationType
                ]
              }}
            </div>
          </v-col>

          <v-col
            cols="12"
            lg="4"
          >
            <v-select
              ref="select"
              :items="appStatus"
              :readonly="readonly"
              label="Application Status"
              item-text="value"
              item-value="id"
              v-model="permitStore.getPermitDetail.application.status"
              @change="$event => updateApplicationStatus($event)"
              dense
              outlined
              :menu-props="{
                offsetY: true,
              }"
            ></v-select>
          </v-col>
        </v-row>
      </v-container>

      <template v-if="state.showApprovedEmailApplicantDialog">
        <ApprovedEmailApplicantDialog
          :applicant-name="
            permitStore.getPermitDetail.application.personalInfo.firstName +
            ' ' +
            permitStore.getPermitDetail.application.personalInfo.lastName
          "
          :applicant-email="permitStore.getPermitDetail.application.userEmail"
          :show-dialog="state.showApprovedEmailApplicantDialog"
          @cancel="handleCancel"
        />
      </template>

      <template v-if="state.showRevocationDialog">
        <RevocationDialog
          :show-dialog="state.showRevocationDialog"
          @cancel="handleCancel"
        />
      </template>
      <template v-if="state.showDenialDialog">
        <DenialDialog
          :show-dialog="state.showDenialDialog"
          @cancel="handleCancel"
        />
      </template>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import ApprovedEmailApplicantDialog from '@core-admin/components/dialogs/ApprovedEmailApplicantDialog.vue'
import DenialDialog from '@core-admin/components/dialogs/DenialDialog.vue'
import RevocationDialog from '@core-admin/components/dialogs/RevocationDialog.vue'
import { useAppointmentsStore } from '@shared-ui/stores/appointmentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentStatus,
} from '@shared-utils/types/defaultTypes'
import { computed, inject, reactive, ref } from 'vue'

const permitStore = usePermitsStore()
const appointmentStore = useAppointmentsStore()
const readonly = inject('readonly')
const dialog = ref(false)

const state = reactive({
  update: '',
  showApprovedEmailApplicantDialog: false,
  showRevocationDialog: false,
  showDenialDialog: false,
})

const appStatus = [
  {
    id: 0,
    value: 'None',
  },
  {
    id: 1,
    value: 'Incomplete',
  },
  {
    id: 2,
    value: 'Submitted',
  },
  {
    id: 18,
    value: 'Waiting For Customer',
  },
  {
    id: 3,
    value: 'Ready for Appointment',
  },
  {
    id: 4,
    value: 'Appointment Complete',
  },
  {
    id: 5,
    value: 'Background in Progress',
  },
  {
    id: 6,
    value: 'Contingently Approved',
  },
  {
    id: 7,
    value: 'Approved',
  },
  {
    id: 8,
    value: 'Permit Delivered',
  },
  {
    id: 9,
    value: 'Suspend',
  },
  {
    id: 10,
    value: 'Revoke',
  },
  {
    id: 11,
    value: 'Canceled',
  },
  {
    id: 12,
    value: 'Denied',
  },
  {
    id: 13,
    value: 'Withdrawn',
  },
  {
    id: 14,
    value: 'Flagged For Review',
  },
  {
    id: 15,
    value: 'Appointment No Show',
  },
  {
    id: 16,
    value: 'Contingently Denied',
  },
  {
    id: 17,
    value: 'Ready To Issue',
  },
  {
    id: 19,
    value: 'Modification Approved',
  },
  {
    id: 20,
    value: 'Renewal Approved',
  },
]

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(state.update),
  {
    onSuccess: () => {
      dialog.value = false
    },
    enabled: false,
  }
)

const submittedDate = computed(() => {
  if (permitStore.getPermitDetail.application.submittedToLicensingDateTime) {
    return new Date(
      permitStore.getPermitDetail.application
        .submittedToLicensingDateTime as string
    )?.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    })
  }

  return 'n/a'
})

function updateApplicationStatus(update: string) {
  state.update = `Changed application status to ${ApplicationStatus[update]}`

  if (
    ApplicationStatus[update] === 'Appointment Complete' &&
    permitStore.getPermitDetail.application.appointmentId
  ) {
    appointmentStore.deleteSlotByApplicationId(permitStore.getPermitDetail.id)
    permitStore.getPermitDetail.application.appointmentDateTime = null
    permitStore.getPermitDetail.application.appointmentId = null
    permitStore.getPermitDetail.application.appointmentStatus =
      AppointmentStatus['Not Scheduled']
  } else if (ApplicationStatus[update] === 'Approved') {
    state.showApprovedEmailApplicantDialog = true
  } else if (ApplicationStatus[update] === 'Revoked') {
    state.showRevocationDialog = true
  } else if (ApplicationStatus[update] === 'Denied') {
    state.showDenialDialog = true
  }

  updatePermitDetails()
}

function handleCancel() {
  state.showApprovedEmailApplicantDialog = false
  state.showRevocationDialog = false
  state.showDenialDialog = false
}
</script>
