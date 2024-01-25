<template>
  <v-card :loading="loading.value">
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        Agency Configuration Settings
        <v-spacer />

        <v-btn
          :disabled="!valid"
          @click="save"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="brandStore.brand.expiredApplicationRenewalPeriod"
              label="Expired application renewal grace period, days"
              hint="Expired applications will have this many days after expiration to submit their renewal"
              :rules="[
                v => v !== null || 'An expiration renewal period is required',
              ]"
              type="number"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            sm="6"
            cols="12"
          >
            <v-text-field
              v-model.number="
                brandStore.brand.archivedApplicationRetentionPeriod
              "
              label="Archived application retention period, years"
              hint="Applications that are inactive will be retained for this many years"
              :rules="[
                v =>
                  v !== null ||
                  'An archived application retention period is required',
              ]"
              type="number"
              color="primary"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useTanstack } from '@shared-ui/composables/useTanstack'

const { loading, setBrandSettings } = useTanstack()

const brandStore = useBrandStore()
const valid = ref(false)

function save() {
  setBrandSettings()
}
</script>
