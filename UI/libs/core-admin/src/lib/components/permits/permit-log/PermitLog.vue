<!-- eslint-disable vue/singleline-html-element-content-newline -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-navigation-drawer
    v-if="isPermitDetail"
    v-model="drawer"
    :mini-variant.sync="mini"
    app
    right
    clipped
    permanent
  >
    <v-card
      class="mx-auto"
      max-width="700"
    >
      <v-card-title class="blue-grey white--text">
        <v-btn
          icon
          @click.stop="mini = !mini"
        >
          <v-icon v-if="mini"> mdi-chevron-left </v-icon>
          <v-icon v-if="!mini"> mdi-chevron-right </v-icon>
        </v-btn>
        <span class="text-h6">History</span>
        <v-spacer></v-spacer>
        <v-btn
          :outlined="interval == null"
          :color="interval == null ? 'white' : 'primary'"
          dark
          @click="interval == null ? start() : stop()"
        >
          Realtime
        </v-btn>
      </v-card-title>
      <v-card-text class="py-0">
        <v-timeline dense>
          <v-slide-x-reverse-transition
            group
            origin
            leave-absolute
          >
            <v-timeline-item
              v-for="item in items"
              :key="item.id"
              :color="item.color"
              class="mb-4"
              small
              fill-dot
            >
              <v-row justify="space-between">
                <v-col cols="7">
                  <v-chip
                    class="white--text ml-0"
                    :color="item.color"
                    label
                    small
                  >
                    APP
                  </v-chip>
                  Digital Downloads fulfilled 1 item.
                </v-col>
                <v-col
                  class="text-right"
                  cols="5"
                >
                  15:25 EDT
                </v-col>
              </v-row>
            </v-timeline-item>
          </v-slide-x-reverse-transition>
        </v-timeline>
      </v-card-text>
    </v-card>
  </v-navigation-drawer>
</template>
<script lang="ts">
const COLORS = ['info', 'warning', 'error', 'success'];
const ICONS = {
  info: 'mdi-information',
  warning: 'mdi-alert',
  error: 'mdi-alert-circle',
  success: 'mdi-check-circle',
};

export default {
  data: () => ({
    drawer: false,
    mini: false,
    interval: null,
    items: [
      {
        id: 1,
        color: 'info',
        icon: ICONS.info,
      },
    ],
    nonce: 2,
  }),

  computed: {
    isPermitDetail() {
      return this.$route.name === 'PermitDetail';
    },
  },

  beforeDestroy() {
    this.stop();
  },

  methods: {
    addEvent() {
      let { color, icon } = this.genAlert();

      const previousColor = this.items[0].color;

      while (previousColor === color) {
        color = this.genColor();
      }

      this.items.unshift({
        id: this.nonce++,
        color,
        icon,
      });

      if (this.nonce > 10) {
        this.items.pop();
      }
    },
    genAlert() {
      const color = this.genColor();

      return {
        color,
        icon: this.genIcon(color),
      };
    },
    genColor() {
      return COLORS[Math.floor(Math.random() * 3)];
    },
    genIcon(color) {
      return ICONS[color];
    },
    start() {
      this.interval = setInterval(this.addEvent, 3000);
    },
    stop() {
      clearInterval(this.interval);
      this.interval = null;
    },
  },
};
</script>
