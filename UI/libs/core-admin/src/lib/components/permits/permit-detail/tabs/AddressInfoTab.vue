<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Address Information') }}
      <v-spacer></v-spacer>
      <SaveButton
        :disabled="!isValid"
        @on-save="handleSave"
      />
    </v-card-title>

    <v-card-text>
      <v-form
        ref="addressForm"
        v-model="addressFormValid"
      >
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress
                  .addressLine1
              "
              :label="$t('Address line 1')"
              :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
              maxlength="150"
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress
                      .addressLine1
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress
                  .addressLine2
              "
              :label="$t('Address line 2')"
              maxlength="150"
              outlined
              dense
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress.country
              "
              :label="$t('Country')"
              :rules="[v => !!v || 'Country cannot be blank']"
              autocomplete="nope"
              outlined
              readonly
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress
                      .country
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-autocomplete
              v-if="
                permitStore.getPermitDetail.application.currentAddress
                  .country === 'United States'
              "
              :items="states"
              :label="$t('State')"
              :rules="[v => !!v || $t('State cannot be blank')]"
              autocomplete="nope"
              v-model="
                permitStore.getPermitDetail.application.currentAddress.state
              "
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress
                      .state
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-autocomplete>
          </v-col>
        </v-row>
        <v-row>
          <v-col>
            <v-text-field
              :label="$t('City')"
              maxlength="100"
              :rules="[v => !!v || $t('City cannot be blank')]"
              v-model="
                permitStore.getPermitDetail.application.currentAddress.city
              "
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress.city
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              maxlength="100"
              :label="$t('County')"
              :rules="[v => !!v || $t('County cannot be blank')]"
              v-model="
                permitStore.getPermitDetail.application.currentAddress.county
              "
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress
                      .county
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              maxlength="10"
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Zip cannot be blank')]"
              v-model="
                permitStore.getPermitDetail.application.currentAddress.zip
              "
              outlined
              dense
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.currentAddress.zip
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>

        <v-checkbox
          v-model="permitStore.getPermitDetail.application.differentMailing"
          :label="$t('Has Different Mailing address')"
        />
      </v-form>
    </v-card-text>

    <template v-if="permitStore.getPermitDetail.application.differentMailing">
      <v-card-title>
        {{ $t('Different Mailing Address') }}
      </v-card-title>

      <v-card-text>
        <v-form
          ref="mailingAddressForm"
          v-model="mailingAddressFormValid"
        >
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                :label="$t('Address line 1')"
                :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress
                    .addressLine1
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .addressLine1
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                :label="$t('Address line 2')"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress
                    .addressLine2
                "
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-combobox
                :items="countries"
                :label="$t('Country')"
                :rules="[v => !!v || $t('Country cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.country
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .country
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-combobox>
            </v-col>
            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application.mailingAddress
                    .country === 'United States'
                "
                :items="states"
                :label="$t('State')"
                :rules="[v => !!v || $t('State cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.state
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .state
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-autocomplete>

              <v-text-field
                v-else
                :items="states"
                :label="$t('State')"
                :rules="[v => !!v || $t('State cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.state
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .state
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="100"
                :label="$t('City')"
                :rules="[v => !!v || $t(' City cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.city
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .city
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                maxlength="100"
                :label="$t('County')"
                :rules="[v => !!v || $t('County cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.county
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .county
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                maxlength="10"
                :label="$t('Zip')"
                :rules="[v => !!v || $t('Zip cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.zip
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.mailingAddress
                        .zip
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </template>

    <v-card-text>
      <v-checkbox
        id="different-spouse"
        :label="$t('Has Different Spouse address')"
        v-model="permitStore.getPermitDetail.application.differentSpouseAddress"
      />
    </v-card-text>

    <template
      v-if="permitStore.getPermitDetail.application.differentSpouseAddress"
    >
      <v-card-title>
        {{ $t('Different Spouse Address') }}
      </v-card-title>

      <v-card-text>
        <v-form
          ref="spouseAddressForm"
          v-model="spouseAddressFormValid"
        >
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                :label="$t('Spouse address line 1')"
                :rules="[
                  v => !!v || $t('Spouse address line 1 cannot be blank'),
                ]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.addressLine1
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.addressLine1
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                :label="$t('Spouse address line 2')"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.addressLine2
                "
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-combobox
                maxlength="25"
                :items="countries"
                :label="$t('Spouse\'s Country')"
                :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.country
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.country
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-combobox>
            </v-col>
            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.country === 'United States'
                "
                :items="states"
                :label="$t('Spouse\'s State')"
                :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.state
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.state
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-autocomplete>
              <v-text-field
                v-else
                :items="states"
                :label="$t('Spouse\'s State')"
                :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.state
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.state
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="100"
                :label="$t('Spouse\'s City')"
                :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.city
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.city
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                maxlength="100"
                :label="$t('Spouse\'s County')"
                :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.county
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.county
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                maxlength="10"
                :label="$t('Spouse\'s Zip')"
                :rules="[v => !!v || $t('Spouse\'s Zip cannot be blank')]"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.zip
                "
                outlined
                dense
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application
                        .spouseAddressInformation.zip
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </template>

    <v-card-title>
      {{ $t('Previous Address') }}
    </v-card-title>

    <v-card-text>
      <PreviousAddressDialog
        @get-previous-address-from-dialog="getPreviousAddressFromDialog"
      />
      <AddressTable
        :addresses="permitStore.getPermitDetail.application.previousAddresses"
        :enable-delete="true"
        @delete="deleteAddress"
      />
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes'
import AddressTable from '@shared-ui/components/tables/AddressTable.vue'
import PreviousAddressDialog from '@shared-ui/components/dialogs/PreviousAddressDialog.vue'
import SaveButton from './SaveButton.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, ref } from 'vue'
import { countries, states } from '@shared-utils/lists/defaultConstants'

const permitStore = usePermitsStore()
const addressFormValid = ref(false)
const mailingAddressFormValid = ref(false)
const spouseAddressFormValid = ref(false)
const emit = defineEmits(['on-save'])

function getPreviousAddressFromDialog(address: AddressInfoType) {
  permitStore.getPermitDetail.application.previousAddresses.push(address)
}

function deleteAddress(index) {
  permitStore.getPermitDetail.application.previousAddresses.splice(index, 1)
}

function handleSave() {
  emit('on-save', 'Address Information')
}

const isValid = computed(() => {
  if (
    permitStore.getPermitDetail.application.differentMailing &&
    permitStore.getPermitDetail.application.differentSpouseAddress
  ) {
    return (
      addressFormValid.value &&
      mailingAddressFormValid.value &&
      spouseAddressFormValid.value
    )
  }

  if (permitStore.getPermitDetail.application.differentMailing) {
    return addressFormValid.value && mailingAddressFormValid.value
  }

  if (permitStore.getPermitDetail.application.differentSpouseAddress) {
    return addressFormValid.value && spouseAddressFormValid.value
  }

  return addressFormValid.value
})
</script>
