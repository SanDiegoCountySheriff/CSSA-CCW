<template>
  <div>
    <v-row justify="center">
      <v-card-title>
        {{ $t('Terms and Agreements') }}
      </v-card-title>
    </v-row>

    <v-container style="max-width: 750px">
      <v-alert
        v-if="isMobile"
        type="warning"
        outlined
      >
        <span :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''">
          Please read each document by clicking on the links below. Checking the
          boxes indicates you accept the terms and agreements
        </span>
      </v-alert>

      <v-row justify="center">
        <v-col
          cols="12"
          :class="isMobile ? 'd-flex justify-center' : ''"
        >
          <v-checkbox
            v-model="
              applicationStore.completeApplication.application.agreements
                .conditionsForIssuanceAgreed
            "
            @click="setAgreedDate('conditionsForIssuanceAgreedDate')"
            hide-details
          >
            <template #label>
              {{ !isMobile ? 'By checking this box, I agree to the ' : '' }}
              <a
                href="#"
                @click.stop
                @click.prevent="handleConditionAgreementLinkClick()"
                @keydown.enter="handleConditionEnterKeyPress()"
                class="mx-2"
              >
                Conditions For Issuance
              </a>

              <template
                v-if="
                  !isMobile &&
                  applicationStore.completeApplication.application.agreements
                    .conditionsForIssuanceAgreedDate
                "
              >
                {{
                  new Date(
                    applicationStore.completeApplication.application.agreements.conditionsForIssuanceAgreedDate
                  ).toLocaleString()
                }}
              </template>
            </template>
          </v-checkbox>
        </v-col>

        <v-col
          cols="12"
          :class="isMobile ? 'd-flex justify-center' : ''"
        >
          <v-checkbox
            v-model="
              applicationStore.completeApplication.application.agreements
                .falseInfoAgreed
            "
            @click="setAgreedDate('falseInfoAgreedDate')"
            hide-details
          >
            <template #label>
              {{ !isMobile ? 'By checking this box, I agree to the ' : '' }}
              <a
                href="#"
                @click.stop
                @click.prevent="handleFalseInfoAgreementLinkClick()"
                @keydown.enter="handleFalseInfoEnterKeyPress()"
                class="mx-2"
              >
                False Info
              </a>

              <template
                v-if="
                  !isMobile &&
                  applicationStore.completeApplication.application.agreements
                    .falseInfoAgreedDate
                "
              >
                {{
                  new Date(
                    applicationStore.completeApplication.application.agreements.falseInfoAgreedDate
                  ).toLocaleString()
                }}
              </template>
            </template>
          </v-checkbox>
        </v-col>
      </v-row>
    </v-container>

    <v-container v-if="!state.previousSignature">
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
            :width="$vuetify.breakpoint.mdAndUp ? '605px' : ''"
            height="105px"
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

      <FormButtonContainer
        :valid="!isSignaturePadEmpty"
        :loading="state.uploading || isLoading || loading"
        :all-steps-complete="props.allStepsComplete"
        :is-final-step="true"
        @continue="handleContinue"
        @save="handleSave"
        @save-match="handleSaveMatch"
        v-on="$listeners"
      />
    </v-container>

    <v-container v-else>
      <v-row justify="center">
        <v-card-title>
          {{ $t('Signature Uploaded') }}
        </v-card-title>

        <v-col
          cols="12"
          class="d-flex align-center justify-center"
        >
          <v-container style="max-width: 600px">
            <v-row
              v-if="
                !applicationStore.completeApplication.application
                  .isUpdatingApplication && !state.isMatching
              "
              justify="center"
            >
              <v-alert
                v-if="!isRenew"
                color="success"
                type="success"
                outlined
              >
                <span
                  :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''"
                >
                  You have already uploaded a signature.
                </span>
              </v-alert>
            </v-row>
          </v-container>
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
          <!-- <v-btn
            color="primary"
            text
            @click="handleModifySignature"
          >
            {{ $t('Modify Signature') }}
          </v-btn> -->
        </v-col>
      </v-row>

      <FormButtonContainer
        :valid="!isSignaturePadEmpty"
        :loading="state.uploading || isLoading || loading"
        :all-steps-complete="props.allStepsComplete"
        :is-final-step="true"
        @continue="handleContinue"
        @save="handleSave"
        @save-match="handleSaveMatch"
        v-on="$listeners"
      />
    </v-container>

    <v-container style="max-width: 750px">
      <v-row
        v-if="
          !applicationStore.completeApplication.application
            .isUpdatingApplication && !state.isMatching
        "
        justify="center"
      >
        <v-alert
          v-if="!isRenew"
          color="primary"
          type="info"
          outlined
        >
          <span :class="themeStore.getThemeConfig.isDark ? 'white--text' : ''">
            You must finalize your application and select an appointment date
            and time in order to submit to Licensing.
          </span>
        </v-alert>
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { ApplicationType } from '@shared-utils/types/defaultTypes'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Endpoints from '@shared-ui/api/endpoints'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import Routes from '@core-public/router/routes'
import SignaturePad from 'signature_pad'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'

