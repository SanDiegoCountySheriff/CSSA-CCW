<template>
  <div class="finalize-view">
    <SideBar
      :options="options"
      :title="'Information Sections'"
      :handle-selection="handleSelection"
    />
    <FinalizeContainer />
    <PaymentContainer />
    <AppointmentContainer />
  </div>
</template>

<script lang="ts" setup>
import AppointmentContainer from '@core-public/components/containers/AppointmentContainer.vue';
import FinalizeContainer from '@core-public/components/containers/FinalizeContainer.vue';
import PaymentContainer from '@core-public/components/containers/PaymentContainer.vue';
import SideBar from '@core-public/components/navbar/SideBar.vue';
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';
//TODO: make the api call here to get the appointments and pass as a prop to the appointmentContainer.
const currentInfoSectionStore = useCurrentInfoSection();

const options = [
  'Personal Information',
  'Spouse Information',
  'Alias Information',
  'Id/Birth Information',
  'Citizenship Information',
  'Current Address Information',
  'Previous Address Information',
  'Mailing Address Information',
  'Physical Appearance',
  'Contact Information',
  'Employment Information',
  'Weapons Information',
];

function handleSelection(target: number) {
  currentInfoSectionStore.setCurrentInfoSection(target);
}
</script>

<style lang="scss" scoped>
.finalize-view {
  height: 100%;
  width: 80%;
}
</style>
