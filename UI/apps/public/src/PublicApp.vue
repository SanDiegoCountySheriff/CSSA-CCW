<!--eslint-disable vue-a11y/click-events-have-key-events -->
<template>
  <v-app>
    <v-container
      v-if="(isLoading || isBrandLoading) && !isError"
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
    <div
      v-else
      id="app"
    >
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
import PageTemplate from '@core-public/components/templates/PageTemplate.vue';
import initialize from '@core-public/api/config';
import { useBrandStore } from '@core-public/stores/brandStore';
import { useQuery } from '@tanstack/vue-query';
import { computed, defineComponent } from 'vue';

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
    const brandStore = useBrandStore();
    const { data, isLoading, isError } = useQuery(['config'], initialize);
    const apiUrl = computed(() =>
      Boolean(data.value?.Configuration?.ServicesBaseUrl)
    );
    const { isLoading: isBrandLoading } = useQuery(
      ['brandSetting'],
      brandStore.getBrandSettingApi,
      {
        enabled: apiUrl,
      }
    );

    return { isLoading, isBrandLoading, isError };
  },
});
</script>

<style lang="scss">
#app {
  font-family: WorkSans, sans-serif;
  text-align: center;
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
