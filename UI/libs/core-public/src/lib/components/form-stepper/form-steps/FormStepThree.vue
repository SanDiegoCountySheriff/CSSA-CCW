<template>
  <div>
    <v-subheader class="sub-header font-weight-bold">
      {{ $t('Address Information') }}
    </v-subheader>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            :label="$t('Address line 1')"
            :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
            v-model="completeApplication.currentAddress.addressLine1"
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
            class="pl-6"
            :label="$t('Address line 2')"
            v-model="completeApplication.currentAddress.addressLine2"
          >
          </v-text-field>
        </v-col>
      </v-row>

      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            :label="$t('City')"
            :rules="[v => !!v || $t('City cannot be blank')]"
            v-model="completeApplication.currentAddress.city"
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
          <v-autocomplete
            dense
            outlined
            :items="states"
            :label="$t('State')"
            :rules="[v => !!v || $t('State cannot be blank')]"
            v-model="completeApplication.currentAddress.state"
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
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            dense
            outlined
            :label="$t('County')"
            :rules="[v => !!v || $t('County cannot be blank')]"
            v-model="completeApplication.currentAddress.county"
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
            :label="$t('Zip')"
            :rules="[v => !!v || $t('Zip cannot be blank')]"
            v-model="completeApplication.currentAddress.zip"
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
          <v-autocomplete
            dense
            outlined
            :items="countries"
            :label="$t('Country')"
            :rules="[v => !!v || 'Country cannot be blank']"
            v-model="completeApplication.currentAddress.country"
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
      <v-row class="ml-5 my-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-checkbox
            id="different-mailing"
            :label="$t('Different Mailing address')"
            v-model="completeApplication.differentMailing"
          />
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-checkbox
            id="different-spouse"
            :label="$t('Different Spouse address')"
            v-model="completeApplication.differentSpouseAddress"
          />
        </v-col>
      </v-row>
      <div v-if="completeApplication.differentSpouseAddress">
        <v-subheader>
          {{ $t('Spouse Address') }}
        </v-subheader>
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('Spouse address line 1')"
              :rules="[v => !!v || $t('Spouse address line 1 cannot be blank')]"
              v-model="
                completeApplication.spouseAddressInformation.addressLine1
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
            </v-text-field>
          </v-col>

          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              class="pl-6"
              :label="$t('Spouse address line 2')"
              v-model="
                completeApplication.spouseAddressInformation.addressLine2
              "
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('Spouse\'s City')"
              :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
              v-model="completeApplication.spouseAddressInformation.city"
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
            <v-autocomplete
              dense
              outlined
              :items="states"
              :label="$t('Spouse\'s State')"
              :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
              v-model="completeApplication.spouseAddressInformation.state"
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
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('Spouse\'s County')"
              :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
              v-model="completeApplication.spouseAddressInformation.county"
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
              :label="$t('Spouse\'s Zip')"
              :rules="[v => !!v || $t('Spouse\'s Zip cannot be blank')]"
              v-model="completeApplication.spouseAddressInformation.zip"
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
            <v-autocomplete
              dense
              outlined
              :items="countries"
              :label="$t('Spouse\'s Country')"
              :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
              v-model="completeApplication.spouseAddressInformation.country"
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
        <v-divider class="my-3" />
      </div>

      <div v-if="completeApplication.differentMailing">
        <v-subheader>
          {{ $t('Mailing Address') }}
        </v-subheader>
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('Address line 1')"
              :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
              v-model="completeApplication.mailingAddress.addressLine1"
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
              class="pl-6"
              :label="$t('Address line 2')"
              v-model="completeApplication.mailingAddress.addressLine2"
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('City')"
              :rules="[v => !!v || $t(' City cannot be blank')]"
              v-model="completeApplication.mailingAddress.city"
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
            <v-autocomplete
              dense
              outlined
              :items="states"
              :label="$t('State')"
              :rules="[v => !!v || $t('State cannot be blank')]"
              v-model="completeApplication.mailingAddress.state"
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
          <v-col
            cols="12"
            lg="6"
          >
            <v-text-field
              dense
              outlined
              :label="$t('County')"
              :rules="[v => !!v || $t('County cannot be blank')]"
              v-model="completeApplication.mailingAddress.county"
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
              :label="$t('Zip')"
              dense
              outlined
              :rules="[v => !!v || $t('Zip cannot be blank')]"
              v-model="completeApplication.mailingAddress.zip"
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
            <v-autocomplete
              dense
              outlined
              :items="countries"
              :label="$t('Country')"
              :rules="[v => !!v || $t('Country cannot be blank')]"
              v-model="completeApplication.mailingAddress.country"
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
      </div>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Previous Address') }}
      </v-subheader>
      <div class="previous-address-container">
        <address-table :addresses="completeApplication.previousAddresses" />
        <PreviousAddressDialog
          :get-previous-address-from-dialog="getPreviousAddressFromDialog"
        />
      </div>
      <v-divider class="my-5" />
      <FormButtonContainer
        :valid="valid"
        @submit="updateMutation.mutate"
        @save="saveMutation.mutate"
        @back="handlePreviousSection"
        @cancel="router.push('/')"
      />
    </v-form>

    <v-snackbar
      :value="snackbar"
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
import { AddressInfoType } from '@shared-utils/types/defaultTypes';
import AddressTable from '@shared-ui/components/tables/AddressTable.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import PreviousAddressDialog from '../../dialogs/PreviousAddressDialog.vue';
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { countries, states } from '@shared-utils/lists/defaultConstants';

interface FormStepThreeProps {
  handleNextSection: () => void;
  handlePreviousSection: CallableFunction;
}

const props = withDefaults(defineProps<FormStepThreeProps>(), {
  handleNextSection: () => null,
});

const valid = ref(false);
const snackbar = ref(false);

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;
const router = useRouter();

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step three complete');
  },
  onSuccess: () => {
    completeApplication.currentStep = 4;
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

function getPreviousAddressFromDialog(address: AddressInfoType) {
  completeApplication.previousAddresses.push(address);
}
</script>

<style lang="scss" scoped>
.sub-header {
  font-size: 1.5rem;
}
.previous-address-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
