<template>
  <v-app>
    <NavBar></NavBar>
    <v-main>
      <template
        v-if="
          !isAgencyLogoLoading &&
          !isBrandSettingLoading &&
          !isAgencyHomePageImageLoading &&
          !isLandingPageImageLoading &&
          authStore.auth.handlingRedirectPromise
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
import Footer from '@shared-ui/components/footer/Footer.vue'
import Loader from '@core-public/views/Loader.vue'
import NavBar from '@core-public/components/navbar/NavBar.vue'
import Vue from 'vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useAuthStore } from '@shared-ui/stores/auth'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import {
  MsalBrowser,
  getMsalInstance,
} from '@shared-ui/api/auth/authentication'
import { computed, getCurrentInstance, onBeforeMount, provide, ref } from 'vue'

const prompt = ref(false)
const app = getCurrentInstance()
const authStore = useAuthStore()
const themeStore = useThemeStore()
const configStore = useAppConfigStore()
const brandStore = useBrandStore()
const msalInstance = ref<MsalBrowser>()

provide(
  'msalInstance',
  computed(() => msalInstance.value)
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

const { isLoading: isAgencyHomePageImageLoading } = useQuery(
  ['homePageImage'],
  brandStore.getAgencyHomePageImageApi,
  {
    enabled: validApiUrl,
  }
)

const { isLoading: isLandingPageImageLoading } = useQuery(
  ['landingPageImage'],
  brandStore.getAgencyLandingPageImageApi,
  {
    enabled: validApiUrl,
  }
)

onBeforeMount(async () => {
  Vue.prototype.$workbox.addEventListener('waiting', () => {
    prompt.value = true
  })

  msalInstance.value = await getMsalInstance()

  if (app) {
    app.proxy.$vuetify.theme.dark = themeStore.getThemeConfig.isDark
  }
})

async function update() {
  prompt.value = false
  await Vue.prototype.$workbox.messageSW({ type: 'SKIP_WAITING' })
}
</script>
