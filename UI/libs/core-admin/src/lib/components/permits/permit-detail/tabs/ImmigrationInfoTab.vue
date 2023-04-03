<template>
  <div>
    <v-form v-model="valid">
      <v-card elevation="0">
        <v-card-title>
          {{ $t('Citizenship Information') }}
          <v-spacer></v-spacer>
          <SaveButton
            :disabled="!valid"
            @on-save="handleSave"
          />
        </v-card-title>

        <v-card-text>
          <v-row>
            <v-col>
              <v-radio-group
                row
                v-model="
                  permitStore.getPermitDetail.application.citizenship.citizen
                "
                :label="$t('Citizen')"
              >
                <v-radio
                  :value="true"
                  :label="$t('Yes')"
                >
                </v-radio>
                <v-radio
                  :value="false"
                  :label="$t('No')"
                >
                </v-radio>
              </v-radio-group>
            </v-col>

            <v-col>
              <v-select
                v-model="
                  permitStore.getPermitDetail.application.citizenship
                    .militaryStatus
                "
                :items="items"
                :label="$t('Military Status')"
                :rules="[v => !!v || 'A military status is required.']"
              />
              <v-alert
                dense
                outlined
                type="warning"
                v-if="
                  permitStore.getPermitDetail.application.citizenship
                    .militaryStatus === 'Discharged'
                "
              >
                {{ $t('discharged-disclaimer') }}
              </v-alert>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>

      <div v-if="!permitStore.getPermitDetail.application.citizenship.citizen">
        <v-divider />
        <v-card elevation="0">
          <v-card-title>
            {{ $t('Immigrant Information') }}
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <v-combobox
                  v-model="
                    permitStore.getPermitDetail.application.immigrantInformation
                      .countryOfCitizenship
                  "
                  :items="countries"
                  :label="$t('Country of Citizenship')"
                  :rules="[v => !!v || $t('You must enter a country')]"
                >
                </v-combobox>
              </v-col>
              <v-col>
                <v-combobox
                  v-model="
                    permitStore.getPermitDetail.application.immigrantInformation
                      .countryOfBirth
                  "
                  :items="countries"
                  :label="$t('Country of Birth')"
                  :rules="[v => !!v || $t('You must enter a country')]"
                >
                </v-combobox>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-radio-group
                  v-model="
                    permitStore.getPermitDetail.application.immigrantInformation
                      .immigrantAlien
                  "
                  :label="$t('Immigrant Alien')"
                  row
                >
                  <v-radio
                    :value="true"
                    :label="$t('Yes')"
                  >
                  </v-radio>
                  <v-radio
                    :value="false"
                    :label="$t('No')"
                  >
                  </v-radio>
                </v-radio-group>
              </v-col>
              <v-col>
                <v-radio-group
                  v-model="
                    permitStore.getPermitDetail.application.immigrantInformation
                      .nonImmigrantAlien
                  "
                  :label="$t('Non-Immigrant Alien')"
                  row
                >
                  <v-radio
                    :value="true"
                    :label="$t('Yes')"
                  >
                  </v-radio>
                  <v-radio
                    :value="false"
                    :label="$t('No')"
                  >
                  </v-radio>
                </v-radio-group>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </div>
    </v-form>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue';
import { countries } from '@shared-utils/lists/defaultConstants';
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const emit = defineEmits(['on-save']);
const items = ref([
  'Active',
  'Reserve',
  'Discharged',
  'Retired',
  'Never Served in the Military',
]);
const valid = ref(false);

const permitStore = usePermitsStore();

function handleSave() {
  emit('on-save', 'Immigration Information');
}
</script>
