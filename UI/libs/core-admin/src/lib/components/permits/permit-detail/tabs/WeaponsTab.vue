<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <v-card elevation="0">
      <v-card-title class="subtitle-2">
        {{ $t('Weapons:') }}
      </v-card-title>
      <div class="weapon-components-container">
        <WeaponsTable
          :weapons="permitStore.getPermitDetail.application.weapons"
          :delete-enabled="true"
          @delete="deleteWeapon"
        />
        <div class="offset-md-8">
          <WeaponsDialog :save-weapon="getWeaponFromDialog" />
        </div>
      </div>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue';
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();

function getWeaponFromDialog(weapon) {
  permitStore.getPermitDetail.application.weapons.push(weapon);
}

function deleteWeapon(index) {
  permitStore.getPermitDetail.application.weapons.splice(index, 1);
}
</script>
