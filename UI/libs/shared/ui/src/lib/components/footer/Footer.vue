<template>
  <v-footer
    padless
    fixed
  >
    <v-col
      class="lighten-2 ml-4 text-center"
      style="color: #667085"
      cols="12"
    >
      © {{ new Date().getFullYear() }} — CSSA
      {{ isDEV ? `—  ${getVersion}` : '' }}
    </v-col>
  </v-footer>
</template>
<script setup lang="ts">
import VERSION from '@shared-utils/version';
import { computed } from 'vue';
import { useAppConfigStore } from '@shared-ui/stores/configStore';

const appConfigStore = useAppConfigStore();

const getVersion = computed(() => VERSION);
const isDEV = appConfigStore.getAppConfig.environmentName === 'DEV';
</script>
