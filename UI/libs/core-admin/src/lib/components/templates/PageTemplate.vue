<template>
  <div>
    <Header />
    <SideBar />
    <template>
      <v-main
        fluid
        class="main h-full"
      >
        <slot></slot>
      </v-main>
    </template>
    <Footer />
  </div>
</template>
<script setup lang="ts">
import Header from '../header/Header.vue';
import SideBar from '../sidebar/SideBar.vue';
import Footer from '../footer/Footer.vue';
</script>

<style lang="scss" scoped>
.main {
  height: 100%;
  background: #e9e9e9;
  min-height: 100vh;
}
</style>
