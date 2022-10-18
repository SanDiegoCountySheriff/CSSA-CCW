<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header, font-weight-bold">
        {{ $t(' Employment Status') }}
      </v-subheader>

      <v-row>
        <v-col>
          <v-select
            id="select"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
            @change="
              v => {
                completeApplicationStore.completeApplication.employment =
                  v.toLowerCase();
              }
            "
          >
          </v-select>
        </v-col>
      </v-row>
      <div
        v-if="
          completeApplicationStore.completeApplication.employment === 'employed'
        "
      >
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Work Information') }}
        </v-subheader>
        <v-row>
          <v-col>
            <v-text-field
              :label="$t('Employer Name')"
              :rules="[v => !!v || $t('You must enter a employer name')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerName
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>

            <v-text-field
              :label="$t('Employer Address Line 1')"
              :rules="[v => !!v || $t('You must enter a address')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerAddressLine1
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
            <v-text-field
              :label="$t('Employer Address Line 2')"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerAddressLine2
              "
            >
            </v-text-field>
            <v-text-field
              :label="$t('Employer City')"
              :rules="[v => !!v || $t('You must enter a city')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerCity
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              :label="$t('Employer State')"
              :rules="[v => !!v || $t('You must enter a state')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerCity
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
            <v-text-field
              :label="$t('Employer Zip Code')"
              :rules="[v => !!v || $t('You must enter a Zip Code')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerZip
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
            <v-text-field
              :label="$t('Employer Phone number')"
              :rules="[v => !!v || $t('You must enter a phone number')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerPhone
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
            <v-text-field
              :label="$t('Employer Country')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="
                completeApplicationStore.completeApplication.workInformation
                  .employerCountry
              "
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-asterisk
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
      </div>
    </v-form>
    <div>
      <WeaponsTable
        :weapons="completeApplicationStore.completeApplication.weapons"
      />
      <WeaponsDialog :save-weapon="getWeaponFromDialog" />
    </div>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { employmentStatus } from '@shared-utils/lists/defaultConstants';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import WeaponsDialog from '@core-public/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';

interface ISecondFormStepOneProps {
  handleNextSection: CallableFunction;
}
const completeApplicationStore = useCompleteApplicationStore();

const props = defineProps<ISecondFormStepOneProps>();

const state = reactive({
  valid: false,
});

function handleSubmit() {
  props.handleNextSection();
}

function getWeaponFromDialog(weapon) {
  completeApplicationStore.completeApplication.weapons.push(weapon);
}
</script>

<style lang="scss" scoped></style>
