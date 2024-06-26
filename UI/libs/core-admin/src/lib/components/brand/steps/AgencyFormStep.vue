<template>
  <v-card :loading="loading.value">
    <v-form
      ref="form"
      v-model="valid"
      lazy-validation
    >
      <v-card-title>
        Agency Information
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

          <v-col cols="6">
            <v-select
              v-model="brandStore.getBrand.licensingManager"
              :items="adminUserStore.allAdminUsers"
              item-text="name"
              item-value="name"
              label="Licensing Manager"
              outlined
            ></v-select>
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
              v-model="brandStore.getBrand.agencyEmail"
              :label="$t('Agency Email Address')"
              :rules="[v => !!v || $t('Agency Email Address is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.ori"
              :label="$t('ORI')"
              :rules="[v => !!v || $t(' ORI is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.localAgencyNumber"
              :label="$t('Local Agency Number')"
              :rules="[v => !!v || $t(' Local Agency Number is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.courthouse"
              :label="$t('Courthouse')"
              :rules="[v => !!v || $t('Courthouse is required')]"
              outlined
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
              v-model="brandStore.getBrand.agencyStreetAddress"
              :label="$t('Street')"
              :rules="[v => !!v || $t('Street address is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyAptBuildingNumber"
              :label="$t('Apt / Building Number')"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyCity"
              :label="$t('City')"
              :rules="[v => !!v || $t('Agency city is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyState"
              :label="$t('State')"
              :rules="[v => !!v || $t('Agency state is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyZip"
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Agency zip is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyCounty"
              :label="$t('County')"
              :rules="[v => !!v || $t('Agency County is required.')]"
              outlined
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
              v-model="brandStore.getBrand.agencyShippingStreetAddress"
              :label="$t('Street')"
              :rules="[v => !!v || $t('Street address is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyShippingAptBuildingNumber"
              :label="$t('Apt / Building Number')"
              outlined
              required
            >
              <template #append>
                <v-icon
                  v-if="brandStore.getBrand.agencyShippingAptBuildingNumber"
                  color="primary"
                  medium
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
              v-model="brandStore.getBrand.agencyShippingCity"
              :label="$t('City')"
              :rules="[v => !!v || $t('Agency city is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyShippingState"
              :label="$t('State')"
              :rules="[v => !!v || $t('Agency state is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyShippingZip"
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Agency zip is required.')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.agencyShippingCounty"
              :label="$t('County')"
              :rules="[v => !!v || $t('Agency County is required.')]"
              outlined
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
              v-model="brandStore.getBrand.agencyBillingNumber"
              :label="$t('Agency Billing Number')"
              :rules="[v => !!v || $t('Agency Billing Number is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.mailCode"
              :label="$t('Mail Code')"
              :rules="[v => !!v || $t('Mail Code is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.contactName"
              :label="$t('Contact Name')"
              :rules="[v => !!v || $t('Contact Name is required')]"
              outlined
              required
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="brandStore.getBrand.contactNumber"
              :label="$t('Contact Number')"
              :rules="[v => !!v || $t('Contact Number is required')]"
              outlined
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
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useTanstack } from '@shared-ui/composables/useTanstack'

const brandStore = useBrandStore()
const valid = ref(false)
const { loading, setBrandSettings } = useTanstack()
const adminUserStore = useAdminUserStore()

async function save() {
  setBrandSettings()
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
