<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('Previous Aliases:') }}
      <small class="text-caption grey--text text--darken-1"
        >{{ $t('&nbsp;(the following aliases were submitted for review)') }}
      </small>
    </v-card-title>
    <AliasTable :aliases="permitStore.getPermitDetail.application.aliases" />
    <v-row>
      <v-col
        cols="12"
        md="5"
        sm="3"
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
</script>
