<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
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
              :dense="isMobile"
              auto-select-first
            >
            </v-autocomplete>
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
import { countries, states } from '@shared-utils/lists/defaultConstants'

interface FormStepTwoProps {
  value: CompleteApplication
}

const props = defineProps<FormStepTwoProps>()
const emit = defineEmits([
  'input',
  'update-step-two-valid',
  'handle-save',
  'handle-submit',
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

function handleSubmit() {
  // TODO: see about abstracting the if statements.
  // if (!model.value.application.differentMailing) {
  //   model.value.application.mailingAddress.zip = '';
  //   model.value.application.mailingAddress.city = '';
  //   model.value.application.mailingAddress.state = '';
  //   model.value.application.mailingAddress.county = '';
  //   model.value.application.mailingAddress.country = '';
  //   model.value.application.mailingAddress.addressLine1 = '';
  //   model.value.application.mailingAddress.addressLine2 = '';
  // }

  // if (!model.value.application.differentSpouseAddress) {
  //   model.value.application.spouseAddressInformation.addressLine1 = '';
  //   model.value.application.spouseAddressInformation.addressLine2 = '';
  //   model.value.application.spouseAddressInformation.zip = '';
  //   model.value.application.spouseAddressInformation.country = '';
  //   model.value.application.spouseAddressInformation.county = '';
  //   model.value.application.spouseAddressInformation.city = '';
  //   model.value.application.spouseAddressInformation.state = '';
  // }

  if (model.value.application.citizenship.citizen) {
    model.value.application.immigrantInformation.countryOfCitizenship = ''
    model.value.application.immigrantInformation.immigrantAlien = false
    model.value.application.immigrantInformation.nonImmigrantAlien = false
  }

  emit('handle-submit')
}
</script>
