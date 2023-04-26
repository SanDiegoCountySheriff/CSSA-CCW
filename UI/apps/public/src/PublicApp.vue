<!--eslint-disable vue-a11y/click-events-have-key-events -->
<template>
  <v-app>
    <v-container
      v-if="configIsLoading && !isError"
      fluid
    >
      <Loader />
    </v-container>
    <div
      v-else
      id="client-app"
    >
      <PageTemplate>
        <router-view :key="$route.fullPath" />
      </PageTemplate>
      <div
        class="update-dialog"
        v-if="prompt"
      >
        <div class="update-dialog__content">
          {{ $t('A new version is found. Refresh to load it?') }}
        </div>
        <div class="update-dialog__actions">
          <!-- eslint-disable-next-line vue-a11y/click-events-have-key-events -->
          <button
            class="update-dialog__button update-dialog__button--confirm"
            @click="update"
          >
            {{ $t('Refresh') }}
          </button>
          <!-- eslint-disable-next-line vue-a11y/click-events-have-key-events -->
          <button
            class="update-dialog__button update-dialog__button--cancel"
            @click="prompt = false"
          >
            {{ $t('Cancel') }}
          </button>
        </div>
      </div>
    </div>
  </v-app>
</template>

<script lang="ts">
import Loader from './Loader.vue'
import PageTemplate from '@core-public/components/templates/PageTemplate.vue'
import initialize from '@core-public/api/config'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { useThemeStore } from '@shared-ui/stores/themeStore'
import { computed, defineComponent, getCurrentInstance } from 'vue'

export default defineComponent({
  name: 'App',
  components: { PageTemplate, Loader },
  methods: {
    async update() {
      this.prompt = false
      await this.$workbox.messageSW({ type: 'SKIP_WAITING' })
    },
  },
  data() {
    return {
      prompt: false,
    }
  },
  created() {
    if (this.$workbox) {
      this.$workbox.addEventListener('waiting', () => {
        this.prompt = true
      })
    }
  },
  setup() {
    const app = getCurrentInstance()
    const brandStore = useBrandStore()
    const themeStore = useThemeStore()
    const {
      data,
      isLoading: configIsLoading,
      isError,
    } = useQuery(['config'], initialize)
    const apiUrl = computed(() => Boolean(data.value?.Configuration))

    const { isLoading: brandIsLoading, isError: brandIsError } = useQuery(
      ['brandSetting'],
      brandStore.getBrandSettingApi,
      {
        enabled: apiUrl,
      }
    )

    useQuery(['logo'], brandStore.getAgencyLogoDocumentsApi, {
      enabled: apiUrl,
    })

    useQuery(['landingPageImage'], brandStore.getAgencyLandingPageImageApi, {
      enabled: apiUrl,
    })

    app.proxy.$vuetify.theme.dark = themeStore.getThemeConfig.isDark

    return { configIsLoading, isError, brandIsLoading, brandIsError }
  },
})
</script>
