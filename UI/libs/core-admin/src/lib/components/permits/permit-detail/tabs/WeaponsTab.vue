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
        <WeaponsTable
          :weapons="permitStore.getPermitDetail.application.weapons"
          :delete-enabled="true"
          @delete-weapon="deleteWeapon"
          @handle-edit-weapon="handleEditWeapon"
          @save-weapon="handleSaveWeapon"
        />
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { set } from 'vue'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()

function handleEditWeapon(data) {
  set(permitStore.getPermitDetail.application.weapons, data.index, {
    ...data.value,
  })
}

function handleSaveWeapon(weapon: WeaponInfoType) {
  permitStore.getPermitDetail.application.weapons.push(weapon)
}

function deleteWeapon(index) {
  permitStore.getPermitDetail.application.weapons.splice(index, 1)
}

function handleSave() {
  emit('on-save', 'Weapon Information')
}
</script>
