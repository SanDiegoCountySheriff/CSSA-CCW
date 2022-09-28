<template>
  <div>
    <v-dialog v-model="dialog">
      <template #activator="{ on, attrs }">
        <v-btn
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
              />
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
              />
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
              />
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
              />
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
              />
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
              />
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
            @click="dialog = false"
          >
            {{ $t('Close') }}
          </v-btn>
        </div>
      </div>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { AddressInfo } from '@shared-ui/types/defualtTypes';

export default defineComponent({
  name: 'PreviousAddressDialog',
  props: {
    getPreviousAddressFromDialog: {
      type: Function as PropType<(address: AddressInfo) => void>,
      default: () => null,
    },
  },
  data() {
    return {
      address: {} as AddressInfo,
      dialog: false,
      valid: false,
    };
  },
  methods: {
    handleSubmit() {
      this.getPreviousAddressFromDialog(this.address);
    },
  },
});
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
