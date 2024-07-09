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

      <v-row>
        <v-col
          cols="12"
          class="pr-7"
        >
          <v-alert
            class="ml-4"
            border="left"
            color="blue"
            text
            type="info"
          >
            Please review the following weapon modification(s).
          </v-alert>
        </v-col>
      </v-row>
      <v-card-text>
        <v-row>
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
            @modify-delete-weapon="deleteWeapon"
            @undo-delete-weapon="handleUndoDeleteWeapon"
            @undo-add-weapon="handleUndoAddWeapon"
          />
        </v-row>

        <template
          v-if="
            permitStore.getPermitDetail.application.modifiedWeaponComplete !==
            null
          "
        >
          <v-row>
            <v-col>
              <v-btn
                @click="handleOpenPdf"
                color="primary"
                class="mr-3"
                outlined
              >
                <v-icon left>mdi-file-document-check</v-icon>
                Check Document
              </v-btn>
            </v-col>

            <v-col>
              <v-btn
                v-if="
                  !permitStore.getPermitDetail.application
                    .modifiedWeaponComplete
                "
                @click="onApproveWeaponChange"
                color="primary"
                style="float: right"
              >
                <v-icon
                  left
                  color="green"
                >
                  mdi-check
                </v-icon>
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
                style="float: right"
              >
                <v-icon left> mdi-undo </v-icon>
                Undo Approve
              </v-btn>
            </v-col>
          </v-row>
        </template>
        <v-row>
          <v-col></v-col>
        </v-row>
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
  const originalSerialNumber = items.value[data.index]?.serialNumber

  if (data.value.added) {
    const index =
      permitStore.getPermitDetail.application.modifyAddWeapons.findIndex(
        weapon => weapon.serialNumber === originalSerialNumber
      )

    if (index !== -1) {
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

function handleUndoDeleteWeapon(weapon: WeaponInfoType) {
  permitStore.getPermitDetail.application.modifyDeleteWeapons =
    permitStore.getPermitDetail.application.modifyDeleteWeapons.filter(w => {
      return w.serialNumber !== weapon.serialNumber
    })
}

function handleUndoAddWeapon(weapon: WeaponInfoType) {
  const index =
    permitStore.getPermitDetail.application.modifyAddWeapons.findIndex(
      w => w.serialNumber === weapon.serialNumber
    )

  if (index !== -1) {
    permitStore.getPermitDetail.application.modifyAddWeapons.splice(index, 1)
  }
}

function handleSaveWeapon(weapon: WeaponInfoType) {
  if (
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== null &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved']
  ) {
    permitStore.getPermitDetail.application.modifyAddWeapons.push(weapon)
  } else {
    permitStore.getPermitDetail.application.weapons.push(weapon)
  }
}

function deleteWeapon(index) {
  if (
    permitStore.getPermitDetail.application.modifiedWeaponComplete !== null &&
    permitStore.getPermitDetail.application.status !==
      ApplicationStatus['Modification Approved']
  ) {
    permitStore.getPermitDetail.application.modifyDeleteWeapons.push(index)
  } else {
    permitStore.getPermitDetail.application.weapons.splice(index, 1)
  }
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
