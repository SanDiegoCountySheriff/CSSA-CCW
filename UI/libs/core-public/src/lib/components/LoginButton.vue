<template>
  <div>
    <Button
      color="primary"
      text="Login"
      @click="handleLogIn"
    >
    </Button>
  </div>
</template>

<script setup lang="ts">
import auth from '@core-public/api/auth/authentication';
import Button from '@shared-ui/components/Button.vue';

function handleLogIn() {
  auth.signIn();
}
</script>
