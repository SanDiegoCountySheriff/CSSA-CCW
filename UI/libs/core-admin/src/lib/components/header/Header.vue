<template>
  <v-app-bar
    app
    color="primary"
    class="flex-grow-0"
    dark
  >
    <v-app-bar-title>
      {{ $t(' NavBar I am under construction') }}
    </v-app-bar-title>
  </v-app-bar>
</template>

<script setup lang="ts"></script>

<style lang="scss" scoped>
.navbar- {
  &container {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }
}
</style>
