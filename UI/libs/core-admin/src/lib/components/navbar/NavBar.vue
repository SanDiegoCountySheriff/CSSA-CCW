<template>
  <header id="nav">
    <div class="navbar-container">
      <div class="navbar-left">
        {{ $t(' NavBar I am under construction') }}
      </div>
      <div class="navbar-right">
        <LoginButton />
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import LoginButton from '@core-admin/components/login/LoginButton.vue';
</script>

<style lang="scss" scoped>
.navbar- {
  &container {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }
}
</style>
