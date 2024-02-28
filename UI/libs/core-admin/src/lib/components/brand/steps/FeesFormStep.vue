<template>
  <v-card :loading="loading.value">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Fees
        <v-spacer />

        <v-btn
          :disabled="!valid"
          @click="save"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.new.standard"
              :label="$t('New Standard Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.new.judicial"
              :label="$t('New Judicial Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.new.reserve"
              :label="$t('New Reserve Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.renew.standard"
              :label="$t('Renew Standard Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.renew.judicial"
              :label="$t('Renew Judicial Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>

          <v-col
            cols="12"
            md="6"
          >
            <v-text-field
              v-model="brandStore.getBrand.cost.renew.reserve"
              :label="$t('Renew Reserve Cost')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.creditFee"
              :label="$t('Credit Fee')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.convenienceFee"
              :label="$t('Convenience Fee')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.modify"
              :label="$t('Modify Fee')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.cost.issuance"
              :label="$t('Issuance Fee')"
              :rules="feeRules"
              type="number"
              color="primary"
              prefix="$"
              required
              outlined
            />
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useTanstack } from '@shared-ui/composables/useTanstack'
import { computed, ref } from 'vue'

const brandStore = useBrandStore()
const valid = ref(false)
const feeRules = computed(() => {
  return [v => Boolean(v) || 'Fee is required']
})

const { loading, setBrandSettings } = useTanstack()

function save() {
  setBrandSettings()
}
</script>
