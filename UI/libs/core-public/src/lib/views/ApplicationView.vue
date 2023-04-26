<template>
  <div class="application">
    <v-container
      fluid
      class="text-center"
    >
      <img
        v-if="store.getDocuments.agencyLandingPageImage"
        alt="Application logo"
        :src="store.getDocuments.agencyLandingPageImage"
      />
    </v-container>
    <AcknowledgementContainer
      :next-route="`${Routes.FORM_ROUTE_PATH}?orderId=${route.params}`"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue'
import Routes from '@core-public/router/routes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useRoute } from 'vue-router/composables'

const store = useBrandStore()
const route = useRoute()
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
.application {
  min-height: 100vh;
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
