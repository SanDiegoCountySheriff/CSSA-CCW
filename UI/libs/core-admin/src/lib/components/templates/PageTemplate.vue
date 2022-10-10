<template>
  <div>
    <NavBar></NavBar>
    <template>
      <v-main fluid>
        <slot></slot>
      </v-main>
    </template>
    <v-footer
      padless
      fixed
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
