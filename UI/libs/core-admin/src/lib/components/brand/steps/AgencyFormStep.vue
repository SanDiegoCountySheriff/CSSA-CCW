<template>
  <v-card :loading="isLoading && isFetching">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Information
        <v-spacer />
        <v-btn
          @click="getFormValues"
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
              v-model="brandStore.getBrand.agencyName"
              :rules="[v => !!v || 'Agency Name is required']"
              :label="$t('Agency Name')"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencySheriffName"
              :rules="[v => !!v || $t('Agency Sheriff Name is required')]"
              :label="$t('Agency Sheriff Name')"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="brandStore.getBrand.chiefOfPoliceName"
              :rules="[v => !!v || $t('Chief of Police name is required')]"
              :label="$t('Chief of Police Name')"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyTelephone"
              :rules="[v => !!v || $t('Agency Telephone is required')]"
              :label="$t('Agency Telephone Number')"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyFax"
              :rules="[v => !!v || $t('Agency Fax is required')]"
              :label="$t('Agency Fax Number')"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Agency Email Address')"
              :rules="[v => !!v || $t('Agency Email Address is required')]"
              v-model="brandStore.getBrand.agencyEmail"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('ORI')"
              :rules="[v => !!v || $t(' ORI is required')]"
              v-model="brandStore.getBrand.ori"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Local Agency Number')"
              :rules="[v => !!v || $t(' Local Agency Number is required')]"
              v-model="brandStore.getBrand.localAgencyNumber"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('Courthouse')"
              :rules="[v => !!v || $t('Courthouse is required')]"
              v-model="brandStore.getBrand.courthouse"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title>Agency Address</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Street')"
              :rules="[v => !!v || $t('Street address is required.')]"
              v-model="brandStore.getBrand.agencyStreetAddress"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('Apt / Building Number')"
              v-model="brandStore.getBrand.agencyAptBuildingNumber"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('City')"
              :rules="[v => !!v || $t('Agency city is required.')]"
              v-model="brandStore.getBrand.agencyCity"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('State')"
              :rules="[v => !!v || $t('Agency state is required.')]"
              v-model="brandStore.getBrand.agencyState"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Agency zip is required.')]"
              v-model="brandStore.getBrand.agencyZip"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('County')"
              :rules="[v => !!v || $t('Agency County is required.')]"
              v-model="brandStore.getBrand.agencyCounty"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title>Agency Shipping Address</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Street')"
              :rules="[v => !!v || $t('Street address is required.')]"
              v-model="brandStore.getBrand.agencyShippingStreetAddress"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('Apt / Building Number')"
              v-model="brandStore.getBrand.agencyShippingAptBuildingNumber"
              required
            >
              <template #append>
                <v-icon
                  medium
                  color="primary"
                  v-if="brandStore.getBrand.agencyShippingAptBuildingNumber"
                >
                  mdi-checkbox-marked-circle
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('City')"
              :rules="[v => !!v || $t('Agency city is required.')]"
              v-model="brandStore.getBrand.agencyShippingCity"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('State')"
              :rules="[v => !!v || $t('Agency state is required.')]"
              v-model="brandStore.getBrand.agencyShippingState"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Agency zip is required.')]"
              v-model="brandStore.getBrand.agencyShippingZip"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('County')"
              :rules="[v => !!v || $t('Agency County is required.')]"
              v-model="brandStore.getBrand.agencyShippingCounty"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title>Live Scan Form</v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Agency Billing Number')"
              :rules="[v => !!v || $t('Agency Billing Number is required')]"
              v-model="brandStore.getBrand.agencyBillingNumber"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('Mail Code')"
              :rules="[v => !!v || $t('Mail Code is required')]"
              v-model="brandStore.getBrand.mailCode"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              outlined
              :label="$t('Contact Name')"
              :rules="[v => !!v || $t('Contact Name is required')]"
              v-model="brandStore.getBrand.contactName"
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              outlined
              :label="$t('Contact Number')"
              :rules="[v => !!v || $t('Contact Number is required')]"
              v-model="brandStore.getBrand.contactNumber"
              required
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'

const brandStore = useBrandStore()
const valid = ref(false)

const {
  isLoading,
  isFetching,
  refetch: queryBrandSettings,
} = useQuery(['setBrandSettings'], brandStore.setBrandSettingApi, {
  enabled: false,
})

async function getFormValues() {
  queryBrandSettings()
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
