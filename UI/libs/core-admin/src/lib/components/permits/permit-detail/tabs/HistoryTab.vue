<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('History:') }}
      <v-spacer></v-spacer>
      <v-btn
        :outlined="state.interval == null"
        :color="state.interval == null ? 'primary' : 'error'"
        @click="state.interval == null ? start() : stop()"
      >
        Realtime
      </v-btn>
    </v-card-title>
    <v-row class="ml-5">
      <v-col cols="12">
        <v-card-text>
          <v-timeline dense>
            <v-slide-x-reverse-transition
              group
              origin
              leave-absolute
            >
              <v-timeline-item
                v-for="item in state.items"
                :key="item.id"
                :color="item.color"
                class="mb-4"
                medium
                fill-dot
              >
                <template #icon>
                  <span class="white--text">SG</span>
                </template>
                <v-row justify="space-between">
                  <v-col cols="6">
                    <v-chip
                      class="white--text ml-0"
                      :color="item.color"
                      label
                      medium
                    >
                      ID Information
                    </v-chip>
                    SSN was provided
                  </v-col>
                  <v-col
                    class="text-right"
                    cols="6"
                  >
                    15:25 EDT
                  </v-col>
                </v-row>
              </v-timeline-item>
            </v-slide-x-reverse-transition>
          </v-timeline>
        </v-card-text>
      </v-col>
    </v-row>
  </v-card>
</template>
<script setup lang="ts">
import { onBeforeUnmount, reactive } from 'vue';

const COLORS = ['info', 'warning', 'error', 'success'];
const ICONS = {
  info: 'mdi-information',
  warning: 'mdi-alert',
  error: 'mdi-alert-circle',
  success: 'mdi-check-circle',
};

const state = reactive({
  interval: null,
  items: [
    {
      id: 1,
      color: 'info',
      icon: ICONS.info,
    },
  ],
  nonce: 2,
});

onBeforeUnmount(() => {
  stop();
});

function addEvent() {
  let { color, icon } = genAlert();

  const previousColor = state.items[0].color;

  while (previousColor === color) {
    color = genColor();
  }

  state.items.unshift({
    id: state.nonce++,
    color,
    icon,
  });

  if (state.nonce > 10) {
    state.items.pop();
  }
}

function genAlert() {
  const color = genColor();

  return {
    color,
    icon: genIcon(color),
  };
}

function genColor() {
  return COLORS[Math.floor(Math.random() * 3)];
}

function genIcon(color) {
  return ICONS[color];
}

function start() {
  state.interval = setInterval(addEvent, 3000);
}

function stop() {
  clearInterval(state.interval);
  state.interval = null;
}
</script>
