<template>
  <div class="home">
    <!-- TODO: Make this the custom logo -->
    <img
      alt="Vue logo"
      src="@shared-ui/assets/logo.png"
    />
    <v-container>
      <router-link :to="{ path: '/application' }">
        <Button
          color="primary"
          text="Start New Application"
        >
        </Button>
      </router-link>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import Button from '@shared-ui/components/Button.vue';
</script>
