<template>
  <v-app-bar
    app
    color="primary"
    class="flex-grow-0 white--text"
    clipped-right
  >
    <ConnectionStatus class="ml-4" />
    <ThemeMode class="ml-4" />
    <v-spacer></v-spacer>
    <v-btn
      v-if="authStore.getAuthState.isAuthenticated"
      aria-label="Logout of application"
      @click="logout"
      color="mr-4"
      small
      text
    >
      <!--eslint-disable-next-line vue/singleline-html-element-content-newline -->
      <v-icon class="pr-1 white--text"> mdi-logout-variant </v-icon>
      <span class="white--text">{{ $t('Logout') }}</span>
    </v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import ConnectionStatus from '@shared-ui/components/status/ConnectionStatus.vue';
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue';
import auth from '@shared-ui/api/auth/authentication';
import { useAuthStore } from '@shared-ui/stores/auth';

const authStore = useAuthStore();

async function logout() {
  await auth.signOut();
}
</script>

<style lang="scss" scoped></style>
