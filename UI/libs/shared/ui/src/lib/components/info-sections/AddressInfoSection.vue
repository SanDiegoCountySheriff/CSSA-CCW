<template>
  <v-container
    fluid
    class="address-info-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t(props.title) }}
    </v-banner>

    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Address Line 1') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.addressLine1 }}
          </div>
        </div>
      </v-col>

      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Address Line 2') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.addressLine2 }}
          </div>
        </div>
      </v-col>
    </v-row>

    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('City') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.city }}
          </div>
        </div>
      </v-col>
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('State') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.state }}
          </div>
        </div>
      </v-col>
    </v-row>

    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('County') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.county }}
          </div>
        </div>
      </v-col>
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Zip') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.zip }}
          </div>
        </div>
      </v-col>
    </v-row>

    <v-row class="ml-5">
      <v-col>
        <div class="info-row">
          <v-chip
            class="font-weight-bold"
            label
            :color="props.color"
          >
            {{ $t('Country') }}
          </v-chip>
          <div class="info-text">
            {{ props.addressInfo.country }}
          </div>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';

interface IAddressInfoSectionProps {
  addressInfo: AddressInfoType;
  title: string;
  color: string;
}
const props = defineProps<IAddressInfoSectionProps>();
</script>

<style lang="scss" scoped>
.address-info-container {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
  margin-left: 0.5rem;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-left: 0.5rem;
  padding-top: 0.2rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
