<template>
  <div>
    <v-row class="mt-5">
      <v-col
        cols="12"
        lg="9"
      >
        <ApplicationTable
          :headers="state.headers"
          :items="state.application"
          :is-loading="!!state.application"
        />
      </v-col>
      <v-col
        cols="12"
        lg="3"
      >
        <v-card>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="info"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    1
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleContinueApplication"
                >
                  {{ $t('Continue Application') }}
                </v-btn>
              </template>
              <span>{{
                $t(' You can only continue incomplete applications.')
              }}</span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="info"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleModifyApplication"
                >
                  {{ $t('Modify Application') }}
                </v-btn>
              </template>
              <span>
                {{
                  $t(` With modify make sure to change anything that need to be changed. Then make sure to
                    check the correct application type in step 7 `)
                }}
              </span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="info"
                  :disabled="
                    applicationStore.completeApplication.application.status !==
                    6
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handleRenewApplication"
                >
                  {{ $t('Renew Application') }}
                </v-btn>
              </template>
              <span>
                {{
                  $t(` With a Renewal Application make sure to change anything that needs to be changed. Then
                  make sure to check the correct application type in step 7 `)
                }}
              </span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <a
                  :href="brand.liveScanURL"
                  target="_blank"
                >
                  <v-btn
                    small
                    color="info"
                    :disabled="
                      applicationStore.completeApplication.application
                        .status === 1 ||
                      applicationStore.completeApplication.application.status >
                        2
                    "
                    v-bind="attrs"
                    v-on="on"
                  >
                    {{ $t('Download Livescan form') }}
                  </v-btn>
                </a>
              </template>
              <span>
                {{
                  $t(
                    'Download the livescan form that you will take with you to the appointment.'
                  )
                }}
              </span>
            </v-tooltip>
          </v-card-text>
          <v-card-text>
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <v-btn
                  small
                  color="error"
                  v-bind="attrs"
                  v-on="on"
                  @click="router.back()"
                >
                  {{ $t('Go Back') }}
                </v-btn>
              </template>
              <span>
                {{ $t('Go back to application list') }}
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
import Routes from '@core-public/router/routes';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRoute, useRouter } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

const applicationStore = useCompleteApplicationStore();
const brandStore = useBrandStore();
const router = useRouter();
const route = useRoute();

const brand = brandStore.getBrand;

const state = reactive({
  application: [applicationStore.completeApplication],
  headers: [
    {
      text: 'ORDER ID',
      align: 'start',
      sortable: false,
      value: 'orderId',
    },
    {
      text: 'COMPLETED',
      value: 'completed',
    },
    {
      text: 'CURRENT STATUS',
      value: 'status',
    },
    {
      text: 'APPOINTMENT STATUS',
      value: 'appointmentStatus',
    },
    {
      text: 'APPOINTMENT DATE',
      value: 'appointmentDate',
    },
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

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.orderId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res);
      });
  }
});

const createMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        orderId: state.application[0].application.orderId,
        isComplete: state.application[0].application.isComplete,
      },
    });
  },
  onError: () => null,
});

const renewMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        orderId: state.application[0].application.orderId,
        isComplete: state.application[0].application.isComplete,
      },
    });
  },
  onError: () => null,
});

function handleContinueApplication() {
  if (applicationStore.completeApplication.application.currentStep === 0) {
    router.push({
      path: Routes.APPLICATION_ROUTE_PATH,
      query: {
        orderId: state.application[0].application.orderId,
        isComplete: state.application[0].application.isComplete,
      },
    });
  } else if (
    applicationStore.completeApplication.application.applicationType ===
      'standard' ||
    applicationStore.completeApplication.application.applicationType ===
      'judicial' ||
    applicationStore.completeApplication.application.applicationType ===
      'reserve'
  ) {
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        orderId: state.application[0].application.orderId,
        isComplete: state.application[0].application.isComplete,
      },
    });
  } else {
    router.push({
      path: Routes.RENEW_FORM_ROUTE_PATH,
      query: {
        orderId: state.application[0].application.orderId,
        isComplete: state.application[0].application.isComplete,
      },
    });
  }
}

function handleModifyApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID();
  applicationStore.completeApplication.application.currentStep = 1;
  applicationStore.completeApplication.application.isComplete = false;
  applicationStore.completeApplication.application.appointmentStatus = false;
  applicationStore.completeApplication.application.status = 1;
  applicationStore.completeApplication.application.applicationType = `modify-${applicationStore.completeApplication.application.applicationType}`;
  renewMutation.mutate();
}

function handleRenewApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID();
  applicationStore.completeApplication.application.currentStep = 1;
  applicationStore.completeApplication.application.isComplete = false;
  applicationStore.completeApplication.application.appointmentStatus = false;
  applicationStore.completeApplication.application.status = 1;
  applicationStore.completeApplication.application.applicationType = `renew-${applicationStore.completeApplication.application.applicationType}`;
  createMutation.mutate();
}
</script>

<style scoped lang="scss">
.item-container {
  max-height: 50vh;
  overflow-y: scroll;
  margin-top: 1em;
}
</style>
