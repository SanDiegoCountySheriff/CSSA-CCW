<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title>
      {{ $t('Address Information') }}

      <v-spacer></v-spacer>

      <SaveButton
        :disabled="!isValid || readonly"
        @on-save="handleSave"
      />
    </v-card-title>

    <template
      v-if="
        permitStore.getPermitDetail.application.modifiedAddressComplete !== null
      "
    >
      <v-row>
        <v-col
          cols="12"
          class="pr-7"
        >
          <v-alert
            class="ml-4"
            border="left"
            color="blue"
            text
            type="info"
          >
            Please review the following address modification(s).
          </v-alert>
        </v-col>
      </v-row>

      <v-card-text>
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.modifiedAddress
                  .streetAddress
              "
              :readonly="readonly"
              label="Modified Street Address"
              outlined
              dense
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.modifiedAddress.city
              "
              :readonly="readonly"
              label="Modified City"
              outlined
              dense
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.modifiedAddress.zip
              "
              :readonly="readonly"
              label="Modified Zip Code"
              outlined
              dense
            ></v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.modifiedAddress.county
              "
              :readonly="readonly"
              label="Modified County"
              outlined
              dense
            ></v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="3">
            <v-btn
              @click="handleOpenPdf"
              :disabled="readonly"
              color="primary"
              outlined
            >
              <v-icon left>mdi-file-document-check</v-icon>
              Check Documents
            </v-btn>
          </v-col>

          <v-col
            cols="3"
            class="mb-5"
            offset-md="6"
          >
            <v-btn
              v-if="
                !permitStore.getPermitDetail.application.modifiedAddressComplete
              "
              :disabled="readonly"
              @click="onApproveAddressChange"
              color="primary"
              style="float: right"
            >
              <v-icon
                left
                color="green"
              >
                mdi-check
              </v-icon>
              Approve
            </v-btn>

            <v-btn
              v-if="
                permitStore.getPermitDetail.application.modifiedAddressComplete
              "
              :disabled="readonly"
              @click="onUndoApproveAddressChange"
              color="primary"
              style="float: right"
            >
              <v-icon left>mdi-undo</v-icon>
              Undo Approve
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </template>

    <v-divider></v-divider>

    <v-card-text>
      <v-row>
        <v-col></v-col>
      </v-row>
      <v-form
        ref="addressForm"
        v-model="addressFormValid"
      >
        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress
                  .streetAddress
              "
              :readonly="readonly"
              :label="$t('Street Address')"
              :rules="[v => !!v || $t('Street address cannot be blank')]"
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
            </v-text-field>
          </v-col>

          <v-col>
            <v-autocomplete
              v-if="
                permitStore.getPermitDetail.application.currentAddress
                  .country === 'United States'
              "
              v-model="
                permitStore.getPermitDetail.application.currentAddress.state
              "
              :readonly="readonly"
              :items="states"
              :label="$t('State')"
              :rules="[v => !!v || $t('State cannot be blank')]"
              autocomplete="nope"
              outlined
              dense
            >
            </v-autocomplete>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress.city
              "
              :label="$t('City')"
              :rules="[v => !!v || $t('City cannot be blank')]"
              :readonly="readonly"
              maxlength="100"
              outlined
              dense
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress.county
              "
              :readonly="readonly"
              :label="$t('County')"
              :rules="[v => !!v || $t('County cannot be blank')]"
              maxlength="100"
              outlined
              dense
            >
            </v-text-field>
          </v-col>

          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.currentAddress.zip
              "
              :readonly="readonly"
              :label="$t('Zip')"
              :rules="[v => !!v || $t('Zip cannot be blank')]"
              maxlength="10"
              outlined
              dense
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-checkbox
          v-model="permitStore.getPermitDetail.application.differentMailing"
          :label="$t('Has Different Mailing address')"
          :disabled="readonly"
        />
      </v-form>
    </v-card-text>

    <template v-if="permitStore.getPermitDetail.application.differentMailing">
      <v-card-title>
        {{ $t('Different Mailing Address') }}
      </v-card-title>

      <v-card-text>
        <v-form
          v-model="mailingAddressFormValid"
          ref="mailingAddressForm"
        >
          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress
                    .streetAddress
                "
                :readonly="readonly"
                :label="$t('Street Address')"
                :rules="[v => !!v || $t('Street address cannot be blank')]"
                maxlength="150"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-combobox
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.country
                "
                :readonly="readonly"
                :items="countries"
                :label="$t('Country')"
                :rules="[v => !!v || $t('Country cannot be blank')]"
                outlined
                dense
              >
              </v-combobox>
            </v-col>

            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application.mailingAddress
                    .country === 'United States'
                "
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.state
                "
                :readonly="readonly"
                :items="states"
                :label="$t('State')"
                :rules="[v => !!v || $t('State cannot be blank')]"
                outlined
                dense
              >
              </v-autocomplete>

              <v-text-field
                v-else
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.state
                "
                :readonly="readonly"
                :items="states"
                :label="$t('State')"
                :rules="[v => !!v || $t('State cannot be blank')]"
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
                  permitStore.getPermitDetail.application.mailingAddress.city
                "
                :readonly="readonly"
                :label="$t('City')"
                :rules="[v => !!v || $t(' City cannot be blank')]"
                maxlength="100"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.county
                "
                :readonly="readonly"
                :label="$t('County')"
                :rules="[v => !!v || $t('County cannot be blank')]"
                maxlength="100"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.mailingAddress.zip
                "
                :readonly="readonly"
                :label="$t('Zip')"
                :rules="[v => !!v || $t('Zip cannot be blank')]"
                maxlength="10"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </template>

    <v-card-text>
      <v-checkbox
        :disabled="readonly"
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
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.streetAddress
                "
                :readonly="readonly"
                :label="$t('Spouse Street Address')"
                :rules="[
                  v => !!v || $t('Spouse street address cannot be blank'),
                ]"
                maxlength="150"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-combobox
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.country
                "
                :readonly="readonly"
                :items="countries"
                :label="$t('Spouse\'s Country')"
                :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
                maxlength="25"
                outlined
                dense
              >
              </v-combobox>
            </v-col>

            <v-col>
              <v-autocomplete
                v-if="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.country === 'United States'
                "
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.state
                "
                :readonly="readonly"
                :items="states"
                :label="$t('Spouse\'s State')"
                :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
                outlined
                dense
              >
              </v-autocomplete>

              <v-text-field
                v-else
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.state
                "
                :items="states"
                :readonly="readonly"
                :label="$t('Spouse\'s State')"
                :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              <v-text-field
                :readonly="readonly"
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.city
                "
                :label="$t('Spouse\'s City')"
                :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
                maxlength="100"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.county
                "
                :readonly="readonly"
                :label="$t('Spouse\'s County')"
                :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
                maxlength="100"
                outlined
                dense
              >
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.zip
                "
                :readonly="readonly"
                :label="$t('Spouse\'s Zip')"
                :rules="[v => !!v || $t('Spouse\'s Zip cannot be blank')]"
                maxlength="10"
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
                  permitStore.getPermitDetail.application
                    .spouseAddressInformation.reason
                "
                :readonly="readonly"
                :label="$t('Reason for different spouse address')"
                :rules="[
                  v =>
                    !!v ||
                    $t('Reason for different spouse address cannot be blank'),
                ]"
                maxlength="150"
                outlined
                dense
              >
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
        :readonly="readonly"
        @get-previous-address-from-dialog="getPreviousAddressFromDialog"
      />
      <AddressTable
        :addresses="permitStore.getPermitDetail.application.previousAddresses"
        :enable-delete="!readonly"
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
import { openPdf } from '@core-admin/components/composables/openDocuments'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, inject, ref } from 'vue'
import { countries, states } from '@shared-utils/lists/defaultConstants'

