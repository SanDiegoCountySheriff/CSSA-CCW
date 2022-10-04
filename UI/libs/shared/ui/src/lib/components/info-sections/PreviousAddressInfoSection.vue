<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-card
      :color="props.color"
      class="rounded"
    >
      <v-card-title class="ml-3">
        {{ $t('Previous Address') }}
      </v-card-title>
      <v-card-text>
        <v-row>
          <v-col>
            <AddressTable :addresses="props.previousAddress" />
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import AddressTable from '../tables/AddressTable';
import { AddressInfoType } from '@shared-ui/types/defaultTypes';

interface PreviousAddressInfoSectionProps {
  previousAddress: Array<AddressInfoType>;
  color: string;
}

const props = defineProps<PreviousAddressInfoSectionProps>();
</script>

<style lang="scss" scoped></style>
