<template>
  <v-container
    fluid
    class="address-info-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t(props.title) }}
    </v-banner>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          class="pl-6"
          readonly
          :label="$t('Address 1')"
          :value="props.spouseAddress.addressLine1"
        />
      </v-col>

      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Address 2')"
          :value="props.spouseAddress.addressLine2"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('City')"
          :value="props.spouseAddress.city"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('State')"
          :value="props.spouseAddress.state"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('County')"
          :value="props.spouseAddress.county"
        />
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Zip')"
          :value="props.spouseAddress.zip"
        />
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-text-field
          outlined
          dense
          readonly
          class="pl-6"
          :label="$t('Country')"
          :value="props.spouseAddress.country"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';

interface ISpouseAddressProps {
  spouseAddress: AddressInfoType;
  title: string;
  color: string;
}

const props = defineProps<ISpouseAddressProps>();
</script>

<style scoped lang="scss">
.address-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 1rem;
  padding-bottom: 0.5rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
