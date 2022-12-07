<template>
  <div>
    <SideBar
      title="Sections"
      :options="options"
      :handle-selection="handleSelection"
    />
    <MoreInformationContainer />
  </div>
</template>

<script setup lang="ts">
import MoreInformationContainer from '@core-public/components/containers/MoreInformationConatainer.vue';
import SideBar from '@core-public/components/navbar/SideBar.vue';
import { useAppInfoStep } from '@core-public/stores/appInfoStepStore';

const options = [
  'Completing Application',
  'DMV',
  'Military',
  'US Residency',
  'Proof of Residency',
];
const useAppInfoStepStore = useAppInfoStep();

function handleSelection(target: number) {
  useAppInfoStepStore.step = target;
}
</script>
