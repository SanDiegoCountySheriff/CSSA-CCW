<template>
  <v-card color="#1565C043">
    <v-card-title>
      {{ $t('General Information') }}
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
        text
        color="primary"
        @click="handleRouteChange"
      >
        {{ $t('Further Info') }}
      </v-btn>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router/composables';

const router = useRouter();

function handleRouteChange() {
  router.push('/more-information');
}
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
