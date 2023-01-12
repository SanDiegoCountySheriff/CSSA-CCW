<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <PermitCard1 />
    <PermitCard2 />
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="8"
        sm="12"
      >
        <v-card>
          <v-tabs
            :v-model="stepIndex + 1"
            class="fixed-tabs-bar"
            color="blue"
            center-active
            grow
          >
            <v-tabs-slider color="blue1"></v-tabs-slider>
            <v-tab
              v-for="(item, index) in state.items"
              class="nav_tab"
              :key="index"
              @click="stepIndex = index + 1"
              @keydown="stepIndex = index + 1"
            >
              <span>
                {{ item }}
              </span>
            </v-tab>
            <v-progress-linear
              :active="isLoading"
              :indeterminate="isLoading"
              absolute
              bottom
              color="primary"
              title="Permit details loading"
            >
            </v-progress-linear>
          </v-tabs>
          <div v-if="!isLoading && !isError">
            <div
              v-for="(item, index) in state.items"
              :key="index"
            >
              <v-form
                ref="form"
                v-model="valid"
                class="ml-4"
                lazy-validation
              >
                <v-container class="permit-form">
                  <v-row dense>
                    <v-col cols="12">
                      <v-stepper
                        v-model="stepIndex"
                        class="elevation-0 pb-0"
                        vertical
                        rounded
                      >
                        <v-stepper-step
                          :complete="stepIndex > 1"
                          editable
                          color="blue"
                          :step="index + 1"
                        >
                          {{ item }}
                        </v-stepper-step>

                        <v-stepper-content :step="index + 1">
                          <component :is="renderTabs(item)" />
                          <v-row class="mt-4 mb-4">
                            <v-col
                              cols="12"
                              md="5"
                              sm="12"
                            >
                              <v-btn
                                min-width="200"
                                small
                                class="mr-4"
                                @click="handleBackStep"
                              >
                                {{ $t('BACK') }}
                              </v-btn>
                            </v-col>
                            <v-col
                              cols="12"
                              md="5"
                              sm="12"
                            >
                              <v-btn
                                color="blue"
                                class="white--text ml-4"
                                min-width="200"
                                @click="handleNextStep"
                                :disabled="!valid"
                                small
                              >
                                {{ $t('NEXT') }}
                              </v-btn>
                            </v-col>
                          </v-row>
                          <v-spacer></v-spacer>
                        </v-stepper-content>
                      </v-stepper>
                    </v-col>
                  </v-row>
                </v-container>
              </v-form>
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
      </v-col>
      <v-col
        cols="12"
        md="4"
        sm="12"
      >
        <PermitStatus />
      </v-col>
    </v-row>
  </div>
</template>
<script setup lang="ts">
import AddressInfoTab from './tabs/AddressInfoTab.vue';
import AliasesTab from './tabs/AliasesTab.vue';
import ApplicationInfoTab from './tabs/ApplicantInfoTab.vue';
import AttachedDocumentsTab from './tabs/AttachedDocumentsTab.vue';
import BirthInformationTab from './tabs/BirthInformationTab.vue';
import ContactInfoTab from './tabs/ContactInfoTab.vue';
import DemographicsTab from './tabs/DemographicsTab.vue';
import ImmigrationInfoTab from './tabs/ImmigrationInfoTab.vue';
import PermitCard1 from '../permit-cards/PermitCard1.vue';
import PermitCard2 from '../permit-cards/PermitCard2.vue';
import PermitStatus from '../permit-status/PermitStatus.vue';
import SurveyInfoTab from './tabs/SurveyInfoTab.vue';
import WeaponsTab from './tabs/WeaponsTab.vue';
import WorkInfoTab from './tabs/WorkInfoTab.vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { onBeforeRouteUpdate, useRoute } from 'vue-router/composables';
import { reactive, ref } from 'vue';

const permitStore = usePermitsStore();
const route = useRoute();

const { isLoading, isError } = useQuery(
  ['permitDetail', route.params.orderId],
  () => permitStore.getPermitDetailApi(route.params.orderId),
  { refetchOnMount: 'always' }
);

const { refetch: queryPermitDetails } = useQuery(
  ['setPermitsDetails'],
  permitStore.updatePermitDetailApi,
  {
    enabled: false,
  }
);

const stepIndex = ref(1);
const valid = ref(false);

const state = reactive({
  tab: 0,
  items: [
    'Applicant Details',
    'Aliases',
    'Birth Details',
    'Immigration',
    'Demographics',
    'Contact Details',
    'Address Details',
    'Employer Details',
    'Weapons',
    'Survey Details',
    'Documents',
  ],
});

function handleNextStep() {
  queryPermitDetails();
  stepIndex.value++;
}

function handleBackStep() {
  stepIndex.value--;
}

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.orderId !== from.params.orderId) {
    /* Todo if needed :'New application call here'); */
    permitStore.getPermitDetailApi(to.params.orderId);
  }
});

const renderTabs = item => {
  switch (item) {
    case 'Aliases':
      return AliasesTab;
    case 'Birth Details':
      return BirthInformationTab;
    case 'Immigration':
      return ImmigrationInfoTab;
    case 'Demographics':
      return DemographicsTab;
    case 'Contact Details':
      return ContactInfoTab;
    case 'Address Details':
      return AddressInfoTab;
    case 'Employer Details':
      return WorkInfoTab;
    case 'Weapons':
      return WeaponsTab;
    case 'Survey Details':
      return SurveyInfoTab;
    case 'Documents':
      return AttachedDocumentsTab;
    default:
      return ApplicationInfoTab;
  }
};
</script>
<style lang="scss" scoped>
.v-application--is-ltr .v-stepper--vertical .v-stepper__content {
  border-left: 1px solid rgba(0, 0, 0, 0.12) !important;
}
.fixed-tabs-bar {
  position: -webkit-sticky;
  position: sticky;
  top: 8.5rem;
  z-index: 7;

  .v-tabs-bar__content {
    padding-top: 15px;
  }
}
</style>
