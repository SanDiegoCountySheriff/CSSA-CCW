<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-simple-table class="my-3">
      <template #default>
        <thead>
          <tr>
            <th scope="col">
              {{ $t('Make') }}
            </th>
            <th scope="col">
              {{ $t('Model') }}
            </th>
            <th scope="col">
              {{ $t('Caliper') }}
            </th>
            <th scope="col">
              {{ $t('Serial Number') }}
            </th>
            <th scope="col"></th>
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
            <td>
              <v-tooltip bottom>
                <template #activator="{ on, attrs }">
                  <v-btn
                    v-if="deleteEnabled"
                    text
                    color="error"
                    @click="handleDelete(index)"
                    v-bind="attrs"
                    v-on="on"
                  >
                    <v-icon color="error"> mdi-close-circle </v-icon>
                  </v-btn>
                </template>
                {{ $t('Delete item') }}
              </v-tooltip>
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
  </v-container>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';

interface IWeaponTableProps {
  weapons: Array<WeaponInfoType>;
  deleteEnabled: boolean;
}

const emit = defineEmits(['delete']);

const props = defineProps<IWeaponTableProps>();

function handleDelete(index) {
  emit('delete', index);
}
</script>
