<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        small
        color="primary"
        v-bind="attrs"
        v-on="on"
      >
        {{ $t(' Add Previous Address') }}
      </v-btn>
    </template>

    <v-card outlined>
      <v-card-title>{{ $t('Previous Address Information') }}</v-card-title>

      <v-card-text>
        <v-form
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                v-model="state.address.addressLine1"
                :label="$t('Address line 1')"
                :rules="[v => !!v || 'Address line 1 cannot be blank']"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="150"
                :label="$t('Address line 2')"
                v-model="state.address.addressLine2"
                outlined
                dense
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="100"
                v-model="state.address.city"
                :label="$t('City')"
                :rules="[v => !!v || 'City cannot be blank']"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
            <v-col>
              <v-combobox
                v-if="state.address.country === 'United States'"
                maxlength="100"
                :items="states"
                v-model="state.address.state"
                :label="$t('State')"
                :rules="[v => !!v || 'State cannot be blank']"
                outlined
                dense
              >
              </v-combobox>
              <v-text-field
                v-if="state.address.country !== 'United States'"
                maxlength="150"
                counter
                :label="$t('State / Region')"
                :rules="[v => !!v || $t('State / Region cannot be blank')]"
                v-model="state.address.state"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                maxlength="100"
                persistent-hint
                :hint="$t('If not applicable enter N/A ')"
                v-model="state.address.county"
                :label="$t('County')"
                :rules="[v => !!v || 'County cannot be blank']"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                maxlength="10"
                required
                persistent-hint
                v-model="state.address.zip"
                :label="$t('Zip')"
                :hint="$t('If not applicable enter N/A ')"
                :rules="[
                  v => !!v || $t('Field is required'),
                  v =>
                    !!v.match(/^\d+$/) ||
                    v === 'N/A' ||
                    v === 'n/a' ||
                    $t('Must contain only numbers'),
                ]"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
            <v-col>
              <v-combobox
                :items="countries"
                v-model="state.address.country"
                :label="$t('Country')"
                :rules="[v => !!v || 'Country cannot be blank']"
                outlined
                dense
              >
              </v-combobox>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-btn
          small
          color="primary"
          @click="handleSubmit"
          :disabled="!valid"
        >
          {{ $t('Submit') }}
        </v-btn>
        <v-btn
          small
          color="primary"
          @click="dialog = false"
        >
          {{ $t('Close') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes'
import { countries, states } from '@shared-utils/lists/defaultConstants'
import { reactive, ref } from 'vue'

const emit = defineEmits(['get-previous-address-from-dialog'])

const state = reactive({
  address: {
    addressLine1: '',
    addressLine2: '',
    city: '',
    country: '',
    county: '',
    state: '',
    zip: '',
  } as AddressInfoType,
})

let dialog = ref(false)
const valid = ref(false)

function handleSubmit() {
  emit('get-previous-address-from-dialog', state.address)

  state.address = reactive({
    addressLine1: '',
    addressLine2: '',
    city: '',
    country: '',
    county: '',
    state: '',
    zip: '',
  } as AddressInfoType)

  dialog.value = false
}
</script>
