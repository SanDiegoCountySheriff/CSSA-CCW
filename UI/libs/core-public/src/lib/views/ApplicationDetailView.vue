<template>
  <div>
    <v-row class="mt-5">
      <v-col
        cols="12"
        lg="8"
      >
        <ApplicationTable
          :headers="state.headers"
          :items="state.application"
          :is-loading="!!state.application"
        />
        <v-container class="item-container">
          <v-sheet class="rounded">
            <v-timeline
              dense
              class="h-full"
            >
              <v-timeline-item
                v-for="(item, index) in state.application[0].history"
                :key="index"
                color="accent"
                small
              >
                <ul class="text-left">
                  <li>{{ item.change }}</li>
                  <li>
                    {{ new Date(item.changeDateTimeUtc).toLocaleString() }}
                  </li>
                  <li>{{ item.changeMadeBy }}</li>
                </ul>
              </v-timeline-item>
            </v-timeline>
          </v-sheet>
        </v-container>
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
                  small
                  color="info"
                  :disabled="
                    applicationStore.completeApplication.application.isComplete
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
                    !applicationStore.completeApplication.application.isComplete
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
                    !applicationStore.completeApplication.application.isComplete
                  "
                  v-bind="attrs"
                  v-on="on"
                  @click="handlRenewApplication"
                >
                  {{ $t('Renew Application') }}
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
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import ApplicationTable from '@core-public/components/tables/ApplicationTable.vue';
import Routes from '@core-public/router/routes';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

const applicationStore = useCompleteApplicationStore();
const router = useRouter();

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
    { text: 'LAST UPDATED', value: 'updated' },
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
const createMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    router.push(Routes.RENEW_APPLICATION_ROUTE_PATH);
  },
  onError: () => null,
});

function handleContinueApplication() {
  if (
    applicationStore.completeApplication.application.applicationType ===
      'standard' ||
    applicationStore.completeApplication.application.applicationType ===
      'judicial' ||
    applicationStore.completeApplication.application.applicationType ===
      'reserve'
  ) {
    router.push(Routes.FORM_ROUTE_PATH);
  } else {
    router.push(Routes.RENEW_FORM_ROUTE_PATH);
  }
}

function handleModifyApplication() {
  applicationStore.completeApplication.application.currentStep = 1;
  applicationStore.completeApplication.application.isComplete = false;
  applicationStore.completeApplication.application.status = 1;
  router.push(Routes.RENEW_FORM_ROUTE_PATH);
}

function handlRenewApplication() {
  applicationStore.completeApplication.id = window.crypto.randomUUID();
  applicationStore.completeApplication.application.currentStep = 1;
  applicationStore.completeApplication.application.isComplete = false;
  applicationStore.completeApplication.application.status = 1;
  applicationStore.completeApplication.application.applicationType =
    'renew-standard';
  createMutation.mutate();
}

onMounted(() => {
  state.application[0].history.reverse();
});
</script>

<style scoped lang="scss">
.item-container {
  height: 50vh;
  overflow-y: scroll;
}
</style>
