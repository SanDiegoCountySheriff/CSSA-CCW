<template>
  <div>
    <v-container
      v-if="isLoading"
      fluid
    >
      <v-skeleton-loader fluid />
    </v-container>
    <v-container
      v-else
      fluid
    >
      <SecondFormContainer />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import SecondFormContainer from '@core-public/components/containers/SecondFormContainer.vue';
import { useQuery } from '@tanstack/vue-query';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';

const completeApplicationStore = useCompleteApplicationStore();

const { isLoading } = useQuery(
  ['completeApplication'],
  completeApplicationStore.getCompleteApplicationFromApi
);
</script>

<style lang="scss" scoped></style>
