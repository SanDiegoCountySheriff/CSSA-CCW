<template>
  <v-app-bar
    app
    color="primary"
    class="flex-grow-0 white--text"
    clipped-right
    height="64"
  >
    <router-link :to="{ path: Routes.HOME_ROUTE_PATH }">
      <div
        class="text-h6 white--text"
        v-if="!isMobile"
      >
        {{ getAppTitle.name }}
        <small> {{ getAppTitle.env }}</small>
      </div>
      <div v-else>
        <v-avatar>
          <v-img
            :src="brandStore.getDocuments.agencyLogo"
            alt="Image"
            contain
          />
        </v-avatar>
      </div>
    </router-link>
    <v-spacer></v-spacer>
    <div class="mr-4 ml-1">
      <ThemeMode />
    </div>
    <LoginButton> </LoginButton>
  </v-app-bar>
</template>

<script setup lang="ts">
import LoginButton from '@core-public/components/login/LoginButton.vue'
import Routes from '@core-public/router/routes'
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue'
import { computed } from 'vue'
import { useBrandStore } from '@shared-ui/stores/brandStore'
import useEnvName from '@shared-ui/composables/useEnvName'
import { useVuetify } from '@shared-ui/composables/useVuetify'

const brandStore = useBrandStore()
const getAppTitle = useEnvName()
const vuetify = useVuetify()
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)
</script>
