<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Previous Aliases') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="false"
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <AliasDialog @save-alias="getAliasFromDialog" />
      <AliasTable
        :aliases="permitStore.getPermitDetail.application.aliases"
        :enable-delete="true"
        @delete="deleteAlias"
      />
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue'
import AliasTable from '@shared-ui/components/tables/AliasTable.vue'
import SaveButton from './SaveButton.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()

function getAliasFromDialog(alias) {
  permitStore.getPermitDetail.application.aliases.unshift(alias)
}

function deleteAlias(index) {
  permitStore.getPermitDetail.application.aliases.splice(index, 1)
}

function handleSave() {
  emit('on-save', 'Alias Information')
}
</script>
