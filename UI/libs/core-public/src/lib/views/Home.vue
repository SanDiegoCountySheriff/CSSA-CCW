<template>
  <div class="home">
    <LoginButton />
    <img
      alt="Vue logo"
      src="@shared-ui/assets/logo.png"
    />
    <HelloWorld :msg="$t('hello', { msg: 'Public' })" />
  </div>
</template>

<script setup lang="ts">
import HelloWorld from '@shared-ui/components/HelloWorld.vue';
import LoginButton from '@core-public/components/LoginButton.vue';
</script>
