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
          cols="12"
          lg="6"
        >
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-text-field
                dense
                outlined
                maxlength="25"
                counter
                persistent-hint
                v-model="completeApplication.idInfo.idNumber"
                :label="$t('Id number')"
                :hint="$t('PERMIT-HINT')"
                :rules="[v => !!v || $t('Id  number is required')]"
                v-bind="attrs"
                v-on="on"
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
            </template>
            {{
              $t(
                ' IMPORTANT! Must exactly match the information on your drivers license or id.'
              )
            }}
          </v-tooltip>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-autocomplete
            outlined
            dense
            autocomplete="none"
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
          cols="12"
          lg="6"
        >
          <v-menu
            v-model="menu"
            :close-on-content-click="false"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template #activator="{ on, attrs }">
              <v-text-field
                outlined
                dense
                readonly
                v-model="completeApplication.dob.birthDate"
                :label="$t('Date of Birth')"
                :rules="[
                  checkFor21,
                  v => !!v || $t('Date of birth is required'),
                ]"
                prepend-icon="mdi-calendar"
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>
            <v-date-picker
              v-model="completeApplication.dob.birthDate"
              no-title
              scrollable
            >
            </v-date-picker>
          </v-menu>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            maxlength="150"
            counter
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

          <v-combobox
            outlined
            dense
            combobox="none"
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
          </v-combobox>

          <v-autocomplete
            v-if="completeApplication.dob.birthCountry === 'United States'"
            outlined
            dense
            maxlength="150"
            counter
            autocomplete="none"
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

          <v-text-field
            v-if="completeApplication.dob.birthCountry !== 'United States'"
            outlined
            dense
            maxlength="150"
            counter
            :label="$t('Birth region')"
            :rules="[v => !!v || $t('Birth region cannot be blank')]"
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
          </v-text-field>
        </v-col>
      </v-row>

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Citizenship Information') }}
      </v-subheader>

      <v-row>
        <v-col
          cols="12"
          lg="6"
          class="pl-5"
        >
          <v-radio-group
            :label="'Citizen'"
            :layout="'row'"
            v-model="completeApplication.citizenship.citizen"
          >
            <v-radio
              :color="$vuetify.theme.dark ? 'info' : 'primary'"
              :label="'Yes'"
              :value="true"
            />
            <v-radio
              :color="$vuetify.theme.dark ? 'info' : 'primary'"
              :label="'No'"
              :value="false"
            />
          </v-radio-group>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-select
            outlined
            dense
            v-model="completeApplication.citizenship.militaryStatus"
            :items="items"
            :label="$t('Military Status')"
            :rules="[v => !!v || $t('Military Status is required')]"
          />

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
              autocomplete="none"
              outlined
              dense
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
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="'Yes'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
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
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="'Yes'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="'No'"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-combobox
              outlined
              dense
              combobox="none"
              :items="countries"
              :label="$t('Country of Birth')"
              :rules="[v => !!v || $t('You must enter a country')]"
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
            </v-combobox>
          </v-col>
        </v-row>
      </v-container>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="valid"
      :submitting="submited"
      @submit="handleSubmit"
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
    <v-snackbar
      :value="formError"
      :timeout="3000"
      outlined
      color="error"
    >
      {{ $t('Military status has not been selected and is required') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { TranslateResult } from 'vue-i18n';
import { i18n } from '@core-public/plugins';
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
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
const menu = ref(false);
const submited = ref(false);
const formError = ref(false);

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const updateMutation = useMutation({
  mutationFn: () => {
    completeApplication.immigrantInformation.countryOfBirth =
      completeApplication.dob.birthCountry;

    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 3;
    props.handleNextSection();
  },
  onError: () => {
    submited.value = false;
    valid.value = true;
    snackbar.value = true;
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
    submited.value = false;
    valid.value = true;
    snackbar.value = true;
  },
});

function handleSubmit() {
  // TODO: see about abstracting the if statements.
  if (!completeApplication.differentMailing) {
    completeApplication.mailingAddress.zip = '';
    completeApplication.mailingAddress.city = '';
    completeApplication.mailingAddress.state = '';
    completeApplication.mailingAddress.county = '';
    completeApplication.mailingAddress.country = '';
    completeApplication.mailingAddress.addressLine1 = '';
    completeApplication.mailingAddress.addressLine2 = '';
  }

  if (!completeApplication.differentSpouseAddress) {
    completeApplication.spouseAddressInformation.addressLine1 = '';
    completeApplication.spouseAddressInformation.addressLine2 = '';
    completeApplication.spouseAddressInformation.zip = '';
    completeApplication.spouseAddressInformation.country = '';
    completeApplication.spouseAddressInformation.county = '';
    completeApplication.spouseAddressInformation.city = '';
    completeApplication.spouseAddressInformation.state = '';
  }

  if (completeApplication.citizenship.citizen) {
    completeApplication.immigrantInformation.countryOfCitizenship = '';
    completeApplication.immigrantInformation.immigrantAlien = false;
    completeApplication.immigrantInformation.nonImmigrantAlien = false;
  }

  valid.value = false;
  submited.value = false;
  updateMutation.mutate();
}

function checkFor21(input: string): boolean | TranslateResult {
  const userDate = input;
  const targetDate = new Date(Date.now());
  const formatedDate = `${targetDate.getFullYear() - 21}-${
    targetDate.getMonth() + 1
  }-${targetDate.getDate()}`;

  return userDate <= formatedDate
    ? true
    : i18n.t('You must be 21 or older to apply for a CCW permit');
}
</script>
