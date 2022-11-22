<template>
  <div class="application">
    <img
      v-if="store.getDocuments.agencyLandingPageImage"
      alt="Application logo"
      :src="store.getDocuments.agencyLandingPageImage"
    />
    <v-container v-if="!state.selected">
      <v-sheet>
        <v-row class="selections">
          <v-col
            cols="12"
            lg="6"
          >
            <v-container
              v-if="isLoading || (!state.showApplications && !isError)"
            >
              <v-skeleton-loader
                fluid
                class="fill-height"
                type="list-item"
              >
              </v-skeleton-loader>
            </v-container>
            <v-card
              class="ml-5"
              v-else
            >
              <v-card-title>
                {{ $t('In progress applications') }}
              </v-card-title>
              <v-list v-if="!isLoading">
                <v-list-item
                  v-if="
                    state.showApplications && state.applications.length === 0
                  "
                >
                  <v-list-item-content>
                    {{
                      $t(' No previous applications. Please create a new one')
                    }}
                  </v-list-item-content>
                </v-list-item>
                <v-list-item
                  v-for="(app, index) in state.applications"
                  :key="index"
                  v-else
                >
                  <v-btn
                    :color="$vuetify.theme.dark ? 'info' : 'primary'"
                    large
                    @click="handleSelectedApplication(app)"
                  >
                    <div class="button-content">
                      <span>
                        {{ $t('Order id: ') }}{{ app.application?.orderId }}
                      </span>
                      <span>
                        {{ $t('Status: ')
                        }}{{
                          app.application?.isComplete
                            ? $t('Complete')
                            : $t('In progress')
                        }}
                      </span>
                    </div>
                  </v-btn>
                </v-list-item>
              </v-list>
            </v-card>
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-btn
              color="primary"
              class="mt-5"
              @click="handleCreateApplication"
            >
              {{ $t('New Application') }}
            </v-btn>
          </v-col>
        </v-row>
      </v-sheet>
    </v-container>
    <AcknowledgementContainer
      v-if="state.selected"
      :next-route="Routes.FORM_ROUTE_PATH"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import Routes from '@core-public/router/routes';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import { reactive } from 'vue';
import { useApplicationTypeStore } from '@core-public/stores/applicationTypeStore';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@core-public/stores/brandStore';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation, useQuery } from '@tanstack/vue-query';

const applicationTypeStore = useApplicationTypeStore();
const store = useBrandStore();
const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();

const state = reactive({
  selected: false,
  applications: [] as Array<CompleteApplication>,
  showApplications: false,
});

const { isLoading, isError } = useQuery(['getIncompleteApplications'], () => {
  const res = applicationStore.getCompleteApplicationFromApi(
    authStore.auth.userEmail,
    false
  );

  res
    .then(data => {
      state.applications.push(data);
      state.showApplications = true;
    })
    .catch(err => {
      window.console.log(err);
      state.showApplications = true;
    });
});

const createMutation = useMutation({
  mutationFn: applicationStore.createApplication,
  onSuccess: () => {
    state.selected = true;
  },
  onError: error => {
    alert(error);
  },
});

async function handleCreateApplication() {
  applicationTypeStore.state.type = 'new';
  applicationStore.completeApplication.application.userEmail =
    authStore.auth.userEmail;
  applicationStore.completeApplication.id = window.crypto.randomUUID();
  applicationStore.completeApplication.application.currentStep = 1;
  createMutation.mutate();
}

function handleSelectedApplication(selected: CompleteApplication) {
  applicationStore.setCompleteApplication(selected);
  state.selected = true;
}
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
.selections {
  width: 100%;
}

.button-content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
</style>
