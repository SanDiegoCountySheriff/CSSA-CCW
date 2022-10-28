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
      standard: `$ ${brandStore.brand.standardCost}`,
      judicial: `$ ${brandStore.brand.standardCost}`,
      reserve: `$ ${brandStore.brand.reserveCost}`,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: '$ 80.00',
      judicial: '$ 80.00',
      reserve: '$ 80.00',
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: '$ 77.00',
      judicial: '$ 99.00',
      reserve: '$ 121.00',
    },
    {
      type: i18n.t('Duplicate/ModificationFee'),
      standard: '$ 10.00',
      judicial: '$ 10.00',
      reserve: '$ 10.00',
    },
  ],
});
</script>

<style lang="scss" scoped></style>
