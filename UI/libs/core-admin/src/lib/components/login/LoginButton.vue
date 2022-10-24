<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <template v-if="authStore.getAuthState.isAuthenticated">
      <v-list-item link>
        <v-list-item-icon>
          <v-icon> mdi-account-circle </v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>
            {{ authStore.getAuthState.userName }}
          </v-list-item-title>
          <v-list-item-subtitle>
            {{ authStore.getAuthState.userEmail }}
          </v-list-item-subtitle>
        </v-list-item-content>
      </v-list-item>
    </template>
    <div v-else>
      <v-list-item
        link
        @click="handleLogIn"
      >
        <v-list-item-icon>
          <v-icon> mdi-account-circle </v-icon>
        </v-list-item-icon>
        <v-list-item-title> Login </v-list-item-title>
      </v-list-item>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import auth from '@shared-ui/api/auth/authentication';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useQuery } from '@tanstack/vue-query';

const authStore = useAuthStore();

onMounted(() => {
  if (authStore.getAuthState.isAuthenticated) {
    useQuery(['verifyEmail'], authStore.postVerifyUserApi);
  }
});

// eslint-disable-next-line @typescript-eslint/no-unused-vars
async function logout() {
  await auth.signOut();
}

function handleLogIn() {
  auth.signIn();
}
</script>

<style lang="scss" scoped>
.v-list-item {
  &__title {
    text-align: left;
  }

  &__subtitle {
    text-align: left;
  }
}
</style>
