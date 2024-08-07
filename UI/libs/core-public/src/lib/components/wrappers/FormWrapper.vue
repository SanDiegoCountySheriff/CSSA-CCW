<template>
  <div>
    <v-container v-if="isGetApplicationsLoading && !state.isError">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>

    <v-container
      v-else-if="!$vuetify.breakpoint.mdAndDown"
      fluid
    >
      <v-stepper
        v-model="stepIndex.step"
        non-linear
        class="stepper"
        @change="updateMutation('Next Step')"
        :alt-labels="$vuetify.breakpoint.lgAndDown"
      >
        <v-stepper-header
          :class="$vuetify.theme.dark ? 'sticky-dark' : 'sticky-light'"
        >
          <v-stepper-step
            :step="1"
            :complete="stepOneValid"
            :rules="[() => stepOneValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Personal') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="2"
            :complete="stepTwoValid"
            :rules="[() => stepTwoValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('ID Information') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="3"
            :complete="stepThreeValid"
            :rules="[() => stepThreeValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Address') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="4"
            :complete="stepFourValid"
            :rules="[() => stepFourValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Employment & Weapons') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="5"
            :complete="stepFiveValid"
            :rules="[() => stepFiveValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Application Type') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="6"
            :complete="stepSixValid"
            :rules="[() => stepSixValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t(' Upload Files') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="7"
            :complete="stepSevenValid"
            :rules="[() => stepSevenValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Qualifying Questions') }}
              </span>
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            :step="8"
            :complete="stepEightValid"
            :rules="[() => stepEightValid]"
            error-icon="mdi-alert-circle"
            edit-icon="mdi-check"
            color="success"
            editable
          >
            <div class="text-center">
              <span
                :class="
                  themeStore.themeConfig.isDark ? 'white--text' : 'black--text'
                "
              >
                {{ $t('Signature') }}
              </span>
            </div>
          </v-stepper-step>

          <v-progress-linear
            :active="isLoading || isSaveLoading || isAgreementLoading"
            indeterminate
          ></v-progress-linear>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <PersonalInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-one-valid="handleUpdateStepOneValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
            />
          </v-stepper-content>
          <v-stepper-content :step="2">
            <IdBirthInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-two-valid="handleUpdateStepTwoValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="3">
            <AddressInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-three-valid="handleUpdateStepThreeValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="4">
            <WorkInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-four-valid="handleUpdateStepFourValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="5">
            <ApplicationTypeStep
              v-model="applicationStore.completeApplication"
              @update-step-five-valid="handleUpdateStepFiveValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="6">
            <FileUploadStep
              v-model="applicationStore.completeApplication"
              @update-step-six-valid="handleUpdateStepSixValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="7">
            <QualifyingQuestionsStep
              v-if="
                applicationStore.completeApplication.application
                  .qualifyingQuestions
              "
              v-model="applicationStore.completeApplication"
              @update-step-seven-valid="handleUpdateStepSevenValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
            <LegacyQualifyingQuestionsStep
              v-else
              v-model="applicationStore.completeApplication"
              @update-step-seven-valid="handleUpdateStepSevenValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-stepper-content>
          <v-stepper-content :step="8">
            <SignatureStep
              v-model="applicationStore.completeApplication"
              :all-steps-complete="allStepsComplete"
              :loading="isSaveLoading"
              @update-step-eight-valid="handleUpdateStepEightValid"
              @handle-save="handleSave"
              @previous-step="handlePrevious"
              @handle-condition-agreement-link="
                handleAgreementLinkClick('Conditions_for_Issuance')
              "
              @handle-falseinfo-agreement-link="
                handleAgreementLinkClick('False_Info')
              "
              @handle-falseinfo-agreement-enter="
                handleEnterKeyPress('Conditions_for_Issuance')
              "
              @handle-condition-agreement-enter="
                handleEnterKeyPress('False_Info')
              "
            />
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-container>

    <v-container
      v-else
      class="pa-0"
    >
      <v-progress-circular
        v-if="isLoading || isSaveLoading || isAgreementLoading"
        indeterminate
        absolute
        class="progress-circular"
      ></v-progress-circular>

      <v-expansion-panels
        v-model="expansionStep"
        accordion
        @change="updateMutation('Next Step')"
      >
        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 1"
            :color="stepIndex.step === 1 ? 'primary' : ''"
            :class="stepIndex.step === 1 ? 'white--text' : ''"
          >
            {{ $t('Personal') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 1 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <PersonalInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-one-valid="handleUpdateStepOneValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 2"
            :color="stepIndex.step === 2 ? 'primary' : ''"
            :class="stepIndex.step === 2 ? 'white--text' : ''"
          >
            {{ $t('ID Information') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 2 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <IdBirthInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-two-valid="handleUpdateStepTwoValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 3"
            :color="stepIndex.step === 3 ? 'primary' : ''"
            :class="stepIndex.step === 3 ? 'white--text' : ''"
          >
            {{ $t('Address') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 3 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <AddressInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-three-valid="handleUpdateStepThreeValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 4"
            :color="stepIndex.step === 4 ? 'primary' : ''"
            :class="stepIndex.step === 4 ? 'white--text' : ''"
          >
            {{ $t(' Employment & Weapons') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 4 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <WorkInfoStep
              v-model="applicationStore.completeApplication"
              @update-step-four-valid="handleUpdateStepFourValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 5"
            :color="stepIndex.step === 5 ? 'primary' : ''"
            :class="stepIndex.step === 5 ? 'white--text' : ''"
          >
            {{ $t('Application Type') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 5 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ApplicationTypeStep
              v-model="applicationStore.completeApplication"
              @update-step-five-valid="handleUpdateStepFiveValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 6"
            :color="stepIndex.step === 6 ? 'primary' : ''"
            :class="stepIndex.step === 6 ? 'white--text' : ''"
          >
            {{ $t(' Upload Files') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 6 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <FileUploadStep
              v-model="applicationStore.completeApplication"
              @update-step-six-valid="handleUpdateStepSixValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 7"
            :color="stepIndex.step === 7 ? 'primary' : ''"
            :class="stepIndex.step === 7 ? 'white--text' : ''"
          >
            {{ $t('Qualifying Questions') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 7 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <QualifyingQuestionsStep
              v-if="
                applicationStore.completeApplication.application
                  .qualifyingQuestions
              "
              v-model="applicationStore.completeApplication"
              @update-step-seven-valid="handleUpdateStepSevenValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
            <LegacyQualifyingQuestionsStep
              v-else
              v-model="applicationStore.completeApplication"
              @update-step-seven-valid="handleUpdateStepSevenValid"
              @handle-save="handleSave"
              @handle-continue="handleContinue"
              @previous-step="handlePrevious"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = 8"
            :color="stepIndex.step === 8 ? 'primary' : ''"
            :class="stepIndex.step === 8 ? 'white--text' : ''"
          >
            {{ $t('Signature') }}
            <template #actions>
              <v-icon :color="stepIndex.step === 8 ? 'white' : ''">
                $expand
              </v-icon>
            </template>
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <SignatureStep
              v-model="applicationStore.completeApplication"
              :all-steps-complete="allStepsComplete"
              :loading="isSaveLoading"
              @update-step-eight-valid="handleUpdateStepEightValid"
              @handle-save="handleSave"
              @previous-step="handlePrevious"
              @handle-condition-agreement-link="
                handleAgreementLinkClick('Conditions_for_Issuance')
              "
              @handle-falseinfo-agreement-link="
                handleAgreementLinkClick('False_Info')
              "
              @handle-falseinfo-agreement-enter="
                handleEnterKeyPress('Conditions_for_Issuance')
              "
              @handle-condition-agreement-enter="
                handleEnterKeyPress('False_Info')
              "
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import AddressInfoStep from '@core-public/components/form-stepper/form-steps/AddressInfoStep.vue'
import ApplicationTypeStep from '@core-public/components/form-stepper/form-steps/ApplicationTypeStep.vue'
import FileUploadStep from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue'
import IdBirthInfoStep from '@core-public/components/form-stepper/form-steps/IdBirthInfoStep.vue'
import LegacyQualifyingQuestionsStep from '../form-stepper/form-steps/LegacyQualifyingQuestionsStep.vue'
import PersonalInfoStep from '@core-public/components/form-stepper/form-steps/PersonalInfoStep.vue'
import QualifyingQuestionsStep from '@core-public/components/form-stepper/form-steps/QualifyingQuestionsStep.vue'
import SignatureStep from '@core-public/components/form-stepper/form-steps/SignatureStep.vue'
import WorkInfoStep from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { computed, onMounted, provide, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const applicationStore = useCompleteApplicationStore()
const themeStore = useThemeStore()
const stepOneValid = ref(false)
const stepTwoValid = ref(false)
const stepThreeValid = ref(false)
const stepFourValid = ref(false)
const stepFiveValid = ref(false)
const stepSixValid = ref(false)
const stepSevenValid = ref(false)
const stepEightValid = ref(false)
const router = useRouter()
const isAgreementLoading = ref(false)

const stepIndex = reactive({
  step: 0,
  previousStep: 0,
})

const state = reactive({
  isLoading: false,
  isError: false,
  isApplicationValid: false,
})

const expansionStep = computed({
  get() {
    return stepIndex.step - 1
  },
  set(value) {
    stepIndex.step = value
  },
})

const { isLoading, mutate: updateMutation } = useMutation({
  mutationFn: (updateReason: string) => {
    return applicationStore.updateApplication(updateReason)
  },
})

provide('isLoading', isLoading)

const { refetch } = useQuery(
  ['getApplicationsByUser'],
  applicationStore.getUserApplication,
  {
    enabled: false,
  }
)

const { isLoading: isSaveLoading, mutate: saveMutation } = useMutation({
  mutationKey: ['saveMutation'],
  mutationFn: (updateReason: string) => {
    if (
      applicationStore.completeApplication.application.isUpdatingApplication
    ) {
      applicationStore.completeApplication.application.isUpdatingApplication =
        false
    }

    return applicationStore.updateApplication(updateReason)
  },
  onSuccess: () => {
    refetch()
    router.push('/')
  },
})

onMounted(() => {
  window.scrollTo(0, 0)
  state.isApplicationValid = Boolean(applicationStore.completeApplication.id)

  stepIndex.step = applicationStore.completeApplication.application.currentStep

  if (stepIndex.step === 10) {
    stepIndex.step = 8
  }
})

const { isLoading: isGetApplicationsLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getUserApplication(),
  {
    enabled: !state.isApplicationValid,
  }
)

async function handleAgreementLinkClick(agreementType) {
  isAgreementLoading.value = true
  await applicationStore.getAgreementPdf(agreementType)
  isAgreementLoading.value = false
}

async function handleEnterKeyPress(agreementType) {
  await applicationStore.getAgreementPdf(agreementType)
}

function handleSave(isMatching = false) {
  if (isMatching) {
    applicationStore.completeApplication.isMatchUpdated = true
    applicationStore.completeApplication.application.isComplete = true
    applicationStore.completeApplication.application.currentStep = 1
  }

  saveMutation('Next Step')
}

function handleContinue() {
  if (applicationStore.completeApplication.application.currentStep < 8) {
    applicationStore.completeApplication.application.currentStep =
      stepIndex.step + 1
  }

  updateMutation('Next Step')
  window.scrollTo(0, 0)
  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}

function handlePrevious() {
  if (applicationStore.completeApplication.application.currentStep > 1) {
    applicationStore.completeApplication.application.currentStep =
      stepIndex.step - 1
  }

  updateMutation('Next Step')
  window.scrollTo(0, 0)
  stepIndex.previousStep = stepIndex.step
  stepIndex.step -= 1
}

function handleUpdateStepOneValid(value: boolean) {
  stepOneValid.value = value
}

function handleUpdateStepTwoValid(value: boolean) {
  stepTwoValid.value = value
}

function handleUpdateStepThreeValid(value: boolean) {
  stepThreeValid.value = value
}

function handleUpdateStepFourValid(value: boolean) {
  stepFourValid.value = value
}

function handleUpdateStepFiveValid(value: boolean) {
  stepFiveValid.value = value
}

function handleUpdateStepSixValid(value: boolean) {
  stepSixValid.value = value
}

function handleUpdateStepSevenValid(value: boolean) {
  stepSevenValid.value = value
}

function handleUpdateStepEightValid(value: boolean) {
  stepEightValid.value = value
}

const allStepsComplete = computed(() => {
  return (
    stepOneValid.value &&
    stepTwoValid.value &&
    stepThreeValid.value &&
    stepFourValid.value &&
    stepFiveValid.value &&
    stepSixValid.value &&
    stepSevenValid.value &&
    stepEightValid.value
  )
})

provide('allStepsComplete', allStepsComplete)
</script>

<style lang="scss">
.theme--dark.v-expansion-panels .v-expansion-panel {
  background: #303030;
}

.v-expansion-panel-content__wrap {
  padding-left: 5px !important;
  padding-right: 5px !important;
}

.stepper {
  overflow: visible;
}

.sticky-light {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: white;
}

.sticky-dark {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: #303030;
}

.progress-circular {
  position: fixed;
  top: 75px;
  left: 50%;
  z-index: 2;
}

.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
