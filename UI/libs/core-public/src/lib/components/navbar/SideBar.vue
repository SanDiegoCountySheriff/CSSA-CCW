<template>
  <v-navigation-drawer
    app
    permanent
    v-model="state.drawer"
    expand-on-hover
  >
    <v-list nav>
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
