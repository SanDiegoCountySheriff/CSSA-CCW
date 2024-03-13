<template>
  <div>
    <v-form>
      <v-card-title>Modify Weapon</v-card-title>

      <v-card-text>
        <v-data-table
          :items="items"
          :headers="headers"
        >
          <template #top>
            <v-toolbar flat>
              <WeaponsDialog @save-weapon="handleSaveWeapon" />
            </v-toolbar>
          </template>

          <template #[`item.actions`]="{ item }">
            <v-icon
              v-if="!item.deleted && !item.added"
              @click="deleteWeapon(item)"
            >
              mdi-delete
            </v-icon>

            <v-icon
              v-if="item.added"
              @click="undoAddWeapon(item)"
            >
              mdi-undo
            </v-icon>

            <v-icon
              v-if="item.deleted"
              @click="undoDeleteWeapon(item)"
            >
              mdi-undo
            </v-icon>
          </template>

          <template #[`item.status`]="{ item }">
            <div v-if="item.deleted">Deleted</div>
            <div v-else-if="item.added">Added</div>
            <div v-else>Existing</div>
          </template>
        </v-data-table>
      </v-card-text>
    </v-form>

    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
    />
  </div>
</template>

<script lang="ts" setup>
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue'
import {
  CompleteApplication,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import { computed, ref } from 'vue'

interface ModifyWeaponProps {
  application: CompleteApplication
}

const props = defineProps<ModifyWeaponProps>()
const emit = defineEmits([
  'handle-continue',
  'handle-save',
  'handle-add-weapon',
  'handle-delete-weapon',
  'undo-add-weapon',
  'undo-delete-weapon',
])

const items = computed(() => {
  const itemArray = []

  for (const weapon of props.application.application.weapons) {
    itemArray.push(weapon)
  }

  for (const weapon of props.application.application.modifyAddWeapons) {
    weapon.added = true
    itemArray.push(weapon)
  }

  for (const weapon of props.application.application.modifyDeleteWeapons) {
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

const valid = ref(true)
const headers = [
  {
    text: 'Make',
    value: 'make',
  },
  {
    text: 'Model',
    value: 'model',
  },
  { text: 'Caliber', value: 'caliber' },
  {
    text: 'Serial Number',
    value: 'serialNumber',
  },
  { text: 'Actions', value: 'actions' },
  { text: 'Status', value: 'status' },
]

function deleteWeapon(weapon: WeaponInfoType) {
  emit('handle-delete-weapon', weapon)
}

function handleSaveWeapon(weapon: WeaponInfoType) {
  emit('handle-add-weapon', weapon)
}

function handleContinue() {
  emit('handle-continue')
}

function handleSave() {
  emit('handle-save')
}

function undoAddWeapon(weapon: WeaponInfoType) {
  emit('undo-add-weapon', weapon)
}

function undoDeleteWeapon(weapon: WeaponInfoType) {
  emit('undo-delete-weapon', weapon)
}
</script>
