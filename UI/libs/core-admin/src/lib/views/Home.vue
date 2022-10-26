<template>
  <v-container class="home">
    <img
      v-if="store.getBrand.agencyLogoDataURL"
      alt="Agency logo"
      :src="store.getBrand.agencyLogoDataURL"
    />
    <v-btn
      v-if="!authStore.getAuthState.isAuthenticated"
      outlined
      color="primary"
      @click="handleLogIn"
    >
      <v-icon
        color="primary"
        class="mb-3"
      >
        mdi-login
      </v-icon>
      {{ $t('Login') }}
    </v-btn>
  </v-container>
</template>

<script setup lang="ts">
import auth from '@shared-ui/api/auth/authentication';
import { useBrandStore } from '@core-admin/stores/brandStore';
import { useAuthStore } from '@shared-ui/stores/auth';

const store = useBrandStore();
const authStore = useAuthStore();

function handleLogIn() {
  auth.signIn();
}
</script>

<style lang="scss" scoped>
img {
  max-width: 30%;
  margin-top: 20px;
}
</style>
