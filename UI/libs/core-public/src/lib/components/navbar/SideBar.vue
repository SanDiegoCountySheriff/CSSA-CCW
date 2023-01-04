<template>
  <v-navigation-drawer
    app
    v-model="state.drawer"
    :mini-variant.sync="mini"
    class="sidebar"
    permanent
    :temporary="!mini"
    hide-overlay
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
      <v-divider />
      <v-list-item-group
        aria-label="Information sections group"
        role="list"
        v-model="state.infoGroup"
      >
        <v-list-item
          v-for="(name, index) in props.options"
          :key="index"
          link
          role="listitem"
          @click="handleSelection(index)"
        >
          <v-list-item-title>
            {{ $t(name) }}
          </v-list-item-title>
        </v-list-item>
      </v-list-item-group>
      <v-list
        class="bottom-list"
        nav
        dense
      >
        <v-card class="mt-2 mb-2 elevation-0">
          <v-list-item
            link
            @click="mini = !mini"
          >
            <v-list-item-icon>
              <v-icon>
                {{ mini ? 'mdi-menu-right-outline' : 'mdi-menu-left-outline' }}
              </v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{ $t('Collapse Menu') }}</v-list-item-title>
          </v-list-item>
        </v-card>
      </v-list>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import Routes from '@core-public/router/routes';
import { reactive, ref } from 'vue';
import { useBrandStore } from '@shared-ui/stores/brandStore';

const brandStore = useBrandStore();
const mini = ref(true);

interface ISideNavProps {
  handleSelection: CallableFunction;
  options: Array<string>;
}
const props = defineProps<ISideNavProps>();

const state = reactive({
  drawer: true,
  infoGroup: true,
});
</script>
