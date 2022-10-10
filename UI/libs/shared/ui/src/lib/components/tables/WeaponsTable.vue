<template>
  <v-simple-table class="text-left my-3">
    <template #default>
      <thead>
        <tr>
          <th>
            {{ $t('Make') }}
          </th>
          <th>
            {{ $t('Model') }}
          </th>
          <th>
            {{ $t('Caliper') }}
          </th>
          <th>
            {{ $t('Serial Number') }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(weapon, index) in weapons"
          :key="index"
        >
          <td>
            {{ weapon.make }}
          </td>
          <td>
            {{ weapon.model }}
          </td>
          <td>
            {{ weapon.caliber }}
          </td>
          <td>
            {{ weapon.serialNumber }}
          </td>
        </tr>
      </tbody>
    </template>
  </v-simple-table>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';

interface IWeaponTableProps {
  weapons: Array<WeaponInfoType>;
}

const props = defineProps<IWeaponTableProps>();
</script>
