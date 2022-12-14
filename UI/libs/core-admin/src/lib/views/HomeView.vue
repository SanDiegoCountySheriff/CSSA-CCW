<template>
  <div class="home">
    <v-container
      v-if="!store.getDocuments.agencyLandingPageImage"
      fill-height
      fluid
      class="option-section"
    >
      <v-skeleton-loader
        width="494"
        height="196"
        type="image"
      >
      </v-skeleton-loader>
    </v-container>
    <img
      v-else
      :class="$vuetify.theme.dark ? 'dark mr-5' : 'mr-5'"
      alt="Agency landing page image"
      :src="store.getDocuments.agencyLandingPageImage"
      width="494"
      height="196"
    />
    <v-container>
      <div
        class="option-section"
        v-if="!authStore.getAuthState.isAuthenticated"
      >
        <v-btn
          outlined
          class="option-button"
          @click="handleLogIn"
        >
          <div class="option-inner">
            <v-icon
              x-large
              class="mb-3"
            >
              mdi-login
            </v-icon>
            {{ $t('Login') }}
          </div>
        </v-btn>
      </div>
      <v-card
        class="search-bar"
        :width="$vuetify.breakpoint.mdAndDown ? '200' : '800'"
        v-else
      >
        <SearchBar />
      </v-card>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import SearchBar from '@core-admin/components/search/SearchBar.vue';
import auth from '@shared-ui/api/auth/authentication';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';

const store = useBrandStore();
const authStore = useAuthStore();

function handleLogIn() {
  auth.signIn();
}
</script>

<style lang="scss" scoped>
img.dark {
  background-color: #bbb;
}

img {
  margin: auto;
  padding: 10px;
  max-width: 100%;
}
.home {
  .search-bar {
    margin: auto;
    padding: 10px;
  }
}

.option {
  &-inner {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }

  &-button {
    height: 6rem !important;
    padding: 0.5rem !important;
    margin: 0.5rem 0 !important;
    min-width: 16rem !important;
  }

  &-section {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
  }
}
</style>
