<template>
  <div>
    <v-btn
      @click="handleShowModifySignatureDialog"
      :disabled="isGetApplicationsLoading || !canWithdrawApplication"
      color="primary"
      tonal
    >
      {{ title }}
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
      The signature upload failed. Please contact us if this error continues.
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
      Your signature was successfully updated.
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
import {
  ApplicationStatus,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

interface IModifySignatureDialog {
  title: string
}

const props = withDefaults(defineProps<IModifySignatureDialog>(), {
  title: 'Modify Signature',
})

const applicationStore = useCompleteApplicationStore()
const signaturePad = ref<SignaturePad>()
const signatureForm = new FormData()
const emit = defineEmits(['on-signature-submit'])

const state = reactive({
  snackbar: false,
  isApplicationValid: false,
  invalidSubmissionDialog: false,
  confirmSubmissionDialog: false,
  modifySignatureDialog: false,
  showUploadFailureSnackbar: false,
  showUploadSuccessSnackbar: false,
})

const { isLoading: isGetApplicationsLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getUserApplication(),
  {
    enabled: !state.isApplicationValid,
  }
)

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
  updateMutationWithoutRouting.mutate('Updated Signature')
}

function handleShowUploadFailureSnackbar() {
  state.showUploadFailureSnackbar = true
  updateMutationWithoutRouting.mutate('Failed to update Signature')
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
