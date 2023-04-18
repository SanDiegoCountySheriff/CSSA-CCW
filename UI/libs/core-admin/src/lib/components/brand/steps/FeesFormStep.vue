<template>
  <v-card
    color="lighten-1"
    class="mb-8 ml-2 mr-2 mt-6 elevation-3"
    height="100%"
  >
    <v-progress-linear
      :active="isLoading && isFetching"
      :indeterminate="isLoading && isFetching"
      absolute
      bottom
      color="primary"
    >
    </v-progress-linear>
    <v-form
      ref="form"
      class="ml-4"
      v-model="valid"
      lazy-validation
    >
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('New Standard Cost')"
            :rules="[v => !!v || $t('New Standard Cost is required')]"
            :hint="$t('Add new standard cost fees for permit ')"
            v-model="brandStore.getBrand.cost.new.standard"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.new.standard"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('New Judicial Cost')"
            :rules="[v => !!v || $t('New Judicial Cost is required')]"
            :hint="$t('Add new judicial cost fees for permit ')"
            v-model="brandStore.getBrand.cost.new.judicial"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.new.judicial"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('New Reserve Cost')"
            :rules="[v => !!v || $t('New Reserve Cost is required')]"
            :hint="$t('Add new reserve cost fees for permit ')"
            v-model="brandStore.getBrand.cost.new.reserve"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.new.reserve"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Renew Standard Cost')"
            :rules="[v => !!v || $t('Renew Standard Cost is required')]"
            :hint="$t('Add renew standard cost fees for permit ')"
            v-model="brandStore.getBrand.cost.renew.standard"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.renew.standard"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Renew Judicial Cost')"
            :rules="[v => !!v || $t('New Judicial Cost is required')]"
            :hint="$t('Add renew judicial cost fees for permit ')"
            v-model="brandStore.getBrand.cost.renew.judicial"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.renew.judicial"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Renew Reserve Cost')"
            :rules="[v => !!v || $t('Renew Reserve Cost is required')]"
            :hint="$t('Add renew reserve cost fees for permit ')"
            v-model="brandStore.getBrand.cost.renew.reserve"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.renew.reserve"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Credit Fee')"
            :rules="[v => !!v || $t('Credit Fee is required')]"
            :hint="$t('Add credit fee for permit ')"
            v-model="brandStore.getBrand.cost.creditFee"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.creditFee"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Convenience Fee')"
            :rules="[v => !!v || $t('Convenience Fee is required')]"
            :hint="$t('Add Convenience Fee for permit ')"
            v-model="brandStore.getBrand.cost.convenienceFee"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.convenienceFee"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Modify Fee')"
            :rules="[v => !!v || $t('Modify Fee is required')]"
            :hint="$t('Add Modify Fee for permit ')"
            v-model="brandStore.getBrand.cost.modify"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.modify"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Issuance Fee')"
            :rules="[v => !!v || $t('Issuance Fee is required')]"
            :hint="$t('Add Issuance Fee for permit ')"
            v-model="brandStore.getBrand.cost.issuance"
            placeholder="XX.XX"
            type="number"
            color="blue1"
            prefix="$"
            required
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.cost.issuance"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="primary"
              >
                mdi-checkbox-marked-circle
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row justify="space-between">
        <v-col
          cols="12"
          sm="6"
        >
          <v-btn @click="handleResetStep">
            {{ $t('Cancel') }}
          </v-btn>
          <v-btn @click="props.handleBackStep">
            {{ $t('Back') }}
          </v-btn>
        </v-col>
        <v-col
          cols="12"
          sm="6"
        >
          <v-btn
            color="primary"
            :disabled="!valid"
            @click="getFormValues"
          >
            {{ $t('Publish') }}
          </v-btn>
        </v-col>
      </v-row>
      <v-spacer></v-spacer>
    </v-form>
  </v-card>
</template>
<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'

interface IAgencyFormStepProps {
  handleNextStep: () => void
  handleBackStep: () => void
  handleResetStep: () => void
}

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
})

const brandStore = useBrandStore()
const valid = ref(false)

const {
  isLoading,
  isFetching,
  refetch: queryBrandSettings,
} = useQuery(['setBrandSettings'], brandStore.setBrandSettingApi, {
  enabled: false,
  onSuccess: () => {
    props.handleNextStep()
  },
})

async function getFormValues() {
  queryBrandSettings()
}
</script>
