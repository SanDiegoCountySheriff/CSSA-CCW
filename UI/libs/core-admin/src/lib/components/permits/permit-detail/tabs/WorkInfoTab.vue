<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Employment') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="
          permitStore.getPermitDetail.application.employment === 'Employed' &&
          !valid
        "
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-row>
        <v-col cols="6">
          <v-select
            v-model="permitStore.getPermitDetail.application.employment"
            :value="permitStore.getPermitDetail.application.employment"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
          >
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!permitStore.getPermitDetail.application.employment"
              >
                mdi-alert-octagon
              </v-icon>
            </template>
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
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col>
              <v-text-field
                :label="$t('Employer Name')"
                :rules="[v => !!v || $t('You must enter a employer name')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerName
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerName
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Occupation')"
                :rules="[v => !!v || $t('You must enter a occupation')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .occupation
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .occupation
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                :label="$t('Employer Address Line 1')"
                :rules="[v => !!v || $t('You must enter a address')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerAddressLine1
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerAddressLine1
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                :label="$t('Employer Address Line 2')"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerAddressLine2
                "
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-combobox
                :items="countries"
                :label="$t('Employer Country')"
                :rules="[v => !!v || $t('You must enter a country')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCountry
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerCountry
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-combobox>
            </v-col>
            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCountry === 'United States'
                "
                maxlength="50"
                :items="states"
                :label="$t('Employer State')"
                :rules="[v => !!v || $t('You must enter a state')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerState
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerState
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-autocomplete>

              <v-text-field
                v-else
                maxlength="50"
                :label="$t('Employer State')"
                :rules="[v => !!v || $t('You must enter a state')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerState
                "
              >
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Employer City')"
                :rules="[v => !!v || $t('You must enter a city')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerCity
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerCity
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                :label="$t('Employer Zip Code')"
                :rules="[v => !!v || $t('You must enter a Zip Code')]"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerZip
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerZip
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Employer Phone number')"
                :rules="phoneRuleSet"
                v-model="
                  permitStore.getPermitDetail.application.workInformation
                    .employerPhone
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.workInformation
                        .employerPhone
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </template>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue';
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import {
  countries,
  employmentStatus,
  states,
} from '@shared-utils/lists/defaultConstants';

const emit = defineEmits(['on-save']);
const permitStore = usePermitsStore();
const valid = ref(false);

function handleSave() {
  emit('on-save', 'Employment Information');
}
</script>
