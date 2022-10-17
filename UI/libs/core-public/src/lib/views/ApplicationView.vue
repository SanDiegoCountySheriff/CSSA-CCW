<template>
  <div class="application">
    <!-- TODO: Make this the custom logo -->
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Application logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
    <AcknowledgementContainer />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import { useBrandStore } from '@core-public/stores/brandStore';

const store = useBrandStore();
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
