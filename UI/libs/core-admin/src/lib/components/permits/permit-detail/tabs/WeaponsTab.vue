<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        {{ $t('Weapons') }}
        <v-spacer></v-spacer>
        <SaveButton
          :disabled="false"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <WeaponsDialog @save-weapon="getWeaponFromDialog" />
        <WeaponsTable
          :weapons="permitStore.getPermitDetail.application.weapons"
          :delete-enabled="true"
          @delete="deleteWeapon"
        />
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()

function getWeaponFromDialog(weapon) {
  permitStore.getPermitDetail.application.weapons.push(weapon)
}

function deleteWeapon(index) {
  permitStore.getPermitDetail.application.weapons.splice(index, 1)
}

function handleSave() {
  emit('on-save', 'Weapon Information')
}
</script>
