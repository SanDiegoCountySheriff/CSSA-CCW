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
          :weapons="items"
          :delete-enabled="true"
          :modifying="true"
          @delete-weapon="deleteWeapon"
          @handle-edit-weapon="handleEditWeapon"
          @save-weapon="handleSaveWeapon"
        />

        <v-btn
          color="primary"
          class="mr-3"
        >
          <v-icon left>mdi-file-document-check</v-icon>
          Check Documents
        </v-btn>

        <v-btn
          @click="onApproveWeaponChange"
          color="primary"
        >
          <v-icon left> mdi-check </v-icon>
          Approve
        </v-btn>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, set } from 'vue'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()

const items = computed(() => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  let itemArray: Array<WeaponInfoType> = []

  for (const weapon of permitStore.getPermitDetail.application.weapons) {
    itemArray.push({ ...weapon })
  }

  for (const weapon of permitStore.getPermitDetail.application
    .modifyAddWeapons) {
    const item = { ...weapon, added: true }

    itemArray.push(item)
  }

  for (const weapon of permitStore.getPermitDetail.application
    .modifyDeleteWeapons) {
    const index = itemArray.findIndex(
      item => item.serialNumber === weapon.serialNumber
    )

    if (index !== -1) {
      const deletedWeapon = itemArray[index]

      deletedWeapon.deleted = true
      itemArray[index] = deletedWeapon
    }
  }

  return itemArray
})

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

function onApproveWeaponChange() {
  permitStore.getPermitDetail.application.modifiedWeaponComplete = true
  emit('on-save', 'Approved weapon change')
}
</script>
