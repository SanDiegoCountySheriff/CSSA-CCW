<template>
  <div>
    <v-card>
      <v-tabs
        v-model="state.tab"
        class="fixed-tabs-bar"
        grow
      >
        <span
          v-for="(item, index) in state.items"
          :key="index"
        >
          <v-tab
            @click="$vuetify.goTo('#sec_' + index)"
            class="nav_tab"
          >
            {{ item }}
          </v-tab>
        </span>
        <v-progress-linear
          :active="isLoading"
          :indeterminate="isLoading"
          absolute
          bottom
          color="primary"
        >
        </v-progress-linear>
      </v-tabs>
      <div v-if="!isLoading && !isError">
        <div
          v-for="(item, index) in state.items"
          :key="index"
        >
          <v-container>
            <v-row dense>
              <v-col cols="12">
                <div :id="'sec_' + index">
                  <span
                    :id="'span_' + index"
                    v-intersect="handleIntersect"
                  ></span>
                  <component :is="renderTabs(item)" />
                </div>
              </v-col>
            </v-row>
          </v-container>
        </div>
      </div>
      <v-alert
        v-if="!isLoading && isError"
        border="right"
        colored-border
        type="error"
        class="grey--text"
        dense
      >
        {{ $t('No data available') }}
      </v-alert>
      <v-alert
        v-if="isLoading && !isError"
        class="grey--text"
        dense
      >
        {{ $t('Loading application detail') }}
      </v-alert>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import AddressInfoTab from './tabs/AddressInfoTab.vue';
import AliasesTab from './tabs/AliasesTab.vue';
import ApplicationInfoTab from './tabs/ApplicantInfoTab.vue';
import AppointmentInfoTab from './tabs/AppointmentInfoTab.vue';
import AttachedDocumentsTab from './tabs/AttachedDocumentsTab.vue';
import BackgroundCheckTab from './tabs/BackgroundCheckTab.vue';
import BirthInformationTab from './tabs/BirthInformationTab.vue';
import ContactInfoTab from './tabs/ContactInfoTab.vue';
import DemographicsTab from './tabs/DemographicsTab.vue';
import HistoryTab from './tabs/HistoryTab.vue';
import InterviewQuestionsTab from './tabs/InterviewQuestionsTab.vue';
import RequestReasonTab from './tabs/RequestReasonTab.vue';
import SurveyInfoTab from './tabs/SurveyInfoTab.vue';
import WorkInfoTab from './tabs/WorkInfoTab.vue';
import { reactive } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { onBeforeRouteUpdate, useRoute } from 'vue-router/composables';

const { getPermitDetailApi } = usePermitsStore();
const route = useRoute();

const { isLoading, isError } = useQuery(
  ['permitDetail', route.params.orderId],
  () => getPermitDetailApi(route.params.orderId)
);

const state = reactive({
  tab: 0,
  items: [
    'Request/Reason',
    'Applicant Info',
    'Aliases',
    'Birth Information',
    'Demographics',
    'Contact Info',
    'Address Info',
    'Work Info',
    'Survey Info',
    'Appointment Info',
    'Background Check',
    'Interview Questions',
    'Documents',
    'History',
  ],
});

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.orderId !== from.params.orderId) {
    window.console.log('New application call here');
  }
});

const renderTabs = item => {
  switch (item) {
    case 'Applicant Info':
      return ApplicationInfoTab;
    case 'Aliases':
      return AliasesTab;
    case 'Birth Information':
      return BirthInformationTab;
    case 'Demographics':
      return DemographicsTab;
    case 'Contact Info':
      return ContactInfoTab;
    case 'Address Info':
      return AddressInfoTab;
    case 'Work Info':
      return WorkInfoTab;
    case 'Survey Info':
      return SurveyInfoTab;
    case 'Appointment Info':
      return AppointmentInfoTab;
    case 'Background Check':
      return BackgroundCheckTab;
    case 'Interview Questions':
      return InterviewQuestionsTab;
    case 'Documents':
      return AttachedDocumentsTab;
    case 'History':
      return HistoryTab;
    default:
      return RequestReasonTab;
  }
};

function handleIntersect(entries) {
  let intersecting_element = entries[0];

  if (intersecting_element.isIntersecting) {
    let id = intersecting_element.target.id;
    let index = Number(id.split('_')[1]);

    state.tab = index;
  }
}
</script>
<style lang="scss">
/* Helper classes */

.fixed-tabs-bar.theme--light.v-tabs > .v-tabs-bar {
  background-color: #f2f2f2 !important;
}

.fixed-tabs-bar {
  position: -webkit-sticky;
  position: sticky;
  top: 4rem;
  z-index: 999;

  .v-tabs-bar__content {
    padding-top: 15px;
  }
}
</style>
