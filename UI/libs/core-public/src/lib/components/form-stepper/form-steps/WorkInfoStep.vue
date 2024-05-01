<template>
  <div>
    <FormButtonContainer
      v-if="$vuetify.breakpoint.lgAndUp"
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />

    <v-form
      ref="form"
      v-model="valid"
    >
      <v-row
        v-if="isRenew"
        justify="center"
        align="center"
      >
        <v-col
          cols="12"
          md="6"
        >
          <v-alert
            type="info"
            color="primary"
            dark
            outlined
            elevation="2"
          >
            Please review your employment and weapon information and ensure
            everything is up to date before proceeding
          </v-alert>
        </v-col>
      </v-row>

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
                v-model="
                  model.application.workInformation.employerStreetAddress
                "
                :label="$t('Employer Street Address')"
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
              md="6"
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
              md="6"
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

      <v-card-title>
        {{ $t('Weapons') }}
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="4"
          >
            <v-alert
              dense
              outlined
              type="info"
              color="primary"
            >
              List below the weapons you desire to carry if granted a CCW
              license. You may carry concealed only the weapon(s) which you list
              and describe herein, and only for the purpose indicated. Any
              misuse will cause an automatic revocation and possible arrest.
            </v-alert>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>

    <v-card-text>
      <WeaponsTable
        :modifying="false"
        :weapons="model.application.weapons"
        @delete-weapon="handleDeleteWeapon"
        @handle-edit-weapon="handleEditWeapon"
        @save-weapon="handleSaveWeapon"
      />
    </v-card-text>

    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { i18n } from '@core-public/plugins'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import {
  ApplicationType,
  CompleteApplication,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, ref, set, watch } from 'vue'
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
  'handle-edit',
  'handle-continue',
  'update-step-four-valid',
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

const isRenew = computed(() => {
  const applicationType = props.value.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

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

function handleContinue() {
  if (model.value.application.employment !== 'Employed') {
    model.value.application.workInformation =
      defaultPermitState.application.workInformation
  }

  emit('handle-continue')
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

function handleSaveWeapon(weapon: WeaponInfoType) {
  model.value.application.weapons.push(weapon)
}

function handleDeleteWeapon(index: number) {
  model.value.application.weapons.splice(index, 1)
}

function handleEditWeapon(data) {
  set(model.value.application.weapons, data.index, { ...data.value })
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
