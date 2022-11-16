<template>
  <v-app-bar
    app
    color="primary"
    class="flex-grow-0 white--text"
    clipped-right
  >
    <ThemeMode class="ml-4" />
    <v-spacer></v-spacer>
    <LoginButton> </LoginButton>
  </v-app-bar>
</template>

<script setup lang="ts">
import LoginButton from '@core-public//components/login/LoginButton.vue';
import ThemeMode from '@shared-ui/components/mode/ThemeMode.vue';
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
