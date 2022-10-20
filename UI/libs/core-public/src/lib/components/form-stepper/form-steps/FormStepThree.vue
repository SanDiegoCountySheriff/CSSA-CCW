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
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Address line 1')"
            :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.currentAddress
                .addressLine1
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
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Address line 2')"
            v-model="
              completeApplicationStore.completeApplication.currentAddress
                .addressLine2
            "
          >
          </v-text-field>
        </v-col>
      </v-row>

      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('City')"
            :rules="[v => !!v || $t('City cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.currentAddress.city
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('State')"
            :rules="[v => !!v || $t('State cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.currentAddress.state
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
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('County')"
            :rules="[v => !!v || $t('County cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.currentAddress.county
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
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Zip')"
            :rules="[v => !!v || $t('Zip cannot be blank')]"
            v-model="
              completeApplicationStore.completeApplication.currentAddress.zip
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Country')"
            :rules="[v => !!v || 'Country cannot be blank']"
            v-model="
              completeApplicationStore.completeApplication.currentAddress
                .country
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
      <v-row class="ml-5 my-5">
        <v-col>
          <v-checkbox
            id="different-mailing"
            :label="$t('Different Mailing address')"
            v-model="
              completeApplicationStore.completeApplication.differentMailing
            "
          />
        </v-col>
        <v-col>
          <v-checkbox
            id="different-spouse"
            :label="$t('Different Spouse address')"
            v-model="
              completeApplicationStore.completeApplication
                .differentSpouseAddress
            "
          />
        </v-col>
      </v-row>
      <div
        v-if="
          completeApplicationStore.completeApplication.differentSpouseAddress
        "
      >
        <v-row class="ml-5">
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse address line 1')"
              :rules="[v => !!v || $t('Spouse address line 1 cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.addressLine1
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse address line 2')"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.addressLine2
              "
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse\'s City')"
              :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.city
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse\'s State')"
              :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.state
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
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse\'s County')"
              :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.county
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
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse\'s Zip')"
              :rules="[v => !!v || $t('Spouse\'s Zip cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.zip
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Spouse\'s Country')"
              :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication
                  .spouseAddressInformation.country
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
      </div>

      <div v-if="completeApplicationStore.completeApplication.differentMailing">
        <v-row class="ml-5">
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Address line 1')"
              :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress
                  .addressLine1
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Address line 2')"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress
                  .addressLine2
              "
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('City')"
              :rules="[v => !!v || $t(' City cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress.city
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('State')"
              :rules="[v => !!v || $t('State cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress
                  .state
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
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('County')"
              :rules="[v => !!v || $t('County cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress
                  .county
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
          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Zip cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress.zip
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

          <v-col
            cols="6"
            md="5"
            sm="3"
          >
            <v-text-field
              :label="$t('Country')"
              :rules="[v => !!v || $t('Country cannot be blank')]"
              v-model="
                completeApplicationStore.completeApplication.mailingAddress
                  .country
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
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Previous Address') }}
      </v-subheader>
      <div class="previous-address-container">
        <address-table
          :addresses="
            completeApplicationStore.completeApplication.previousAddress
          "
        />
        <PreviousAddressDialog
          :get-previous-address-from-dialog="getPreviousAddressFromDialog"
        />
      </div>
      <FormButtonContainer
        :valid="valid"
        @submit="handleSubmit"
        @save="handleSave"
      />
    </v-form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { AddressInfoType } from '@shared-utils/types/defaultTypes';
import PreviousAddressDialog from '../../dialogs/PreviousAddressDialog.vue';
import AddressTable from '@shared-ui/components/tables/AddressTable.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface FormStepThreeProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepThreeProps>(), {
  handleNextSection: () => null,
});

const valid = ref(false);

const completeApplicationStore = useCompleteApplicationStore();

function getPreviousAddressFromDialog(address: AddressInfoType) {
  completeApplicationStore.completeApplication.previousAddress.push(address);
}

function handleSubmit() {
  props.handleNextSection();
}

function handleSave() {
  completeApplicationStore.postCompleteApplicationFromApi;
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
