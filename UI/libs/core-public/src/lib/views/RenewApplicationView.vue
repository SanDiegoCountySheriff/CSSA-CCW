<template>
  <div class="application">
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Application logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
    <AcknowledgementContainer :next-route="'/renew-form'" />
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@core-public/stores/brandStore';
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';

const store = useBrandStore();
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
