<template>
  <div>
    <v-container v-if="!state.previousSignature">
      <v-row justify="center">
        <v-col
          cols="12"
          class="text-center"
        >
          <canvas
            :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
            id="signature"
            class="signature"
          ></canvas>
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

      <v-row justify="center">
        <FormButtonContainer
          :valid="!isSignaturePadEmpty"
          :loading="state.uploading"
          :all-steps-complete="props.allStepsComplete"
          @submit="handleSubmit"
          @save="handleSave"
        />
      </v-row>
    </v-container>

    <v-container v-else>
      <v-row justify="center">
        <v-alert
          outlined
          type="success"
        >
          {{
            $t(
              'Signature has already been submitted. Press continue to move forward.'
            )
          }}
        </v-alert>
      </v-row>

      <v-row justify="center">
        <FormButtonContainer
          v-if="state.previousSignature"
          :valid="true"
          :submitting="state.submitted"
          :all-steps-complete="props.allStepsComplete"
          @submit="handleSkipSubmit"
          @save="router.push('/')"
          @cancel="router.push('/')"
        />
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import SignaturePad from 'signature_pad'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'

interface ISecondFormStepFourProps {
  routes: unknown
  value: CompleteApplication
  allStepsComplete: boolean
}

const props = defineProps<ISecondFormStepFourProps>()
const emit = defineEmits([
  'input',
  'handle-submit',
  'handle-save',
  'update-step-eight-valid',
])

const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const signaturePad = ref<SignaturePad>()

const state = reactive({
  file: {},
  signature: '',
  previousSignature: false,
  submitted: false,
  uploading: false,
})

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

onMounted(() => {
  for (let item of model.value.application.uploadedDocuments) {
    if (item.documentType === 'signature') {
      state.previousSignature = true
      emit('update-step-eight-valid', true)
    }
  }

  if (!state.previousSignature) {
    nextTick(() => {
      const canvas = document.getElementById('signature') as HTMLCanvasElement

      signaturePad.value = new SignaturePad(canvas, {
        backgroundColor: 'rgba(255, 255, 255, 0)',
      })
    })
  }
})

function handleClearSignature() {
  signaturePad.value?.clear()
}

const isSignaturePadEmpty = computed(() => {
  return signaturePad.value?.isEmpty()
})

const fileMutation = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    model.value.application.currentStep = 10
    applicationStore.updateApplication()
    router.push({
      path: props.routes.FINALIZE_ROUTE_PATH,
      query: {
        applicationId: model.value.id,
        isComplete: model.value.application.isComplete.toString(),
      },
    })
  },
  onError: () => {
    state.submitted = false
  },
})

async function handleSubmit() {
  state.submitted = true
  state.uploading = true
  const image = document.getElementById('signature')

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  image.toBlob(blob => {
    const file = new File([blob], 'signature.png', { type: 'image/png' })
    const form = new FormData()

    form.append('fileToUpload', file)

    state.file = form

    fileMutation.mutate()
  })

  emit('handle-submit')
}

function handleSave() {
  emit('handle-save')
}

async function handleFileUpload() {
  const newFileName = `${applicationStore.completeApplication.application.personalInfo.lastName}_${applicationStore.completeApplication.application.personalInfo.firstName}_signature`

  const uploadDoc: UploadedDocType = {
    documentType: 'signature',
    name: newFileName,
    uploadedBy: applicationStore.completeApplication.application.userEmail,
    uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
  }

  await axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
      state.file
    )
    .then(() => {
      applicationStore.completeApplication.application.uploadedDocuments.push(
        uploadDoc
      )
    })
    .catch(e => {
      window.console.warn(e)
      Promise.reject()
    })
    .finally(() => {
      state.uploading = false
    })
}

function handleSkipSubmit() {
  applicationStore.completeApplication.application.currentStep = 8
  applicationStore.updateApplication()
  router.push({
    path: props.routes.FINALIZE_ROUTE_PATH,
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete:
        applicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}

watch(isSignaturePadEmpty, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-eight-valid', !newValue)
  }
})
</script>

<style lang="scss" scoped>
.signature {
  border: 2px solid black;
  border-radius: 5px;
}
</style>
