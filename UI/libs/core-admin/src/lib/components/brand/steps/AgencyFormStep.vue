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
            :label="$t('ORI')"
            :rules="[v => !!v || $t(' ORI is required')]"
            :hint="$t('ORI')"
            color="blue1"
            v-model="brandStore.getBrand.ori"
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
                v-if="!brandStore.getBrand.ori"
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
            :label="$t('Local Agency Number')"
            :rules="[v => !!v || $t(' Local Agency Number is required')]"
            :hint="$t('Local Agency Number')"
            color="blue1"
            v-model="brandStore.getBrand.localAgencyNumber"
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
                v-if="!brandStore.getBrand.localAgencyNumber"
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
            :label="$t('Courthouse')"
            :rules="[v => !!v || $t('Courthouse is required')]"
            :hint="$t('Courthouse')"
            color="blue1"
            v-model="brandStore.getBrand.courthouse"
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
                v-if="!brandStore.getBrand.courthouse"
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
        <v-divider></v-divider>
      </v-row>
      <v-row>
        <p>Agency Address</p>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Street')"
            :rules="[v => !!v || $t('Street address is required.')]"
            :hint="$t('Street address')"
            color="blue1"
            v-model="brandStore.getBrand.agencyStreetAddress"
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
                v-if="!brandStore.getBrand.agencyStreetAddress"
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
            :label="$t('Apt / Building Number')"
            :hint="$t('Apt / Building Number')"
            color="blue1"
            v-model="brandStore.getBrand.agencyAptBuildingNumber"
            required
          >
            <template #append>
              <v-icon
                medium
                color="primary"
                v-if="brandStore.getBrand.agencyAptBuildingNumber"
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
            :label="$t('City')"
            :rules="[v => !!v || $t('Agency city is required.')]"
            :hint="$t('City')"
            color="blue1"
            v-model="brandStore.getBrand.agencyCity"
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
                v-if="!brandStore.getBrand.agencyCity"
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
            :label="$t('State')"
            :rules="[v => !!v || $t('Agency state is required.')]"
            :hint="$t('State')"
            color="blue1"
            v-model="brandStore.getBrand.agencyState"
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
                v-if="!brandStore.getBrand.agencyState"
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
            :label="$t('Zip')"
            :rules="[v => !!v || $t('Agency zip is required.')]"
            :hint="$t('Zip Code')"
            color="blue1"
            v-model="brandStore.getBrand.agencyZip"
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
                v-if="!brandStore.getBrand.agencyZip"
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
            :label="$t('County')"
            :rules="[v => !!v || $t('Agency County is required.')]"
            :hint="$t('County')"
            color="blue1"
            v-model="brandStore.getBrand.agencyCounty"
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
                v-if="!brandStore.getBrand.agencyCounty"
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
        <v-divider></v-divider>
      </v-row>
      <v-row>
        <p>Agency Shipping Address</p>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Street')"
            :rules="[v => !!v || $t('Street address is required.')]"
            :hint="$t('Street address')"
            color="blue1"
            v-model="brandStore.getBrand.agencyShippingStreetAddress"
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
                v-if="!brandStore.getBrand.agencyShippingStreetAddress"
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
            :label="$t('Apt / Building Number')"
            :hint="$t('Apt / Building Number')"
            color="blue1"
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
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('City')"
            :rules="[v => !!v || $t('Agency city is required.')]"
            :hint="$t('City')"
            color="blue1"
            v-model="brandStore.getBrand.agencyShippingCity"
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
                v-if="!brandStore.getBrand.agencyShippingCity"
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
            :label="$t('State')"
            :rules="[v => !!v || $t('Agency state is required.')]"
            :hint="$t('State')"
            color="blue1"
            v-model="brandStore.getBrand.agencyShippingState"
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
                v-if="!brandStore.getBrand.agencyShippingState"
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
            :label="$t('Zip')"
            :rules="[v => !!v || $t('Agency zip is required.')]"
            :hint="$t('Zip Code')"
            color="blue1"
            v-model="brandStore.getBrand.agencyShippingZip"
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
                v-if="!brandStore.getBrand.agencyShippingZip"
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
            :label="$t('County')"
            :rules="[v => !!v || $t('Agency County is required.')]"
            :hint="$t('County')"
            color="blue1"
            v-model="brandStore.getBrand.agencyShippingCounty"
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
                v-if="!brandStore.getBrand.agencyShippingCounty"
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
        <v-divider></v-divider>
      </v-row>
      <v-row>
        <p>Live Scan Form</p>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="6"
        >
          <v-text-field
            dense
            filled
            :label="$t('Agency Billing Number')"
            :rules="[v => !!v || $t('Agency Billing Number is required')]"
            :hint="$t('Billing Number')"
            color="blue1"
            v-model="brandStore.getBrand.agencyBillingNumber"
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
                v-if="!brandStore.getBrand.agencyBillingNumber"
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
            :label="$t('Mail Code')"
            :rules="[v => !!v || $t('Mail Code is required')]"
            :hint="$t('Mail Code')"
            color="blue1"
            v-model="brandStore.getBrand.mailCode"
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
                v-if="!brandStore.getBrand.mailCode"
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
            :label="$t('Contact Name')"
            :rules="[v => !!v || $t('Contact Name is required')]"
            :hint="$t('Contact Name')"
            color="blue1"
            v-model="brandStore.getBrand.contactName"
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
                v-if="!brandStore.getBrand.contactName"
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
            :label="$t('Contact Number')"
            :rules="[v => !!v || $t('Contact Number is required')]"
            :hint="$t('Contact Number')"
            color="blue1"
            v-model="brandStore.getBrand.contactNumber"
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
                v-if="!brandStore.getBrand.contactNumber"
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
  handleResetStep: () => void
}

const props = withDefaults(defineProps<IAgencyFormStepProps>(), {
  handleNextStep: () => null,
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
