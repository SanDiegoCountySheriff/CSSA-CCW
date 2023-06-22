<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-data-table
      hide-default-footer
      :headers="headers"
      :items="weapons"
      mobile-breakpoint="800"
    >
      <template #[`item.actions`]="{ item }">
        <v-tooltip
          top
          open-delay="500"
        >
          <template #activator="{ on, attrs }">
            <v-icon
              v-if="deleteEnabled"
              v-bind="attrs"
              @click="handleDelete(item)"
              v-on="on"
            >
              mdi-delete
            </v-icon>
            <v-icon v-if="!deleteEnabled">mdi-delete-off</v-icon>
          </template>
          <span>{{ $t('Delete item') }}</span>
        </v-tooltip>
      </template>
    </v-data-table>
  </v-container>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'

interface IWeaponTableProps {
  weapons: Array<WeaponInfoType>
  deleteEnabled: boolean
}

const emit = defineEmits(['delete'])

const props = defineProps<IWeaponTableProps>()

const headers = [
  { text: 'Make', value: 'make' },
  { text: 'Model', value: 'model' },
  { text: 'Caliper', value: 'caliber' },
  { text: 'Serial Number', value: 'serialNumber' },
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
