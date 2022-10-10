<template>
  <div>
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
            :rules="[v => !!v || 'Address line 1 cannot be blank']"
            v-model="address.addressLine1"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Address line 2')"
            v-model="address.addressLine2"
          />
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
            :rules="[v => !!v || ' City cannot be blank']"
            v-model="address.city"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('State')"
            :rules="[v => !!v || 'State cannot be blank']"
            v-model="address.state"
          />
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('County')"
            :rules="[v => !!v || 'County cannot be blank']"
            v-model="address.county"
          />
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Zip')"
            :rules="[v => !!v || 'Zip cannot be blank']"
            v-model="address.zip"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Country')"
            :rules="[v => !!v || 'Country cannot be blank']"
            v-model="address.country"
          />
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Previous Address') }}
      </v-subheader>
      <div class="previous-address-container">
        <address-table :addresses="previousAddress" />
        <PreviousAddressDialog
          :get-previous-address-from-dialog="getPreviousAddressFromDialog"
        />
      </div>
      <FormButtonContainer
        :valid="valid"
        @submit="handleSubmit"
      />
    </v-form>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useCurrentAddressStore } from '@shared-ui/stores/currentAddress';
import { usePreviousAddressesStore } from '@shared-ui/stores/previousAddress';
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

const address = reactive({} as AddressInfoType);
const previousAddress = ref([] as Array<AddressInfoType>);
const valid = ref(false);

const currentAddressStore = useCurrentAddressStore();
const previousAddressesStore = usePreviousAddressesStore();

function getPreviousAddressFromDialog(address: AddressInfoType) {
  previousAddress.value.push(address);
}

function handleSubmit() {
  currentAddressStore.setCurrentAddress(address);
  previousAddressesStore.setPreviousAddresses(previousAddress.value);
  props.handleNextSection();
}
</script>

<style lang="scss" scoped>
.sub-header {
  font-size: 1.5rem;
}
.previous-address-container {
  display: flex;
  flex-direction: column;
  width: 90%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
