<template>
  <v-container
    fluid
    class="blue-grey lighten-3 elevation-3 rounded form-container"
  >
    <RenewFormWrapper
      :routes="Routes"
      :admin="false"
    />
  </v-container>
</template>

<script setup lang="ts">
import RenewFormWrapper from '@core-public/components/wrappers/RenewFormWrapper.vue';
import Routes from '@core-public/router/routes';
</script>

<style lang="scss"></style>
