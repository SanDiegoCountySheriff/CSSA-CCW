<!-- stylelint-disable selector-class-pattern -->
<!-- stylelint-disable selector-max-compound-selectors -->
<!--eslint-disable vue-a11y/click-events-have-key-events -->
<template>
  <v-app>
    <v-container
      v-if="false"
      fluid
    >
      <Loader />
    </v-container>
    <div v-else>
      <PageTemplate>
        <router-view />
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
            {{ $t('Update') }}
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

<script setup lang="ts">
import Loader from './Loader.vue';
import PageTemplate from '@core-admin/components/templates/PageTemplate.vue';
import initialize from '@core-admin/api/config';
import interceptors from '@core-admin/api/interceptors';
import { useAppConfigStore } from '@shared-ui/stores/configStore';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useThemeStore } from '@shared-ui/stores/themeStore';
import {
  computed,
  getCurrentInstance,
  onBeforeMount,
  onMounted,
  ref,
  watch,
} from 'vue';

const prompt = ref(false);
const app = getCurrentInstance();
const authStore = useAuthStore();
const brandStore = useBrandStore();
const themeStore = useThemeStore();
const configStore = useAppConfigStore();
const permitsStore = usePermitsStore();

const isAuthenticated = computed(() => authStore.getAuthState.isAuthenticated);
const validApiUrl = computed(
  () => configStore.appConfig.applicationApiBaseUrl.length !== 0
);

// const { isLoading } = useQuery(['config'], initialize);

useQuery(['brandSetting'], brandStore.getBrandSettingApi, {
  enabled: validApiUrl,
});

useQuery(['logo'], brandStore.getAgencyLogoDocumentsApi, {
  enabled: validApiUrl,
});

useQuery(['landingPageImage'], brandStore.getAgencyLandingPageImageApi, {
  enabled: validApiUrl,
});

useQuery(['permits'], permitsStore.getAllPermitsApi, {
  enabled: isAuthenticated,
});

const { isError, error } = useQuery(['adminUser'], authStore.getAdminUserApi, {
  enabled: isAuthenticated,
});

onBeforeMount(async () => {
  await initialize();
  interceptors();

  if (app) {
    app.proxy.$vuetify.theme.dark = themeStore.getThemeConfig.isDark;
  }
  //   if (app?.appContext.config.globalProperties.$workbox) {
  //     app.appContext.config.globalProperties.$workbox.addEventListener('waiting', () => {
  //       prompt.value = true;
  //     });
  //   }
});

onMounted(() => {
  window.console.log(
    'auth store',
    computed(() => authStore.getAuthState.isAuthenticated).value
  );
  window.console.error('INITAL STATE OF IS ERROR', isError.value);
  // window.console.log('is loading', isLoading.value);
});

watch(
  () => authStore.getAuthState.isAuthenticated,
  newVal => {
    window.console.log('WE UPDATED!', newVal);
  }
);
watch(
  () => error,
  newVal => {
    window.console.log('IS ERROR CHANGED!', newVal);
  }
);
watch(
  () => isError.value,
  newVal => {
    authStore.setValidAdminUser(!newVal);
  }
);
// watch(
//   () => isLoading,
//   newVal => {
//     window.console.log('WE UPDATED IS LOADING!', newVal);
//   }
// );

async function update() {
  prompt.value = false;
  //   await $workbox.messageSW({ type: 'SKIP_WAITING' });
}
</script>

<style lang="scss">
#app {
  font-display: swap;
  font-family: WorkSans, sans-serif;
  font-style: normal;
  text-align: center;

  .v-btn {
    text-transform: none !important;
  }

  .v-tab {
    text-transform: none !important;
  }

  .theme--light {
    .v-card {
      &__subtitle {
        color: white !important;
      }
    }
  }

  textarea,
  input,
  input:focus,
  input:active {
    background-color: inherit !important;
  }

  .v-label {
    color: #747474;
    font-size: 14px;
  }

  .v-input {
    input {
      font-size: 15px;
    }
  }

  .v-radio {
    .v-label {
      color: #111;
    }
  }
}

#nav {
  min-height: 1rem;
  background: #263b65;
  color: aliceblue;
}

.update-dialog {
  position: fixed;
  bottom: 64px;
  left: 50%;
  max-width: 576px;
  padding: 12px;
  background-color: #2c3e50;
  border-radius: 4px;
  box-shadow: 0 0 10px rgb(0 0 0 / 50%);
  color: white;
  text-align: left;
  transform: translateX(-50%);

  &__actions {
    display: flex;
    margin-top: 8px;
  }

  &__button {
    margin-right: 8px;

    &--confirm {
      margin-left: auto;
    }
  }
}
</style>
