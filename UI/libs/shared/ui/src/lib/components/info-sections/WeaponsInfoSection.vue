<template>
  <v-container
    fluid
    class="info-section-container rounded"
  >
    <v-banner class="font-weight-bold text-xl text-left mb-5">
      {{ $t('Weapons Information') }}
    </v-banner>
    <v-row>
      <v-col>
        <WeaponsTable
          :weapons="props.weapons"
          :delete-enabled="false"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';

interface IWeaponsInfoSectionProps {
  weapons: Array<WeaponInfoType>;
}

const props = defineProps<IWeaponsInfoSectionProps>();
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 80%;
  height: 100%;
  margin: 0.5em 0;
  padding: 0;
}
</style>
