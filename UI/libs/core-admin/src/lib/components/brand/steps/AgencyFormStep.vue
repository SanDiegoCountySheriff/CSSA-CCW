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
      color="accent"
    >
    </v-progress-linear>
    <v-form
      ref="form"
      v-model="valid"
      class="ml-4"
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
            :label="$t('Agency Name')"
            :rules="[v => !!v || 'Agency Name is required']"
            :hint="$t('Enter your Agency name')"
            color="blue1"
            v-model="brandStore.getBrand.agencyName"
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
                v-if="!brandStore.getBrand.agencyName"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Agency Sheriff Name')"
            :rules="[v => !!v || $t('Agency Sheriff Name is required')]"
            :hint="$t('Enter your Sheriff name')"
            color="blue1"
            v-model="brandStore.getBrand.agencySheriffName"
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
                v-if="!brandStore.getBrand.agencySheriffName"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Chief of Police Name')"
            :rules="[v => !!v || $t('Chief of Police name is required')]"
            :hint="$t('Optionally, Enter your Chief of Police name')"
            color="blue1"
            v-model="brandStore.getBrand.chiefOfPoliceName"
            class="pl-3"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
                v-icon
              >
              </v-icon>
            </template>
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="!brandStore.getBrand.chiefOfPoliceName"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Agency Address')"
            :rules="[v => !!v || $t('Agency Address is required')]"
            :hint="$t('Enter Agency address')"
            color="blue1"
            v-model="brandStore.getBrand.agencyAddress"
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
                v-if="!brandStore.getBrand.agencyAddress"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Agency Telephone Number')"
            :rules="[v => !!v || $t('Agency Telephone is required')]"
            :hint="$t('Enter Agency Telephone number')"
            color="blue1"
            v-model="brandStore.getBrand.agencyTelephone"
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
                v-if="!brandStore.getBrand.agencyTelephone"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Agency Fax Number')"
            :rules="[v => !!v || $t('Agency Fax is required')]"
            :hint="$t('Enter Agency Fax number')"
            color="blue1"
            v-model="brandStore.getBrand.agencyFax"
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
                v-if="!brandStore.getBrand.agencyFax"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
            :label="$t('Agency Email Address')"
            :rules="[v => !!v || $t('Agency Email Address is required')]"
            :hint="$t('Enter Aagency email')"
            color="blue1"
            v-model="brandStore.getBrand.agencyEmail"
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
                v-if="!brandStore.getBrand.agencyEmail"
              >
                mdi-alert-octagon
              </v-icon>
              <v-icon
                v-else
                medium
                color="accent"
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
          justify="space-between"
        >
          <v-btn @click="handleResetStep">
            {{ $t('Cancel') }}
          </v-btn>
        </v-col>
        <v-col
          cols="12"
          sm="6"
        >
          <v-btn
            color="accent"
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
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useQuery } from '@tanstack/vue-query';

interface IAgencyFormStepProps {
  handleNextStep: () => void;
  handleResetStep: () => void;
}

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
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

async function getFormValues() {
  queryBrandSettings();
}
</script>
