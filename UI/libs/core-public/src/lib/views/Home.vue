<template>
  <div class="home">
    <!-- TODO: Make this the custom logo -->
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Application logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
    <v-container>
      <router-link :to="{ path: '/application' }">
        <Button
          color="primary"
          text="NEW Concealed Carry Weapon License"
        >
        </Button>
      </router-link>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import Button from '@shared-ui/components/Button.vue';
import { useBrandStore } from '@core-public/stores/brandStore';

const store = useBrandStore();
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
