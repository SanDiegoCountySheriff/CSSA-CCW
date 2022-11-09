<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('Current Concealed Permit Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col cols="10">
        <v-card
          color="primary"
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
</template>
