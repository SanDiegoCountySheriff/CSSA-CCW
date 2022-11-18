<template>
  <div>
    <v-container v-if="isLoading && !isError">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item"
      >
      </v-skeleton-loader>
    </v-container>
    <v-container v-if="state.id">
      <ApplicationStatusContainer />
    </v-container>
    <v-container v-if="isError">
      <v-card>
        <v-card-title>
          {{ $t(' Something went wrong ') }}
        </v-card-title>
        <v-card-text>
          {{ $t(' Refresh or return to the home page and try again') }}
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import ApplicationStatusContainer from '@core-public/components/containers/ApplicationStatusContainer.vue';
import { reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';

const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();

const state = reactive({
  id: '',
});

const { isLoading, isError } = useQuery(['getCompleteApplications'], () => {
  const res = applicationStore.getCompleteApplicationFromApi(
    authStore.auth.userEmail,
    true
  );

  res.then(data => {
    applicationStore.setCompleteApplication(data);
    state.id = applicationStore.completeApplication.id;
  });
});
</script>
