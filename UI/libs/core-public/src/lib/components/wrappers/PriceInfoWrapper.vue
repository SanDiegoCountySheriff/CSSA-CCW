<template>
  <div>
    <v-card v-if="state.loading">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="card"
      >
      </v-skeleton-loader>
    </v-card>
    <v-card v-else>
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
  </div>
</template>

<script setup lang="ts">
import { i18n } from '@shared-ui/plugins'
import { onBeforeRouteUpdate } from 'vue-router/composables'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { onMounted, reactive } from 'vue'

const brandStore = useBrandStore()
const state = reactive({
  loading: true,
  headers: [
    { text: i18n.t('Permit'), value: 'type' },
    { text: i18n.t('Standard 2 year'), value: 'standard' },
    { text: i18n.t('Judicial 3 year'), value: 'judicial' },
    { text: i18n.t('Reserve 4 year'), value: 'reserve' },
  ],
  items: [] as unknown,
})

onBeforeRouteUpdate(async () => {
  const brand = brandStore.getBrand

  state.items = [
    {
      type: i18n.t('Initial Fee'),
      standard: `$ ${brand.cost.new.standard}`,
      judicial: `$ ${brand.cost.new.judicial}`,
      reserve: `$ ${brand.cost.new.reserve}`,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: `$ ${brand.cost.issuance}`,
      judicial: `$ ${brand.cost.issuance}`,
      reserve: `$ ${brand.cost.issuance}`,
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: `$ ${brand.cost.renew.standard}`,
      judicial: `$ ${brand.cost.renew.judicial}`,
      reserve: `$ ${brand.cost.renew.reserve}`,
    },
    {
      type: i18n.t('Duplicate/Modification Fee'),
      standard: `$ ${brand.cost.modify}`,
      judicial: `$ ${brand.cost.modify}`,
      reserve: `$ ${brand.cost.modify}`,
    },
  ]
  state.loading = false
})

onMounted(() => {
  const brand = brandStore.getBrand

  state.items = [
    {
      type: i18n.t('Initial Fee'),
      standard: `$ ${brand.cost.new.standard}`,
      judicial: `$ ${brand.cost.new.judicial}`,
      reserve: `$ ${brand.cost.new.reserve}`,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: `$ ${brand.cost.issuance}`,
      judicial: `$ ${brand.cost.issuance}`,
      reserve: `$ ${brand.cost.issuance}`,
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: `$ ${brand.cost.renew.standard}`,
      judicial: `$ ${brand.cost.renew.judicial}`,
      reserve: `$ ${brand.cost.renew.reserve}`,
    },
    {
      type: i18n.t('Duplicate/Modification Fee'),
      standard: `$ ${brand.cost.modify}`,
      judicial: `$ ${brand.cost.modify}`,
      reserve: `$ ${brand.cost.modify}`,
    },
  ]
  state.loading = false
})
</script>
