<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title v-if="!isMobile">
        {{ $t('Contact Information') }}
      </v-card-title>

      <v-card-subtitle v-else>
        {{ $t('Contact Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="model.application.contact.primaryPhoneNumber"
              :hint="$t('Only numbers no spaces or dashes')"
              :label="$t('Primary phone number')"
              :rules="phoneRuleSet"
              :dense="isMobile"
              maxlength="10"
              outlined
            />
          </v-col>
          <v-col>
            <v-text-field
              v-model="model.application.contact.cellPhoneNumber"
              :hint="$t('Only numbers no spaces or dashes')"
              :label="$t('Cell phone number')"
              :rules="notRequiredPhoneRuleSet"
              :dense="isMobile"
              maxlength="10"
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="model.application.contact.workPhoneNumber"
              :hint="$t('Only numbers no spaces or dashes')"
              :label="$t('Work phone number')"
              :rules="notRequiredPhoneRuleSet"
              :dense="isMobile"
              maxlength="10"
              outlined
            />
          </v-col>
        </v-row>
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
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, onMounted, ref, watch } from 'vue'
import {
  notRequiredPhoneRuleSet,
  phoneRuleSet,
} from '@shared-ui/rule-sets/ruleSets'

interface FormStepFiveProps {
  value: CompleteApplication
}

const props = defineProps<FormStepFiveProps>()
const emit = defineEmits([
  'input',
  'handle-submit',
  'handle-save',
  'update-step-five-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const form = ref()
const valid = ref(false)
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-five-valid', newValue)
  }
})

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

function handleSubmit() {
  emit('handle-submit')
}

function handleSave() {
  emit('handle-save')
}
</script>
