<template>
  <div class="signature-container">
    <v-container v-if="!state.previousSignature">
      <v-row class="mb-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-form
            ref="form"
            v-model="valid"
          >
            <v-text-field
              outlined
              persistent-hint
              dense
              :label="$t('Signature')"
              :rules="[v => !!v || $t(' Signature cannot be blank ')]"
              v-model="state.signature"
              @keydown.enter.prevent
            />
          </v-form>
        </v-col>
        <v-col
          cols="12"
          lg="5"
        >
          <div :class="$vuetify.theme.dark ? 'dark-preview' : 'preview'">
            <canvas
              id="signatureCanvas"
              ref="signatureCanvas"
              height="100"
              width="300"
            ></canvas>
            <v-btn
              text
              class="m-5"
              color="error"
              @click="handleCanvasClear"
            >
              {{ $t('clear') }}
            </v-btn>
          </div>
        </v-col>
      </v-row>
      <v-divider class="mb-5" />
      <FormButtonContainer
        :valid="valid"
        @submit="handleSubmit"
        @save="handleSave"
      />
    </v-container>
    <v-container
      fluid
      v-else
    >
      <v-row :style="{ width: '100%' }">
        <v-icon
          color="success"
          x-large
        >
          mdi-check-circle-outline
        </v-icon>
        <v-subheader class="sub-header pt-2">
          {{
            $t(
              'Signature has already been submitted. Press continue to move forward.'
            )
          }}
        </v-subheader>
      </v-row>
      <v-row>
        <FormButtonContainer
          :style="{ width: '100%' }"
          v-if="state.previousSignature"
          :valid="true"
          :submitting="state.submited"
          @submit="handleSkipSubmit"
          @save="router.push('/')"
          @cancel="router.push('/')"
          @back="handlePreviousSection"
        />
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import {
  computed,
  getCurrentInstance,
  onMounted,
  reactive,
  ref,
  watch,
} from 'vue'

interface ISecondFormStepFourProps {
  routes: unknown
  value: CompleteApplication
}

const app = getCurrentInstance()
const props = defineProps<ISecondFormStepFourProps>()
const emit = defineEmits([
  'input',
  'handle-submit',
  'handle-save',
  'update-step-eight-valid',
])
const signatureCanvas = ref<HTMLCanvasElement | null>(null)
const router = useRouter()
const valid = ref(false)
const applicationStore = useCompleteApplicationStore()

const state = reactive({
  file: {},
  signature: '',
  previousSignature: false,
  submited: false,
})

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-eight-valid', newValue)
  }
})

onMounted(() => {
  for (let item of model.value.application.uploadedDocuments) {
    if (item.documentType === 'signature') {
      state.previousSignature = true
    }
  }
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
        isComplete: model.value.application.isComplete,
      },
    })
  },
  onError: () => {
    state.submited = false
  },
})

async function handleSubmit() {
  state.submited = true
  const image = document.getElementById('signatureCanvas')

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
}

function handleCanvasClear() {
  const canvas = signatureCanvas.value
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas?.getContext('2d')

  ctx?.clearRect(0, 0, 300, 100)
  state.signature = ''
}

watch(state, () => {
  handleCanvasUpdate()
})

function handleCanvasUpdate() {
  const canvas = signatureCanvas.value
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas.getContext('2d')

  if (ctx) {
    ctx.font = '30px Brush Script MT'
    ctx.clearRect(0, 0, 300, 100)
    app?.proxy.$vuetify.theme.dark
      ? (ctx.fillStyle = '#111')
      : (ctx.fillStyle = '#FFF')
    ctx.fillRect(0, 0, 300, 100)
    app?.proxy.$vuetify.theme.dark
      ? (ctx.fillStyle = '#FFF')
      : (ctx.fillStyle = '#111')
    ctx.fillText(state.signature, 10, 50)
  }
}

function handleSkipSubmit() {
  valid.value = false
  applicationStore.completeApplication.application.currentStep = 10
  applicationStore.updateApplication()
  router.push({
    path: props.routes.FINALIZE_ROUTE_PATH,
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}
</script>

<style scoped lang="scss">
.signature-container {
  width: 100%;
}
.preview {
  border: 1px solid #333;
  border-radius: 10px;
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: flex-end;
  margin-left: 1rem;
}

.dark-preview {
  border: 1px solid #ddd;
  border-radius: 10px;
  color: #ddd;
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: flex-end;
  margin-left: 1rem;
}
</style>
