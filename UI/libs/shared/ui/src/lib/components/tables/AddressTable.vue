<template>
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
</template>

<script lang="ts">
import Vue from 'vue';
import { AddressInfo } from '@shared-ui/types/defualtTypes';

export default Vue.extend({
  name: 'AddressTable',
  props: {
    addresses: {
      type: Array<AddressInfo>,
      default: () => [],
    },
  },
});
</script>
