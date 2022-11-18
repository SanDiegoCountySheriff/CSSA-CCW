<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card
    elevation="3"
    class="mt-4"
  >
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-card
          class="order-details-card text-left"
          justify="align-left"
          max-width="500"
          outlined
          elevation="3"
          color="primary lighten-2"
        >
          <v-card-text>
            {{ $t('CCW Order Number:') }}
            <span> {{ permitStore.getPermitDetail.application.orderId }} </span>
          </v-card-text>
          <v-card-text>
            {{ $t('CCW Order Received:') }} <span> 06/25/2022 </span>
          </v-card-text>
          <v-card-text>
            {{ $t('Status:') }}
            <span> {{ permitStore.getPermitDetail.application.status }} </span>
          </v-card-text>
          <v-card-text>
            {{ $t('Processor:') }} <span> NONE </span>
          </v-card-text>
        </v-card>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-card
          justify="align-left"
          max-width="500"
          outlined
          elevation="3"
          color="primary lighten-2"
          class="text-left expiry-card"
        >
          <v-card-text>
            {{ $t('Order Expired:') }} <span> 06/25/2022 </span>
          </v-card-text>
          <v-card-text>
            {{ $t('Payment History:') }}
            <span> $113.11, CREDITCARD, 06065D </span>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-row class="ml-5 mr-5">
      <v-col cols="12">
        <v-card
          class="text-left new-permit-card"
          justify="align-left"
          outlined
          elevation="3"
          color="primary lighten-2"
        >
          <v-card-text>
            {{ $t('New Application:') }}
            <span> New Standard 2 year CCW License</span>
          </v-card-text>
          <v-card-text>
            {{ $t('Application Type:') }}
            <span>
              {{ permitStore.getPermitDetail.application.applicationType }}
            </span>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
    <v-divider class="mt-3 mb-3"></v-divider>

    <v-card elevation="3">
      <v-card-title class="text-h5">
        {{ $t('Current Concealed Permit Information:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col cols="10">
          <v-card
            color="primary lighten-2 text-left"
            justify="align-left"
            max-width="500"
            outlined
            elevation="2"
          >
            <v-card-subtitle color="white">
              This is an application for a NEW or EMERGENCY Permit <br />
              No existing Permit.
            </v-card-subtitle>
          </v-card>
        </v-col>
      </v-row>
    </v-card>
  </v-card>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();
</script>
<style lang="scss" scoped>
.v-card__text {
  padding: 6px;

  span {
    text-decoration: underline;
  }
}

.theme--light.v-card > .v-card__text {
  color: white !important;
}
</style>
