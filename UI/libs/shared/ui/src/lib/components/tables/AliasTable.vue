<template>
  <v-data-table
    hide-default-footer
    :headers="headers"
    :items="aliases"
    mobile-breakpoint="800"
  >
    <template #[`item.actions`]="{ item }">
      <v-tooltip
        top
        open-delay="500"
      >
        <template #activator="{ on, attrs }">
          <v-icon
            v-bind="attrs"
            @click="handleDelete(item)"
            v-on="on"
          >
            mdi-delete
          </v-icon>
        </template>
        <span>{{ $t('Delete item') }}</span>
      </v-tooltip>
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { AliasType } from '@shared-utils/types/defaultTypes'

interface AliasTableProps {
  aliases?: Array<AliasType>
  enableDelete: boolean
}

const emit = defineEmits(['delete'])

const props = withDefaults(defineProps<AliasTableProps>(), {
  aliases: () => [],
})

const headers = [
  { text: 'Previous Last name', value: 'prevLastName' },
  { text: 'Previous First name', value: 'prevFirstName' },
  { text: 'Previous Middle name', value: 'prevMiddleName' },
  { text: 'City where changed', value: 'cityWhereChanged' },
  { text: 'State or region where changed', value: 'stateWhereChanged' },
  { text: 'Court file number', value: 'courtFileNumber' },
  { text: 'Actions', value: 'actions' },
]

function handleDelete(index) {
  emit('delete', index)
}
</script>

<style lang="scss" scoped>
.theme--dark.v-data-table {
  background: #303030;
}
</style>
