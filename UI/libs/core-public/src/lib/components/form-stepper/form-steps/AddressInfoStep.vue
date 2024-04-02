<template>
  <div>
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
            Please review your address information and ensure everything is up
            to date before proceeding
          </v-alert>
        </v-col>
      </v-row>
      <v-card-title v-if="!isMobile">
        {{ $t('Address Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Address Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.currentAddress.streetAddress"
              :rules="[v => !!v || $t('Street address cannot be blank')]"
              :label="$t('Street Address')"
              :dense="isMobile"
              maxlength="150"
              outlined
            >
            </v-text-field>
          </v-col>

          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.currentAddress.city"
              :rules="[v => !!v || $t('City cannot be blank')]"
              :label="$t('City')"
              :dense="isMobile"
              maxlength="100"
              outlined
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
            <v-text-field
              :label="$t('Country')"
              :dense="isMobile"
              :value="
                (model.application.currentAddress.country = 'United States')
              "
              readonly
              :hint="$t('Applicant must reside in the United States')"
              persistent-hint
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            md="6"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-autocomplete
              v-if="
                model.application.currentAddress.country === 'United States'
              "
              v-model="model.application.currentAddress.state"
              :rules="[v => !!v || $t('State cannot be blank')]"
              :label="$t('State')"
              :dense="isMobile"
              :items="states"
              auto-select-first
              maxlength="100"
              outlined
            >
            </v-autocomplete>

            <v-text-field
              v-if="
                model.application.currentAddress.country !== 'United States'
              "
              v-model="model.application.currentAddress.state"
              :rules="[v => !!v || $t('Region cannot be blank')]"
              :label="$t('Region')"
              :dense="isMobile"
              maxlength="100"
              outlined
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
            <v-text-field
              v-model="model.application.currentAddress.county"
              :rules="[v => !!v || $t('County cannot be blank')]"
              :hint="$t('If not applicable enter N/A ')"
              :label="$t('County')"
              :dense="isMobile"
              persistent-hint
              maxlength="100"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="6"
          >
            <v-text-field
              v-model="model.application.currentAddress.zip"
              :hint="$t('If not applicable enter N/A ')"
              :rules="zipRuleSet"
              :dense="isMobile"
              :label="$t('Zip')"
              persistent-hint
              maxlength="10"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-checkbox
          v-model="model.application.differentMailing"
          :label="$t('Different Mailing address')"
          @change="handleValidateForm"
        />
      </v-card-text>

      <div v-if="model.application.differentMailing">
        <v-card-title v-if="!isMobile">
          {{ $t('Mailing Address') }}
        </v-card-title>

        <v-card-subtitle v-else>
          {{ $t('Mailing Address') }}
        </v-card-subtitle>

        <v-card-text>
          <v-row>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.mailingAddress.streetAddress"
                :rules="[v => !!v || $t('Street address cannot be blank')]"
                :label="$t('Street Address')"
                :dense="isMobile"
                maxlength="150"
                outlined
              >
              </v-text-field>
            </v-col>

            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.mailingAddress.city"
                :rules="[v => !!v || $t('City cannot be blank')]"
                :label="$t('City')"
                :dense="isMobile"
                maxlength="100"
                outlined
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
                v-model="model.application.mailingAddress.country"
                :rules="[v => !!v || $t('Country cannot be blank')]"
                :label="$t('Country')"
                :items="countries"
                :dense="isMobile"
                auto-select-first
                outlined
              >
              </v-combobox>
            </v-col>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-autocomplete
                v-if="
                  model.application.mailingAddress.country === 'United States'
                "
                v-model="model.application.mailingAddress.state"
                :rules="[v => !!v || $t('State cannot be blank')]"
                :label="$t('State')"
                :dense="isMobile"
                :items="states"
                auto-select-first
                maxlength="100"
                outlined
              >
              </v-autocomplete>

              <v-text-field
                v-if="
                  model.application.mailingAddress.country !== 'United States'
                "
                v-model="model.application.mailingAddress.state"
                :rules="[v => !!v || $t('Region cannot be blank')]"
                :label="$t('Region')"
                :dense="isMobile"
                maxlength="100"
                outlined
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
              <v-text-field
                v-model="model.application.mailingAddress.county"
                :rules="[v => !!v || $t('County cannot be blank')]"
                :hint="$t('If not applicable enter N/A ')"
                :label="$t('County')"
                :dense="isMobile"
                persistent-hint
                maxlength="100"
                outlined
              >
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="model.application.mailingAddress.zip"
                :hint="$t('If not applicable enter N/A')"
                :rules="zipRuleSet"
                :label="$t('Zip')"
                :dense="isMobile"
                persistent-hint
                maxlength="10"
                outlined
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-card-text>
      </div>

      <v-card-text>
        <v-checkbox
          v-model="model.application.differentSpouseAddress"
          :label="$t('Different Spouse address')"
          @change="handleValidateForm"
        />
      </v-card-text>

      <div v-if="model.application.differentSpouseAddress">
        <v-card-title v-if="!isMobile">
          {{ $t('Spouse Address') }}
        </v-card-title>

        <v-card-subtitle v-else>
          {{ $t('Spouse Address') }}
        </v-card-subtitle>

        <v-card-text>
          <v-row>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="
                  model.application.spouseAddressInformation.streetAddress
                "
                :rules="[
                  v => !!v || $t('Spouse street address cannot be blank'),
                ]"
                :label="$t('Spouse street address')"
                :dense="isMobile"
                maxlength="150"
                outlined
              ></v-text-field>
            </v-col>

            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-text-field
                v-model="model.application.spouseAddressInformation.city"
                :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
                :label="$t('Spouse\'s City')"
                :dense="isMobile"
                maxlength="100"
                outlined
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
                v-model="model.application.spouseAddressInformation.country"
                :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
                :label="$t('Spouse\'s Country')"
                :items="countries"
                :dense="isMobile"
                auto-select-first
                outlined
              >
              </v-combobox>
            </v-col>
            <v-col
              md="6"
              cols="12"
              :class="isMobile ? 'pb-0' : ''"
            >
              <v-autocomplete
                v-if="
                  model.application.spouseAddressInformation.country ===
                  'United States'
                "
                v-model="model.application.spouseAddressInformation.state"
                :rules="[v => !!v || $t('State cannot be blank')]"
                :label="$t('State')"
                :dense="isMobile"
                :items="states"
                auto-select-first
                maxlength="100"
                outlined
              >
              </v-autocomplete>

              <v-text-field
                v-if="
                  model.application.spouseAddressInformation.country !==
                  'United States'
                "
                v-model="model.application.spouseAddressInformation.state"
                :rules="[v => !!v || $t('Region cannot be blank')]"
                :label="$t('Region')"
                :dense="isMobile"
                maxlength="100"
                outlined
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
              <v-text-field
                v-model="model.application.spouseAddressInformation.county"
                :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
                :hint="$t('If not applicable enter N/A')"
                :label="$t('Spouse\'s County')"
                :dense="isMobile"
                persistent-hint
                maxlength="100"
                outlined
              >
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="model.application.spouseAddressInformation.zip"
                :hint="$t('If not applicable enter N/A')"
                :label="$t('Spouse\'s Zip')"
                :rules="zipRuleSet"
                :dense="isMobile"
                persistent-hint
                maxlength="10"
                outlined
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              md="6"
            >
              <v-text-field
                v-model="model.application.spouseAddressInformation.reason"
                :label="$t('Reason for different spouse address')"
                :rules="requireReasonRuleSet"
                :dense="isMobile"
                persistent-hint
                maxlength="50"
                outlined
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-card-text>
      </div>
      <v-card-title>
        {{ $t(' Previous Addresses') }}
      </v-card-title>

      <v-card-text>
        <v-radio-group
          v-model="hasPreviousAddresses"
          :label="'Have you lived anywhere else besides your current residence in the last 5 years?'"
          :row="!isMobile"
          :rules="previousAddressRules"
        >
          <v-radio
            color="primary"
            :label="$t('Yes')"
            :value="true"
          ></v-radio>
          <v-radio
            color="primary"
            :label="$t('No')"
            :value="false"
          ></v-radio>
        </v-radio-group>
      </v-card-text>

      <v-card-text v-if="hasPreviousAddresses">
        <p>
          {{ $t('Please provide residences for the ') }}
          <strong>{{ $t('past 5 years') }}</strong>
        </p>

        <PreviousAddressDialog
          @get-previous-address-from-dialog="getPreviousAddressFromDialog"
        />
        <AddressTable
          :addresses="model.application.previousAddresses"
          :enable-delete="true"
          @delete="deleteAddress"
        />
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
import AddressTable from '@shared-ui/components/tables/AddressTable.vue'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import PreviousAddressDialog from '@shared-ui/components/dialogs/PreviousAddressDialog.vue'
import { requireReasonRuleSet } from '@shared-ui/rule-sets/ruleSets'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { zipRuleSet } from '@shared-ui/rule-sets/ruleSets'
import {
  AddressInfoType,
  ApplicationType,
} from '@shared-utils/types/defaultTypes'
import { computed, nextTick, onMounted, ref, watch } from 'vue'
import { countries, states } from '@shared-utils/lists/defaultConstants'

interface FormStepThreeProps {
  value: CompleteApplication
}

const props = defineProps<FormStepThreeProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-three-valid',
])

const form = ref()
const valid = ref(false)
const hasPreviousAddresses = ref(false)
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

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

const previousAddressRules = computed(() => {
  if (
    hasPreviousAddresses.value === true &&
    model.value.application.previousAddresses.length === 0
  ) {
    return ['Previous address is required']
  }

  return []
})

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }

  if (model.value.application.previousAddresses.length > 0) {
    hasPreviousAddresses.value = true
  }
})

function getPreviousAddressFromDialog(address: AddressInfoType) {
  model.value.application.previousAddresses.push(address)
}

function deleteAddress(index) {
  model.value.application.previousAddresses.splice(index, 1)
}

function handleContinue() {
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

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-three-valid', newValue)
  }
})
</script>
