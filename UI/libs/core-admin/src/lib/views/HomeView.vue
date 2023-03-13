<template>
  <div class="home">
    <v-container
      v-if="!store.getDocuments.agencyLandingPageImage"
      fill-height
      fluid
    >
      <v-skeleton-loader
        width="494"
        height="196"
        type="image"
      >
      </v-skeleton-loader>
    </v-container>
    <v-img
      v-else
      alt="Agency Landing Page Image"
      :src="store.getDocuments.agencyLogo"
    ></v-img>
    <!-- <img
      v-else
      alt="Agency landing page image"
      :src="store.getDocuments.agencyLogo"
    /> -->
    <v-container class="text-center">
      <div v-if="!authStore.getAuthState.isAuthenticated">
        <v-btn
          outlined
          color="primary"
          x-large
          @click="handleLogIn"
        >
          <v-icon class="mr-2"> mdi-login </v-icon>
          {{ $t('Login') }}
        </v-btn>
      </div>
      <v-card v-else>
        <v-container>
          <SearchBar />
        </v-container>
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
  background-color: #616161;
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
