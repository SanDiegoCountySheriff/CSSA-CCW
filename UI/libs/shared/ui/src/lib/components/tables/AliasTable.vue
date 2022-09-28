<template>
  <v-simple-table class="my-3">
    <template #default>
      <thead>
        <tr>
          <th>{{ $t('Previous Last name') }}</th>
          <th>{{ $t('Previous First name') }}</th>
          <th>{{ $t('Previous Middle name') }}</th>
          <th>{{ $t('City where changed') }}</th>
          <th>{{ $t('State or region where changed') }}</th>
          <th>{{ $t('Court file number') }}</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(alias, index) in aliases"
          :key="index"
        >
          <td>
            {{ alias.prevLastName }}
          </td>
          <td>
            {{ alias.prevFirstName }}
          </td>
          <td>
            {{ alias.prevMiddleName }}
          </td>
          <td>
            {{ alias.cityWhereChanged }}
          </td>
          <td>
            {{ alias.stateWhereChanged }}
          </td>
          <td>
            {{ alias.courtFileNumber }}
          </td>
        </tr>
      </tbody>
    </template>
  </v-simple-table>
</template>

<script lang="ts">
import { Alias } from '@shared-ui/types/defualtTypes';
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'AliasTable',
  props: {
    aliases: {
      type: Array<Alias>,
      default: () => [],
    },
  },
});
</script>
