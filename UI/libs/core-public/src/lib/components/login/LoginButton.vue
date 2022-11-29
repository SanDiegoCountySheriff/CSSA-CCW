<template>
  <div>
    <template v-if="authStore.getAuthState.isAuthenticated">
      <div class="d-flex">
        <h3 class="text-center white--text font-weight-bold mr-4 ml-1">
          {{ authStore.getAuthState.userName }}
        </h3>
        <v-btn
          v-if="authStore.getAuthState.isAuthenticated"
          aria-label="Sign out of application"
          @click="signOut"
          class="mr-4 ml-1"
          color="primary lighten-2"
          small
        >
          <!--eslint-disable-next-line vue/singleline-html-element-content-newline -->
          <v-icon
            v-if="$vuetify.breakpoint.mdAndDown"
            class="pr-1 white--text"
          >
            mdi-logout-variant
          </v-icon>
          <span
            v-if="$vuetify.breakpoint.mdAndUp"
            class="white--text"
            >{{ $t('Sign out') }}</span
          >
        </v-btn>
      </div>
    </template>
    <div v-else>
      <Button
        color="primary"
        text="Login"
        @click="handleLogIn"
      >
      </Button>
    </div>
  </div>
</template>

<script setup lang="ts">
import Button from '@shared-ui/components/Button.vue';
import auth from '@shared-ui/api/auth/authentication';
import { onMounted } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import useInterval from '@shared-ui/composables/useInterval';
import { useQuery } from '@tanstack/vue-query';

const authStore = useAuthStore();
const brandStore = useBrandStore();

onMounted(() => {
  if (authStore.getAuthState.isAuthenticated) {
    useQuery(['verifyEmail'], authStore.postVerifyUserApi);
    useInterval(auth.tokenInterval, brandStore.getBrand.refreshTokenTime);
  }
});

async function signOut() {
  await auth.signOut();
}

function handleLogIn() {
  auth.signIn();
}
</script>
