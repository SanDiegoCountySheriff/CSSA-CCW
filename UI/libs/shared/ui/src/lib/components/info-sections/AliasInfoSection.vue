<template>
  <v-container class="confirm-info-section rounded">
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t(' Alias Information: ') }}
    </v-banner>
    <v-row>
      <v-col>
        <AliasTable
          :color="props.color"
          :aliases="props.aliasInfo"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { AliasType } from '@shared-utils/types/defaultTypes';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';

interface IAliasInfoSectionProps {
  aliasInfo: Array<AliasType>;
  color: string;
}

const props = defineProps<IAliasInfoSectionProps>();
</script>

<style lang="scss">
.confirm-info-section {
  width: 80%;
  height: 100%;
  margin: 0;
  padding: 0;
}
</style>