interface ISecondFormStepFourProps {
  value: CompleteApplication
  allStepsComplete: boolean
  loading: boolean
}

const props = defineProps<ISecondFormStepFourProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-eight-valid',
  'handle-condition-agreement-link',
  'handle-falseinfo-agreement-link',
  'handle-condition-agreement-enter',
  'handle-falseinfo-agreement-enter',
])

const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const themeStore = useThemeStore()
const signaturePad = ref<SignaturePad>()
const vuetify = useVuetify()
const state = reactive({
  file: {},
  signature: '',
  previousSignature: false,
  submitted: false,
  uploading: false,
  isMatching: false,
})

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const isMobile = computed(() => {
  return vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
})

onMounted(() => {
  for (let item of model.value.application.uploadedDocuments) {
    if (item.documentType === 'Signature') {
      state.previousSignature = true
      emit('update-step-eight-valid', true)
    }
  }

  if (!state.previousSignature) {
    nextTick(() => {
      const canvas = document.getElementById('signature') as HTMLCanvasElement

      signaturePad.value = new SignaturePad(canvas, {
        backgroundColor: 'rgba(0, 0, 0, 0)',
        minDistance: 5,
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

// const isSignatureMinLength = computed(() => {
//   const points = signaturePad.value?.toData()
//   // eslint-disable-next-line prefer-spread
//   const pointCount = [].concat.apply([], points).length

//   return points && pointCount >= 2
// })

const isFalseInfoAgreed = computed(() => {
  return applicationStore.completeApplication.application.agreements
    .falseInfoAgreed
})

const isConditionsForIssuanceAgreed = computed(() => {
  return applicationStore.completeApplication.application.agreements
    .conditionsForIssuanceAgreed
})

const isRenew = computed(() => {
  const applicationType = model.value.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const { mutate: fileMutation, isLoading } = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    if (!state.isMatching) {
      model.value.application.currentStep = 10
      applicationStore.updateApplication()
      router.push({
        path: Routes.FINALIZE_ROUTE_PATH,
        query: {
          applicationId: model.value.id,
          isComplete: model.value.application.isComplete.toString(),
        },
      })
    } else if (state.isMatching) {
      emit('handle-save', true)
    }
  },
  onError: () => {
    state.submitted = false
  },
})

async function handleContinue() {
  state.submitted = true
  state.uploading = true
  const image = document.getElementById('signature')

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  image.toBlob(blob => {
    const file = new File([blob], 'signature.png', { type: 'image/png' })
    const form = new FormData()

    form.append('fileToUpload', file)

    state.file = form

    fileMutation()
  })

  emit('handle-continue')
}

async function handleSaveMatch() {
  state.submitted = true
  state.uploading = true
  state.isMatching = true
  const image = document.getElementById('signature')

  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  // @ts-ignore
  image.toBlob(blob => {
    const file = new File([blob], 'signature.png', { type: 'image/png' })
    const form = new FormData()

    form.append('fileToUpload', file)

    state.file = form

    fileMutation()
  })
}

function handleSave() {
  emit('handle-save')
}

async function handleFileUpload() {
  const uploadDoc: UploadedDocType = {
    documentType: 'Signature',
    name: 'Signature',
    uploadedBy: applicationStore.completeApplication.application.userEmail,
    uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
  }

  await axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${'Signature'}`,
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

async function handleConditionEnterKeyPress() {
  emit('handle-condition-agreement-enter')
}

async function handleConditionAgreementLinkClick() {
  emit('handle-condition-agreement-link')
}

async function handleFalseInfoEnterKeyPress() {
  emit('handle-falseinfo-agreement-enter')
}

async function handleFalseInfoAgreementLinkClick() {
  emit('handle-falseinfo-agreement-link')
}

function setAgreedDate(agreedDateKey) {
  if (
    applicationStore.completeApplication.application.agreements[
      agreedDateKey
    ] === null ||
    applicationStore.completeApplication.application.agreements[
      agreedDateKey
    ] === ''
  ) {
    applicationStore.completeApplication.application.agreements[agreedDateKey] =
      new Date().toLocaleString()
  } else {
    applicationStore.completeApplication.application.agreements[agreedDateKey] =
      null
  }
}

// function handleContinueWithoutUpload() {
//   applicationStore.completeApplication.application.currentStep = 8
//   applicationStore.updateApplication()
//   router.push({
//     path: Routes.FINALIZE_ROUTE_PATH,
//     query: {
//       applicationId: applicationStore.completeApplication.id,
//       isComplete:
//         applicationStore.completeApplication.application.isComplete.toString(),
//     },
//   })
// }

// const { mutate: updateMutation } = useMutation({
//   mutationFn: () => {
//     return applicationStore.updateApplication()
//   },
//   onSuccess: () => {
//     for (let item of applicationStore.completeApplication.application
//       .uploadedDocuments) {
//       switch (item.documentType.toLowerCase()) {
//         case 'signature':
//           state.signature = item.name
//           break
//         default:
//           break
//       }
//     }

//     state.file = {}
//   },
//   onError: () => {
//     state.submitted = false
//   },
// })

// function handleModifySignature() {
//   applicationStore.completeApplication.application.uploadedDocuments.forEach(
//     file => {
//       if (file.documentType === 'Signature') {
//         deleteFile(file.name)
//       }
//     }
//   )
// }

// async function deleteFile(name) {
//   const documentToDelete =
//     applicationStore.completeApplication.application.uploadedDocuments.find(
//       doc => doc.name === name
//     )

//   if (!documentToDelete) {
//     return
//   }

//   try {
//     await axios
//       .delete(
//         `${Endpoints.DELETE_DOCUMENT_FILE_PUBLIC_ENDPOINT}?applicantFileName=${name}`
//       )
//       .then(() => {
//         applicationStore.completeApplication.application.uploadedDocuments.pop()
//       })

//     const updatedDocuments =
//       applicationStore.completeApplication.application.uploadedDocuments.filter(
//         doc => doc.name !== name
//       )

//     applicationStore.completeApplication.application.uploadedDocuments =
//       updatedDocuments

//     updateMutation()
//     // eslint-disable-next-line no-empty
//   } finally {
//   }
// }

watch(
  [isSignaturePadEmpty, isFalseInfoAgreed, isConditionsForIssuanceAgreed],
  newValues => {
    const [isSigPadEmpty, ...otherValues] = newValues
    const allTrueExceptSigPad = otherValues.every(val => val)
    const shouldEmit = !isSigPadEmpty && allTrueExceptSigPad

    if (shouldEmit) {
      emit('update-step-eight-valid', true)
    } else {
      emit('update-step-eight-valid', false)
    }
  }
)
</script>

<style lang="scss" scoped>
.signature {
  border: black;
  border-radius: 5px;
}
</style>
