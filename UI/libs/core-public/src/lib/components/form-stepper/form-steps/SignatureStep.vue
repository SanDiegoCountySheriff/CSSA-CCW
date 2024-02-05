<template>
  <div>
    <v-row justify-content="center">
      <v-container
        class="mb-10"
        style="max-width: 750px; padding-left: 55px"
      >
        <div>
          <v-card-title v-if="!isMobile">
            {{ $t('Terms and Agreements') }}
          </v-card-title>
          <template v-else>
            <v-card-title style="padding-bottom: 1px; padding-left: 20px">
              {{ $t('Terms and Agreements') }}
            </v-card-title>
            <div style="font-size: 14px; padding-top: 0px; padding-left: 0px">
              {{ $t('(Please read each document below and agree)') }}
            </div>
          </template>
        </div>
        <template>
          <v-row class="ml-4">
            <div style="display: flex; align-items: center">
              <span>
                <v-checkbox
                  v-if="!isMobile"
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .goodMoralCharacterAgreed
                  "
                  @click="setAgreedDate('goodMoralCharacterAgreedDate')"
                  hide-details
                  label="By checking this box, I agree to the "
                ></v-checkbox>
                <v-checkbox
                  v-else
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .goodMoralCharacterAgreed
                  "
                  @click="setAgreedDate('goodMoralCharacterAgreedDate')"
                  hide-details
                ></v-checkbox>
              </span>
              <template>
                <a
                  style="padding-top: 19px; padding-left: 10px"
                  href="#"
                  @click.prevent="
                    handleAgreementLinkClick('GoodMoralCharacter')
                  "
                  @keydown.enter="handleEnterKeyPress('GoodMoralCharacter')"
                  >Good Moral Character</a
                >
              </template>
              <template>
                <div
                  v-if="!isMobile"
                  style="padding-top: 19px; padding-left: 10px"
                >
                  {{
                    applicationStore.completeApplication.application.agreements
                      .goodMoralCharacterAgreedDate
                      ? formatDate(
                          applicationStore.completeApplication.application
                            .agreements.goodMoralCharacterAgreedDate
                        )
                      : ''
                  }}&nbsp;{{
                    applicationStore.completeApplication.application.agreements
                      .goodMoralCharacterAgreedDate
                      ? formatTime(
                          applicationStore.completeApplication.application
                            .agreements.goodMoralCharacterAgreedDate
                        )
                      : ''
                  }}
                </div>
              </template>
            </div>
          </v-row>
          <v-row class="ml-4">
            <div style="display: flex; align-items: center">
              <span>
                <v-checkbox
                  v-if="!isMobile"
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .conditionsForIssuanceAgreed
                  "
                  @click="setAgreedDate('conditionsForIssuanceAgreedDate')"
                  hide-details
                  label="By checking this box, I agree to the "
                ></v-checkbox>
                <v-checkbox
                  v-else
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .conditionsForIssuanceAgreed
                  "
                  @click="setAgreedDate('conditionsForIssuanceAgreedDate')"
                  hide-details
                ></v-checkbox>
              </span>
              <template>
                <a
                  style="padding-top: 19px; padding-left: 10px"
                  href="#"
                  @click.prevent="
                    handleAgreementLinkClick('ConditionsForIssuance')
                  "
                  @keydown.enter="handleEnterKeyPress('ConditionsForIssuance')"
                  >Conditions For Issuance</a
                >
              </template>
              <template>
                <div
                  v-if="!isMobile"
                  style="padding-top: 19px; padding-left: 10px"
                >
                  {{
                    applicationStore.completeApplication.application.agreements
                      .conditionsForIssuanceAgreedDate
                      ? formatDate(
                          applicationStore.completeApplication.application
                            .agreements.conditionsForIssuanceAgreedDate
                        )
                      : ''
                  }}&nbsp;{{
                    applicationStore.completeApplication.application.agreements
                      .conditionsForIssuanceAgreedDate
                      ? formatTime(
                          applicationStore.completeApplication.application
                            .agreements.conditionsForIssuanceAgreedDate
                        )
                      : ''
                  }}
                </div>
              </template>
            </div>
          </v-row>
          <v-row class="ml-4">
            <div style="display: flex; align-items: center">
              <span>
                <v-checkbox
                  v-if="!isMobile"
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .falseInfoAgreed
                  "
                  @click="setAgreedDate('falseInfoAgreedDate')"
                  hide-details
                  label="By checking this box, I agree to the "
                ></v-checkbox>
                <v-checkbox
                  v-else
                  v-model="
                    applicationStore.completeApplication.application.agreements
                      .falseInfoAgreed
                  "
                  @click="setAgreedDate('falseInfoAgreedDate')"
                  hide-details
                ></v-checkbox>
              </span>
              <template>
                <a
                  style="padding-top: 19px; padding-left: 10px"
                  href="#"
                  @click.prevent="handleAgreementLinkClick('FalseInfo')"
                  @keydown.enter="handleEnterKeyPress('FalseInfo')"
                  >False Info</a
                >
              </template>
              <template>
                <div
                  v-if="!isMobile"
                  style="padding-top: 19px; padding-left: 10px"
                >
                  {{
                    applicationStore.completeApplication.application.agreements
                      .falseInfoAgreedDate
                      ? formatDate(
                          applicationStore.completeApplication.application
                            .agreements.falseInfoAgreedDate
                        )
                      : ''
                  }}&nbsp;{{
                    applicationStore.completeApplication.application.agreements
                      .falseInfoAgreedDate
                      ? formatTime(
                          applicationStore.completeApplication.application
                            .agreements.falseInfoAgreedDate
                        )
                      : ''
                  }}
                </div>
              </template>
            </div>
          </v-row>
        </template>
      </v-container>
    </v-row>
    <v-container v-if="!state.previousSignature">
      <v-row justify="center">
        <v-card-title style="padding-bottom: 1px; padding-left: 20px">
          {{ $t('Please Sign Here') }}
        </v-card-title>
        <v-col
          cols="12"
          class="text-center"
        >
          <canvas
            :width="$vuetify.breakpoint.mdAndUp ? '600px' : ''"
            id="signature"
            class="signature"
            style="border: 2px solid"
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
          @continue="handleContinue"
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
          @submit="handleContinueWithoutUpload"
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
import SignaturePad from 'signature_pad'
import { UploadedDocType } from '@shared-utils/types/defaultTypes'
import axios from 'axios'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useMutation } from '@tanstack/vue-query'
import { useRouter } from 'vue-router/composables'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, nextTick, onMounted, reactive, ref, watch } from 'vue'
import {
  formatDate,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

interface ISecondFormStepFourProps {
  routes: unknown
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
    if (item.documentType === 'signature') {
      state.previousSignature = true
      emit('update-step-eight-valid', true)
    }
  }

  if (!state.previousSignature) {
    nextTick(() => {
      const canvas = document.getElementById('signature') as HTMLCanvasElement

      signaturePad.value = new SignaturePad(canvas, {
        backgroundColor: 'white',
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

function handleEnterKeyPress(agreementType) {
  applicationStore.getAgreementPdf(agreementType)
}

function handleAgreementLinkClick(agreementType) {
  applicationStore.getAgreementPdf(agreementType)
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
    path: props.routes.FINALIZE_ROUTE_PATH,
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

<style lang="scss" scoped>
.signature {
  border: black;
  border-radius: 5px;
}
</style>
