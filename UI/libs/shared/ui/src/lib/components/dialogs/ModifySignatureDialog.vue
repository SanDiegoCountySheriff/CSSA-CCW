<template>
  <v-container fluid>
    <v-btn
      v-if="showInitialWithdrawButton && canWithdrawApplication"
      @click="handleShowModifySignatureDialog"
      :disabled="isGetApplicationsLoading || !canWithdrawApplication"
      color="primary"
      tonal
    >
      Modify Signature
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
  </v-container>
</template>

<script setup lang="ts">
//import { AppointmentType } from '../../../../../utils/src/lib/types/defaultTypes'
import Endpoints from '../../../lib/api/endpoints'
//import Routes from '../../../../../../core-public/src/lib/router/routes'
import SignaturePad from 'signature_pad'
import axios from 'axios'
import { useCompleteApplicationStore } from '../../stores/completeApplication'
import { usePaymentStore } from '../../stores/paymentStore'
import {
  ApplicationStatus,
  ApplicationType,
  AppointmentType,
  CompleteApplication,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
// import Endpoints from '../../../lib/api/endpoints'
//import Routes from '../../../../../../core-public/src/lib/router/routes'
// import Routes from '@core-public/router/routes'
// import SignaturePad from 'signature_pad'
//import { UploadedDocType } from '../../../../../utils/src/lib/types/defaultTypes'
//import axios from 'axios'
//import { useBrandStore } from '../../stores/brandStore'
// import { useCompleteApplicationStore } from '../../stores/completeApplication'
//import { usePaymentStore } from '../../stores/paymentStore'
//import { useThemeStore } from '../../stores/themeStore'
// import {
//   ApplicationStatus,
//   ApplicationType,
//   CompleteApplication,
//   PaymentType,
// } from '../../../../../utils/src/lib/types/defaultTypes'
import { computed, nextTick, onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'
import { useRoute, useRouter } from 'vue-router/composables'

const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
//const brandStore = useBrandStore()
//const themeStore = useThemeStore()
const router = useRouter()
const route = useRoute()
const paymentSnackbar = ref(false)
const signaturePad = ref<SignaturePad>()
const signatureForm = new FormData()

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

const updateMutation = useMutation({
  mutationFn: applicationStore.updateApplication,
  onSuccess: () => {
    router.push({
      path: `/application-details`,
      query: {
        applicationId: state.application[0].id,
      },
    })
  },
  onError: () => null,
})

// const { mutate: makePayment, isLoading: isMakePaymentLoading } = useMutation({
//   mutationFn: () => {
//     let cost: number
//     let paymentType: string
//     let livescanAmount: number | null | undefined

//     switch (applicationStore.completeApplication.application.applicationType) {
//       case ApplicationType.Standard:
//         if (
//           applicationStore.completeApplication.application
//             .readyForInitialPayment
//         ) {
//           paymentType =
//             PaymentType['CCW Application Initial Payment'].toString()
//           livescanAmount =
//             applicationStore.completeApplication.application.cost
//               .standardLivescanFee ?? brandStore.brand.cost.standardLivescanFee
//           cost =
//             applicationStore.completeApplication.application.cost.new
//               .standard ?? brandStore.brand.cost.new.standard
//         } else {
//           window.console.log('this one')
//           paymentType =
//             PaymentType['CCW Application Issuance Payment'].toString()
//           window.console.log(paymentType)
//           cost =
//             applicationStore.completeApplication.application.cost.issuance ??
//             brandStore.brand.cost.issuance
//           window.console.log(cost)
//         }

//         break

//       case ApplicationType.Judicial:
//         if (
//           applicationStore.completeApplication.application
//             .readyForInitialPayment
//         ) {
//           paymentType =
//             PaymentType['CCW Application Initial Judicial Payment'].toString()
//           livescanAmount =
//             applicationStore.completeApplication.application.cost
//               .judicialLivescanFee ?? brandStore.brand.cost.judicialLivescanFee
//           cost =
//             applicationStore.completeApplication.application.cost.new
//               .judicial ?? brandStore.brand.cost.new.judicial
//         } else {
//           paymentType =
//             PaymentType['CCW Application Issuance Payment'].toString()
//           cost =
//             applicationStore.completeApplication.application.cost.issuance ??
//             brandStore.brand.cost.issuance
//         }

//         break

//       case ApplicationType.Reserve:
//         if (
//           applicationStore.completeApplication.application
//             .readyForInitialPayment
//         ) {
//           paymentType =
//             PaymentType['CCW Application Initial Reserve Payment'].toString()
//           livescanAmount =
//             applicationStore.completeApplication.application.cost
//               .reserveLivescanFee ?? brandStore.brand.cost.reserveLivescanFee
//           cost =
//             applicationStore.completeApplication.application.cost.new.reserve ??
//             brandStore.brand.cost.new.reserve
//         } else {
//           paymentType =
//             PaymentType['CCW Application Issuance Payment'].toString()
//           cost =
//             applicationStore.completeApplication.application.cost.issuance ??
//             brandStore.brand.cost.issuance
//         }

//         break

//       case ApplicationType.Employment:
//         if (
//           applicationStore.completeApplication.application
//             .readyForInitialPayment
//         ) {
//           paymentType =
//             PaymentType['CCW Application Initial Employment Payment'].toString()
//           livescanAmount =
//             applicationStore.completeApplication.application.cost
//               .employmentLivescanFee ??
//             brandStore.brand.cost.employmentLivescanFee
//           cost =
//             applicationStore.completeApplication.application.cost.new
//               .employment ?? brandStore.brand.cost.new.employment
//         } else {
//           paymentType =
//             PaymentType['CCW Application Issuance Payment'].toString()
//           cost =
//             applicationStore.completeApplication.application.cost.issuance ??
//             brandStore.brand.cost.issuance
//         }

//         break

//       case ApplicationType['Renew Standard']:
//         paymentType = PaymentType['CCW Application Renewal Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.renew
//             .standard ?? brandStore.brand.cost.renew.standard
//         break

//       case ApplicationType['Renew Judicial']:
//         paymentType =
//           PaymentType['CCW Application Renewal Judicial Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.renew
//             .judicial ?? brandStore.brand.cost.renew.judicial
//         break

//       case ApplicationType['Renew Reserve']:
//         paymentType =
//           PaymentType['CCW Application Renewal Reserve Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.renew.reserve ??
//           brandStore.brand.cost.renew.reserve
//         break

//       case ApplicationType['Renew Employment']:
//         paymentType =
//           PaymentType['CCW Application Renewal Employment Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.renew
//             .employment ?? brandStore.brand.cost.renew.employment
//         break

//       case ApplicationType['Modify Standard']:
//         paymentType =
//           PaymentType['CCW Application Modification Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.modify ??
//           brandStore.brand.cost.modify
//         break

//       case ApplicationType['Modify Judicial']:
//         paymentType =
//           PaymentType[
//             'CCW Application Modification Judicial Payment'
//           ].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.modify ??
//           brandStore.brand.cost.modify
//         break

//       case ApplicationType['Modify Reserve']:
//         paymentType =
//           PaymentType['CCW Application Modification Reserve Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.modify ??
//           brandStore.brand.cost.modify
//         break

//       case ApplicationType['Modify Employment']:
//         paymentType =
//           PaymentType[
//             'CCW Application Modification Employment Payment'
//           ].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.modify ??
//           brandStore.brand.cost.modify
//         break

//       default:
//         paymentType = PaymentType['CCW Application Initial Payment'].toString()
//         cost =
//           applicationStore.completeApplication.application.cost.new.standard ??
//           brandStore.brand.cost.new.standard
//     }

//     return paymentStore.getPayment(
//       applicationStore.completeApplication.id,
//       cost,
//       livescanAmount,
//       applicationStore.completeApplication.application.orderId,
//       paymentType
//     )
//   },
//   onError: () => {
//     paymentSnackbar.value = true
//   },
// })

const {
  isLoading: isUploadSignatureDocumentLoading,
  mutate: uploadSignatureDocument,
} = useMutation(
  ['uploadSignatureDocument'],
  async () => await postUploadSignatureFile(signatureForm, 'signature'),
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
  state.showUploadSuccessSnackbar = true
  updateMutation.mutate()
}

function handleShowUploadFailureSnackbar() {
  state.showUploadFailureSnackbar = true
  updateMutation.mutate()
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
