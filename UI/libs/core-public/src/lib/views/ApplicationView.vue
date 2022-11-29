<template>
  <div class="application">
    <img
      v-if="store.getDocuments.agencyLandingPageImage"
      alt="Application logo"
      :src="store.getDocuments.agencyLandingPageImage"
    />
    <AcknowledgementContainer :next-route="Routes.FORM_ROUTE_PATH" />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import Routes from '@core-public/router/routes';
import { useBrandStore } from '@shared-ui/stores/brandStore';

const store = useBrandStore();
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
.selections {
  width: 100%;
}

.button-content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
</style>
