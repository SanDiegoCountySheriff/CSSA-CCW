<template>
  <div>
    <NavBar></NavBar>
    <v-main>
      <slot></slot>
    </v-main>
    <Footer />
  </div>
</template>

<script setup lang="ts">
import Footer from '@shared-ui/components/footer/Footer.vue'
import NavBar from '../navbar/NavBar.vue'
</script>
