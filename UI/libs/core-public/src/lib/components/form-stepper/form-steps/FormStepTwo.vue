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
            v-model="completeApplicationStore.completeApplication.id.idNumber"
            :label="$t('Id number')"
            :rules="[v => !!v || $t('Id  number is required')]"
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t(' Issuing State')"
            :rules="[v => !!v || $t('Issuing state is required')]"
            v-model="
              completeApplicationStore.completeApplication.id.issuingState
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
            v-model="completeApplicationStore.completeApplication.DOB.birthDate"
            label="Date of birth"
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="!completeApplicationStore.completeApplication.DOB.birthDate"
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
            :label="$t('Current Age')"
            :rules="[v => !!v || $t('Current age is required')]"
            v-model="
              completeApplicationStore.completeApplication.DOB.currentAge
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
            :label="$t('Birth city')"
            :rules="[v => !!v || $t('Birth city cannot be blank')]"
            v-model="completeApplicationStore.completeApplication.DOB.birthCity"
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
            :label="$t('Birth state')"
            :rules="[v => !!v || $t('Birth state cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.DOB.birthState
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
            :label="$t('Birth country')"
            :rules="[v => !!v || $t('Birth country cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.DOB.birthCountry
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
          <RadioGroupInput
            :label="'Citizen'"
            :layout="'row'"
            :options="[
              { label: 'Yes', value: true },
              { label: 'No', value: false },
            ]"
            @input="
              v => {
                completeApplicationStore.completeApplication.citizenship.citizen =
                  v;
              }
            "
          />
          <v-alert
            dense
            outlined
            type="error"
            v-if="
              !completeApplicationStore.completeApplication.citizenship.citizen
            "
          >
            {{ $t('Must select Yes or No!') }}
          </v-alert>
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-select
            v-model="
              completeApplicationStore.completeApplication.citizenship
                .militaryStatus
            "
            :items="items"
            :label="$t('Military Status')"
          />

          <v-alert
            dense
            outlined
            type="error"
            v-if="
              !completeApplicationStore.completeApplication.citizenship
                .militaryStatus
            "
          >
            {{ $t('Must select a status') }}
          </v-alert>
          <v-alert
            dense
            outlined
            type="warning"
            v-if="
              completeApplicationStore.completeApplication.citizenship
                .militaryStatus === 'Discharged'
            "
          >
            {{ $t('discharged-disclaimer') }}
          </v-alert>
        </v-col>
      </v-row>
      <v-container
        fluid
        v-if="!completeApplicationStore.completeApplication.citizenship.citizen"
      >
        <v-divider class="mb-3" />
        <v-subheader class="sub-header font-weight-bold">
          {{ $t('Immigrant Information') }}
        </v-subheader>
        <v-row class="ml-5">
          <v-col>
            <v-text-field
              :label="$t('Country of Citizenship')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="
                completeApplicationStore.completeApplication
                  .immigrantInformation.countryOfCitizenship
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
            <RadioGroupInput
              :label="'Immigrant Alien'"
              :layout="'row'"
              :options="[
                { label: 'Yes', value: true },
                { label: 'No', value: false },
              ]"
              @input="
                v => {
                  completeApplicationStore.completeApplication.immigrantInformation.immigrantAlien =
                    v;
                }
              "
            />
            <RadioGroupInput
              :label="'Non-Immigrant Alien'"
              :layout="'row'"
              :options="[
                { label: 'Yes', value: true },
                { label: 'No', value: false },
              ]"
              @input="
                v => {
                  completeApplicationStore.completeApplication.immigrantInformation.nonImmigrantAlien =
                    v;
                }
              "
            />
          </v-col>
          <v-col>
            <v-text-field
              :label="$t('Country of Birth')"
              :rules="[v => !!v || $t('You must enter a country')]"
              v-model="
                completeApplicationStore.completeApplication
                  .immigrantInformation.countryOfBirth
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
      </v-container>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface FormStepOneProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepOneProps>(), {
  handleNextSection: () => null,
});

const items = ref(['Active', 'Reserve', 'Discharged', 'Retired', 'N/A']);
const valid = ref(false);

const completeApplicationStore = useCompleteApplicationStore();

function handleSubmit() {
  props.handleNextSection();
}

function handleSave() {
  completeApplicationStore.postCompleteApplicationFromApi;
}
</script>