const permitStore = usePermitsStore()
const addressFormValid = ref(false)
const mailingAddressFormValid = ref(false)
const spouseAddressFormValid = ref(false)
const emit = defineEmits(['on-save'])
const readonly = inject<boolean>('readonly')

async function handleOpenPdf() {
  const modifyNameDocument =
    permitStore.getPermitDetail.application.uploadedDocuments.find(d => {
      if (
        d.name.indexOf(
          `ModifyAddress-${permitStore.getPermitDetail.application.modificationNumber}`
        ) >= 0
      ) {
        return d
      }

      return null
    })

  if (modifyNameDocument) {
    await openPdf(modifyNameDocument)
  }
}

function getPreviousAddressFromDialog(address: AddressInfoType) {
  permitStore.getPermitDetail.application.previousAddresses.push(address)
}

function deleteAddress(index) {
  permitStore.getPermitDetail.application.previousAddresses.splice(index, 1)
}

function handleSave() {
  emit('on-save', 'Address Information')
}

function onApproveAddressChange() {
  permitStore.getPermitDetail.application.modifiedAddressComplete = true
  emit('on-save', 'Approved address change')
}

function onUndoApproveAddressChange() {
  permitStore.getPermitDetail.application.modifiedAddressComplete = false
  emit('on-save', 'Undo approved address change')
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
