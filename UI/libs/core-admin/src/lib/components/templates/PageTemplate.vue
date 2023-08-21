<template>
  <div>
    <Header
      @on-expand-menu="handleExpandMenu"
      :drawer-showing="drawerShowing"
    />
    <SideBar
      v-if="authStore.getAuthState.isAuthenticated"
      :expand-menu="expandMenu"
      @on-change-drawer="handleChangeDrawer"
    />
    <template>
      <v-main fluid>
        <slot> </slot>
      </v-main>
    </template>
    <Footer />
  </div>
</template>
<script setup lang="ts">
import Footer from '@shared-ui/components/footer/Footer.vue'
import Header from '../header/Header.vue'
import SideBar from '../navigation/SideBar.vue'
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'

const authStore = useAuthStore()
const expandMenu = ref(false)
const drawerShowing = ref(true)

function handleExpandMenu() {
  expandMenu.value = !expandMenu.value
}

function handleChangeDrawer() {
  drawerShowing.value = !drawerShowing.value
}
</script>
