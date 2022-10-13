<template>
  <div class="home">
    <!-- TODO: Make this the custom logo -->
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Agency logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
  </div>
</template>

<script setup lang="ts">
import { useBrandStore } from '@core-admin/stores/brandStore';

const store = useBrandStore();
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
