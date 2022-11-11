<template>
  <v-card>
    <v-card-title>
      {{ $t('Pricing') }}
    </v-card-title>
    <v-card-text>
      <v-data-table
        :headers="state.headers"
        :items="state.items"
        :disable-filtering="true"
        :disable-pagination="true"
        :disable-sort="true"
        :hide-default-footer="true"
      >
      </v-data-table>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { i18n } from '@shared-ui/plugins';
import { reactive } from 'vue';
import { useBrandStore } from '@core-public/stores/brandStore';

const brandStore = useBrandStore();
const state = reactive({
  headers: [
    { text: i18n.t('Permit'), value: 'type' },
    { text: i18n.t('Standard 2 year'), value: 'standard' },
    { text: i18n.t('Judicial 3 year'), value: 'judicial' },
    { text: i18n.t('Reserve 4 year'), value: 'reserve' },
  ],
  items: [
    {
      type: i18n.t('Initial Fee'),
      standard: `$ ${brandStore.brand.cost.new.standard}`,
      judicial: `$ ${brandStore.brand.cost.new.judicial}`,
      reserve: `$ ${brandStore.brand.cost.new.reserve}`,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: `$ ${brandStore.brand.cost.issuance}`,
      judicial: `$ ${brandStore.brand.cost.issuance}`,
      reserve: `$ ${brandStore.brand.cost.issuance}`,
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: `$ ${brandStore.brand.cost.renew.standard}`,
      judicial: `$ ${brandStore.brand.cost.renew.judicial}`,
      reserve: `$ ${brandStore.brand.cost.renew.reserve}`,
    },
    {
      type: i18n.t('Duplicate/ModificationFee'),
      standard: `$ ${brandStore.brand.cost.modify}`,
      judicial: `$ ${brandStore.brand.cost.modify}`,
      reserve: `$ ${brandStore.brand.cost.modify}`,
    },
  ],
});
</script>

<style lang="scss" scoped></style>
