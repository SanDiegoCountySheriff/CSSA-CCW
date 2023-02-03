<template>
  <v-app-bar
    app
    color="accent"
    class="flex-grow-0 white--text"
    clipped-right
  >
    <h3
      v-if="authStore.getAuthState.isAuthenticated"
      class="white--text"
    >
      {{
        $vuetify.breakpoint.mdAndDown
          ? authStore.getAuthState.userName.split(',')[0]
          : authStore.getAuthState.userName
      }}
    </h3>
    <v-spacer></v-spacer>
    <div class="mr-4 ml-1">
      <ThemeMode />
    </div>
    <div
      v-if="
        authStore.getAuthState.isAuthenticated && $vuetify.breakpoint.mdAndUp
      "
      class="caption font-weight-bold mr-4 ml-1"
    >
      {{ $t('Session started at') }} {{ formatTime(sessionTime) }}
    </div>
    <v-btn
      v-if="authStore.getAuthState.isAuthenticated"
      aria-label="Sign out of application"
      @click="signOut"
      class="mr-4 ml-1"
      :color="$vuetify.breakpoint.mdAndDown ? 'accent' : 'accent lighten-2'"
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
        v-else
        class="white--text"
        >{{ $t('Sign out') }}</span
      >
    </v-btn>
  </v-app-bar>
</template>

<script setup lang="ts">
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue';
import auth from '@shared-ui/api/auth/authentication';
import { formatTime } from '@shared-utils/formatters/defaultFormatters';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useBrandStore } from '@shared-ui/stores/brandStore';
import { computed, onBeforeUnmount, onMounted } from 'vue';

const authStore = useAuthStore();
const brandStore = useBrandStore();
const sessionTime = computed(() => authStore.getAuthState.sessionStarted);

let silentRefresh;

onMounted(() => {
  if (authStore.getAuthState.isAuthenticated) {
    silentRefresh = setInterval(
      auth.acquireToken,
      brandStore.getBrand.refreshTokenTime * 1000 * 60
    );
  }
});

onBeforeUnmount(() => clearInterval(silentRefresh));

async function signOut() {
  await auth.signOut();
}
</script>
