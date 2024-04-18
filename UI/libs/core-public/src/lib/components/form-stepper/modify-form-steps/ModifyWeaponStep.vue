<template>
  <div>
    <v-form v-model="valid">
      <v-card-title>Modify Weapons</v-card-title>

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
      valid
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
import { computed, ref, watch } from 'vue'

interface ModifyWeaponProps {
  application: CompleteApplication
}

const valid = ref(false)

const props = defineProps<ModifyWeaponProps>()
const emit = defineEmits([
  'handle-continue',
  'handle-save',
  'handle-add-weapon',
  'handle-delete-weapon',
  'undo-add-weapon',
  'undo-delete-weapon',
  'update-step-three-valid',
])

const items = computed(() => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  let itemArray: Array<any> = []

  for (const weapon of props.application.application.weapons) {
    itemArray.push({ ...weapon })
  }

  for (const weapon of props.application.application.modifyAddWeapons) {
    const item = { ...weapon, added: true }

    itemArray.push(item)
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

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-three-valid', newValue)
  }
})
</script>
