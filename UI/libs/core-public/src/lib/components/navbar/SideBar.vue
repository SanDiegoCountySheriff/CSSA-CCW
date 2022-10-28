<template>
  <v-navigation-drawer
    app
    v-model="drawer"
    :mini-variant="$vuetify.breakpoint.mdAndDown"
    :clipped="$vuetify.breakpoint.mdAndUp"
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
            {{ $t('Information sections') }}
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-divider />
      <v-list-item-group v-model="state.infoGroup">
        <v-list-item
          v-for="(title, index) in options"
          :key="index"
          link
          @click="handleSelection(index)"
        >
          <v-list-item-title>
            {{ $t(title) }}
          </v-list-item-title>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { useCurrentInfoSection } from '@core-public/stores/currentInfoSection';
import { reactive } from 'vue';

const currentInfoSectionStore = useCurrentInfoSection();

const state = reactive({
  drawer: true,
  infoGroup: true,
});
const options = [
  'Personal Information',
  'Spouse Information',
  'Alias Information',
  'Id/Birth Information',
  'Citizenship Information',
  'Current Address Information',
  'Previous Address Information',
  'Mailing Address Information',
  'Physical Appearance',
  'Contact Information',
  'Employment Information',
  'Weapons Information',
];

function handleSelection(target: number) {
  currentInfoSectionStore.setCurrentInfoSection(target);
}
</script>

<style lang="scss" scoped></style>
