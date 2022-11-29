<!--eslint-disable vue-a11y/click-events-have-key-events -->
<template>
  <v-app>
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

<script lang="ts">
import PageTemplate from '@core-admin/components/templates/PageTemplate.vue';
import initialize from '@core-admin/api/config';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';
import { useThemeStore } from '@shared-ui/stores/themeStore';
import { computed, defineComponent, getCurrentInstance } from 'vue';

export default defineComponent({
  name: 'App',
  components: { PageTemplate },
  methods: {
    async update() {
      this.prompt = false;
      await this.$workbox.messageSW({ type: 'SKIP_WAITING' });
    },
  },
  data() {
    return {
      prompt: false,
    };
  },
  created() {
    if (this.$workbox) {
      this.$workbox.addEventListener('waiting', () => {
        this.prompt = true;
      });
    }
  },
  setup() {
    const app = getCurrentInstance();
    const brandStore = useBrandStore();
    const themeStore = useThemeStore();
    const { getAllPermitsApi } = usePermitsStore();
    const { data, isLoading, isError } = useQuery(['config'], initialize);
    const apiUrl = computed(() => Boolean(data.value?.Configuration));

    useQuery(['brandSetting'], brandStore.getBrandSettingApi, {
      enabled: apiUrl,
    });

    useQuery(['logo'], brandStore.getAgencyLogoDocumentsApi, {
      enabled: apiUrl,
    });

    useQuery(['landingPageImage'], brandStore.getAgencyLandingPageImageApi, {
      enabled: apiUrl,
    });

    useQuery(['permits'], getAllPermitsApi, {
      enabled: apiUrl,
    });

    app.proxy.$vuetify.theme.dark = themeStore.getThemeConfig.isDark;

    return { isLoading, isError };
  },
});
</script>

<style lang="scss">
#app {
  font-display: swap;
  font-family: WorkSans, sans-serif;
  text-align: center;

  .theme--light {
    .v-card {
      &__subtitle {
        color: white !important;
      }
    }
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
