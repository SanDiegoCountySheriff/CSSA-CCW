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
              type="info"
              outlined
              color="primary"
            >
              You will be asked to pay after a Licensing employee reviews your
              modification application.
            </v-alert>
          </v-row>
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
              :loading="isUpdateApplicationLoading || uploading"
              :disabled="isSignaturePadEmpty"
              @click="handleSubmit"
              color="primary"
            >
              Submit
            </v-btn>
          </v-row>
        </v-container>
      </v-form>
    </v-card>
  </v-container>
</template>

<script lang="ts" setup>
import Endpoints from '@shared-ui/api/endpoints'
import Routes from '@core-public/router/routes'
import SignaturePad from 'signature_pad'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import {
  ApplicationStatus,
  ApplicationType,
  UploadedDocType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, ref } from 'vue'

const isLoading = ref(false)
const uploading = ref(false)
const submitted = ref(false)
const file = ref({})
const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const form = ref()
const valid = ref(false)
const signaturePad = ref<SignaturePad>()
const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

onMounted(() => {
  nextTick(() => {
    const canvas = document.getElementById('signature') as HTMLCanvasElement

    signaturePad.value = new SignaturePad(canvas, {
      backgroundColor: 'rgba(255, 255, 255, 0)',
    })
  })
})

const { isLoading: isUpdateApplicationLoading, mutate: updateMutation } =
  useMutation({
    mutationFn: (updateReason: string) => {
      return applicationStore.updateApplication(updateReason)
    },
    onSuccess: () => {
      router.push(Routes.RECEIPT_PATH)
    },
  })

const fileMutation = useMutation({
  mutationFn: async () => {
    const newFileName = `Signature`

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
    applicationStore.completeApplication.application.modificationSubmittedToLicensingDateTime =
      new Date().toISOString()

    updateMutation('Submit Modification')
  },
  onError: () => {
    uploading.value = false
    submitted.value = false
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
