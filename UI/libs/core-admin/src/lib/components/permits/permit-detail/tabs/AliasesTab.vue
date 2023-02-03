<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Previous Aliases:') }}
      <small class="text-caption grey--text text--darken-1"
        >{{ $t('&nbsp;(the following aliases were submitted for review)') }}
      </small>
    </v-card-title>
    <AliasTable
      :aliases="permitStore.getPermitDetail.application.aliases"
      :enable-delete="true"
      @delete="deleteAlias"
    />
    <v-row>
      <v-col
        cols="12"
        md="5"
        sm="12"
        offset-md="8"
      >
        <AliasDialog :save-alias="getAliasFromDialog" />
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();

function getAliasFromDialog(alias) {
  permitStore.getPermitDetail.application.aliases.unshift(alias);
}

function deleteAlias(index) {
  permitStore.getPermitDetail.application.aliases.splice(index, 1);
}
</script>
