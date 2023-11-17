<template>
  <MoreInformationContainer />
  <!-- <AcknowledgementContainer
      :next-route="`${Routes.FORM_ROUTE_PATH}?orderId=${route.params}`"
    /> -->
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue'
import MoreInformationContainer from '@core-public/components/containers/MoreInformationContainer.vue'
import Routes from '@core-public/router/routes'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useRoute } from 'vue-router/composables'

const store = useBrandStore()
const route = useRoute()
</script>
