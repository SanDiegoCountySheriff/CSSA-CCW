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
        @change="updateMutation"
        :alt-labels="$vuetify.breakpoint.lgAndDown"
      >
        <v-stepper-header
          :class="$vuetify.theme.dark ? 'sticky-dark' : 'sticky-light'"
        >
          <v-stepper-step
            editable
            :complete="stepOneValid"
            :edit-icon="stepOneValid ? 'mdi-check' : '$edit'"
            :color="stepOneValid ? 'success' : 'primary'"
            :step="1"
          >
            <div class="text-center">{{ $t('Personal') }}</div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepTwoValid"
            :edit-icon="stepTwoValid ? 'mdi-check' : '$edit'"
            :color="stepTwoValid ? 'success' : 'primary'"
            :step="2"
          >
            <div class="text-center">
              {{ $t('ID Information') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepThreeValid"
            :edit-icon="stepThreeValid ? 'mdi-check' : '$edit'"
            :color="stepThreeValid ? 'success' : 'primary'"
            :step="3"
          >
            <div class="text-center">
              {{ $t('Address') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepFourValid"
            :edit-icon="stepFourValid ? 'mdi-check' : '$edit'"
            :color="stepFourValid ? 'success' : 'primary'"
            :step="4"
          >
            <div class="text-center">
              {{ $t('Employment & Weapons') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepFiveValid"
            :edit-icon="stepFiveValid ? 'mdi-check' : '$edit'"
            :color="stepFiveValid ? 'success' : 'primary'"
            :step="5"
          >
            <div class="text-center">
              {{ $t('Application Type') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepSixValid"
            :edit-icon="stepSixValid ? 'mdi-check' : '$edit'"
            :color="stepSixValid ? 'success' : 'primary'"
            :step="6"
          >
            <div class="text-center">
              {{ $t(' Upload Files') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepSevenValid"
            :edit-icon="stepSevenValid ? 'mdi-check' : '$edit'"
            :color="stepSevenValid ? 'success' : 'primary'"
            :step="7"
          >
            <div class="text-center">{{ $t('Qualifying Questions') }}</div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepEightValid"
            :edit-icon="stepEightValid ? 'mdi-check' : '$edit'"
            :color="stepEightValid ? 'success' : 'primary'"
            :step="8"
          >
            <div class="text-center">
              {{ $t('Signature') }}
            </div>
          </v-stepper-step>

          <v-progress-linear
            :active="isLoading || isSaveLoading"
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
        v-if="isLoading || isSaveLoading"
        indeterminate
        absolute
        class="progress-circular"
      ></v-progress-circular>

      <v-expansion-panels
        v-model="expansionStep"
        accordion
        @change="updateMutation"
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
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FileUploadStep from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue'
import IdBirthInfoStep from '@core-public/components/form-stepper/form-steps/IdBirthInfoStep.vue'
import PersonalInfoStep from '@core-public/components/form-stepper/form-steps/PersonalInfoStep.vue'
import QualifyingQuestionsStep from '@core-public/components/form-stepper/form-steps/QualifyingQuestionsStep.vue'
import SignatureStep from '@core-public/components/form-stepper/form-steps/SignatureStep.vue'
import WorkInfoStep from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { computed, onMounted, provide, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const applicationStore = useCompleteApplicationStore()
const stepOneValid = ref(false)
const stepTwoValid = ref(false)
const stepThreeValid = ref(false)
const stepFourValid = ref(false)
const stepFiveValid = ref(false)
const stepSixValid = ref(false)
const stepSevenValid = ref(false)
const stepEightValid = ref(false)
const router = useRouter()

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
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
})

const { refetch } = useQuery(
  ['getApplicationsByUser'],
  applicationStore.getAllUserApplicationsApi,
  {
    enabled: false,
  }
)

const { isLoading: isSaveLoading, mutate: saveMutation } = useMutation({
  mutationKey: ['saveMutation'],
  mutationFn: () => {
    if (
      applicationStore.completeApplication.application.isUpdatingApplication
    ) {
      applicationStore.completeApplication.application.isUpdatingApplication =
        false
    }

    return applicationStore.updateApplication()
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
  () => applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !state.isApplicationValid,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
    },
  }
)

function handleSave(isMatching = false) {
  if (isMatching) {
    applicationStore.completeApplication.isMatchUpdated = true
    applicationStore.completeApplication.application.isComplete = true
    applicationStore.completeApplication.application.currentStep = 1
  }

  saveMutation()
}

function handleContinue() {
  if (applicationStore.completeApplication.application.currentStep < 8) {
    applicationStore.completeApplication.application.currentStep =
      stepIndex.step + 1
  }

  updateMutation()
  window.scrollTo(0, 0)
  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}

function handlePrevious() {
  if (applicationStore.completeApplication.application.currentStep > 1) {
    applicationStore.completeApplication.application.currentStep =
      stepIndex.step - 1
  }

  updateMutation()
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
