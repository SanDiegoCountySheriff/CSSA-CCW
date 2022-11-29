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
          >
            <v-card
              :elevation="1"
              style="width: 100%"
              min-height="90vh"
              class="page-template-card-color"
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
    background: #fff;
  }
}

.theme--dark {
  .page-template-card-color {
    background-color: #303030;
  }
}
</style>
