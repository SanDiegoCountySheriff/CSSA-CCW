<!--eslint-disable vue-a11y/click-events-have-key-events -->
<template>
  <v-app>
    <div id="app">
      <div id="nav">
        <router-link to="/">
          {{ $t(' Home ') }}
        </router-link>
        <router-link to="/about">
          {{ $t('About') }}
        </router-link>
      </div>
      <v-main>
        <router-view />
      </v-main>

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
import { defineComponent } from 'vue';
import { useQuery } from '@tanstack/vue-query';
import initialize from '@core-public/api/config';

export default defineComponent({
  name: 'App',
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
    useQuery(['config'], initialize);
  },
});
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
}

#nav {
  padding: 30px;

  a {
    font-weight: bold;

    &.router-link-exact-active {
      color: #42b983;
    }
  }
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
