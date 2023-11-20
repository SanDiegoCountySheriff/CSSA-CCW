<template>
  <MoreInformationContainer
    :information-only="route.params.informationOnly === 'true'"
  />
</template>

<script setup lang="ts">
import MoreInformationContainer from '@core-public/components/containers/MoreInformationContainer.vue'
import { useRoute } from 'vue-router/composables'

const route = useRoute()
</script>
