<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        {{ $t(' Employment Status') }}
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="4"
          >
            <v-select
              v-model="model.application.employment"
              :label="$t(' Employment Status')"
              :items="employmentStatus"
              :rules="employmentRules"
              :dense="isMobile"
              @change="handleValidateForm"
              outlined
            />
          </v-col>
        </v-row>
      </v-card-text>

      <template v-if="model.application.employment === 'Employed'">
        <v-card-title>
          {{ $t('Work Information') }}
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerName"
                :label="$t('Employer Name')"
                :rules="employerNameRules"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.occupation"
                :label="$t('Occupation')"
                :rules="occupationRules"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerAddressLine1"
                :label="$t('Employer Address Line 1')"
                :rules="employerAddressRules"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerAddressLine2"
                :label="$t('Employer Address Line 2')"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-combobox
                v-model="model.application.workInformation.employerCountry"
                :label="$t('Employer Country')"
                :rules="employerCountryRules"
                :items="countries"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-if="!isUnitedStates"
                v-model="model.application.workInformation.employerState"
                :label="$t('Employer Region')"
                :rules="employerRegionRules"
                :dense="isMobile"
                maxlength="50"
                outlined
              />

              <v-autocomplete
                v-if="isUnitedStates"
                v-model="model.application.workInformation.employerState"
                :label="$t('Employer State')"
                :rules="employerStateRules"
                :dense="isMobile"
                :items="states"
                outlined
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerCity"
                :label="$t('Employer City')"
                :rules="employerCityRules"
                :dense="isMobile"
                maxlength="50"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerZip"
                :label="$t('Employer Zip Code')"
                :rules="zipRuleSet"
                :dense="isMobile"
                maxlength="10"
                outlined
              />
            </v-col>
            <v-col
              cols="12"
              md="4"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.workInformation.employerPhone"
                @input="formatPhone('employerPhone')"
                :label="$t('Employer Phone number')"
                :rules="phoneRuleSet"
                :dense="isMobile"
                outlined
              />
            </v-col>
          </v-row>
        </v-card-text>
      </template>
    </v-form>

    <v-card-text>
      <WeaponsDialog @save-weapon="getWeaponFromDialog" />

      <WeaponsTable
        :weapons="model.application.weapons"
        :delete-enabled="true"
        @delete="deleteWeapon"
      />
    </v-card-text>

    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { i18n } from '@core-public/plugins'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import {
  CompleteApplication,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import {
  countries,
  defaultPermitState,
  employmentStatus,
  states,
} from '@shared-utils/lists/defaultConstants'
import { phoneRuleSet, zipRuleSet } from '@shared-ui/rule-sets/ruleSets'

interface FormStepSixProps {
  value: CompleteApplication
}

const props = defineProps<FormStepSixProps>()
const emit = defineEmits([
  'input',
  'handle-save',
  'handle-submit',
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

function formatPhone(modelName1) {
  let validInput = model.value.application.workInformation[modelName1].replace(
    /\D/g,
    ''
  )
  const match = validInput.match(/^(\d{1,3})(\d{0,3})(\d{0,4})$/)

  if (match) {
    model.value.application.workInformation[modelName1] = `(${match[1]})${
      match[2] ? ' ' : ''
    }${match[2]}${match[3] ? '-' : ''}${match[3]}`
  }
}

function handleSubmit() {
  if (model.value.application.employment !== 'Employed') {
    model.value.application.workInformation =
      defaultPermitState.application.workInformation
  }

  emit('handle-submit')
}

function handleSave() {
  emit('handle-save')
}

function handleValidateForm() {
  if (form.value) {
    nextTick(() => {
      form.value.validate()
    })
  }
}

function getWeaponFromDialog(weapon: WeaponInfoType) {
  model.value.application.weapons.push(weapon)
}

function deleteWeapon(index: number) {
  model.value.application.weapons.splice(index, 1)
}

const isUnitedStates = computed(() => {
  return (
    model.value.application.workInformation.employerCountry === 'United States'
  )
})

const employmentRules = computed(() => {
  return [v => Boolean(v) || i18n.t(' Employment status is required')]
})

const employerNameRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a employer name')]
})

const occupationRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a occupation')]
})

const employerAddressRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a address')]
})

const employerCountryRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a country')]
})

const employerRegionRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a region')]
})

const employerStateRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a state')]
})

const employerCityRules = computed(() => {
  return [v => Boolean(v) || i18n.t('You must enter a city')]
})
</script>
