<template xmlns:v-card="http://www.w3.org/1999/html">
  <v-container
    fluid
    class="confirm-info-section rounded"
  >
    <v-card :color="props.color">
      <v-card-title class="ml-3">
        {{ $t(' Alias Information') }}
      </v-card-title>
      <v-card-text>
        <AliasTable
          :color="props.color"
          :aliases="props.aliasInfo"
        />
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { AliasType } from '@shared-ui/types/defaultTypes';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';

interface IAliasInfoSectionProps {
  aliasInfo: Array<AliasType>;
  color: string;
}

const props = defineProps<IAliasInfoSectionProps>();
</script>

<style lang="scss"></style>
