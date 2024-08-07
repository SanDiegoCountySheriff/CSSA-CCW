<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-row v-if="permitStore.viewingHistorical">
      <v-col class="pb-0">
        <v-alert
          class="mb-1"
          type="info"
          outlined
        >
          Application Audit View. This application audit history was created
          {{
            permitStore.permitDetail.historicalDate
              ? new Date(
                  permitStore.permitDetail.historicalDate
                ).toLocaleString()
              : ''
          }}
        </v-alert>
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <PermitCard1 />
      </v-col>
    </v-row>

    <v-row>
      <v-col cols="12">
        <PermitCard2
          :is-loading="isLoading || isFetching"
          :user-photo="state.userPhoto"
          @refetch="refetch"
          @on-check-name="handleCheckName"
          @on-check-address="handleCheckAddress"
          @on-check-weapons="handleCheckWeapons"
          @on-check-documents="handleCheckDocuments"
          @on-check-questions="handleCheckQuestions"
        />
      </v-col>
    </v-row>
    <v-row>
      <v-col
        cols="8"
        class="pt-0 pr-0"
      >
        <v-card
          :loading="isUpdatePermitLoading || isLoading || isFetching"
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
              :active="isLoading || isFetching"
              :indeterminate="isLoading || isFetching"
              absolute
              bottom
              color="primary"
              title="Permit details loading"
            >
            </v-progress-linear>
          </v-tabs>

          <v-tabs-items
            v-model="state.tab"
            v-if="!isLoading && !isFetching"
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
        <PermitStatus :is-loading="isLoading || isFetching" />
      </v-col>
    </v-row>

    <v-btn
      @click="reveal = !reveal"
      color="primary"
      fab
      bottom
      right
      fixed
      x-large
    >
      <v-icon>
        {{ reveal ? 'mdi-comment-minus-outline' : 'mdi-comment-plus-outline' }}
      </v-icon>
    </v-btn>

    <v-sheet
      v-if="reveal"
      rounded
      outlined
      color="primary"
      class="sticky-card"
      elevation="20"
      width="450"
    >
      <v-card
        class="card-overflow"
        outlined
        max-height="650"
      >
        <v-card-title>Comments</v-card-title>
        <v-card-text class="card-text-overflow">
          <CommentsTab />
        </v-card-text>
      </v-card>
    </v-sheet>
  </v-container>
</template>

<script setup lang="ts">
import AddressInfoTab from './tabs/AddressInfoTab.vue'
import AdminDocumentsTab from './tabs/AdminDocumentsTab.vue'
import AliasesTab from './tabs/AliasesTab.vue'
import ApplicationInfoTab from './tabs/ApplicantInfoTab.vue'
import AttachedDocumentsTab from './tabs/AttachedDocumentsTab.vue'
import BirthInformationTab from './tabs/BirthInformationTab.vue'
import CommentsTab from '../permit-detail/tabs/CommentsTab.vue'
import ContactInfoTab from './tabs/ContactInfoTab.vue'
import DemographicsTab from './tabs/DemographicsTab.vue'
import ImmigrationInfoTab from './tabs/ImmigrationInfoTab.vue'
import PermitCard1 from '../permit-cards/PermitCard1.vue'
import PermitCard2 from '../permit-cards/PermitCard2.vue'
import PermitStatus from '../permit-status/PermitStatus.vue'
import SurveyInfoTab from './tabs/SurveyInfoTab.vue'
import WeaponsTab from './tabs/WeaponsTab.vue'
import WorkInfoTab from './tabs/WorkInfoTab.vue'
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useRoute } from 'vue-router/composables'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import {
  computed,
  onBeforeUnmount,
  onMounted,
  provide,
  reactive,
  ref,
} from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const permitStore = usePermitsStore()
const themeStore = useThemeStore()
const documentsStore = useDocumentsStore()
const route = useRoute()

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
    'Admin Documents',
  ],
  updatedSection: '',
  userPhoto: '',
})

onMounted(() => {
  permitStore.viewingPermitDetail = true
})

onBeforeUnmount(() => {
  permitStore.viewingPermitDetail = false
  permitStore.viewingHistorical = false
})

const { refetch: getPortrait } = useQuery(
  ['getPortrait'],
  () => documentsStore.getUserPortrait(),
  {
    enabled: false,
    onSuccess: (response: string) => {
      if (response === 'File/image does not exist') {
        state.userPhoto = ''
      } else {
        state.userPhoto = response
      }
    },
  }
)

const { isLoading, isFetching, refetch } = useQuery(
  ['permitDetail'],
  () =>
    permitStore.getPermitDetailApi(route.params.orderId, route.params.isLegacy),
  {
    refetchOnMount: 'always',
    onSuccess: () => getPortrait(),
  }
)

const readonly = computed(
  () => Boolean(route.params.isLegacy) || permitStore.viewingHistorical
)

provide('readonly', readonly)

const stepIndex = ref(1)
const reveal = ref(false)

const { isLoading: isUpdatePermitLoading, mutate: setPermitDetails } =
  useMutation(['setPermitsDetails'], () =>
    permitStore.updatePermitDetailApi(state.updatedSection)
  )

function handleSave(item: string) {
  state.updatedSection = `Updated ${item}`
  setPermitDetails()
}

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
    case 'Admin Documents':
      return AdminDocumentsTab
    default:
      return ApplicationInfoTab
  }
}

function handleCheckName() {
  state.tab = 0
}

function handleCheckAddress() {
  state.tab = 6
}

function handleCheckWeapons() {
  state.tab = 8
}

function handleCheckDocuments() {
  state.tab = 10
}

function handleCheckQuestions() {
  state.tab = 9
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}

.sticky-card {
  width: 600px;
  position: fixed;
  bottom: 10vh;
  z-index: 2;
  right: 1vw;
}

.card-overflow {
  display: flex !important;
  flex-direction: column;
}

.card-text-overflow {
  flex-grow: 1;
  overflow: auto;
}
</style>
