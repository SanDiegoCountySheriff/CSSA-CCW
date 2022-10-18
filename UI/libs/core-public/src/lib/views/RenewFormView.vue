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
      <RenewFormContainer />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import RenewFormContainer from '@core-public/components/containers/RenewFormContainer.vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';

const completeApplicationStore = useCompleteApplicationStore();

const { isLoading } = useQuery(
  ['completeApplication'],
  completeApplicationStore.getCompleteApplicationFromApi
);
</script>
