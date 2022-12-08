<template>
  <v-container
    fluid
    class="blue-grey lighten-3 elevation-3 rounded form-container"
  >
    <FormWrapper
      :admin="false"
      :routes="Routes"
    />
  </v-container>
</template>

<script setup lang="ts">
import FormWrapper from '@core-public/components/wrappers/FormWrapper.vue';
import Routes from '@core-public/router/routes';
</script>

<style lang="scss" scoped>
.form-container {
  min-height: 50%;
}
</style>
