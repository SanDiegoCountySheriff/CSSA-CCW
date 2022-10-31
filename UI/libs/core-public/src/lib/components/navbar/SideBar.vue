<template>
  <v-navigation-drawer
    app
    v-model="state.drawer"
    :mini-variant="$vuetify.breakpoint.mdAndDown"
    class="sidebar"
    permanent
  >
    <v-list nav>
      <v-list-item>
        <v-list-item-avatar
          height="36"
          width="36"
        >
          <img
            src="/img/icons/agency_logo.svg"
            width="36"
            height="36"
            alt="Image"
            loading="lazy"
          />
        </v-list-item-avatar>
        <v-list-item-content>
          <v-list-item-title class="text-h6"> CCW </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-list-item-content>
          <v-list-item-title>
            {{ $t(props.title) }}
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider />
      <v-list-item-group v-model="state.infoGroup">
        <v-list-item
          v-for="(name, index) in props.options"
          :key="index"
          link
          @click="handleSelection(index)"
        >
          <v-list-item-title>
            {{ $t(name) }}
          </v-list-item-title>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { reactive } from 'vue';

interface ISideNavProps {
  handleSelection: CallableFunction;
  options: Array<string>;
  title: string;
}
const props = defineProps<ISideNavProps>();

const state = reactive({
  drawer: true,
  infoGroup: true,
});
</script>

<style lang="scss" scoped></style>
