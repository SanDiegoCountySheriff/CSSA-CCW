<template>
  <div class="finalize-view">
    <SideBar />
    <FinalizeContainer />
    <PaymentContainer />
    <AppointmentContainer />
  </div>
</template>

<script lang="ts" setup>
import SideBar from '@core-public/components/navbar/SideBar.vue';
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue';
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue';
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue';
//TODO: make the api call here to get the appointments and pass as a prop to the appointmentContainer.
</script>

<style lang="scss" scoped>
.finalize-view {
  height: 100%;
  width: 80%;
}
</style>
