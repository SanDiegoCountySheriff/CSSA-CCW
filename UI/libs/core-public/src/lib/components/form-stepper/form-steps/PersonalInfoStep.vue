<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-row v-if="model.application.applicationType.includes('renew')">
        <v-col
          cols="12"
          md="4"
        >
          <v-alert
            type="info"
            color="primary"
            dark
            outlined
            elevation="2"
          >
            Please review your personal information and ensure everything is up
            to date before proceeding
          </v-alert>
        </v-col>
      </v-row>
      <v-card-title v-if="!isMobile">
        {{ $t('Personal Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Personal Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.firstName"
              :label="$t('First name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.middleName"
              :label="$t('Middle name')"
              :rules="notRequiredNameRuleSet"
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
              v-model="model.application.personalInfo.lastName"
              :label="$t('Last name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.suffix"
              :label="$t('Suffix')"
              :dense="isMobile"
              maxlength="10"
              outlined
            />
          </v-col>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.maidenName"
              :label="$t('Maiden name')"
              :rules="notRequiredNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            />
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Birth Information') }}
      </v-card-title>

      <v-card-subtitle v-else>
        {{ $t('Birth Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-menu
              v-model="menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  v-model="model.application.dob.birthDate"
                  :label="$t('Date of Birth')"
                  :rules="[
                    validateDate,
                    v => !!v || $t('Date of birth is required'),
                    checkFor21,
                    v => !!v || $t('Date of birth is required'),
                  ]"
                  outlined
                  :dense="isMobile"
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                  readonly
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="model.application.dob.birthDate"
                color="primary"
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.dob.birthCity"
              :label="$t('Birth city')"
              :rules="[v => !!v || $t('Birth city cannot be blank')]"
              outlined
              :dense="isMobile"
              maxlength="150"
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-combobox
              v-model="model.application.dob.birthCountry"
              :items="countries"
              :label="$t('Birth country')"
              :rules="[v => !!v || $t('Birth country cannot be blank')]"
              outlined
              :dense="isMobile"
            >
            </v-combobox>
          </v-col>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-autocomplete
              v-if="model.application.dob.birthCountry === 'United States'"
              v-model="model.application.dob.birthState"
              :items="states"
              :label="$t('Birth state')"
              :rules="[v => !!v || $t('Birth state cannot be blank')]"
              outlined
              :dense="isMobile"
              maxlength="150"
              auto-select-first
            >
            </v-autocomplete>
            <v-text-field
              v-if="model.application.dob.birthCountry !== 'United States'"
              v-model="model.application.dob.birthState"
              :label="$t('Birth region')"
              :rules="[v => !!v || $t('Birth region cannot be blank')]"
              outlined
              :dense="isMobile"
              maxlength="150"
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

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
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Contact Information') }}
      </v-card-title>

      <v-card-subtitle v-else>
        {{ $t('Contact Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="6"
            cols="12"
          >
            <v-text-field
              v-model="model.application.contact.primaryPhoneNumber"
              @input="formatPhone('contact', 'primaryPhoneNumber')"
              :label="$t('Primary phone number')"
              :rules="phoneRuleSet"
              :dense="isMobile"
              maxlength="14"
              outlined
            />
          </v-col>
          <v-col
            md="6"
            cols="12"
          >
            <v-text-field
              v-model="model.application.contact.cellPhoneNumber"
              @input="formatPhone('contact', 'cellPhoneNumber')"
              :label="$t('Cell phone number')"
              :rules="notRequiredPhoneRuleSet"
              :dense="isMobile"
              maxlength="14"
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col
            md="6"
            cols="12"
          >
            <v-text-field
              v-model="model.application.contact.workPhoneNumber"
              @input="formatPhone('contact', 'workPhoneNumber')"
              :label="$t('Work phone number')"
              :rules="notRequiredPhoneRuleSet"
              :dense="isMobile"
              maxlength="14"
              outlined
            />
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Social Security Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Social Security Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="formattedSSN"
              :label="$t('Social Security Number')"
              :rules="[
                v => !!v || $t('SSN cannot be blank'),
                v =>
                  /^[\d-]+$/.test(v) ||
                  $t('SSN must contain only numbers and dashes'),
                v =>
                  (v.match(/\d/g) || []).length === 9 ||
                  $t('SSN must be 9 characters in length'),
              ]"
              :dense="isMobile"
              @change="handleValidateForm"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="formattedSSNConfirm"
              :label="$t('Confirm SSN')"
              :rules="[
                v => !!v || $t('SSN cannot be blank'),
                v =>
                  (v.match(/\d/g) || []).length === 9 ||
                  $t('SSN must be 9 characters in length'),
                v =>
                  v.replace(/\D/g, '') === model.application.personalInfo.ssn ||
                  $t('SSN does not match'),
              ]"
              :dense="isMobile"
              @change="handleValidateForm"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Marital Status') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Marital Status') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-select
              v-model="model.application.personalInfo.maritalStatus"
              :label="'Marital status'"
              :hint="'Marital Status is required'"
              :rules="[v => !!v || $t('Marital status is required')]"
              :items="['Married', 'Single', 'Widowed', 'Divorced']"
              :dense="isMobile"
              @change="handleValidateForm"
              outlined
            >
            </v-select>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="isMarried && !isMobile">
        {{ $t('Spouse Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMarried && isMobile">
        {{ $t('Spouse Information') }}
      </v-card-subtitle>

      <v-card-text v-if="isMarried">
        <v-row>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.lastName"
              :label="$t('Last Name')"
              :rules="isMarried ? requireNameRuleSet : []"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.firstName"
              :label="$t('First Name')"
              :rules="isMarried ? requireNameRuleSet : []"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.middleName"
              :label="$t('Middle Name')"
              :rules="notRequiredNameRuleSet"
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
            <v-text-field
              v-model="model.application.spouseInformation.maidenName"
              :label="$t('Maiden Name')"
              :rules="notRequiredNameRuleSet"
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
              v-model="model.application.spouseInformation.phoneNumber"
              @input="formatPhone('spouseInformation', 'phoneNumber')"
              :label="$t('Phone number')"
              :rules="isMarried ? phoneRuleSet : []"
              :dense="isMobile"
              maxlength="14"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Military Status') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Military Status') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-select
              v-model="model.application.citizenship.militaryStatus"
              :items="items"
              :label="$t('Military Status')"
              :rules="[v => !!v || $t('Military Status is required')]"
              outlined
              :dense="isMobile"
              @change="handleValidateForm"
            />
            <v-alert
              v-if="
                model.application.citizenship.militaryStatus === 'Discharged' ||
                model.application.citizenship.militaryStatus === 'Retired'
              "
              :dense="isMobile"
              outlined
              type="warning"
            >
              {{ $t('discharged-disclaimer') }}
            </v-alert>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Aliases') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Aliases') }}
      </v-card-subtitle>

      <v-card-text>
        <v-radio-group
          v-model="showAlias"
          :label="$t('In the past have you ever gone by a different name?')"
          :row="!isMobile"
        >
          <v-radio
            color="primary"
            :label="$t('Yes')"
            :value="true"
          />
          <v-radio
            color="primary"
            :label="$t('No')"
            :value="false"
          />
        </v-radio-group>
        <AliasDialog
          v-if="showAlias"
          @save-alias="getAliasFromDialog"
        />
        <AliasTable
          v-if="showAlias"
          :aliases="model.application.aliases"
          :enable-delete="true"
          @delete="deleteAlias"
        />
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Character References') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Character References') }}
      </v-card-subtitle>

      <v-card-text>
        <v-alert
          outlined
          type="info"
          color="primary"
        >
          {{ $t('CHARACTER-REFERENCE-DISCLAIMER') }}
        </v-alert>

        <v-row
          v-for="(reference, index) in model.application.characterReferences"
          :key="index"
          :class="{ 'pt-4': index === 0 }"
        >
          <v-col
            cols="12"
            md="3"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="reference.name"
              :dense="isMobile"
              :rules="[v => !!v || $t('Name cannot be blank')]"
              :label="$t('Reference') + ' ' + (index + 1) + ' - ' + $t('Name')"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="3"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="reference.relationship"
              :dense="isMobile"
              :rules="[v => !!v || $t('Relationship cannot be blank')]"
              :label="
                $t('Reference') + ' ' + (index + 1) + ' - ' + $t('Relationship')
              "
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="3"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="reference.phoneNumber"
              @input="formatReferencePhone(reference)"
              :dense="isMobile"
              :rules="phoneRuleSet"
              :label="
                $t('Reference') + ' ' + (index + 1) + ' - ' + $t('Phone Number')
              "
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="3"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="reference.email"
              :dense="isMobile"
              :rules="[
                v => !!v || $t('Email address cannot be blank'),
                v => /.+@.+\..+/.test(v) || $t('Email address must be valid'),
              ]"
              :label="
                $t('Reference') +
                ' ' +
                (index + 1) +
                ' - ' +
                $t('Email Address')
              "
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <FormButtonContainer
        :valid="valid"
        @continue="handleContinue"
        @save="handleSave"
      />
    </v-form>
  </div>
