<template>
  <v-app-bar
    app
    color="primary"
    class="flex-grow-0 white--text"
    clipped-right
  >
    <router-link :to="{ path: Routes.HOME_ROUTE_PATH }">
      <div class="white--text">
        {{ getAppTitle.name }} <small> {{ getAppTitle.env }}</small>
      </div>
    </router-link>

    <ThemeMode class="ml-4" />
    <v-spacer></v-spacer>
    <LoginButton> </LoginButton>
  </v-app-bar>
</template>

<script setup lang="ts">
import LoginButton from '@core-public//components/login/LoginButton.vue';
import Routes from '@core-public/router/routes';
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue';
import useEnvName from '@shared-ui/composables/useEnvName';

const getAppTitle = useEnvName();
</script>

<style lang="scss" scoped>
.navbar- {
  &container {
    width: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 0.3rem;
  }
  &left {
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
  }
  &title {
    color: aliceblue;
  }
}
</style>
