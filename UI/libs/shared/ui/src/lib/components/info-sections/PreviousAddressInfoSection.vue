<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="sub-header font-weight-bold text-xl text-left mb-5">
      {{ $t(' Previous Addresses: ') }}
    </v-banner>
    <v-row>
      <v-col>
        <AddressTable
          :enable-delete="false"
          :addresses="props.previousAddress"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';
import AddressTable from '../tables/AddressTable.vue';

interface PreviousAddressInfoSectionProps {
  previousAddress: Array<AddressInfoType>;
  color: string;
}

const props = defineProps<PreviousAddressInfoSectionProps>();
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 80%;
  height: 100%;
  margin: 0.5em 0;
  padding: 0;
}
</style>
