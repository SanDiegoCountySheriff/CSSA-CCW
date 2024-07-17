<template>
  <div>
    <v-btn
      v-if="showInitialWithdrawButton && canWithdrawApplication"
      @click="handleShowModifySignatureDialog"
      :disabled="isGetApplicationsLoading || !canWithdrawApplication"
      color="primary"
      tonal
    >
      {{ title }}
    </v-btn>

    <v-btn
      v-else-if="
        applicationStore.completeApplication.application.status ===
        ApplicationStatus.Withdrawn
      "
      color="primary"
      block
      @click="handleSubmit"
      :disabled="isGetApplicationsLoading"
    >
      Submit
    </v-btn>

    <v-dialog
      v-model="state.modifySignatureDialog"
      max-width="600"
    >
      <v-card :loading="isUploadSignatureDocumentLoading">
        <v-card-title> Edit Signature </v-card-title>

        <v-card-title>Signature</v-card-title>

        <v-card-text>
          <v-card
            light
            flat
            width="555px"
            height="105px"
            outlined
            style="border: solid 2px black"
          >
            <canvas
              width="550px"
              height="100px"
              id="signature"
              class="signature"
            ></canvas>
          </v-card>
        </v-card-text>

        <v-card-actions>
          <v-btn
            color="primary"
            @click="handleClearSignature"
          >
            Clear Signature
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            :valid="!isSignaturePadEmpty"
            color="primary"
            @click="handleSaveSignature"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar
      v-model="state.showUploadFailureSnackbar"
      color="primary"
    >
      The signature modification failed. Please contact us if this error
      continues.
      <template #action="{ attrs }">
        <v-btn
          text
          v-bind="attrs"
          @click="state.showUploadFailureSnackbar = false"
        >
          Ok
        </v-btn>
      </template>
    </v-snackbar>

    <v-snackbar
      v-model="state.showUploadSuccessSnackbar"
      color="primary"
    >
      Your signature was successfully modified. Please inform Licensing so your
      application can continue processing.
      <template #action="{ attrs }">
        <v-btn
          text
          v-bind="attrs"
          @click="state.showUploadSuccessSnackbar = false"
        >
          Ok
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import Endpoints from '../../../lib/api/endpoints'
import SignaturePad from 'signature_pad'
import axios from 'axios'
import { useCompleteApplicationStore } from '../../stores/completeApplication'
import { usePaymentStore } from '../../stores/paymentStore'
import { useRoute } from 'vue-router/composables'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentType,
  CompleteApplication,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

interface IModifySignatureDialog {
  title: string
}

const props = withDefaults(defineProps<IModifySignatureDialog>(), {
  title: 'Modify Signature',
})

const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const route = useRoute()
const signaturePad = ref<SignaturePad>()
const signatureForm = new FormData()
const emit = defineEmits(['on-signature-submit'])

const state = reactive({
  snackbar: false,
  isApplicationValid: false,
  cancelAppointmentDialog: false,
  invalidSubmissionDialog: false,
  confirmSubmissionDialog: false,
  rescheduling: false,
  withdrawDialog: false,
  modifySignatureDialog: false,
  showUploadFailureSnackbar: false,
  showUploadSuccessSnackbar: false,
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

const updateMutationWithoutRouting = useMutation({
  mutationFn: applicationStore.updateApplication,
  onSuccess: () => {
    state.modifySignatureDialog = false
  },
  onError: () => null,
})

const {
  isLoading: isUploadSignatureDocumentLoading,
  mutate: uploadSignatureDocument,
} = useMutation(
  ['uploadSignatureDocument'],
  async () => await postUploadSignatureFile(signatureForm, 'Signature'),
  {
    onSuccess: async () => {
      const uploadDoc: UploadedDocType = {
        documentType: 'Signature',
        name: 'Signature',
        uploadedBy: applicationStore.completeApplication.application.userEmail,
        uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
      }

      if (applicationStore.completeApplication.application.uploadedDocuments) {
        applicationStore.completeApplication.application.uploadedDocuments =
          applicationStore.completeApplication.application.uploadedDocuments.filter(
            document => {
              return document.documentType !== 'Signature'
            }
          )
      } else {
        applicationStore.completeApplication.application.uploadedDocuments = []
      }

      applicationStore.completeApplication.application.uploadedDocuments.push(
        uploadDoc
      )

      handleShowUploadSuccessSnackbar()
    },
    onError: () => {
      handleShowUploadFailureSnackbar()
    },
  }
)

const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

function handleSubmit() {
  if (
    applicationStore.completeApplication.application.appointmentStatus === 1
  ) {
    state.invalidSubmissionDialog = true
  } else {
    state.confirmSubmissionDialog = true
  }
}

function handleShowModifySignatureDialog() {
  state.modifySignatureDialog = true

  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgba(0, 0, 0, 0)',
      minDistance: 5,
    })
  })
}

function handleShowUploadSuccessSnackbar() {
  emit('on-signature-submit')
  state.showUploadSuccessSnackbar = true
  updateMutationWithoutRouting.mutate()
}

function handleShowUploadFailureSnackbar() {
  state.showUploadFailureSnackbar = true
  updateMutationWithoutRouting.mutate()
}

async function postUploadSignatureFile(data: FormData, target: string) {
  await axios.post(
    `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${target}`,
    data
  )
}

async function handleSaveSignature() {
  const canvas = document.getElementById('signature') as HTMLCanvasElement

  canvas.toBlob(async blob => {
    signatureForm.append('fileToUpload', blob as Blob)

    uploadSignatureDocument()
  })
}

function handleClearSignature() {
  signaturePad.value?.clear()
}
</script>
