<template>
  <v-card class="text-left">
    <v-card-title>
      {{ $t('General Information') }}
    </v-card-title>

    <v-card-text>
      {{ $t('GENERAL-INFORMATION') }}
    </v-card-text>

    <v-card-text>
      {{ $t('GENERAL-RENEWAL') }}
    </v-card-text>

    <v-card-text>
      <ul>
        <li class="mt-2">
          {{ $t('GENERAL-LIST-ITEM-ONE') }}
        </li>
        <li class="mt-2">
          {{ $t('GENERAL-LIST-ITEM-TWO') }}
        </li>
        <li class="mt-2">
          {{ $t('GENERAL-LIST-ITEM-THREE') }}
        </li>
      </ul>
    </v-card-text>

    <v-card-text>
      <strong> {{ $t('GENERAL-FURTHER-INFO') }} </strong>
      <br />
      {{ $t('GENERAL-INFO-COMPETENCY') }}
    </v-card-text>

    <v-card-text>
      <v-btn
        outlined
        :small="$vuetify.breakpoint.smAndDown ? true : false"
        color="primary"
        @click="router.push(Routes.MORE_INFORMATION_ROUTE_PATH)"
      >
        {{ $t('Application Process Information') }}
      </v-btn>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import Routes from '@core-public/router/routes'
import { useRouter } from 'vue-router/composables'

const router = useRouter()
</script>
