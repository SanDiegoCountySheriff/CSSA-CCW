<template>
  <v-tooltip bottom>
    <template #activator="{ on, attrs }">
      <v-icon
        aria-label="Online indicator"
        size="22"
        v-bind="attrs"
        v-on="on"
        class="white--text"
      >
        {{ getOnlineIcon }}
      </v-icon>
    </template>
    <span>{{ $t('Online') }}</span>
  </v-tooltip>
</template>
<script setup lang="ts">
import { onlineManager } from '@tanstack/vue-query';
import { useStatusStore } from '@shared-ui/stores/statusStore';
import { computed, onBeforeMount, onMounted } from 'vue';

const statusStore = useStatusStore();

const getOnlineIcon = computed(() =>
  statusStore.getConnectionStatus ? 'mdi-wifi' : 'mdi-wifi-strength-off'
);

const listener = async () => {
  if (navigator.onLine) {
    statusStore.setConnectionStatus(Boolean(onlineManager.isOnline()));
  } else if (!navigator.onLine) {
    statusStore.setConnectionStatus(false);
  }
};

onMounted(async () => {
  await listener();
  window.addEventListener('online', listener);
  window.removeEventListener('offline', listener);
});
onBeforeMount(() => {
  window.removeEventListener('online', listener);
  window.removeEventListener('offline', listener);
});
</script>
