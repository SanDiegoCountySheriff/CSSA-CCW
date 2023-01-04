<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<!-- eslint-disable vue/valid-v-slot -->
<!-- eslint-disable vue-a11y/no-autofocus -->
<template>
  <div class="applications-table mt-5">
    <v-container
      v-if="isLoading && !isError && !state.dataLoaded"
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
          @delete="handleDelete"
        />
      </v-col>
      <v-col
        cols="12"
        lg="4"
      >
        <v-card>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  :disabled="state.hasIncomplete"
                  small
                  color="accent"
                  @click="handleCreateApplication"
                  v-bind="attrs"
                  v-on="on"
                >
                  {{ $t('Create New Application') }}
                </v-btn>
              </template>
              <span v-if="state.hasIncomplete">
                {{
                  $t(
                    'Cannot create a new application with a incomplete application'
                  )
                }}
              </span>
              <span v-else
                >{{
                  $t(
                    ' Create a new blank application. Do not use for modifications or renewals'
                  )
                }}
              </span>
            </v-tooltip>
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
import { defaultPermitState } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { useMutation, useQuery } from '@tanstack/vue-query';

const {
  getAllUserApplicationsApi,
  getCompleteApplicationFromApi,
  createApplication,
  completeApplication,
  setCompleteApplication,
  deleteApplication,
} = useCompleteApplicationStore();
const authStore = useAuthStore();

const router = useRouter();

const state = reactive({
  applications: [] as Array<CompleteApplication>,
  dataLoaded: false,
  hasIncomplete: false,
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
    {
      text: '',
      value: 'delete',
    },
  ],
});

const { isLoading, isError } = useQuery(
  ['getApplicationsByUser'],
  getAllUserApplicationsApi,
  {
    onSuccess: data => {
      state.applications = data;
      data.forEach((item: CompleteApplication) => {
        if (item.application.status === 1) {
          state.hasIncomplete = true;
        }
      });
      state.dataLoaded = true;
    },
    onError: () => {
      state.dataLoaded = true;
    },
    refetchOnMount: 'always',
    refetchOnWindowFocus: 'always',
  }
);

const createMutation = useMutation({
  mutationFn: createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        orderId: completeApplication.application.orderId,
        isComplete: completeApplication.application.isComplete,
      },
    });
  },
  onError: () => null,
});

function handleDelete(orderId) {
  deleteApplication(orderId);
  const filterd = state.applications.filter(item => {
    return item.application.orderId !== orderId;
  });

  state.applications = filterd;
  state.hasIncomplete = false;
  state.applications.forEach(item => {
    if (item.application.status === 1) {
      state.hasIncomplete = true;
    }
  });
}

function handleSelection(application) {
  getCompleteApplicationFromApi(
    application.application.orderId,
    application.application.isComplete
  ).then(data => {
    setCompleteApplication(data);
    router.push({
      path: Routes.APPLICATION_DETAIL_ROUTE,
      query: {
        orderId: completeApplication.application.orderId,
        isComplete: completeApplication.application.isComplete,
      },
    });
  });
}

function handleCreateApplication() {
  //make sure the application is blank
  setCompleteApplication(defaultPermitState);
  completeApplication.application.appointmentDateTime = new Date(
    2001,
    1,
    1
  ).toISOString();
  completeApplication.application.userEmail = authStore.auth.userEmail;
  completeApplication.id = window.crypto.randomUUID();
  completeApplication.application.currentStep = 0;
  completeApplication.application.applicationType = 'standard';
  createMutation.mutate();
}
</script>
