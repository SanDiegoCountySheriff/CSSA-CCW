<template>
  <div class="main">
    <NavBar></NavBar>
    <template>
      <v-main
        fluid
        class="pb-8 content"
      >
        <slot></slot>
      </v-main>
    </template>
    <v-footer
      fixed
      padless
    >
      <v-col
        class="text-center"
        cols="12"
      >
        {{ $t('© CSSA') }}
      </v-col>
    </v-footer>
  </div>
</template>
<script setup lang="ts">
import NavBar from '../navbar/NavBar.vue';
</script>

<style lang="scss" scoped>
.main {
  height: 100vh;
}
.content {
  min-height: 97%;
}
</style>
