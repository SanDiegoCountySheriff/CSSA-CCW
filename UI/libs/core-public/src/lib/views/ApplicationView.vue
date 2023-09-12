<template>
  <div>
    <v-container class="text-center">
      <v-img
        v-if="store.getDocuments.agencyLandingPageImage"
        alt="Application logo"
        :src="store.getDocuments.agencyLandingPageImage"
        max-width="500"
        class="mx-auto"
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
