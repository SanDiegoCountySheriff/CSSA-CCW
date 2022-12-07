<template>
  <v-container fluid>
    <v-simple-table class="my-3">
      <template #default>
        <thead>
          <tr>
            <th scope="col">{{ $t(' Address line 1') }}</th>
            <th scope="col">{{ $t('Address line 2') }}</th>
            <th scope="col">{{ $t('City') }}</th>
            <th scope="col">{{ $t('State') }}</th>
            <th scope="col">{{ $t('County') }}</th>
            <th scope="col">{{ $t('Zip') }}</th>
            <th scope="col">{{ $t('Country') }}</th>
            <th scope="col"></th>
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
            <td>
              <v-tooltip bottom>
                <template #activator="{ on, attrs }">
                  <v-btn
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
import { AddressInfoType } from '@shared-utils/types/defaultTypes';

interface AddressTableProps {
  addresses?: Array<AddressInfoType>;
}

const emit = defineEmits(['delete']);

const props = withDefaults(defineProps<AddressTableProps>(), {
  addresses: () => [],
});

function handleDelete(index) {
  emit('delete', index);
}
</script>
