<template>
  <div>
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header, font-weight-bold">
        {{ $t(' Employment Status') }}
      </v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-select
            dense
            outlined
            v-model="completeApplication.employment"
            :value="completeApplication.employment"
            id="select"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
          >
          </v-select>
        </v-col>
      </v-row>
      <v-divider />
      <div v-if="completeApplication.employment === 'Employed'">
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Work Information') }}
        </v-subheader>
        <v-row>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="50"
              counter
              :label="$t('Employer Name')"
              :rules="[v => !!v || $t('You must enter a employer name')]"
              v-model="completeApplication.workInformation.employerName"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="50"
              counter
              :label="$t('Occupation')"
              :rules="[v => !!v || $t('You must enter a occupation')]"
              v-model="completeApplication.workInformation.occupation"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="50"
              counter
              :label="$t('Employer Address Line 1')"
              :rules="[v => !!v || $t('You must enter a address')]"
              v-model="completeApplication.workInformation.employerAddressLine1"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="50"
              counter
              class="pl-6"
              :label="$t('Employer Address Line 2')"
              v-model="completeApplication.workInformation.employerAddressLine2"
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            lg="6"
          >
            <v-combobox
              dense
              outlined
              maxlength="50"
              counter
              autocomplete="none"
              :items="countries"
              :label="$t('Employer Country')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="completeApplication.workInformation.employerCountry"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-combobox>
          </v-col>

          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              v-if="
                completeApplication.workInformation.employerCountry !==
                'United States'
              "
              outlined
              dense
              maxlength="50"
              counter
              :label="$t('Employer Region')"
              :rules="[v => !!v || $t('You must enter a region')]"
              v-model="completeApplication.workInformation.employerState"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
            <v-autocomplete
              v-if="
                completeApplication.workInformation.employerCountry ===
                'United States'
              "
              dense
              outlined
              autocomplete="none"
              :items="states"
              :label="$t('Employer State')"
              :rules="[v => !!v || $t('You must enter a state')]"
              v-model="completeApplication.workInformation.employerState"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-autocomplete>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="50"
              counter
              :label="$t('Employer City')"
              :rules="[v => !!v || $t('You must enter a city')]"
              v-model="completeApplication.workInformation.employerCity"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>

          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              maxlength="10"
              counter
              :label="$t('Employer Zip Code')"
              :rules="zipRuleSet"
              v-model="completeApplication.workInformation.employerZip"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('Employer Phone number')"
              :rules="phoneRuleSet"
              v-model="completeApplication.workInformation.employerPhone"
            >
              <template #prepend>
                <v-icon
                  x-small
                  color="error"
                >
                  mdi-star
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
      </div>
    </v-form>
    <div class="weapon-components-container">
      <WeaponsTable
        :weapons="completeApplication.weapons"
        :delete-enabled="true"
        @delete="deleteWeapon"
      />
      <WeaponsDialog :save-weapon="getWeaponFromDialog" />
    </div>
    <v-divider clase="mt-5" />
    <FormButtonContainer
      :valid="state.valid"
      @submit="handleSubmit"
      @save="saveMutation.mutate"
      @back="props.handlePreviousSection"
      @cancel="router.push('/')"
    />
    <v-snackbar
      :value="state.snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';
import { defaultPermitState } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import {
  countries,
  employmentStatus,
  states,
} from '@shared-utils/lists/defaultConstants';
import { phoneRuleSet, zipRuleSet } from '@shared-ui/rule-sets/ruleSets';

interface ISecondFormStepOneProps {
  routes: unknown;
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}
const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const props = defineProps<ISecondFormStepOneProps>();
const router = useRouter();

const state = reactive({
  valid: false,
  snackbar: false,
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    state.valid = false;
    completeApplication.currentStep = 7;
    props.handleNextSection();
  },
  onError: () => {
    state.snackbar = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    state.snackbar = true;
  },
});

function getWeaponFromDialog(weapon) {
  completeApplication.weapons.push(weapon);
}

function handleSubmit() {
  // Verify correct fields have correct information.
  if (completeApplication.employment !== 'Employed') {
    completeApplication.workInformation =
      defaultPermitState.application.workInformation;
  }

  updateMutation.mutate();
}

function deleteWeapon(index) {
  completeApplication.weapons.splice(index, 1);
}
</script>

<style lang="scss" scoped>
.weapon-components-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
