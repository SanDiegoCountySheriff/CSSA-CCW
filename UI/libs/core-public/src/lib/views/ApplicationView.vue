<template>
  <div class="application">
    <!-- TODO: Make this the custom logo -->
    <img
      alt="Vue logo"
      src="@shared-ui/assets/logo.png"
    />
    <AcknowledgementContainer />
  </div>
</template>

<script setup lang="ts">
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
</script>
