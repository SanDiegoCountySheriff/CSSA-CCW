<template>
  <h3 class="white--text">
    {{ route.name || '404' }}
  </h3>
</template>
<script setup lang="ts">
import { useRoute } from 'vue-router/composables'

const route = useRoute()
</script>
