<template>
  <div>
    <Header />
    <SideBar v-if="authStore.getAuthState.isAuthenticated" />
    <template>
      <v-main
        fluid
        class="main h-full"
      >
        <v-container fluid>
          <v-layout
            justify-center
            fill-height
            style="height: 92vh"
          >
            <v-card
              :elevation="6"
              style="width: 98%"
            >
              <slot> </slot>
            </v-card>
          </v-layout>
        </v-container>
      </v-main>
    </template>
    <!--<Footer />-->
  </div>
</template>
<script setup lang="ts">
// Remove header and footer as per new comps
import Header from '../header/Header.vue';
//import Footer from '../footer/Footer.vue';
import SideBar from '../navigation/SideBar.vue';
import { useAuthStore } from '@shared-ui/stores/auth';

const authStore = useAuthStore();
</script>

<style lang="scss" scoped>
.main {
  height: 100%;
  min-height: 100vh;
}
.theme--light {
  .main {
    background: #e9e9e9;
  }
}
</style>
