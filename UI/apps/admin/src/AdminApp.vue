<template>
  <v-app>
    <v-container
      v-if="(isPermitsLoading || isAdminUserLoading) && isAuthenticated"
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
import Loader from './Loader.vue';
import PageTemplate from '@core-admin/components/templates/PageTemplate.vue';
import Vue from 'vue';
import initialize from '@core-admin/api/config';
import interceptors from '@core-admin/api/interceptors';
import { useAdminUserStore } from '@core-admin/stores/adminUserStore';
import { useAppConfigStore } from '@shared-ui/stores/configStore';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useThemeStore } from '@shared-ui/stores/themeStore';
import { computed, getCurrentInstance, onBeforeMount, ref, watch } from 'vue';

const prompt = ref(false);
const app = getCurrentInstance();
const authStore = useAuthStore();
const brandStore = useBrandStore();
const themeStore = useThemeStore();
const configStore = useAppConfigStore();
const permitsStore = usePermitsStore();
const adminUserStore = useAdminUserStore();

const isAuthenticated = computed(() => authStore.getAuthState.isAuthenticated);
const validApiUrl = computed(
  () => configStore.appConfig.applicationApiBaseUrl.length !== 0
);

useQuery(['brandSetting'], brandStore.getBrandSettingApi, {
  enabled: validApiUrl,
});

useQuery(['logo'], brandStore.getAgencyLogoDocumentsApi, {
  enabled: validApiUrl,
});

useQuery(['landingPageImage'], brandStore.getAgencyLandingPageImageApi, {
  enabled: validApiUrl,
});

const { isLoading: isPermitsLoading } = useQuery(
  ['permits'],
  permitsStore.getAllPermitsApi,
  {
    enabled: isAuthenticated && validApiUrl,
  }
);

const { isLoading: isAdminUserLoading, isError } = useQuery(
  ['adminUser'],
  adminUserStore.getAdminUserApi,
  {
    enabled: isAuthenticated && validApiUrl,
  }
);

onBeforeMount(async () => {
  Vue.prototype.$workbox.addEventListener('waiting', () => {
    prompt.value = true;
  });

  await initialize();
  interceptors();

  if (app) {
    app.proxy.$vuetify.theme.dark = themeStore.getThemeConfig.isDark;
  }
});

watch(
  () => isError.value,
  newVal => {
    adminUserStore.setValidAdminUser(!newVal);
  }
);

async function update() {
  prompt.value = false;
  await Vue.prototype.$workbox.messageSW({ type: 'SKIP_WAITING' });
}
</script>
