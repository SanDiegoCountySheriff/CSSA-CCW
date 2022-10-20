<template>
  <div>
    <v-dialog v-model="dialog.state">
      <template #activator="{ on, attrs }">
        <v-btn
          id="add-previous-address-btn"
          color="primary my-5"
          v-bind="attrs"
          v-on="on"
        >
          {{ $t(' Add previous address') }}
        </v-btn>
      </template>
      <div class="address-container">
        <v-form
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="address-line-1"
                v-model="address.addressLine1"
                label="Address line 1"
                :rules="[v => !!v || 'Address line 1 cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                v-model="address.addressLine2"
                label="Address line 2"
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="city"
                v-model="address.city"
                label="City"
                :rules="[v => !!v || 'City cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="state"
                v-model="address.state"
                label="State"
                :rules="[v => !!v || 'State cannot be blank&quot']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="county"
                v-model="address.county"
                label="County"
                :rules="[v => !!v || 'County cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="zip"
                v-model="address.zip"
                label="Zip"
                :rules="[v => !!v || 'Zip cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="country"
                v-model="address.country"
                label="Country"
                :rules="[v => !!v || 'Country cannot be blank']"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
        <div class="mt-2 btn-container">
          <v-btn
            id="pre-address-submit-btn"
            color="success"
            @click="handleSubmit"
            class="mr-2"
            :disabled="!valid"
          >
            {{ $t('Submit') }}
          </v-btn>
          <v-btn
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
import { reactive, ref } from 'vue';

interface PreviousAddressDialogProps {
  getPreviousAddressFromDialog?: (address: AddressInfoType) => void;
}

const props = withDefaults(defineProps<PreviousAddressDialogProps>(), {
  getPreviousAddressFromDialog: () => {
    return;
  },
});

const address = reactive({} as AddressInfoType);
let dialog = reactive({ state: false });
const valid = ref(false);

function handleSubmit() {
  props.getPreviousAddressFromDialog(address);
  dialog.state = false;
}
</script>

<style lang="scss" scoped>
.address-container {
  display: flex;
  flex-direction: column;
  height: 50vh;
  width: 90%;
  justify-content: center;
  align-items: center;
  background: aliceblue;
  border-radius: 12px;
}
.btn-container {
  display: flex;
  width: 75%;
  justify-content: flex-end;
}
</style>
