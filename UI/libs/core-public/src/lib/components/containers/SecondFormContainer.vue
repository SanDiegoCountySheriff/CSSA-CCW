<template>
  <v-container
    fluid
    class="blue-grey lighten-3 elevation-3 rounded form-container"
  >
    <SecondFormWrapper />
  </v-container>
</template>

<script setup lang="ts">
import SecondFormWrapper from '@core-public/components/wrappers/SecondFormWrapper.vue';
</script>

<style lang="scss" scoped>
.form-container {
  min-height: 50%;
}
</style>
