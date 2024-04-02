<template>
  <v-container>
    <v-card
      :loading="isLoading"
      flat
    >
      <v-form
        ref="form"
        v-model="valid"
      >
        <v-container>
          <v-row justify="center">
            <v-alert
              v-if="isPaymentComplete"
              :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
              color="primary"
              type="info"
              outlined
            >
              Modification Payment is complete.
            </v-alert>
          </v-row>
        </v-container>

        <template>
          <v-container v-if="!isPaymentComplete">
            <v-row justify="center">
              <v-card-title>
                Modification Payment of ${{ brandStore.brand.cost.modify }} is
                required
              </v-card-title>
            </v-row>

            <v-row
              v-if="brandStore.brand.cost.convenienceFee > 0"
              justify="center"
              class="text-center"
            >
              <v-card-text>
                In order to pay online with a credit card a convenience fee of
                {{ brandStore.brand.cost.convenienceFee }}% will be added to the
                transaction.
              </v-card-text>
            </v-row>

            <v-row justify="center">
              <v-btn
                color="primary"
                :loading="isMakePaymentLoading || isUpdatePaymentHistoryLoading"
                @click="makePayment"
              >
                Pay Now
              </v-btn>
            </v-row>
          </v-container>

          <v-container>
            <v-row justify="center">
              <v-card-title>
                {{ $t('Please Sign Here') }}
              </v-card-title>

              <v-col
                cols="12"
                class="d-flex align-center justify-center"
              >
                <v-card
                  light
                  flat
                  :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
                  height="100px"
                  outlined
                  style="border: solid 2px black"
                >
                  <canvas
                    :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
                    height="100px"
                    id="signature"
                    class="signature"
                  ></canvas>
                </v-card>
              </v-col>

              <v-col
                cols="12"
                class="text-center"
              >
                <v-btn
                  color="primary"
                  text
                  @click="handleClearSignature"
                >
                  {{ $t('Clear Signature') }}
                </v-btn>
              </v-col>
            </v-row>

            <v-row
              justify="center"
              class="mb-5"
            >
              <v-btn
                :loading="
                  isUpdateApplicationLoading ||
                  isMakePaymentLoading ||
                  uploading
                "
                :disabled="isSignaturePadEmpty || !isPaymentComplete"
                @click="handleSubmit"
                color="primary"
              >
                Submit
              </v-btn>
            </v-row>
          </v-container>
        </template>
      </v-form>
    </v-card>

    <v-snackbar
      v-model="paymentSnackbar"
      :timeout="-1"
      color="primary"
      persistent
    >
      {{ $t('There was a problem processing the payment, please try again.') }}
      <v-btn
        @click="paymentSnackbar = !paymentSnackbar"
        icon
      >
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </v-snackbar>
  </v-container>
</template>

<script lang="ts" setup>
import Endpoints from '@shared-ui/api/endpoints'
import Routes from '@core-public/router/routes'
import SignaturePad from 'signature_pad'
import axios from 'axios'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import {
  ApplicationStatus,
  ApplicationType,
  PaymentType,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router/composables'

const isLoading = ref(false)
const route = useRoute()
const uploading = ref(false)
const submitted = ref(false)
const file = ref({})
const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const form = ref()
const valid = ref(false)
const paymentSnackbar = ref(false)
const signaturePad = ref<SignaturePad>()
const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

const isPaymentComplete = computed(() => {
  return applicationStore.completeApplication.paymentHistory.some(ph => {
    return ph.paymentType === 4 && ph.successful === true
  })
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
  if (!applicationStore.completeApplication.application.orderId) {
    isLoading.value = true
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId as string,
        Boolean(route.query.isComplete)
      )
      .then(res => {
        applicationStore.setCompleteApplication(res)
        isLoading.value = false
      })
  } else {
    isLoading.value = false
  }

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

  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgba(255, 255, 255, 0)',
    })
  })
})

const { isLoading: isUpdateApplicationLoading, mutate: updateMutation } =
  useMutation({
    mutationFn: () => {
      return applicationStore.updateApplication()
    },
    onSuccess: () => {
      router.push(Routes.RECEIPT_PATH)
    },
  })

const fileMutation = useMutation({
  mutationFn: async () => {
    const newFileName = `${applicationStore.completeApplication.application.personalInfo.lastName}_${applicationStore.completeApplication.application.personalInfo.firstName}_Signature`

    const uploadDoc: UploadedDocType = {
      documentType: 'Signature',
      name: newFileName,
      uploadedBy: applicationStore.completeApplication.application.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    }

    applicationStore.completeApplication.application.uploadedDocuments.push(
      uploadDoc
    )

    await axios.post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
      file.value
    )
  },
  onSuccess: () => {
    applicationStore.completeApplication.application.applicationType =
      ApplicationType['Modify Standard']
    applicationStore.completeApplication.application.status =
      ApplicationStatus.Submitted

    updateMutation()
  },
  onError: () => {
    uploading.value = false
    submitted.value = false
  },
})

const { mutate: makePayment, isLoading: isMakePaymentLoading } = useMutation({
  mutationFn: () => {
    const cost = brandStore.brand.cost.modify
    let paymentType: string

    switch (applicationStore.completeApplication.application.applicationType) {
      case ApplicationType['Modify Standard']:
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
        break
      case ApplicationType['Modify Judicial']:
        paymentType =
          PaymentType[
            'CCW Application Modification Judicial Payment'
          ].toString()
        break
      case ApplicationType['Modify Reserve']:
        paymentType =
          PaymentType['CCW Application Modification Reserve Payment'].toString()
        break
      case ApplicationType['Modify Employment']:
        paymentType =
          PaymentType[
            'CCW Application Modification Employment Payment'
          ].toString()
        break
      default:
        paymentType =
          PaymentType['CCW Application Modification Payment'].toString()
    }

    return paymentStore.getPayment(
      applicationStore.completeApplication.id,
      cost,
      applicationStore.completeApplication.application.orderId,
      paymentType
    )
  },
  onError: () => {
    paymentSnackbar.value = true
  },
})

function handleSubmit() {
  uploading.value = true
  const image = document.getElementById('signature')

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  image.toBlob(blob => {
    const signature = new File([blob], 'signature.png', { type: 'image/png' })
    const formData = new FormData()

    formData.append('fileToUpload', signature)

    file.value = formData

    fileMutation.mutate()
  })
}

function handleClearSignature() {
  signaturePad.value?.clear()
}
</script>
