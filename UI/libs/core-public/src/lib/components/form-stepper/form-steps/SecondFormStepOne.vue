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
        <v-col cols="6">
          <v-select
            id="select"
            :items="employmentStatus"
            :label="$t(' Employment Status')"
            :rules="[v => !!v || $t(' Employment status is required')]"
            @change="
              v => {
                completeApplication.employment = v.toLowerCase();
              }
            "
          >
          </v-select>
        </v-col>
      </v-row>
      <v-divider />
      <div v-if="completeApplication.employment === 'employed'">
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Work Information') }}
        </v-subheader>
        <v-row>
          <v-col>
            <v-text-field
              :label="$t('Employer Name')"
              :rules="[v => !!v || $t('You must enter a employer name')]"
              v-model="completeApplication.workInformation.employerName"
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
              v-model="completeApplication.workInformation.employerAddressLine1"
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
              v-model="completeApplication.workInformation.employerAddressLine2"
            >
            </v-text-field>
            <v-text-field
              :label="$t('Employer City')"
              :rules="[v => !!v || $t('You must enter a city')]"
              v-model="completeApplication.workInformation.employerCity"
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
              v-model="completeApplication.workInformation.employerCity"
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
              v-model="completeApplication.workInformation.employerZip"
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
              v-model="completeApplication.workInformation.employerPhone"
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
              v-model="completeApplication.workInformation.employerCountry"
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
    <div class="weapon-components-container">
      <WeaponsTable :weapons="completeApplication.weapons" />
      <WeaponsDialog :save-weapon="getWeaponFromDialog" />
    </div>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="updateMutation.mutate"
      @save="saveMutation.mutate"
    />
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import WeaponsDialog from '@core-public/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';
import { employmentStatus } from '@shared-utils/lists/defaultConstants';
import { i18n } from '@shared-ui/plugins';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

interface ISecondFormStepOneProps {
  handleNextSection: CallableFunction;
}
const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const props = defineProps<ISecondFormStepOneProps>();
const router = useRouter();

const state = reactive({
  valid: false,
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step six complete');
  },
  onSuccess: () => {
    props.handleNextSection();
  },
  onError: error => {
    alert(error);
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Save and quit');
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    alert(i18n.t('Save unsuccessful, please try again'));
  },
});

function getWeaponFromDialog(weapon) {
  completeApplication.weapons.push(weapon);
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
