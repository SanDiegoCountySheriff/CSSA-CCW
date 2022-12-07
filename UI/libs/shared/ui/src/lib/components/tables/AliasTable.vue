<template>
  <v-container fluid>
    <v-simple-table class="my-3">
      <template #default>
        <thead>
          <tr>
            <th scope="col">{{ $t('Previous Last name') }}</th>
            <th scope="col">{{ $t('Previous First name') }}</th>
            <th scope="col">{{ $t('Previous Middle name') }}</th>
            <th scope="col">{{ $t('City where changed') }}</th>
            <th scope="col">{{ $t('State or region where changed') }}</th>
            <th scope="col">{{ $t('Court file number') }}</th>
            <th scope="col"></th>
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
import { AliasType } from '@shared-utils/types/defaultTypes';

interface AliasTableProps {
  aliases?: Array<AliasType>;
}

const emit = defineEmits(['delete']);

const props = withDefaults(defineProps<AliasTableProps>(), {
  aliases: () => [],
});

function handleDelete(index) {
  emit('delete', index);
}
</script>
