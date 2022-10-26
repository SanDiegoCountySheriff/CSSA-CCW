<template>
  <v-container class="finalize-view">
    <SideBar />
    <FinalizeContainer />
    <PaymentContainer />
  </v-container>
</template>

<script lang="ts" setup>
import SideBar from '@core-public/components/navbar/SideBar.vue';
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue';
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue';
//TODO: make the api call here to get the appointments.
</script>

<style lang="scss" scoped>
.finalize-view {
  height: 100%;
}
</style>
