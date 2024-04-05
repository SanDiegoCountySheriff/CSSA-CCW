<template>
  <div>
    <ApplicationInfoSection v-if="!isRenew" />
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        {{ $t('Application Type') }}
      </v-card-title>

      <v-card-text v-if="!isRenew">
        <v-radio-group
          v-model="model.application.applicationType"
          :rules="applicationTypeRules"
        >
          <v-radio
            color="primary"
            label="Standard"
            :value="ApplicationType['Standard']"
          />
          <v-switch
            v-model="isJudgeConfirmed"
            label="Are you a Judge?"
            color="primary"
          />
          <v-radio
            v-if="isJudgeConfirmed"
            color="primary"
            label="Judicial"
            :value="ApplicationType['Judicial']"
          />
          <v-switch
            v-model="isReserveOfficerConfirmed"
            label="Are you a Reserve Law Enforcement Officer?"
            color="primary"
          />
          <v-radio
            v-if="isReserveOfficerConfirmed"
            color="primary"
            label="Reserve"
            :value="ApplicationType['Reserve']"
          />
          <v-radio
            v-if="brandStore.brand.employmentLicense"
            color="warning"
            label="Employment"
            :value="ApplicationType['Employment']"
          />
        </v-radio-group>

        <v-alert
          dense
          outlined
          type="warning"
          v-if="model.application.applicationType === ApplicationType.Judicial"
        >
          <strong>
            {{ $t('Judicial-warning') }}
          </strong>
        </v-alert>

        <v-alert
          dense
          outlined
          type="warning"
          v-if="model.application.applicationType === ApplicationType.Reserve"
        >
          <strong>
            {{ $t('Judicial-reserve') }}
          </strong>
        </v-alert>
        <v-alert
          dense
          outlined
          type="warning"
          v-if="
            model.application.applicationType === ApplicationType.Employment
          "
        >
          <strong>
            {{ $t('Employment-warning') }}
          </strong>
        </v-alert>
      </v-card-text>
      <v-card-text v-else>
        <v-radio-group
          v-model="model.application.applicationType"
          :rules="applicationTypeRules"
        >
          <v-radio
            color="primary"
            label="Renew Standard"
            :value="ApplicationType['Renew Standard']"
            disabled
          />
          <v-radio
            color="warning"
            label="Renew Judicial"
            :value="ApplicationType['Renew Judicial']"
            disabled
          />
          <v-radio
            color="warning"
            label="Renew Reserve"
            :value="ApplicationType['Renew Reserve']"
            disabled
          />
          <v-radio
            v-if="brandStore.brand.employmentLicense"
            color="warning"
            label="Renew Employment"
            :value="ApplicationType['Renew Employment']"
            disabled
          />
        </v-radio-group>
        <v-alert
          dense
          outlined
          type="warning"
          v-if="
            model.application.applicationType ===
            ApplicationType['Renew Judicial']
          "
        >
          <strong>
            {{ $t('Judicial-warning') }}
          </strong>
        </v-alert>

        <v-alert
          dense
          outlined
          type="warning"
          v-if="
            model.application.applicationType ===
            ApplicationType['Renew Reserve']
          "
        >
          <strong>
            {{ $t('Judicial-reserve') }}
          </strong>
        </v-alert>
        <v-alert
          dense
          outlined
          type="warning"
          v-if="
            model.application.applicationType ===
            ApplicationType['Renew Employment']
          "
        >
          <strong>
            {{ $t('Employment-warning') }}
          </strong>
        </v-alert>
      </v-card-text>
    </v-form>

    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import ApplicationInfoSection from '@shared-ui/components/info-sections/ApplicationInfoSection.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, ref, watch } from 'vue'

interface FormStepSevenProps {
  value: CompleteApplication
}

const props = defineProps<FormStepSevenProps>()
const brandStore = useBrandStore()
const emit = defineEmits([
  'handle-save',
  'handle-continue',
  'input',
  'update-step-five-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const valid = ref(false)
const form = ref()
const isJudgeConfirmed = ref(false)
const isReserveOfficerConfirmed = ref(false)

const isRenew = computed(() => {
  const applicationType = model.value.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-five-valid', newValue)
  }
})

function handleContinue() {
  emit('handle-continue')
}

function handleSave() {
  emit('handle-save')
}

const applicationTypeRules = computed(() => {
  const checked = model.value.application.applicationType
  const isValid = checked !== ApplicationType.None

  return [isValid !== false || 'Application Type is required']
})
</script>
