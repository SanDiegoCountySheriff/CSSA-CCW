<template>
  <div>
    <v-container
      v-if="!$vuetify.breakpoint.mdAndDown"
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
            <div class="text-center">
              {{ $t('Address Change') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <template v-if="!hasExhaustedModifications">
            <v-stepper-step
              editable
              :complete="stepTwoValid"
              :edit-icon="stepTwoValid ? 'mdi-check' : '$edit'"
              :color="stepTwoValid ? 'success' : 'primary'"
              :step="2"
            >
              <div class="text-center">{{ $t('Name Change') }}</div>
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
                {{ $t('Weapon Change') }}
              </div>
            </v-stepper-step>

            <v-divider></v-divider>
          </template>

          <v-stepper-step
            editable
            :complete="stepFourValid"
            :edit-icon="stepFourValid ? 'mdi-check' : '$edit'"
            :color="stepFourValid ? 'success' : 'primary'"
            :step="hasExhaustedModifications ? 2 : 4"
          >
            <div class="text-center">
              {{ $t('Supporting Documents') }}
            </div>
          </v-stepper-step>

          <v-progress-linear
            :active="
              isGetApplicationLoading ||
              isUpdateApplicationLoading ||
              isSaveLoading ||
              isRefetching
            "
            indeterminate
          ></v-progress-linear>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <ModifyAddressStep
              v-model="modifyingAddress"
              :application="applicationStore.completeApplication"
              @update-step-one-valid="handleUpdateStepOneValid"
              @handle-save="handleSaveAddress"
              @handle-continue="handleContinueAddress"
            />
          </v-stepper-content>
        </v-stepper-items>

        <template v-if="!hasExhaustedModifications">
          <v-stepper-items>
            <v-stepper-content :step="2">
              <ModifyNameStep
                v-model="modifyingName"
                :application="applicationStore.completeApplication"
                @update-step-two-valid="handleUpdateStepTwoValid"
                @handle-save="handleSaveName"
                @previous-step="handlePreviousStep"
                @handle-continue="handleContinueName"
              />
            </v-stepper-content>
          </v-stepper-items>

          <v-stepper-items>
            <v-stepper-content :step="3">
              <ModifyWeaponStep
                @update-step-three-valid="handleUpdateStepThreeValid"
                @handle-save="handleSaveWeapon"
                @handle-continue="handleContinueWeapon"
                @handle-add-weapon="handleAddWeapon"
                @handle-delete-weapon="handleDeleteWeapon"
                @undo-add-weapon="handleUndoAddWeapon"
                @undo-delete-weapon="handleUndoDeleteWeapon"
                @previous-step="handlePreviousStep"
              />
            </v-stepper-content>
          </v-stepper-items>
        </template>

        <v-stepper-items>
          <v-stepper-content :step="hasExhaustedModifications ? 2 : 4">
            <ModifySupportingDocumentsStep
              :application="applicationStore.completeApplication"
              :modifying-name="modifyingName"
              :modifying-address="modifyingAddress"
              :modifying-weapons="modifyingWeapons"
              @update-step-four-valid="handleUpdateStepFourValid"
              @handle-continue="handleContinueFile"
              @handle-save="handleSaveFile"
              @previous-step="handlePreviousStep"
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
        v-if="
          isGetApplicationLoading ||
          isUpdateApplicationLoading ||
          isSaveLoading ||
          isRefetching
        "
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
          <v-expansion-panel-header @click.native="stepIndex.step = 1">
            {{ $t('Address Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyAddressStep
              v-model="modifyingAddress"
              :application="applicationStore.completeApplication"
              @handle-save="handleSaveAddress"
              @handle-continue="handleContinueAddress"
              @previous-step="handlePreviousStep"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel v-if="!hasExhaustedModifications">
          <v-expansion-panel-header @click.native="stepIndex.step = 2">
            {{ $t('Name Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyNameStep
              v-model="modifyingName"
              :application="applicationStore.completeApplication"
              @handle-save="handleSaveName"
              @handle-continue="handleContinueName"
              @update-step-two-valid="handleUpdateStepTwoValid"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel v-if="!hasExhaustedModifications">
          <v-expansion-panel-header @click.native="stepIndex.step = 3">
            {{ $t('Weapon Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyWeaponStep
              @handle-save="handleSaveWeapon"
              @handle-continue="handleContinueWeapon"
              @handle-add-weapon="handleAddWeapon"
              @handle-delete-weapon="handleDeleteWeapon"
              @undo-add-weapon="handleUndoAddWeapon"
              @undo-delete-weapon="handleUndoDeleteWeapon"
              @previous-step="handlePreviousStep"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header
            @click.native="stepIndex.step = hasExhaustedModifications ? 2 : 4"
          >
            {{ $t('Supporting Documents') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifySupportingDocumentsStep
              :application="applicationStore.completeApplication"
              :modifying-name="modifyingName"
              :modifying-address="modifyingAddress"
              :modifying-weapons="modifyingWeapons"
              @handle-continue="handleContinueFile"
              @handle-save="handleSaveFile"
              @previous-step="handlePreviousStep"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>

    <FinalizeModifyConfirmDialog v-model="dialog" />
  </div>
</template>

<script lang="ts" setup>
import FinalizeModifyConfirmDialog from '../dialogs/FinalizeModifyConfirmDialog.vue'
import ModifyAddressStep from '@core-public/components/form-stepper/modify-form-steps/ModifyAddressStep.vue'
import ModifyNameStep from '@core-public/components/form-stepper/modify-form-steps/ModifyNameStep.vue'
import ModifySupportingDocumentsStep from '@core-public/components/form-stepper/modify-form-steps/ModifySupportingDocumentsStep.vue'
import ModifyWeaponStep from '@core-public/components/form-stepper/modify-form-steps/ModifyWeaponStep.vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import {
  CompleteApplication,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, provide, reactive, ref, watch } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const applicationStore = useCompleteApplicationStore()
const brandStore = useBrandStore()
const isApplicationValid = ref(false)
const dialog = ref(true)
const stepIndex = reactive({
  step: 1,
  previousStep: 1,
})
const modifyingName = ref(false)
const modifyingAddress = ref(false)
const modifyingWeapons = computed(() => {
  return (
    applicationStore.completeApplication.application.modifyDeleteWeapons
      ?.length > 0 ||
    applicationStore.completeApplication.application.modifyAddWeapons?.length >
      0
  )
})
const stepOneValid = ref(false)
const stepTwoValid = ref(false)
const stepThreeValid = ref(false)
const stepFourValid = ref(false)
const router = useRouter()

const hasExhaustedModifications = computed(() => {
  return (
    applicationStore.completeApplication.application.modificationNumber >
    brandStore.getBrand.numberOfModificationsBetweenRenewals
  )
})

provide('hasExhaustedModifications', hasExhaustedModifications)

const allStepsComplete = computed(() => {
  if (!hasExhaustedModifications) {
    return (
      stepOneValid.value &&
      stepTwoValid.value &&
      stepThreeValid.value &&
      stepFourValid.value
    )
  }

  return stepOneValid.value && stepFourValid.value
})

provide('allStepsComplete', allStepsComplete)

const expansionStep = computed({
  get() {
    return stepIndex.step - 1
  },
  set(value) {
    stepIndex.step = value
  },
})

const { isLoading: isGetApplicationLoading, isRefetching } = useQuery(
  ['getApplicationsByUser'],
  async () => await applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !isApplicationValid.value,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
      stepIndex.step =
        applicationStore.completeApplication.application.currentStep
    },
  }
)

const { isLoading: isUpdateApplicationLoading, mutate: updateMutation } =
  useMutation({
    mutationFn: () => {
      applicationStore.completeApplication.application.currentStep =
        stepIndex.step

      return applicationStore.updateApplication()
    },
  })

const { isLoading: isSaveLoading, mutate: saveMutation } = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication()
  },
  onSuccess: () => {
    router.push('/')
  },
})

onMounted(() => {
  window.scrollTo(0, 0)
  isApplicationValid.value = Boolean(applicationStore.completeApplication.id)

  stepIndex.step = applicationStore.completeApplication.application.currentStep
})

function handleSaveName(name) {
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    name.firstName
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    name.middleName
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    name.lastName
  applicationStore.completeApplication.application.currentStep = 1

  saveMutation()
}

function handleSaveAddress(address) {
  applicationStore.completeApplication.application.modifiedAddress = address
  applicationStore.completeApplication.application.currentStep = 2

  saveMutation()
}

function handleSaveWeapon() {
  applicationStore.completeApplication.application.currentStep = 3

  saveMutation()
}

function handleSaveFile() {
  applicationStore.completeApplication.application.currentStep = 4

  saveMutation()
}

function handleContinueName(name) {
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    name.firstName
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    name.middleName
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    name.lastName
  applicationStore.completeApplication.application.currentStep = 3

  updateMutation()

  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}

function handleContinueAddress(address) {
  applicationStore.completeApplication.application.modifiedAddress = address
  applicationStore.completeApplication.application.currentStep = 2

  updateMutation()

  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}

function handleContinueWeapon() {
  applicationStore.completeApplication.application.currentStep = 4

  updateMutation()

  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}

function handleContinueFile() {
  applicationStore.completeApplication.application.currentStep = 5

  updateMutation()

  router.push('/ModifyFinalize')
}

function handleContinueFileConfirm() {
  applicationStore.completeApplication.application.currentStep = 5

  updateMutation()

  router.push('/ModifyFinalize')
}

function handlePreviousStep() {
  stepIndex.previousStep = stepIndex.step
  stepIndex.step -= 1
}

function handleAddWeapon(weapon: WeaponInfoType) {
  applicationStore.completeApplication.application.modifyAddWeapons.push(weapon)

  updateMutation()
}

function handleDeleteWeapon(weapon: WeaponInfoType) {
  applicationStore.completeApplication.application.modifyDeleteWeapons.push(
    weapon
  )

  updateMutation()
}

function handleUndoAddWeapon(weapon: WeaponInfoType) {
  const index =
    applicationStore.completeApplication.application.modifyAddWeapons.findIndex(
      w => w.serialNumber === weapon.serialNumber
    )

  if (index !== -1) {
    applicationStore.completeApplication.application.modifyAddWeapons.splice(
      index,
      1
    )
  }

  updateMutation()
}

function handleUndoDeleteWeapon(weapon: WeaponInfoType) {
  applicationStore.completeApplication.application.modifyDeleteWeapons =
    applicationStore.completeApplication.application.modifyDeleteWeapons.filter(
      w => {
        return w.serialNumber !== weapon.serialNumber
      }
    )

  updateMutation()
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

watch(modifyingName, newValue => {
  if (!newValue) {
    applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
      ''
    applicationStore.completeApplication.application.personalInfo.modifiedLastName =
      ''
    applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
      ''

    updateMutation()
  }
})

watch(modifyingAddress, newValue => {
  if (!newValue) {
    applicationStore.completeApplication.application.modifiedAddress = {
      streetAddress: '',
      city: '',
      county: '',
      state: '',
      country: '',
      zip: '',
    }

    updateMutation()
  }
})
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
