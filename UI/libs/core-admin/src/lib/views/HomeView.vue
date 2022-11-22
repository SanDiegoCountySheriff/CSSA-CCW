<template>
  <div class="home">
    <img
      v-if="store.getDocuments.agencyLandingPageImage"
      alt="Agency landing page image"
      :src="store.getDocuments.agencyLandingPageImage"
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
import { useBrandStore } from '@core-admin/stores/brandStore';

const store = useBrandStore();
const authStore = useAuthStore();

function handleLogIn() {
  auth.signIn();
}
</script>

<style lang="scss" scoped>
img {
  margin: auto;
  width: 50%;
  padding: 10px;
  max-width: 600px;

  @media screen and (max-width: 768px) {
    max-width: 360px;
  }

  @media screen and (max-width: 480px) {
    max-width: 260px;
  }
}
.home {
  .search-bar {
    margin: auto;
    width: 50%;
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
