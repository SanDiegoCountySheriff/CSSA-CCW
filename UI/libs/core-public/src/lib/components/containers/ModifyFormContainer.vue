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
            <div class="text-center">{{ $t('Name Change') }}</div>
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
              {{ $t('Address Change') }}
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
              {{ $t('Weapon Change') }}
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
              {{ $t('Supporting Documents') }}
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
              {{ $t('Finalize') }}
            </div>
          </v-stepper-step>

          <v-progress-linear
            :active="
              isGetApplicationLoading ||
              isUpdateApplicationLoading ||
              isSaveLoading
            "
            indeterminate
          ></v-progress-linear>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <ModifyNameStep
              v-model="modifyingName"
              :application="applicationStore.completeApplication"
              @handle-save="handleSaveName"
              @handle-continue="handleContinueName"
            />
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="2">
            <ModifyAddressStep />
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="3">
            <ModifyWeaponStep />
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="4">
            <ModifySupportingDocumentsStep />
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="5">
            <ModifyFinalizeStep />
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
          isGetApplicationLoading || isUpdateApplicationLoading || isSaveLoading
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
            {{ $t('Name Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyNameStep
              v-model="modifyingName"
              :application="applicationStore.completeApplication"
              @handle-save="handleSaveName"
              @handle-continue="handleContinueName"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header @click.native="stepIndex.step = 2">
            {{ $t('Address Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyAddressStep />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header @click.native="stepIndex.step = 3">
            {{ $t('Weapon Change') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyWeaponStep />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header @click.native="stepIndex.step = 4">
            {{ $t('Supporting Documents') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifySupportingDocumentsStep />
          </v-expansion-panel-content>
        </v-expansion-panel>

        <v-expansion-panel>
          <v-expansion-panel-header @click.native="stepIndex.step = 5">
            {{ $t('Finalize') }}
          </v-expansion-panel-header>

          <v-expansion-panel-content eager>
            <ModifyFinalizeStep />
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-container>
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import ModifyAddressStep from '@core-public/components/form-stepper/modify-form-steps/ModifyAddressStep.vue'
import ModifyFinalizeStep from '@core-public/components/form-stepper/modify-form-steps/ModifyFinalizeStep.vue'
import ModifyNameStep from '@core-public/components/form-stepper/modify-form-steps/ModifyNameStep.vue'
import ModifySupportingDocumentsStep from '@core-public/components/form-stepper/modify-form-steps/ModifySupportingDocumentsStep.vue'
import ModifyWeaponStep from '@core-public/components/form-stepper/modify-form-steps/ModifyWeaponStep.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'
import { computed, onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const applicationStore = useCompleteApplicationStore()
const isApplicationValid = ref(false)
const stepIndex = reactive({
  step: 1,
  previousStep: 1,
})
const stepOneValid = ref(false)
const modifyingName = ref(false)
const stepTwoValid = ref(false)
const stepThreeValid = ref(false)
const stepFourValid = ref(false)
const stepFiveValid = ref(false)
const router = useRouter()

const expansionStep = computed({
  get() {
    return stepIndex.step - 1
  },
  set(value) {
    stepIndex.step = value
  },
})

onMounted(() => {
  window.scrollTo(0, 0)
  isApplicationValid.value = Boolean(applicationStore.completeApplication.id)
})

const { isLoading: isGetApplicationLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !isApplicationValid.value,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
    },
  }
)

const { isLoading: isUpdateApplicationLoading, mutate: updateMutation } =
  useMutation({
    mutationFn: () => {
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

function handleSaveName(name) {
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    name.firstName
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    name.middleName
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    name.lastName

  saveMutation()
}

function handleContinueName(name) {
  applicationStore.completeApplication.application.personalInfo.modifiedFirstName =
    name.firstName
  applicationStore.completeApplication.application.personalInfo.modifiedMiddleName =
    name.middleName
  applicationStore.completeApplication.application.personalInfo.modifiedLastName =
    name.lastName

  updateMutation()

  stepIndex.previousStep = stepIndex.step
  stepIndex.step += 1
}
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