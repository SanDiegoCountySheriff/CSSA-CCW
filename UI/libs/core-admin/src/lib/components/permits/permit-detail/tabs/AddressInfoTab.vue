<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Address Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          maxlength="150"
          counter
          :label="$t('Address line 1')"
          :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
          v-model="
            permitStore.getPermitDetail.application.currentAddress.addressLine1
          "
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
      <v-col
        cols="12"
        md="5"
        sm="12"
        class="pl-8"
      >
        <v-text-field
          dense
          outlined
          maxlength="150"
          counter
          :label="$t('Address line 2')"
          v-model="
            permitStore.getPermitDetail.application.currentAddress.addressLine2
          "
        >
        </v-text-field>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          :label="$t('City')"
          maxlength="100"
          counter
          :rules="[v => !!v || $t('City cannot be blank')]"
          v-model="permitStore.getPermitDetail.application.currentAddress.city"
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
              v-if="
                !permitStore.getPermitDetail.application.currentAddress.city
              "
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-combobox
          dense
          outlined
          :items="states"
          :label="$t('State')"
          :rules="[v => !!v || $t('State cannot be blank')]"
          autocomplete="nope"
          v-model="permitStore.getPermitDetail.application.currentAddress.state"
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
              v-if="
                !permitStore.getPermitDetail.application.currentAddress.state
              "
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-combobox>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          maxlength="100"
          counter
          :label="$t('County')"
          :rules="[v => !!v || $t('County cannot be blank')]"
          v-model="
            permitStore.getPermitDetail.application.currentAddress.county
          "
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
              v-if="
                !permitStore.getPermitDetail.application.currentAddress.county
              "
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          maxlength="10"
          counter
          :label="$t('Zip')"
          :rules="[v => !!v || $t('Zip cannot be blank')]"
          v-model="permitStore.getPermitDetail.application.currentAddress.zip"
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
              v-if="!permitStore.getPermitDetail.application.currentAddress.zip"
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-combobox
          dense
          outlined
          :items="countries"
          :label="$t('Country')"
          :rules="[v => !!v || 'Country cannot be blank']"
          autocomplete="nope"
          v-model="
            permitStore.getPermitDetail.application.currentAddress.country
          "
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
              v-if="
                !permitStore.getPermitDetail.application.currentAddress.country
              "
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-combobox>
      </v-col>
    </v-row>
    <v-row class="ml-5 my-5">
      <v-col>
        <v-checkbox
          id="different-mailing"
          :label="$t('Different Mailing address')"
          v-model="permitStore.getPermitDetail.application.differentMailing"
        />
      </v-col>
      <v-col>
        <v-checkbox
          id="different-spouse"
          :label="$t('Different Spouse address')"
          v-model="
            permitStore.getPermitDetail.application.differentSpouseAddress
          "
        />
      </v-col>
    </v-row>
    <div v-if="permitStore.getPermitDetail.application.differentSpouseAddress">
      <v-divider></v-divider>
      <v-card-title class="subtitle-2">
        {{ $t('Different Spouse Address:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="150"
            counter
            :label="$t('Spouse address line 1')"
            :rules="[v => !!v || $t('Spouse address line 1 cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .addressLine1
            "
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
        <v-col
          cols="12"
          md="5"
          sm="12"
          class="pl-8"
        >
          <v-text-field
            dense
            outlined
            maxlength="150"
            counter
            :label="$t('Spouse address line 2')"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .addressLine2
            "
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="100"
            counter
            :label="$t('Spouse\'s City')"
            :rules="[v => !!v || $t('Spouse\'s City cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .city
            "
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
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="states"
            :label="$t('Spouse\'s State')"
            :rules="[v => !!v || $t('Spouse\'s State cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .state
            "
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
                v-if="
                  !permitStore.getPermitDetail.application
                    .spouseAddressInformation.state
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-combobox>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="100"
            counter
            :label="$t('Spouse\'s County')"
            :rules="[v => !!v || $t('Spouse\'s County cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .county
            "
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
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="10"
            counter
            :label="$t('Spouse\'s Zip')"
            :rules="[v => !!v || $t('Spouse\'s Zip cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .zip
            "
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
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="countries"
            :label="$t('Spouse\'s Country')"
            :rules="[v => !!v || $t('Spouse\'s Country cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.spouseAddressInformation
                .country
            "
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
      </v-row>
      <v-divider />
    </div>
    <div v-if="permitStore.getPermitDetail.application.differentMailing">
      <v-divider></v-divider>
      <v-card-title class="subtitle-2">
        {{ $t('Different Mailing Address:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="150"
            counter
            :label="$t('Address line 1')"
            :rules="[v => !!v || $t('Address line 1 cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress
                .addressLine1
            "
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
        <v-col
          cols="12"
          md="5"
          sm="12"
          class="pl-8"
        >
          <v-text-field
            dense
            outlined
            maxlength="150"
            counter
            :label="$t('Address line 2')"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress
                .addressLine2
            "
          >
          </v-text-field>
        </v-col>
      </v-row>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="100"
            counter
            :label="$t('City')"
            :rules="[v => !!v || $t(' City cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress.city
            "
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
                v-if="
                  !permitStore.getPermitDetail.application.mailingAddress.city
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="states"
            :label="$t('State')"
            :rules="[v => !!v || $t('State cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress.state
            "
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
                v-if="
                  !permitStore.getPermitDetail.application.mailingAddress.state
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-combobox>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="100"
            counter
            :label="$t('County')"
            :rules="[v => !!v || $t('County cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress.county
            "
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
                v-if="
                  !permitStore.getPermitDetail.application.mailingAddress.county
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            maxlength="10"
            counter
            :label="$t('Zip')"
            :rules="[v => !!v || $t('Zip cannot be blank')]"
            v-model="permitStore.getPermitDetail.application.mailingAddress.zip"
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
                v-if="
                  !permitStore.getPermitDetail.application.mailingAddress.zip
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-text-field>
        </v-col>

        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-combobox
            dense
            outlined
            :items="countries"
            :label="$t('Country')"
            :rules="[v => !!v || $t('Country cannot be blank')]"
            v-model="
              permitStore.getPermitDetail.application.mailingAddress.country
            "
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
      </v-row>
    </div>
    <v-divider></v-divider>
    <div>
      <v-card-title class="subtitle-2">
        {{ $t('Previous Address:') }}
      </v-card-title>
      <AddressTable
        :addresses="permitStore.getPermitDetail.application.previousAddresses"
      />
      <div class="offset-md-8">
        <PreviousAddressDialog
          :get-previous-address-from-dialog="getPreviousAddressFromDialog"
        />
      </div>
    </div>
  </v-card>
</template>
<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';
import AddressTable from '@shared-ui/components/tables/AddressTable.vue';
import PreviousAddressDialog from '@shared-ui/components/dialogs/PreviousAddressDialog.vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { countries, states } from '@shared-utils/lists/defaultConstants';

const permitStore = usePermitsStore();

function getPreviousAddressFromDialog(address: AddressInfoType) {
  permitStore.getPermitDetail.application.previousAddresses.push(address);
}
</script>
