<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        :color="$vuetify.theme.dark ? 'white' : 'primary'"
        text
        :height="$vuetify.breakpoint.lgAndUp ? '180' : '100'"
        :x-large="$vuetify.breakpoint.lgAndUp"
        :small="$vuetify.breakpoint.smAndDown"
        v-bind="attrs"
        v-on="on"
      >
        <v-container>
          <v-row>
            <v-col>
              <v-icon x-large> mdi-card-account-phone-outline </v-icon>
            </v-col>
          </v-row>

          <v-row>
            <v-col>
              {{ $t('Contact') }}
            </v-col>
          </v-row>
        </v-container>
      </v-btn>
    </template>

    <v-card>
      <v-card-title>
        {{ brandStore.brand.agencyName }} Contact Information
      </v-card-title>

      <v-card-text>
        <strong>{{ $t(' Address: ') }}</strong>
        {{ brandStore.brand.agencyStreetAddress }}
      </v-card-text>

      <v-card-text>
        <strong>{{ $t(' Telephone: ') }}</strong>
        {{ brandStore.brand.agencyTelephone }}
      </v-card-text>

      <v-card-text>
        <strong>{{ $t(' Fax: ') }}</strong>
        {{ brandStore.brand.agencyFax }}
      </v-card-text>

      <v-card-text>
        <strong>{{ $t(' Email: ') }}</strong>
        {{ brandStore.brand.agencyEmail }}
      </v-card-text>

      <v-card-actions>
        <v-btn
          @click="dialog = false"
          color="primary"
          text
        >
          Close
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'

const dialog = ref(false)
const brandStore = useBrandStore()
</script>
