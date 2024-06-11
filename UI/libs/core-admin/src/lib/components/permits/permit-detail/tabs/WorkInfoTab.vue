<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Employment') }}

      <v-spacer></v-spacer>

      <SaveButton
        :disabled="
          (permitStore.getPermitDetail.application.employment === 'Employed' &&
            !valid) ||
          readonly
        "
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-row>
        <v-col cols="6">
          <v-select
            v-model="permitStore.getPermitDetail.application.employment"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
            :menu-props="{ bottom: true, offsetY: true }"
            :readonly="readonly"
            outlined
            dense
          >
          </v-select>
        </v-col>
      </v-row>
    </v-card-text>

    <template
      v-if="permitStore.getPermitDetail.application.employment === 'Employed'"
    >
      <v-card-title>
        {{ $t('Work Information') }}
      </v-card-title>

      <v-card-text>
        <v-form
          v-model="valid"
          ref="form"
        >
          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerName
                "
                :label="$t('Employer Name')"
                :rules="[v => !!v || $t('You must enter a employer name')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .occupation
                "
                :label="$t('Occupation')"
                :rules="[v => !!v || $t('You must enter a occupation')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerStreetAddress
                "
                :label="$t('Employer Street Address')"
                :rules="[v => !!v || $t('You must enter an address')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-combobox
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCountry
                "
                :items="countries"
                :label="$t('Employer Country')"
                :rules="[v => !!v || $t('You must enter a country')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-combobox>
            </v-col>

            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCountry === 'United States'
                "
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerState
                "
                :items="states"
                :label="$t('Employer State')"
                :rules="[v => !!v || $t('You must enter a state')]"
                :readonly="readonly"
                maxlength="50"
                outlined
                dense
              >
              </v-autocomplete>

              <v-text-field
                v-else
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerState
                "
                :label="$t('Employer State')"
                :rules="[v => !!v || $t('You must enter a state')]"
                :readonly="readonly"
                maxlength="50"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCity
                "
                :label="$t('Employer City')"
                :rules="[v => !!v || $t('You must enter a city')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerZip
                "
                :label="$t('Employer Zip Code')"
                :rules="[v => !!v || $t('You must enter a Zip Code')]"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerPhone
                "
                :label="$t('Employer Phone number')"
                :rules="phoneRuleSet"
                :readonly="readonly"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </template>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import {
  countries,
  employmentStatus,
  states,
} from '@shared-utils/lists/defaultConstants'
import { inject, ref } from 'vue'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const valid = ref(false)
const readonly = inject<boolean>('readonly')

function handleSave() {
  emit('on-save', 'Employment Information')
}
</script>
