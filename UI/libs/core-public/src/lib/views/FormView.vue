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
      <FormContainer />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import FormContainer from '@core-public/components/containers/FormContainer.vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';

const completeApplicationStore = useCompleteApplicationStore();

const { isLoading } = useQuery(
  ['completeApplication'],
  completeApplicationStore.getCompleteApplicationFromApi
);
</script>
