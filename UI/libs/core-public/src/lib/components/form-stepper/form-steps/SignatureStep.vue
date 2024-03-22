<template>
  <div>
    <v-row justify="center">
      <v-card-title>
        {{ $t('Terms and Agreements') }}
      </v-card-title>

      <v-card-text
        v-if="isMobile"
        class="text-center"
      >
        {{ $t('(Please read each document below and agree)') }}
      </v-card-text>
    </v-row>

    <v-container style="max-width: 750px">
      <v-row justify="center">
        <v-col
          cols="12"
          :class="isMobile ? 'd-flex justify-center' : ''"
        >
          <v-checkbox
            v-model="
              applicationStore.completeApplication.application.agreements
                .goodMoralCharacterAgreed
            "
            @click="setAgreedDate('goodMoralCharacterAgreedDate')"
            hide-details
          >
            <template #label>
              {{ !isMobile ? 'By checking this box, I agree to the' : '' }}
              <a
                href="#"
                @click.stop
                @click.prevent="
                  handleAgreementLinkClick('Good_Moral_Character')
                "
                @keydown.enter="handleEnterKeyPress('Good_Moral_Character')"
                class="mx-2"
              >
                Good Moral Character
              </a>

              <template
                v-if="
                  !isMobile &&
                  applicationStore.completeApplication.application.agreements
                    .goodMoralCharacterAgreedDate
                "
              >
                {{
                  new Date(
                    applicationStore.completeApplication.application.agreements.goodMoralCharacterAgreedDate
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
                @click.prevent="
                  handleAgreementLinkClick('Conditions_for_Issuance')
                "
                @keydown.enter="handleEnterKeyPress('Conditions_for_Issuance')"
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
                @click.prevent="handleAgreementLinkClick('False_Info')"
                @keydown.enter="handleEnterKeyPress('False_Info')"
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

      <v-row justify="center">
        <FormButtonContainer
          :valid="!isSignaturePadEmpty"
          :loading="state.uploading"
          :all-steps-complete="props.allStepsComplete"
          :is-final-step="true"
          @continue="handleContinue"
          @save="handleSave"
        />
      </v-row>
    </v-container>

    <v-container v-else>
      <v-row justify="center">
        <v-alert
          v-if="!model.application.isUpdatingApplication"
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
          :is-final-step="true"
          @continue="handleContinueWithoutUpload"
          @save="handleSave"
        />
      </v-row>
    </v-container>
  </div>
</template>

<script setup lang="ts">
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
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'

interface ISecondFormStepFourProps {
  value: CompleteApplication
  allStepsComplete: boolean
}

const props = defineProps<ISecondFormStepFourProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-eight-valid',
])

const router = useRouter()
const applicationStore = useCompleteApplicationStore()
const signaturePad = ref<SignaturePad>()
const vuetify = useVuetify()
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

const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

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

const isFalseInfoAgreed = computed(() => {
  return applicationStore.completeApplication.application.agreements
    .falseInfoAgreed
})

const isConditionsForIssuanceAgreed = computed(() => {
  return applicationStore.completeApplication.application.agreements
    .conditionsForIssuanceAgreed
})

const isGoodMoralCharacterAgreed = computed(() => {
  return applicationStore.completeApplication.application.agreements
    .goodMoralCharacterAgreed
})

const fileMutation = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    model.value.application.currentStep = 10
    applicationStore.updateApplication()
    router.push({
      path: Routes.FINALIZE_ROUTE_PATH,
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

async function handleContinue() {
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

  emit('handle-continue')
}

function handleSave() {
  emit('handle-save')
}

async function handleFileUpload() {
  const newFileName = `${applicationStore.completeApplication.application.personalInfo.lastName}_${applicationStore.completeApplication.application.personalInfo.firstName}_Signature`

  const uploadDoc: UploadedDocType = {
    documentType: 'Signature',
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

async function handleEnterKeyPress(agreementType) {
  await applicationStore.getAgreementPdf(agreementType)
}

async function handleAgreementLinkClick(agreementType) {
  await applicationStore.getAgreementPdf(agreementType)
}

function setAgreedDate(agreedDateKey) {
  if (
    applicationStore.completeApplication.application.agreements[
      agreedDateKey
    ] == null
  ) {
    applicationStore.completeApplication.application.agreements[agreedDateKey] =
      new Date().toLocaleString()
  } else {
    applicationStore.completeApplication.application.agreements[agreedDateKey] =
      null
  }
}

function handleContinueWithoutUpload() {
  applicationStore.completeApplication.application.currentStep = 8
  applicationStore.updateApplication()
  router.push({
    path: Routes.FINALIZE_ROUTE_PATH,
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete:
        applicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}

watch(
  [
    isSignaturePadEmpty,
    isFalseInfoAgreed,
    isConditionsForIssuanceAgreed,
    isGoodMoralCharacterAgreed,
  ],
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
