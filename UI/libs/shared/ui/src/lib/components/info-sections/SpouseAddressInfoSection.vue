<template>
  <v-container
    fluid
    class="address-info-container rounded mt-5"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t(props.title) }}
    </v-banner>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Address Line 1: ') }}
          </strong>
          {{ props.spouseAddress.addressLine1 }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Address Line 2: ') }}
          </strong>
          {{ props.spouseAddress.addressLine2 }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Country: ') }}
          </strong>
          {{ props.spouseAddress.country }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('State or Region: ') }}
          </strong>
          {{ props.spouseAddress.state }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('City: ') }}
          </strong>
          {{ props.spouseAddress.city }}
        </v-banner>
      </v-col>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('Zip: ') }}
          </strong>
          {{ props.spouseAddress.zip }}
        </v-banner>
      </v-col>
    </v-row>

    <v-row>
      <v-col
        cols="12"
        lg="6"
      >
        <v-banner
          single-line
          class="text-left"
        >
          <v-icon
            left
            color="accent"
          >
            mdi-home
          </v-icon>
          <strong>
            {{ $t('County: ') }}
          </strong>
          {{ props.spouseAddress.county }}
        </v-banner>
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
