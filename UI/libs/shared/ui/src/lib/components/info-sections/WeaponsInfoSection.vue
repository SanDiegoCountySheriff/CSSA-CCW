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
        <WeaponsTable :weapons="props.weapons" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';

interface IWeaponsInfoSectionProps {
  weapons: Array<WeaponInfoType>;
}

const props = defineProps<IWeaponsInfoSectionProps>();
</script>

<style lang="scss" scoped></style>
