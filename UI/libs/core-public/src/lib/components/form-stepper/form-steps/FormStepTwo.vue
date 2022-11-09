<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Id Information') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="completeApplication.idInfo.idNumber"
            :label="$t('Id number')"
            :rules="[v => !!v || $t('Id  number is required')]"
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
          cols="6"
          md="5"
          sm="3"
        >
          <v-autocomplete
            :items="states"
            :label="$t(' Issuing State')"
            :rules="[v => !!v || $t('Issuing state is required')]"
            v-model="completeApplication.idInfo.issuingState"
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

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Date of birth') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-date-picker
            v-model="completeApplication.dob.birthDate"
            label="Date of birth"
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="!completeApplication.dob.birthDate"
          >
            {{ $t('Date of birth cannot be blank!') }}
          </v-alert>
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Birth city')"
            :rules="[v => !!v || $t('Birth city cannot be blank')]"
            v-model="completeApplication.dob.birthCity"
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
            :items="states"
            :label="$t('Birth state')"
            :rules="[v => !!v || $t('Birth state cannot be blank')]"
            v-model="completeApplication.dob.birthState"
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

          <v-autocomplete
            :items="countries"
            :label="$t('Birth country')"
            :rules="[v => !!v || $t('Birth country cannot be blank')]"
            v-model="completeApplication.dob.birthCountry"
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

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Citizenship Information') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="6"
          md="5"
          sm="3"
          class="pl-5"
        >
          <v-radio-group
            :label="'Citizen'"
            :layout="'row'"
            v-model="completeApplication.citizenship.citizen"
          >
            <v-radio
              :label="'Yes'"
              :value="true"
            />
            <v-radio
              :label="'No'"
              :value="false"
            />
          </v-radio-group>
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-select
            v-model="completeApplication.citizenship.militaryStatus"
            :items="items"
            :label="$t('Military Status')"
          />

          <v-alert
            dense
            outlined
            type="error"
            v-if="!completeApplication.citizenship.militaryStatus"
          >
            {{ $t('Must select a status') }}
          </v-alert>
          <v-alert
            dense
            outlined
            type="warning"
            v-if="
              completeApplication.citizenship.militaryStatus === 'Discharged'
            "
          >
            {{ $t('discharged-disclaimer') }}
          </v-alert>
        </v-col>
      </v-row>
      <v-container
        fluid
        v-if="!completeApplication.citizenship.citizen"
      >
        <v-divider class="mb-3" />
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Immigrant Information') }}
        </v-subheader>
        <v-row class="ml-5">
          <v-col>
            <v-autocomplete
              :items="countries"
              :label="$t('Country of Citizenship')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="
                completeApplication.immigrantInformation.countryOfCitizenship
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
            </v-autocomplete>
            <v-radio-group
              :label="'Immigrant Alien'"
              v-model="completeApplication.immigrantInformation.immigrantAlien"
              row
            >
              <v-radio
                :label="'Yes'"
                :value="true"
              />
              <v-radio
                :label="'No'"
                :value="false"
              />
            </v-radio-group>
            <v-radio-group
              :label="'Non-Immigrant Alien'"
              row
              v-model="
                completeApplication.immigrantInformation.nonImmigrantAlien
              "
            >
              <v-radio
                :label="'Yes'"
                :value="true"
              />
              <v-radio
                :label="'No'"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col>
            <v-autocomplete
              :items="countries"
              :label="$t('Country of Birth')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="completeApplication.immigrantInformation.countryOfBirth"
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
      </v-container>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="valid"
      @submit="updateMutation.mutate"
      @save="saveMutation.mutate"
      @back="handlePreviousSection"
      @cancel="router.push('/')"
    />
    <v-snackbar
      :value="snackbar"
      :timeout="3000"
      outlined
      color="error"
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { countries, states } from '@shared-utils/lists/defaultConstants';

interface FormStepTwoProps {
  handleNextSection: () => void;
  handlePreviousSection: CallableFunction;
}

const props = withDefaults(defineProps<FormStepTwoProps>(), {
  handleNextSection: () => null,
});

const router = useRouter();

const items = ref(['Active', 'Reserve', 'Discharged', 'Retired', 'N/A']);
const snackbar = ref(false);
const valid = ref(false);

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step two complete');
  },
  onSuccess: () => {
    completeApplication.currentStep = 3;
    props.handleNextSection();
  },
  onError: () => {
    snackbar.value = true;
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
    snackbar.value = true;
  },
});
</script>
