<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        {{ $t('Weapons') }}

        <v-spacer></v-spacer>

        <SaveButton
          :disabled="readonly"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <WeaponsTable
          :weapons="items"
          :readonly="readonly"
          :edit-enable="!readonly"
          :modifying="
            permitStore.getPermitDetail.application.modifiedWeaponComplete !==
            null
          "
          @delete-weapon="deleteWeapon"
          @handle-edit-weapon="handleEditWeapon"
          @save-weapon="handleSaveWeapon"
        />

        <template
          v-if="
            permitStore.getPermitDetail.application.modifiedWeaponComplete !==
            null
          "
        >
          <v-btn
            @click="handleOpenPdf"
            color="primary"
            class="mr-3"
          >
            <v-icon left>mdi-file-document-check</v-icon>
            Check Documents
          </v-btn>

          <v-btn
            v-if="
              !permitStore.getPermitDetail.application.modifiedWeaponComplete
            "
            @click="onApproveWeaponChange"
            color="primary"
          >
            <v-icon left> mdi-check </v-icon>
            Approve
          </v-btn>

          <v-btn
            :disabled="
              permitStore.getPermitDetail.application.status ===
              ApplicationStatus['Modification Approved']
            "
            v-else
            @click="onUndoApproveWeaponChange"
            color="primary"
          >
            <v-icon left> mdi-undo </v-icon>
            Undo Approve
          </v-btn>
        </template>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import {
  ApplicationStatus,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { openPdf } from '@core-admin/components/composables/openDocuments'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { computed, inject, set } from 'vue'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()
const readonly = inject<boolean>('readonly')

const items = computed(() => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  let itemArray: Array<WeaponInfoType> = []

  for (const weapon of permitStore.getPermitDetail.application.weapons) {
    itemArray.push({ ...weapon })
  }

  if (
    permitStore.getPermitDetail.application.modifyAddWeapons &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved']
  ) {
    for (const weapon of permitStore.getPermitDetail.application
      .modifyAddWeapons) {
      const item = { ...weapon, added: true }

      itemArray.push(item)
    }
  }

  if (
    permitStore.getPermitDetail.application.modifyDeleteWeapons &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved']
  ) {
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
  }

  return itemArray
})

function handleEditWeapon(data) {
  if (data.value.added) {
    const index =
      permitStore.getPermitDetail.application.modifyAddWeapons.findIndex(
        weapon => weapon.serialNumber === data.value.serialNumber
      )

    if (index !== 1) {
      window.console.log(index)

      set(permitStore.getPermitDetail.application.modifyAddWeapons, index, {
        ...data.value,
      })
    }
  } else {
    set(permitStore.getPermitDetail.application.weapons, data.index, {
      ...data.value,
    })
  }
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

function onUndoApproveWeaponChange() {
  permitStore.getPermitDetail.application.modifiedWeaponComplete = false
  emit('on-save', 'Undo approved weapon change')
}

async function handleOpenPdf() {
  const modifyNameDocument =
    permitStore.getPermitDetail.application.uploadedDocuments.find(d => {
      if (
        d.name.indexOf(
          `ModifyWeapons-${permitStore.getPermitDetail.application.modificationNumber}`
        ) >= 0
      ) {
        return d
      }

      return null
    })

  if (modifyNameDocument) {
    await openPdf(modifyNameDocument)
  }
}
</script>
