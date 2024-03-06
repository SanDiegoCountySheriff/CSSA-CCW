<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :color="$vuetify.theme.dark ? 'white' : 'primary'"
        text
        :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
        :x-large="$vuetify.breakpoint.lgAndUp"
        :small="$vuetify.breakpoint.smAndDown"
        v-bind="attrs"
        v-on="on"
      >
        <v-container>
          <v-row>
            <v-col>
              <v-icon x-large> mdi-currency-usd </v-icon>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              {{ $t('Pricing') }}
            </v-col>
          </v-row>
        </v-container>
      </v-btn>
    </template>

    <v-card>
      <v-card-title>CCW Pricing</v-card-title>

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

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          color="primary"
          text
        >
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { i18n } from '@shared-ui/plugins'
import { onBeforeRouteUpdate } from 'vue-router/composables'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { onMounted, reactive, ref } from 'vue'

const brandStore = useBrandStore()
const dialog = ref(false)
const state = reactive({
  headers: [
    { text: i18n.t('Permit'), value: 'type' },
    { text: i18n.t('Standard 2 year'), value: 'standard' },
    { text: i18n.t('Judicial 3 year'), value: 'judicial' },
    { text: i18n.t('Reserve 4 year'), value: 'reserve' },
    ...(brandStore.getBrand.employmentLicense
      ? [{ text: i18n.t('Employment 90 Day'), value: 'employment' }]
      : []),
  ],
  items: [] as unknown,
})

onBeforeRouteUpdate(async () => {
  const brand = brandStore.getBrand

  state.items = [
    {
      type: i18n.t('Initial Fee'),
      standard: `$ ${brand.cost.new.standard.toFixed(2)}`,
      judicial: `$ ${brand.cost.new.judicial.toFixed(2)}`,
      reserve: `$ ${brand.cost.new.reserve.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.new.employment.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: `$ ${brand.cost.issuance.toFixed(2)}`,
      judicial: `$ ${brand.cost.issuance.toFixed(2)}`,
      reserve: `$ ${brand.cost.issuance.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.issuance.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: `$ ${brand.cost.renew.standard.toFixed(2)}`,
      judicial: `$ ${brand.cost.renew.judicial.toFixed(2)}`,
      reserve: `$ ${brand.cost.renew.reserve.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.renew.employment.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t('Duplicate/Modification Fee'),
      standard: `$ ${brand.cost.modify.toFixed(2)}`,
      judicial: `$ ${brand.cost.modify.toFixed(2)}`,
      reserve: `$ ${brand.cost.modify.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.modify.toFixed(2)}`
        : null,
    },
  ]
})

onMounted(() => {
  const brand = brandStore.getBrand

  state.items = [
    {
      type: i18n.t('Initial Fee'),
      standard: `$ ${brand.cost.new.standard.toFixed(2)}`,
      judicial: `$ ${brand.cost.new.judicial.toFixed(2)}`,
      reserve: `$ ${brand.cost.new.reserve.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.new.employment.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t(
        'Issuance Fee: ( Paid upon the approval of the application )'
      ),
      standard: `$ ${brand.cost.issuance.toFixed(2)}`,
      judicial: `$ ${brand.cost.issuance.toFixed(2)}`,
      reserve: `$ ${brand.cost.issuance.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.issuance.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t('Renewal Fee'),
      standard: `$ ${brand.cost.renew.standard.toFixed(2)}`,
      judicial: `$ ${brand.cost.renew.judicial.toFixed(2)}`,
      reserve: `$ ${brand.cost.renew.reserve.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.renew.employment.toFixed(2)}`
        : null,
    },
    {
      type: i18n.t('Duplicate/Modification Fee'),
      standard: `$ ${brand.cost.modify.toFixed(2)}`,
      judicial: `$ ${brand.cost.modify.toFixed(2)}`,
      reserve: `$ ${brand.cost.modify.toFixed(2)}`,
      employment: brandStore.getBrand.employmentLicense
        ? `$ ${brand.cost.modify.toFixed(2)}`
        : null,
    },
  ]
})
</script>
