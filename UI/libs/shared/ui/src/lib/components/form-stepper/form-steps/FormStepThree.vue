<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="address.addressLine1"
            label="Address line 1"
            :rules="[ v => !!v || 'Address line 1 cannot be blank']"
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

      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="address.city"
            label="City"
            :rules="[ v => !!v || ' City cannot be blank']"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="address.state"
            label="State"
            :rules="[ v => !!v || 'State cannot be blank&quot']"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="address.zip"
            label="Zip"
            :rules="[ v => !!v || 'Zip cannot be blank&quot']"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-model="address.country"
            label="Country"
            :rules="[ v => !!v || 'Country cannot be blank']"
          />
        </v-col>
      </v-row>

      <v-row>
        <PreviousAddressDialog :get-previous-address-from-dialog="getPreviousAddressFromDialog" />
      </v-row>
    </v-form>
  </div>
</template>

<script lang='ts'>

import { AddressInfo } from '@shared-ui/types/defualtTypes';
import { defineComponent, PropType } from 'vue';
import PreviousAddressDialog from '@shared-ui/components/dialogs/PreviousAddressDialog.vue';

export default defineComponent( {
  name: 'FormStepThree',
  components: { PreviousAddressDialog },
  props: {
    handleNextSection: {
      type: Function as PropType<() => void>,
      default: () => null,
    },
  },
  data(){
    return {
      address: {} as AddressInfo,
      previousAddress: [] as Array<AddressInfo>,
      valid: false
    }
  },
  methods:{
     getPreviousAddressFromDialog(address: AddressInfo){
       this.previousAddress.push(address)
     }
  }
});
</script>

<style lang='scss' scoped>
.subHeader {
  font-size: 1.5rem;
}
</style>
