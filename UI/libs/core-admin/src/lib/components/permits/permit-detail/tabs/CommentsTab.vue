<template>
  <div>
    <v-textarea
      fluid
      fill-height
      auto-grow
      clearable
      :rules="[
        v =>
          !v ||
          (v && v.length <= 1000) ||
          $t('Maximum 1000 characters are allowed'),
      ]"
      :label="$t('Interview questions / Work comments')"
      v-model="permitStore.getPermitDetail.application.comments"
    />
    <v-btn
      color="blue"
      class="white--text ml-4"
      min-width="200"
      @click="updatePermitDetails"
      small
    >
      {{ $t('Update') }}
    </v-btn>
  </div>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const permitStore = usePermitsStore();

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi('Added Comment'),
  {
    enabled: false,
  }
);
</script>
