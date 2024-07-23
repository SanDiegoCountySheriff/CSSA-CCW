<template>
  <v-app>
    <v-container
      v-if="
        (isPermitsLoading ||
          isAdminUserLoading ||
          isAllAdminUsersLoading ||
          isRefundRequestLoading ||
          isGetUnmatchedUsersCountLoading ||
          isAssignedApplicationsLoading) &&
        isAuthenticated
      "
      fluid
    >
      <Loader />
    </v-container>
    <div v-else>
      <PageTemplate>
        <router-view />
      </PageTemplate>

      <v-snackbar
        color="primary"
        v-model="prompt"
      >
        {{ $t('A new version is found.') }}

        <template #action="{ attrs }">
          <v-btn
            color="primary"
            v-bind="attrs"
            @click="update"
          >
            {{ $t('Update') }}
          </v-btn>
          <v-btn
            color="primary"
            v-bind="attrs"
            @click="prompt = false"
          >
            {{ $t('Cancel') }}
          </v-btn>
        </template>
      </v-snackbar>
    </div>
  </v-app>
</template>

<script setup lang="ts">
import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import Loader from './Loader.vue'
import PageTemplate from '@core-admin/components/templates/PageTemplate.vue'
import Vue from 'vue'
import { useAdminUserStore } from '@core-admin/stores/adminUserStore'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { usePaymentStore } from '@shared-ui/stores/paymentStore'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import {
  MsalBrowser,
  getMsalInstance,
} from '@core-admin/api/auth/authentication'
import {
  computed,
  getCurrentInstance,
  onBeforeMount,
  provide,
  ref,
  watch,
} from 'vue'

const prompt = ref(false)
const app = getCurrentInstance()
const authStore = useAuthStore()
const paymentStore = usePaymentStore()
const brandStore = useBrandStore()
const configStore = useAppConfigStore()
const userStore = useUserStore()
const permitsStore = usePermitsStore()
const adminUserStore = useAdminUserStore()
const themeStore = useThemeStore()
const msalInstance = ref<MsalBrowser>()

provide(
  'msalInstance',
  computed(() => msalInstance.value)
)

const isAuthenticated = computed(() => authStore.getAuthState.isAuthenticated)
const validApiUrl = computed(
  () => configStore.appConfig.applicationApiBaseUrl.length !== 0
)
const enableAllAdminUsers = computed(
  () =>
    authStore.auth.roles.includes('CCW-ADMIN-ROLE') ||
    authStore.auth.roles.includes('CCW-SYSTEM-ADMINS-ROLE')
)
const enablePermitsEndpoints = computed(() => {
  return isAuthenticated.value && validApiUrl.value
})

useQuery(['brandSetting'], brandStore.getBrandSettingApi, {
  enabled: validApiUrl,
})

useQuery(['logo'], brandStore.getAgencyLogoDocumentsApi, {
  enabled: validApiUrl,
})

const { isLoading: isRefundRequestLoading } = useQuery(
  ['getAllRefundRequests'],
  paymentStore.getAllRefundRequests,
  {
    enabled: enablePermitsEndpoints,
  }
)

const { isLoading: isGetUnmatchedUsersCountLoading } = useQuery(
  ['getUnmatchedUsers'],
  userStore.getUnmatchedUsers,
  {
    enabled: enablePermitsEndpoints,
  }
)

const { isFetching: isAllAdminUsersLoading } = useQuery(
  ['getAllAdminUsers'],
  () => adminUserStore.getAllAdminUsers(),
  {
    enabled: enableAllAdminUsers,
  }
)

const { isLoading: isPermitsLoading } = useQuery(
  ['applicationSummaryCount'],
  () => permitsStore.getApplicationSummaryCount(),
  {
    enabled: enablePermitsEndpoints,
  }
)

const { isLoading: isAssignedApplicationsLoading } = useQuery(
  ['assignedApplications'],
  () => permitsStore.getAssignedApplicationsSummary(),
  {
    enabled: enablePermitsEndpoints,
  }
)

const { isLoading: isAdminUserLoading, isError } = useQuery(
  ['adminUser'],
  () => adminUserStore.getAdminUserApi(),
  {
    enabled: enablePermitsEndpoints,
  }
)

onBeforeMount(async () => {
  Vue.prototype.$workbox.addEventListener('waiting', () => {
    prompt.value = true
  })

  msalInstance.value = await getMsalInstance()

  const darkMode = localStorage.getItem('dark-mode')

  if (app && darkMode) {
    app.proxy.$vuetify.theme.dark = darkMode === 'true'
    themeStore.getThemeConfig.isDark = darkMode === 'true'
  }

  const appInsights = new ApplicationInsights({
    config: {
      connectionString:
        configStore.appConfig.applicationInsightsConnectionString,
    },
  })

  const referrer = document.referrer

  appInsights.loadAppInsights()
  appInsights.trackPageView({ properties: { referrer } })
})

watch(
  () => isError.value,
  newVal => {
    adminUserStore.setValidAdminUser(!newVal)
  }
)

async function update() {
  prompt.value = false
  await Vue.prototype.$workbox.messageSW({ type: 'SKIP_WAITING' })
}
</script>
