<template>
  <div>
    <ApplicationInfoSection />
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        {{ $t('Application Type') }}
      </v-card-title>

      <v-card-text>
        <v-radio-group
          v-model="model.application.applicationType"
          :rules="applicationTypeRules"
        >
          <v-radio
            color="primary"
            label="Standard"
            value="standard"
          />
          <v-radio
            color="warning"
            label="Judicial"
            value="judicial"
          />
          <v-radio
            color="warning"
            label="Reserve"
            value="reserve"
          />
        </v-radio-group>

        <v-alert
          dense
          outlined
          type="warning"
          v-if="model.application.applicationType === 'judicial'"
        >
          <strong>
            {{ $t('Judicial-warning') }}
          </strong>
        </v-alert>

        <v-alert
          dense
          outlined
          type="warning"
          v-if="model.application.applicationType === 'reserve'"
        >
          <strong>
            {{ $t('Judicial-reserve') }}
          </strong>
        </v-alert>
      </v-card-text>
    </v-form>

    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import ApplicationInfoSection from '@shared-ui/components/info-sections/ApplicationInfoSection.vue'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { computed, ref, watch } from 'vue'

interface FormStepSevenProps {
  value: CompleteApplication
}

const props = defineProps<FormStepSevenProps>()
const emit = defineEmits([
  'handle-save',
  'handle-submit',
  'input',
  'update-step-six-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const valid = ref(false)
const form = ref()

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-six-valid', newValue)
  }
})

function handleSubmit() {
  emit('handle-submit')
}

function handleSave() {
  emit('handle-save')
}

const applicationTypeRules = computed(() => {
  const checked = model.value.application.applicationType
  const isValid = checked !== ''

  return [isValid !== false || 'Application Type is required']
})
</script>
