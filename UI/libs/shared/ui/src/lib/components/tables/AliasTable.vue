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

<script setup lang="ts">
import { AliasType } from '@shared-ui/types/defaultTypes';

interface AliasTableProps {
  aliases?: Array<AliasType>;
}

const props = withDefaults(defineProps<AliasTableProps>(), {
  aliases: () => [],
});
</script>
