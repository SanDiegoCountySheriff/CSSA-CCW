<template>
  <v-container>
    <v-snackbar
      :value="snackbar"
      :timeout="2000"
      color="primary"
      absolute
      bottom
      left
      text
      app
    >
      {{ $t('Updated settings') }} <strong>{{ $t('successfully.') }}</strong>
    </v-snackbar>

    <v-container
      v-if="isLoading && !isError"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
              divider, list-item-three-line,
              card-heading, image, image, image,
              image, actions"
      >
      </v-skeleton-loader>
    </v-container>

    <v-tabs
      v-model="tabs"
      :color="themeStore.getThemeConfig.isDark ? 'white' : 'black'"
    >
      <v-tabs-slider color="primary"></v-tabs-slider>

      <v-tab> Agency </v-tab>

      <v-tab> Assets </v-tab>

      <v-tab> Color Scheme </v-tab>

      <v-tab> Configuration </v-tab>

      <v-tab> Fees </v-tab>

      <v-tab> Documents </v-tab>
    </v-tabs>

    <v-tabs-items v-model="tabs">
      <v-tab-item>
        <AgencyFormStep />
      </v-tab-item>

      <v-tab-item>
        <AssetsFormStep />
      </v-tab-item>

      <v-tab-item>
        <ColorSchemeFormStep />
      </v-tab-item>

      <v-tab-item>
        <ConfigurationFormStep />
      </v-tab-item>

      <v-tab-item>
        <FeesFormStep />
      </v-tab-item>

      <v-tab-item>
        <DocumentFormStep />
      </v-tab-item>
    </v-tabs-items>
  </v-container>
</template>

<script setup lang="ts">
import AgencyFormStep from './steps/AgencyFormStep.vue'
import AssetsFormStep from './steps/AssetsFormStep.vue'
import ColorSchemeFormStep from './steps/ColorSchemeFormStep.vue'
import ConfigurationFormStep from './steps/ConfigurationFormStep.vue'
import DocumentFormStep from './steps/DocumentFormStep.vue'
import FeesFormStep from './steps/FeesFormStep.vue'
import { ref } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import { useQuery } from '@tanstack/vue-query'
import { useThemeStore } from '@shared-ui/stores/themeStore'

const snackbar = ref(false)
const tabs = ref(null)
const themeStore = useThemeStore()
const brandStore = useBrandStore()

const { isLoading, isError } = useQuery(
  ['brandSetting'],
  brandStore.getBrandSettingApi
)
</script>
