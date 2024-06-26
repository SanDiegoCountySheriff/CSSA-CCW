<template>
  <v-app>
    <NavBar></NavBar>
    <v-main>
      <template
        v-if="
          isAgencyLogoLoading ||
          isBrandSettingLoading ||
          isUserFetching ||
          authStore.auth.handlingRedirectPromise ||
          isFetching
        "
      >
        <Loader />
      </template>

      <router-view
        v-else
        :key="$route.fullPath"
      />

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
    </v-main>
    <Footer />
  </v-app>
</template>

<script setup lang="ts">
import { ApplicationInsights } from '@microsoft/applicationinsights-web'
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import Footer from '@shared-ui/components/footer/Footer.vue'
import Loader from '@core-public/views/Loader.vue'
import NavBar from '@core-public/components/navbar/NavBar.vue'
import Vue from 'vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { useUserStore } from '@shared-ui/stores/userStore'
import {
  MsalBrowser,
  getMsalInstance,
} from '@shared-ui/api/auth/authentication'
import { computed, getCurrentInstance, onBeforeMount, provide, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const prompt = ref(false)
const app = getCurrentInstance()
const authStore = useAuthStore()
const completeApplicationStore = useCompleteApplicationStore()
const configStore = useAppConfigStore()
const themeStore = useThemeStore()
const brandStore = useBrandStore()
const msalInstance = ref<MsalBrowser>()
const userStore = useUserStore()
const canFetch = computed(() => authStore.getAuthState.isAuthenticated)

provide(
  'msalInstance',
  computed(() => msalInstance.value)
)

const { mutate: createUser } = useMutation(
  ['createUserProfile'],
  () => userStore.putCreateUser(),
  {
    onSuccess: res => {
      userStore.setUser(res)
    },
  }
)

const { isFetching: isUserFetching } = useQuery(
  ['getUserProfile'],
  userStore.getUser,
  {
    enabled: canFetch,
    onSuccess: response => {
      if (response.status === 204) {
        createUser()
      }
    },
  }
)

const { isFetching } = useQuery(
  ['getApplicationsByUser'],
  completeApplicationStore.getAllUserApplicationsApi,
  {
    refetchOnMount: 'always',
    onSuccess: response => {
      if (response.status !== 204 && response.data) {
        completeApplicationStore.setCompleteApplication(
          response.data[0] as CompleteApplication
        )
      }
    },
    enabled: canFetch,
  }
)

const validApiUrl = computed(
  () => configStore.appConfig.applicationApiBaseUrl.length !== 0
)

const { isLoading: isBrandSettingLoading } = useQuery(
  ['brandSetting'],
  brandStore.getBrandSettingApi,
  {
    enabled: validApiUrl,
  }
)

const { isLoading: isAgencyLogoLoading } = useQuery(
  ['logo'],
  brandStore.getAgencyLogoDocumentsApi,
  {
    enabled: validApiUrl,
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

async function update() {
  prompt.value = false
  await Vue.prototype.$workbox.messageSW({ type: 'SKIP_WAITING' })
}
</script>
