<template>
  <v-footer
    padless
    app
    inset
  >
    <v-col class="text-center">
      © {{ new Date().getFullYear() }} — CSSA — {{ getVersion }}
    </v-col>
  </v-footer>
</template>
<script setup lang="ts">
import VERSION from '@shared-utils/version'
import { computed } from 'vue'

const getVersion = computed(() => VERSION)
</script>
