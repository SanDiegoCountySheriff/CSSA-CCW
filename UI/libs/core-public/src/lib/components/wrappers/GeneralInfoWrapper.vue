<template>
  <v-card color="#1565C015">
    <v-card-title>
      <h1 class="text-md-h4">
        {{ $t('General Information') }}
      </h1>
    </v-card-title>
    <v-card-text class="text-left gen-info-text">
      {{ $t('GENERAL-INFORMATION') }}
    </v-card-text>
    <v-card-text class="text-left font-weight-bold">
      {{ $t('GENERAL-RENEWAL') }}
    </v-card-text>
    <v-card-text>
      <ul class="text-left font-weight-bold">
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
    <v-card-text class="text-left">
      <strong> {{ $t('GENERAL-FURTHER-INFO') }} </strong>
      <br />
      {{ $t('GENERAL-INFO-COMPETENCY') }}
    </v-card-text>
    <v-card-text class="text-right">
      <v-btn
        outlined
        :color="$vuetify.theme.dark ? 'accent' : 'accent'"
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

<style lang="scss" scoped>
.gen {
  &-info {
    &-text {
      font-size: 1rem;
    }
  }
}
</style>
