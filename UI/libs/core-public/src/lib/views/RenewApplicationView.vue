<template>
  <div class="application">
    <img
      v-if="store.getDocuments.agencyLogo"
      alt="Application logo"
      :src="store.getDocuments.agencyLogo"
    />
    <AcknowledgementContainer :next-route="Routes.RENEW_FORM_ROUTE_PATH" />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue'
import Routes from '@core-public/router/routes'
import { useBrandStore } from '@shared-ui/stores/brandStore'

const store = useBrandStore()
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
