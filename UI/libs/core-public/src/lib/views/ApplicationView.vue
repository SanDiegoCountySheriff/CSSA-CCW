<template>
  <div class="application">
    <img
      v-if="store.getDocuments.agencyLogo"
      alt="Application logo"
      :src="store.getDocuments.agencyLogo"
    />
    <v-container v-if="!state.selected">
      <v-sheet>
        <v-row class="selections">
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
            <v-card
              class="ml-5"
              v-else
            >
              <v-card-title>
                {{ $t('In progress applications') }}
              </v-card-title>
              <v-list v-if="!isLoading">
                <v-list-item v-if="state.applications.length === 0">
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
                    outlined
                    color="primary"
                    @click="handleSelectedApplication(app)"
                  >
                    {{ $t('Order id: ') }}{{ app.application.orderId }} -
                    {{ $t('Status: ')
                    }}{{
                      app.application.isComplete
                        ? $t('Complete')
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
      :next-route="'/form'"
    />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import { reactive } from 'vue';
import { useApplicationTypeStore } from '@core-public/stores/applicationTypeStore';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@core-public/stores/brandStore';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation, useQuery } from '@tanstack/vue-query';
import { unformatNumber } from '@shared-utils/formatters/defaultFormatters';

const applicationTypeStore = useApplicationTypeStore();
const store = useBrandStore();
const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();

const state = reactive({
  selected: false,
  applications: [] as any,
});

const { isLoading } = useQuery(['getIncompleteApplications'], () => {
  const res = applicationStore.getCompleteApplicationFromApi(
    authStore.auth.userEmail,
    false
  );

  res.then(data => {
    state.applications.push(data);
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
  state.selected = true;
}
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
.selections {
  width: 80vw;
}
</style>
