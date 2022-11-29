<template>
  <v-navigation-drawer
    app
    v-model="state.drawer"
    :mini-variant="$vuetify.breakpoint.mdAndDown"
    class="sidebar"
    permanent
  >
    <v-list nav>
      <v-list-item :to="Routes.HOME_ROUTE_PATH">
        <v-list-item-avatar
          height="32"
          width="32"
        >
          <img
            :src="brandStore.getDocuments.agencyLogo"
            width="32"
            height="32"
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
import Routes from '@core-public/router/routes';
import { reactive } from 'vue';
import { useBrandStore } from '@shared-ui/stores/brandStore';

const brandStore = useBrandStore();

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
