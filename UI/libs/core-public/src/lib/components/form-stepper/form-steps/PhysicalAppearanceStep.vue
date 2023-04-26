<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title v-if="!isMobile">
        {{ $t(' Physical Appearance') }}
      </v-card-title>

      <v-card-subtitle v-else>
        {{ $t(' Physical Appearance') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.physicalAppearance.heightFeet"
              :label="$t('Height feet')"
              :rules="heightFeetRules"
              :dense="isMobile"
              type="number"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.physicalAppearance.heightInch"
              :label="$t('Height inches')"
              :rules="heightInchesRules"
              :dense="isMobile"
              type="number"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.physicalAppearance.weight"
              :hint="$t('Enter weight in pounds(lbs)')"
              :label="$t('Weight')"
              :rules="weightRules"
              :dense="isMobile"
              persistent-hint
              type="number"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-select
              v-model="model.application.physicalAppearance.hairColor"
              :label="$t('Hair Color')"
              :rules="hairColorRules"
              :items="hairColors"
              :dense="isMobile"
              outlined
            >
            </v-select>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-select
              v-model="model.application.physicalAppearance.eyeColor"
              :label="$t('Eye Color')"
              :rules="eyeColorRules"
              :items="eyeColors"
              :dense="isMobile"
              outlined
            >
            </v-select>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-radio-group
              v-model="model.application.physicalAppearance.gender"
              :rules="genderRules"
              label="Gender"
              row
            >
              <v-radio
                label="Male"
                value="male"
                color="primary"
              />
              <v-radio
                label="Female"
                value="female"
                color="primary"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            md="4"
          >
            <v-textarea
              v-model="model.application.physicalAppearance.physicalDesc"
              :label="$t('Physical Description')"
              :rules="physicalDescriptionRules"
              :dense="isMobile"
              maxlength="1000"
              clearable
              no-resize
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
import { i18n } from '@core-public/plugins'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, onMounted, ref, watch } from 'vue'
import { eyeColors, hairColors } from '@shared-utils/lists/defaultConstants'

interface FormStepFourProps {
  value: CompleteApplication
}

const props = defineProps<FormStepFourProps>()
const emit = defineEmits([
  'input',
  'update-step-four-valid',
  'handle-save',
  'handle-submit',
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
    emit('update-step-four-valid', newValue)
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

const heightFeetRules = computed(() => {
  return [
    v =>
      (v > 0 && v < 10) ||
      i18n.t('Height feet must be greater than 0 and less than 10'),
    v => Boolean(v) || i18n.t('Height feet is required'),
  ]
})

const heightInchesRules = computed(() => {
  return [
    v => Boolean(v) || i18n.t('Height inches is required'),
    v =>
      (v >= 0 && v < 12) ||
      i18n.t('Height in inches must be 0 or greater and less than 11'),
  ]
})

const weightRules = computed(() => {
  return [
    v => Boolean(v) || i18n.t('Weight is required'),
    v =>
      (v > 0 && v < 1500) ||
      i18n.t('Weight must greater than 0 and less than 1500'),
  ]
})

const hairColorRules = computed(() => {
  return [v => Boolean(v) || i18n.t('Hair color is required')]
})

const eyeColorRules = computed(() => {
  return [v => Boolean(v) || i18n.t('Eye color is required')]
})

const genderRules = computed(() => {
  const checked = model.value.application.physicalAppearance.gender
  const isValid = checked !== ''

  return [isValid !== false || 'A gender is required.']
})

const physicalDescriptionRules = computed(() => {
  return [
    v => Boolean(v) || i18n.t('Physical description is required'),
    v =>
      (v && v.length <= 1000) || i18n.t('Maximum 1000 characters are allowed'),
  ]
})
</script>
