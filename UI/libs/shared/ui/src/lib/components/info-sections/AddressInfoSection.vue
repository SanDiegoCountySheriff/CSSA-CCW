<template>
  <v-container
    fluid
    class="address-info-container rounded"
  >
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t(' Current Address Information') }}
      </v-card-title>

      <v-card-text class="m-5">
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t(' Address line 1: ') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.addressLine1 }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>
                {{ $t(' Address line 2: ') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.addressLine2 }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('City:') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.city }}
              </p>
            </div>
          </v-col>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('State:') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.state }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Zip:') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.zip }}
              </p>
            </div>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('County:') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.county }}
              </p>
            </div>
          </v-col>

          <v-col>
            <div class="info-row">
              <h3>
                {{ $t('Country:') }}
              </h3>
              <p class="info-text">
                {{ props.addressInfo.country }}
              </p>
            </div>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-ui/types/defaultTypes';

interface IAddressInfoSectionProps {
  addressInfo: AddressInfoType;
  color: string;
}
const props = defineProps<IAddressInfoSectionProps>();
</script>

<style lang="scss" scoped>
.address-info-container {
  width: 100%;
  padding: 0;
  margin: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
  margin-left: 0.5rem;
}

.info-text {
  margin-left: 0.5rem;
}
</style>
