<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <div class="applications-table mt-5">
    <v-container
      v-if="isLoading && !isError && state.dataLoaded"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-row v-else>
      <v-col
        cols="12"
        lg="8"
      >
        <ApplicationTable
          :headers="state.headers"
          :items="state.applications"
          :is-loading="state.dataLoaded"
          @selected="handleSelection"
        />
      </v-col>
      <v-col
        cols="12"
        lg="4"
      >
        <v-card>
          <v-card-text>
            <v-btn
              small
              color="accent"
              @click="handleCreateApplication"
            >
              {{ $t('Create Application') }}
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import ApplicationTable from '@core-public/components/tables/ApplicationTable.vue';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import Routes from '@core-public/router/routes';
import { reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { useMutation, useQuery } from '@tanstack/vue-query';

const {
  getAllUserApplications,
  getCompleteApplicationFromApi,
  setCompleteApplication,
  createApplication,
  completeApplication,
} = useCompleteApplicationStore();
const authStore = useAuthStore();

const router = useRouter();

const state = reactive({
  applications: [] as Array<CompleteApplication>,
  dataLoaded: false,
  search: '',
  headers: [
    {
      text: 'ORDER ID',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    { text: 'COMPLETED', value: 'completed' },
    { text: 'CURRENT STATUS', value: 'status' },
    { text: 'APPOINTMENT STATUS', value: 'appointmentStatus' },
    {
      text: 'CURRENT STEP',
      value: 'step',
    },
    {
      text: 'APPLICATION TYPE',
      value: 'type',
    },
  ],
});

const { isLoading, isError } = useQuery(['getApplicationsByUser'], () => {
  getAllUserApplications(authStore.auth.userEmail).then(data => {
    state.applications = data;
    state.dataLoaded = true;
  });
});

const createMutation = useMutation({
  mutationFn: createApplication,
  onSuccess: () => {
    router.push(Routes.APPLICATION_ROUTE_PATH);
  },
  onError: () => null,
});

function handleSelection(application) {
  getCompleteApplicationFromApi(
    application.application.orderId,
    application.application.isComplete
  ).then(data => {
    setCompleteApplication(data);
    router.push(Routes.APPLICATION_DETAIL_ROUTE);
  });
}

async function handleCreateApplication() {
  completeApplication.application.userEmail = authStore.auth.userEmail;
  completeApplication.id = window.crypto.randomUUID();
  completeApplication.application.currentStep = 1;
  completeApplication.application.applicationType = 'standard';
  createMutation.mutate();
}
</script>
