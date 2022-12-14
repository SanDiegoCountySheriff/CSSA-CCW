<template>
  <div>
    <v-dialog v-model="dialog.state">
      <template #activator="{ on, attrs }">
        <v-btn
          small
          id="add-previous-address-btn"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          v-bind="attrs"
          v-on="on"
        >
          {{ $t(' Add previous address') }}
        </v-btn>
      </template>
      <div
        class="address-container"
        :style="{ background: $vuetify.theme.dark ? '#222' : '#EEE' }"
      >
        <v-form
          ref="form"
          v-model="valid"
          class="form-container"
        >
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                maxlength="150"
                counter
                dense
                id="address-line-1"
                v-model="state.address.addressLine1"
                label="Address line 1"
                :rules="[v => !!v || 'Address line 1 cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                maxlength="150"
                counter
                dense
                class="pl-6"
                v-model="state.address.addressLine2"
                label="Address line 2"
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                maxlength="100"
                counter
                dense
                id="city"
                v-model="state.address.city"
                label="City"
                :rules="[v => !!v || 'City cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                dense
                maxlength="100"
                counter
                id="state"
                v-model="state.address.state"
                label="State"
                :rules="[v => !!v || 'State cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                maxlength="100"
                counter
                dense
                id="county"
                v-model="state.address.county"
                label="County"
                :rules="[v => !!v || 'County cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                maxlength="10"
                counter
                dense
                id="zip"
                v-model="state.address.zip"
                label="Zip"
                :rules="[v => !!v || 'Zip cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              lg="6"
            >
              <v-autocomplete
                outlined
                dense
                :items="countries"
                id="country"
                v-model="state.address.country"
                label="Country"
                :rules="[v => !!v || 'Country cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-autocomplete>
            </v-col>
          </v-row>
        </v-form>
        <div class="mt-2 btn-container">
          <v-btn
            small
            id="pre-address-submit-btn"
            color="success"
            @click="handleSubmit"
            class="mr-2"
            :disabled="!valid"
          >
            {{ $t('Submit') }}
          </v-btn>
          <v-btn
            small
            color="error"
            @click="dialog.state = false"
          >
            {{ $t('Close') }}
          </v-btn>
        </div>
      </div>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { AddressInfoType } from '@shared-utils/types/defaultTypes';
import { countries } from '@shared-utils/lists/defaultConstants';
import { reactive, ref } from 'vue';

interface PreviousAddressDialogProps {
  getPreviousAddressFromDialog?: (address: AddressInfoType) => void;
}

const props = withDefaults(defineProps<PreviousAddressDialogProps>(), {
  getPreviousAddressFromDialog: () => null,
});

const state = {
  address: {} as AddressInfoType,
};
let dialog = reactive({ state: false });
const valid = ref(false);

function handleSubmit() {
  props.getPreviousAddressFromDialog(state.address);
  state.address = reactive({} as AddressInfoType);
  dialog.state = false;
}
</script>

<style lang="scss" scoped>
.address-container {
  display: flex;
  width: 90%;
  height: auto;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: 12px;
}

.btn-container {
  display: flex;
  width: 75%;
  justify-content: flex-end;
  margin-bottom: 2em;
}

.form-container {
  width: 90%;
  margin: 2em;
}
</style>
