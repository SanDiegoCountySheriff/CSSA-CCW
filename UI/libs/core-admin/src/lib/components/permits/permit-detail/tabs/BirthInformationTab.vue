<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Information Related to Your Birth:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-menu
          v-model="menu"
          :close-on-content-click="false"
          transition="scale-transition"
          offset-y
          min-width="auto"
        >
          <template #activator="{ on, attrs }">
            <v-text-field
              dense
              outlined
              v-model="permitStore.getPermitDetail.application.dob.birthDate"
              :label="$t('Date of birth')"
              hint="YYYY-MM-DD format"
              persistent-hint
              readonly
              v-bind="attrs"
              v-on="on"
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
                <v-icon> mdi-calendar </v-icon>
                <v-icon
                  color="error"
                  medium
                  v-if="!permitStore.getPermitDetail.application.dob.birthDate"
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </template>
          <v-date-picker
            v-model="permitStore.getPermitDetail.application.dob.birthDate"
            no-title
            @input="menu = false"
            scrollable
          >
          </v-date-picker>
        </v-menu>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          dense
          outlined
          :label="$t('Birth city')"
          :rules="[v => !!v || $t('Birth city cannot be blank')]"
          v-model="permitStore.getPermitDetail.application.dob.birthCity"
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
              v-if="!permitStore.getPermitDetail.application.dob.birthCity"
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-text-field>
        <v-autocomplete
          dense
          outlined
          :items="states"
          :label="$t('Birth state')"
          :rules="[v => !!v || $t('Birth state cannot be blank')]"
          v-model="permitStore.getPermitDetail.application.dob.birthState"
          autocomplete="nope"
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
              v-if="!permitStore.getPermitDetail.application.dob.birthState"
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-autocomplete>

        <v-autocomplete
          dense
          outlined
          :items="countries"
          :label="$t('Birth country')"
          :rules="[v => !!v || $t('Birth country cannot be blank')]"
          v-model="permitStore.getPermitDetail.application.dob.birthCountry"
          autocomplete="nope"
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
              v-if="!permitStore.getPermitDetail.application.dob.birthCountry"
            >
              mdi-alert-octagon
            </v-icon>
          </template>
        </v-autocomplete>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { countries, states } from '@shared-utils/lists/defaultConstants';

const menu = ref(false);

const permitStore = usePermitsStore();
</script>
