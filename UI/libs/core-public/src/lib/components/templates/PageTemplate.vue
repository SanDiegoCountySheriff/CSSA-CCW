<template>
  <div>
    <NavBar></NavBar>
    <template>
      <v-main
        fluid
        class="pb-8 content"
      >
        <slot></slot>
      </v-main>
    </template>
    <Footer />
  </div>
</template>

<script setup lang="ts">
import Footer from '@shared-ui/components/footer/Footer.vue'
import NavBar from '../navbar/NavBar.vue'
</script>
