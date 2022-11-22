<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <v-card elevation="3">
      <v-card-title class="text-h5">
        {{ $t('Employment:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="3"
        >
          <v-select
            dense
            filled
            v-model="permitStore.getPermitDetail.application.employment"
            :value="permitStore.getPermitDetail.application.employment"
            id="select"
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
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-select>
        </v-col>
      </v-row>
      <v-divider />
      <div
        v-if="permitStore.getPermitDetail.application.employment === 'Employed'"
      >
        <v-card-title class="text-h5">
          {{ $t('Work Information:') }}
        </v-card-title>
        <v-row class="ml-5">
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Employer Name')"
              :rules="[v => !!v || $t('You must enter a employer name')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerName
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Occupation')"
              :rules="[v => !!v || $t('You must enter a occupation')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .occupation
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Employer Address Line 1')"
              :rules="[v => !!v || $t('You must enter a address')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerAddressLine1
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              class="pl-6"
              :label="$t('Employer Address Line 2')"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerAddressLine2
              "
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Employer City')"
              :rules="[v => !!v || $t('You must enter a city')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerCity
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-autocomplete
              dense
              filled
              autocomplete="none"
              :items="states"
              :label="$t('Employer State')"
              :rules="[v => !!v || $t('You must enter a state')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerState
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-autocomplete>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Employer Zip Code')"
              :rules="[v => !!v || $t('You must enter a Zip Code')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerZip
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-text-field
              dense
              filled
              :label="$t('Employer Phone number')"
              :rules="phoneRuleSet"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerPhone
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            md="5"
            sm="3"
          >
            <v-autocomplete
              dense
              filled
              autocomplete="none"
              :items="countries"
              :label="$t('Employer Country')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="
                permitStore.getPermitDetail.application.workInformation
                  .employerCountry
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
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
                <v-icon
                  v-else
                  medium
                  color="primary"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-autocomplete>
          </v-col>
        </v-row>
      </div>
    </v-card>
    <v-divider class="mt-3 mb-3"></v-divider>
    <v-card elevation="3">
      <v-card-title class="text-h5">
        {{ $t('Weapons:') }}
      </v-card-title>
      <div class="weapon-components-container">
        <WeaponsTable
          :weapons="permitStore.getPermitDetail.application.weapons"
        />
        <div class="offset-md-8">
          <WeaponsDialog :save-weapon="getWeaponFromDialog" />
        </div>
      </div>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import {
  countries,
  employmentStatus,
  states,
} from '@shared-utils/lists/defaultConstants';

const permitStore = usePermitsStore();

function getWeaponFromDialog(weapon) {
  permitStore.getPermitDetail.application.weapons.push(weapon);
}
</script>
