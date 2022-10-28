<template>
  <div>
    <SideBar
      title="Sections"
      :options="options"
      :handle-selection="handleSelection"
    />
  </div>
</template>

<script setup lang="ts">
import SideBar from '@core-public/components/navbar/SideBar.vue';
import { reactive } from 'vue';

const options = ['Completing Application'];

const state = reactive({
  step: 0,
});

function handleSelection(target: number) {
  state.step = target;
}
</script>

<style lang="scss" scoped></style>