</template>

<script setup lang="ts">
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue'
import AliasTable from '@shared-ui/components/tables/AliasTable.vue'
import { CharacterReferenceType } from '@shared-utils/types/defaultTypes'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { TranslateResult } from 'vue-i18n'
import { i18n } from '@core-public/plugins'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import { countries, states } from '@shared-utils/lists/defaultConstants'
import {
  notRequiredNameRuleSet,
  notRequiredPhoneRuleSet,
  phoneRuleSet,
  requireNameRuleSet,
} from '@shared-ui/rule-sets/ruleSets'

interface FormStepOneProps {
  value: CompleteApplication
}

const props = defineProps<FormStepOneProps>()
const brandStore = useBrandStore()
const hairColors = computed(() => {
  return brandStore.brand.agencyHairColors.map(h => {
    return h.name
  })
})
const eyeColors = computed(() => {
  return brandStore.brand.agencyEyeColors.map(e => {
    return e.name
  })
})

const emit = defineEmits([
  'input',
  'update-step-one-valid',
  'handle-save',
  'handle-continue',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const isMarried = computed(
  () =>
    model.value.application.personalInfo.maritalStatus.toLowerCase() ===
    'married'
)
const form = ref()
const valid = ref(false)
const menu = ref(false)

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-one-valid', newValue)
  }
})

