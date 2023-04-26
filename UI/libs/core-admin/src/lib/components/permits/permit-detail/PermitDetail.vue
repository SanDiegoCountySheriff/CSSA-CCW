<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <PermitCard1 />
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <PermitCard2 />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="8"
        class="pt-0 pr-0"
      >
        <v-card
          :loading="isUpdatePermitLoading || isLoading"
          min-height="500"
          outlined
        >
          <v-tabs
            v-model="state.tab"
            :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
            center-active
            grow
          >
            <v-tabs-slider color="primary"></v-tabs-slider>
            <v-tab
              v-for="(item, index) in state.items"
              :key="index"
              @click="stepIndex = index + 1"
              @keydown="stepIndex = index + 1"
            >
              {{ item }}
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

          <v-tabs-items
            v-model="state.tab"
            v-if="!isLoading"
          >
            <v-tab-item
              v-for="(item, index) in state.items"
              :key="index"
            >
              <component
                @on-save="handleSave"
                :is="renderTabs(item)"
              />
            </v-tab-item>
          </v-tabs-items>
        </v-card>
      </v-col>
      <v-col
        cols="4"
        class="pt-0"
      >
        <PermitStatus />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import AddressInfoTab from './tabs/AddressInfoTab.vue'
import AliasesTab from './tabs/AliasesTab.vue'
import ApplicationInfoTab from './tabs/ApplicantInfoTab.vue'
import AttachedDocumentsTab from './tabs/AttachedDocumentsTab.vue'
import BirthInformationTab from './tabs/BirthInformationTab.vue'
import ContactInfoTab from './tabs/ContactInfoTab.vue'
import DemographicsTab from './tabs/DemographicsTab.vue'
import ImmigrationInfoTab from './tabs/ImmigrationInfoTab.vue'
import PermitCard1 from '../permit-cards/PermitCard1.vue'
import PermitCard2 from '../permit-cards/PermitCard2.vue'
import PermitStatus from '../permit-status/PermitStatus.vue'
import SurveyInfoTab from './tabs/SurveyInfoTab.vue'
import WeaponsTab from './tabs/WeaponsTab.vue'
import WorkInfoTab from './tabs/WorkInfoTab.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { onBeforeRouteUpdate, useRoute } from 'vue-router/composables'
import { reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const permitStore = usePermitsStore()
const themeStore = useThemeStore()
const route = useRoute()

const { isLoading } = useQuery(
  ['permitDetail', route.params.orderId],
  () => permitStore.getPermitDetailApi(route.params.orderId),
  { refetchOnMount: 'always' }
)

const stepIndex = ref(1)
const valid = ref(false)

const state = reactive({
  tab: null,
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
  updatedSection: '',
})

const { isLoading: isUpdatePermitLoading, mutate: setPermitDetails } =
  useMutation(['setPermitsDetails'], () =>
    permitStore.updatePermitDetailApi(state.updatedSection)
  )

function handleSave(item: string) {
  state.updatedSection = `Updated ${item}`
  setPermitDetails()
}

onBeforeRouteUpdate(async (to, from) => {
  if (to.params.orderId !== from.params.orderId) {
    /* Todo if needed :'New application call here'); */
    permitStore.getPermitDetailApi(to.params.orderId)
  }
})

const renderTabs = item => {
  switch (item) {
    case 'Aliases':
      return AliasesTab
    case 'Birth Details':
      return BirthInformationTab
    case 'Immigration':
      return ImmigrationInfoTab
    case 'Demographics':
      return DemographicsTab
    case 'Contact Details':
      return ContactInfoTab
    case 'Address Details':
      return AddressInfoTab
    case 'Employer Details':
      return WorkInfoTab
    case 'Weapons':
      return WeaponsTab
    case 'Survey Details':
      return SurveyInfoTab
    case 'Documents':
      return AttachedDocumentsTab
    default:
      return ApplicationInfoTab
  }
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
