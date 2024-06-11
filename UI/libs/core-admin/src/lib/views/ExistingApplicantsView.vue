<template>
  <v-container fluid>
    <v-tabs v-model="tab">
      <v-tabs-slider color="primary"></v-tabs-slider>

      <v-tab> Link Existing Applications </v-tab>

      <v-tab> Undo Application Link </v-tab>
    </v-tabs>

    <v-tabs-items v-model="tab">
      <v-tab-item>
        <LinkExistingApplications />
      </v-tab-item>

      <v-tab-item>
        <UndoApplicationLink />
      </v-tab-item>
    </v-tabs-items>
  </v-container>
</template>

<script setup lang="ts">
import LinkExistingApplications from '@core-admin/components/linking/LinkExistingApplications.vue'
import UndoApplicationLink from '@core-admin/components/linking/UndoApplicationLink.vue'
import { ref } from 'vue'

const tab = ref(0)
</script>
