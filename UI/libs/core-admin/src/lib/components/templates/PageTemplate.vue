<template>
  <div>
    <Header />
    <SideBar v-if="authStore.getAuthState.isAuthenticated" />
    <template>
      <v-main fluid>
        <v-layout
          justify-center
          fill-height
        >
          <slot> </slot>
        </v-layout>
      </v-main>
    </template>
    <Footer />
  </div>
</template>
<script setup lang="ts">
import Footer from '@shared-ui/components/footer/Footer.vue'
import Header from '../header/Header.vue'
import SideBar from '../navigation/SideBar.vue'
import { useAuthStore } from '@shared-ui/stores/auth'

const authStore = useAuthStore()
</script>
