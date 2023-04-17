<template>
  <v-container fluid>
    <v-data-table
      hide-default-footer
      :headers="headers"
      :items="addresses"
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
  </v-container>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes'

interface AddressTableProps {
  addresses?: Array<AddressInfoType>
  enableDelete: boolean
}

const emit = defineEmits(['delete'])

const props = withDefaults(defineProps<AddressTableProps>(), {
  addresses: () => [],
})

const headers = [
  { text: 'Address line 1', value: 'addressLine1' },
  { text: 'Address line 2', value: 'addressLine2' },
  { text: 'City', value: 'city' },
  { text: 'State', value: 'state' },
  { text: 'County', value: 'county' },
  { text: 'Zip', value: 'zip' },
  { text: 'Country', value: 'country' },
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
