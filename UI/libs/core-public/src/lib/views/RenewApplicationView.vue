<template>
  <div class="application">
    <img
      v-if="store.getDocuments.agencyLogo"
      alt="Application logo"
      :src="store.getDocuments.agencyLogo"
    />
    <v-container v-if="!state.selected">
      <v-sheet>
        <v-row>
          <v-col
            lg="5"
            sm="1"
          >
            <v-container v-if="isLoading">
              <v-skeleton-loader
                fluid
                type="list-item"
              >
              </v-skeleton-loader>
            </v-container>
            <v-card v-else>
              <v-card-title>
                {{ $t('Select Application to modify') }}
              </v-card-title>
              <v-list>
                <v-list-item
                  v-if="
                    !state.showApplications && state.applications.length === 0
                  "
                >
                  <v-list-item-content>
                    {{
                      $t(
                        ' No previously completed applications. Please go back to new applications'
                      )
                    }}
                  </v-list-item-content>
                </v-list-item>
                <v-list-item
                  v-for="app in state.applications"
                  :key="app.id"
                  v-else
                >
                  <v-btn
                    outlined
                    color="primary"
                    @click="handleSelectedApplication(app)"
                  >
                    {{ $t('Order id: ') }}{{ app.application.orderId }} -
                    {{ $t('Status: ')
                    }}{{
                      app.application.isComplete
                        ? $t('completed')
                        : $t('In progress')
                    }}
                  </v-btn>
                </v-list-item>
              </v-list>
            </v-card>
          </v-col>
          <v-col
            lg="4"
            sm="1"
          >
            <v-btn
              color="primary"
              class="mt-5"
              @click="handleRenewApplication"
            >
              {{ $t(' Create Renewal Application') }}
            </v-btn>
          </v-col>
        </v-row>
      </v-sheet>
    </v-container>
    <AcknowledgementContainer
      v-if="state.selected"
      :next-route="'/renew-form'"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import { reactive } from 'vue';
import { unformatNumber } from '@shared-utils/formatters/defaultFormatters';
import { useApplicationTypeStore } from '@core-public/stores/applicationTypeStore';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@core-public/stores/brandStore';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation, useQuery } from '@tanstack/vue-query';

const applicationStore = useCompleteApplicationStore();
const applicationTypeStore = useApplicationTypeStore();
const authStore = useAuthStore();
const store = useBrandStore();
const completeApplication = applicationStore.completeApplication.application;

const state = reactive({
  selected: false,
  applications: [] as Array<CompleteApplication>,
  showApplications: false,
});

const { isLoading } = useQuery(['getCompleteApplications'], () => {
  const res = applicationStore.getCompleteApplicationFromApi(
    authStore.auth.userEmail,
    true
  );

  res.then(data => {
    state.applications.push(data);
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

function handleSelectedApplication(selected) {
  selected.application.contact.primaryPhoneNumber = unformatNumber(
    selected.application.contact.primaryPhoneNumber
  );
  selected.application.contact.cellPhoneNumber = unformatNumber(
    selected.application.contact.cellPhoneNumber
  );
  selected.application.contact.workPhoneNumber = unformatNumber(
    selected.application.contact.workPhoneNumber
  );
  selected.application.contact.faxPhoneNumber = unformatNumber(
    selected.application.contact.faxPhoneNumber
  );
  selected.application.personalInfo.ssn = unformatNumber(
    selected.application.personalInfo.ssn
  );
  applicationStore.setCompleteApplication(selected);
  applicationTypeStore.state.type = 'modify';
  state.selected = true;
}

async function handleRenewApplication() {
  applicationTypeStore.state.type = 'renewal';
  completeApplication.userEmail = authStore.auth.userEmail;
  applicationStore.completeApplication.id = window.crypto.randomUUID();
  createMutation.mutate();
}
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
