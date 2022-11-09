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
      </v-tabs>
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
    </v-card>
  </div>
</template>
<script setup lang="ts">
import AddressInfoTab from './tabs/AddressInfoTab.vue';
import AliasesTab from './tabs/AliasesTab.vue';
import ApplicationInfoTab from './tabs/ApplicantInfoTab.vue';
import AppointmentInfoTab from './tabs/AppointmentInfoTab.vue';
import BirthInformationTab from './tabs/BirthInformationTab.vue';
import ContactInfoTab from './tabs/ContactInfoTab.vue';
import DemographicsTab from './tabs/DemographicsTab.vue';
import RequestReasonTab from './tabs/RequestReasonTab.vue';
import SurveyInfoTab from './tabs/SurveyInfoTab.vue';
import { reactive } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { onBeforeRouteUpdate, useRoute } from 'vue-router/composables';

const { getPermitDetailApi } = usePermitsStore();
const route = useRoute();

const { data } = useQuery(['permitDetail', route.params.orderId], () =>
  getPermitDetailApi(route.params.orderId)
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
    'Survey Info',
    'Appointment Info',
  ],
  text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.',
});

window.console.log(data);

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
    case 'Survey Info':
      return SurveyInfoTab;
    case 'Appointment Info':
      return AppointmentInfoTab;
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
}
</style>
