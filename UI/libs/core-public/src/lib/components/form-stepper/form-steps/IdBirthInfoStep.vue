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
            :class="{ 'mt-5': isMobile }"
            type="info"
            color="primary"
            dark
            outlined
            elevation="2"
          >
            Please review your ID information and ensure everything is up to
            date before proceeding
          </v-alert>
        </v-col>
      </v-row>
      <v-card-title v-if="!isMobile">
        {{ $t("Driver's License") }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t("Driver's License") }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.idInfo.idNumber"
              :label="$t('Driver\'s License Number')"
              :rules="[v => !!v || $t('Driver\'s License Number is required')]"
              :dense="isMobile"
              outlined
              maxlength="25"
            >
            </v-text-field>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-autocomplete
              v-model="model.application.idInfo.issuingState"
              :items="states"
              :label="$t(' Issuing State')"
              :rules="[v => !!v || $t('Issuing state is required')]"
              outlined
              :hint="militaryOutOfStateHint"
              persistent-hint
              :dense="isMobile"
              auto-select-first
            >
            </v-autocomplete>
          </v-col>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.idInfo.restrictions"
              :label="$t('Driver\'s License Restrictions')"
              :dense="isMobile"
              outlined
              maxlength="25"
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Citizenship Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Citizenship Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-radio-group
          v-model="model.application.citizenship.citizen"
          label="Citizen"
          row
        >
          <v-radio
            :value="true"
            color="primary"
            label="Yes"
          />
          <v-radio
            :value="false"
            color="primary"
            label="No"
          />
        </v-radio-group>

        <template v-if="!model.application.citizenship.citizen">
          <v-row>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-autocomplete
                v-model="
                  model.application.immigrantInformation.countryOfCitizenship
                "
                :items="countries"
                :label="$t('Country of Citizenship')"
                :rules="[v => !!v || $t('You must enter a country')]"
                auto-select-first
                outlined
                :dense="isMobile"
              >
              </v-autocomplete>
            </v-col>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-combobox
                v-model="model.application.dob.birthCountry"
                :items="countries"
                :label="$t('Country of Birth')"
                :rules="[v => !!v || $t('You must enter a country')]"
                auto-select-first
                outlined
                :dense="isMobile"
              >
              </v-combobox>
            </v-col>
          </v-row>

          <v-row>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-radio-group
                v-model="model.application.immigrantInformation.immigrantAlien"
                label="Immigrant Alien"
                row
              >
                <v-radio
                  :value="true"
                  color="primary"
                  label="Yes"
                />
                <v-radio
                  :value="false"
                  color="primary"
                  label="No"
                />
              </v-radio-group>
              <v-alert
                v-if="
                  model.application.immigrantInformation.immigrantAlien ===
                  false
                "
                :dense="isMobile"
                outlined
                type="warning"
              >
                {{ $t('IMMIGRANT-DISCLAIMER') }}
              </v-alert>
            </v-col>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-radio-group
                v-model="
                  model.application.immigrantInformation.nonImmigrantAlien
                "
                label="Non-Immigrant Alien"
                row
              >
                <v-radio
                  :value="true"
                  color="primary"
                  label="Yes"
                />
                <v-radio
                  :value="false"
                  color="primary"
                  label="No"
                />
              </v-radio-group>
            </v-col>
          </v-row>
        </template>
      </v-card-text>
    </v-form>

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
import { useVuetify } from '@shared-ui/composables/useVuetify'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, ref, watch } from 'vue'
import { countries, states } from '@shared-utils/lists/defaultConstants'

interface FormStepTwoProps {
  value: CompleteApplication
}

const props = defineProps<FormStepTwoProps>()
const emit = defineEmits([
  'input',
  'update-step-two-valid',
  'handle-save',
  'handle-continue',
])

const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)
const valid = ref(false)
const form = ref()
const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const isRenew = computed(() => {
  const applicationType = model.value.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const militaryOutOfStateHint = computed(() => {
  const militaryStatus = model.value.application.citizenship.militaryStatus;
  const driverLicenseState = model.value.application.idInfo.issuingState;


  if (militaryStatus === 'Active' && driverLicenseState !== 'California') {
    return 'You will need to upload your military orders in the required documents section.';
  }

  return ''
})

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-two-valid', newValue)
  }
})

function handleSave() {
  emit('handle-save')
}

function handleContinue() {
  if (model.value.application.citizenship.citizen) {
    model.value.application.immigrantInformation.countryOfCitizenship = ''
    model.value.application.immigrantInformation.immigrantAlien = false
    model.value.application.immigrantInformation.nonImmigrantAlien = false
  }

  emit('handle-continue')
}
</script>
