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
            :label="$t('Refresh Token Time (in minutes)')"
            :rules="[v => !!v || $t('Refresh token time is required')]"
            :hint="$t('Add token refresh time in minutes ')"
            v-model="brandStore.getBrand.refreshTokenTime"
            type="number"
            color="blue1"
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
                v-if="!brandStore.getBrand.refreshTokenTime"
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
            :label="$t('Payment URL')"
            :rules="[
              v => !!v || $t('Payment URL is required'),
              v => isURL(v) || $t('Payment URL is not valid'),
            ]"
            :hint="$t('Add your payment URL for processing payment')"
            v-model="brandStore.getBrand.paymentURL"
            color="blue1"
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
                v-if="!brandStore.getBrand.paymentURL"
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
import { ref } from 'vue';
import { useBrandStore } from '@core-admin/stores/brandStore';
import { useQuery } from '@tanstack/vue-query';

interface IAgencyFormStepProps {
  handleNextStep: () => void;
  handleBackStep: () => void;
  handleResetStep: () => void;
}

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
  handleBackStep: () => null,
  handleResetStep: () => null,
});

const brandStore = useBrandStore();
const valid = ref(false);

const {
  isLoading,
  isFetching,
  refetch: queryBrandSettings,
} = useQuery(['setBrandSettings'], brandStore.setBrandSettingApi, {
  enabled: false,
  onSuccess: () => {
    props.handleNextStep();
  },
});

function isURL(str) {
  let url;

  try {
    // eslint-disable-next-line node/no-unsupported-features/node-builtins
    url = new URL(str);
  } catch (_) {
    return false;
  }

  return url.protocol === 'http:' || url.protocol === 'https:';
}

async function getFormValues() {
  queryBrandSettings();
}
</script>
