<template>
  <v-container fluid>
    <v-simple-table class="my-3">
      <template #default>
        <thead>
          <tr>
            <th>{{ $t(' Address line 1') }}</th>
            <th>{{ $t('Address line 2') }}</th>
            <th>{{ $t('City') }}</th>
            <th>{{ $t('State') }}</th>
            <th>{{ $t('County') }}</th>
            <th>{{ $t('Zip') }}</th>
            <th>{{ $t('Country') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(address, index) in addresses"
            :key="index"
          >
            <td>
              {{ address.addressLine1 }}
            </td>
            <td>
              {{ address.addressLine2 }}
            </td>
            <td>
              {{ address.city }}
            </td>
            <td>
              {{ address.state }}
            </td>
            <td>
              {{ address.county }}
            </td>
            <td>
              {{ address.zip }}
            </td>
            <td>
              {{ address.country }}
            </td>
          </tr>
        </tbody>
      </template>
    </v-simple-table>
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';

interface AddressTableProps {
  addresses?: Array<AddressInfoType>;
}

const props = withDefaults(defineProps<AddressTableProps>(), {
  addresses: () => [],
});
</script>
