<template>
  <header id="nav">
    <div class="navbar-container">
      <div class="navbar-left">
        <img
          src="/img/icons/agency_logo.svg"
          width="36"
          height="36"
          alt="logo"
          loading="lazy"
        />
        <!-- TODO: and the env here -->
        <h3 class="navbar-title">
          {{ $t('CCW Permit Application') }}
        </h3>
      </div>
      <div class="navbar-right">
        <LoginButton />
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import LoginButton from '@core-public/components/login/LoginButton.vue';
</script>

<style lang="scss" scoped>
.navbar- {
  &container {
    width: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 0.3rem;
  }
  &left {
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
  }
  &title {
    color: aliceblue;
  }
}
</style>