const showAlias = ref(false)
let ssnConfirm = ref('')
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

const formattedSSN = computed({
  get() {
    return formatSSN(model.value.application.personalInfo.ssn)
  },
  set(value) {
    model.value.application.personalInfo.ssn = value.replace(/\D/g, '')
  },
})

const formattedSSNConfirm = computed({
  get() {
    return formatSSN(ssnConfirm.value)
  },
  set(value) {
    ssnConfirm.value = value.replace(/\D/g, '')
  },
})

onMounted(() => {
  if (model.value.application.personalInfo.ssn) {
    ssnConfirm.value = model.value.application.personalInfo.ssn
  }

  if (
    !model.value.application.characterReferences ||
    model.value.application.characterReferences.length === 0
  ) {
    model.value.application.characterReferences = [
      { name: '', relationship: '', phoneNumber: '', email: '' },
      { name: '', relationship: '', phoneNumber: '', email: '' },
      { name: '', relationship: '', phoneNumber: '', email: '' },
    ]
  }

  if (form.value) {
    form.value.validate()
  }

  if (
    model.value.application.aliases &&
    model.value.application.aliases.length > 0
  ) {
    showAlias.value = true
  }
})

function handleSave() {
  emit('handle-save')
}

async function handleContinue() {
  if (
    model.value.application.personalInfo.maritalStatus.toLowerCase() ===
    'single'
  ) {
    model.value.application.spouseInformation.lastName = ''
    model.value.application.spouseInformation.firstName = ''
    model.value.application.spouseInformation.maidenName = ''
    model.value.application.spouseInformation.middleName = ''
    model.value.application.spouseInformation.phoneNumber = ''
  }

  emit('handle-continue')
}

function getAliasFromDialog(alias) {
  model.value.application.aliases.push(alias)
}

function deleteAlias(index) {
  model.value.application.aliases.splice(index, 1)
}

function handleValidateForm() {
  if (form.value) {
    nextTick(() => {
      form.value.validate()
    })
  }
}

function formatPhone(modelName1, modelName2) {
  let validInput = model.value.application[modelName1][modelName2].replace(
    /\D/g,
    ''
  )
  const match = validInput.match(/^(\d{1,3})(\d{0,3})(\d{0,4})$/)

  if (match) {
    model.value.application[modelName1][modelName2] = `(${match[1]})${
      match[2] ? ' ' : ''
    }${match[2]}${match[3] ? '-' : ''}${match[3]}`
  }
}

function formatReferencePhone(reference: CharacterReferenceType) {
  let formattedNumber = reference.phoneNumber.replace(/\D/g, '')

  const match = formattedNumber.match(/^(\d{1,3})(\d{0,3})(\d{0,4})$/)

  if (match) {
    reference.phoneNumber = `(${match[1]})${match[2] ? ' ' : ''}${match[2]}${
      match[3] ? '-' : ''
    }${match[3]}`
  }
}

const items = ref([
  'Active',
  'Reserve',
  'Discharged',
  'Retired',
  'Never Served in the Military',
])

function checkFor21(input: string): boolean | TranslateResult {
  const userDate = input
  const targetDate = new Date(Date.now())
  const formatedDate = `${targetDate.getFullYear() - 21}-${
    targetDate.getMonth() + 1
  }-${targetDate.getDate()}`

  return userDate <= formatedDate
    ? true
    : i18n.t('You must be 21 or older to apply for a CCW permit')
}

function validateDate(inputDate: string | null | undefined): boolean | string {
  const DATE_PATTERN = /^\d{4}-\d{2}-\d{2}$/

  if (!inputDate) {
    return true
  }

  if (!DATE_PATTERN.test(inputDate)) {
    return 'Date must be in the format YYYY-MM-DD'
  }

  return true
}

function formatSSN(ssn) {
  return ssn
    .replace(/\D/g, '')
    .replace(/(\d{3})(\d{1,2})?(\d{1,4})?/, (_, p1, p2, p3) => {
      return p2 ? (p3 ? `${p1}-${p2}-${p3}` : `${p1}-${p2}`) : p1
    })
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
</script>
