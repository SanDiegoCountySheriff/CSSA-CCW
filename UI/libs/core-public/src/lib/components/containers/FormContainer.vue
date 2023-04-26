<template>
  <FormWrapper
    :admin="false"
    :routes="Routes"
  />
</template>

<script setup lang="ts">
import FormWrapper from '@core-public/components/wrappers/FormWrapper.vue'
import Routes from '@core-public/router/routes'
</script>
