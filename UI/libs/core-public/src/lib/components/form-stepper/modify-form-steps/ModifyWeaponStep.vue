<template>
  <div>
    <v-form v-model="valid">
      <v-card-title>Modify Weapons</v-card-title>

      <v-card-text>
        <WeaponsTable
          :weapons="items"
          :modifying="true"
          :readonly="false"
          @handle-edit-weapon="handleEditWeapon"
          @modify-delete-weapon="deleteWeapon"
          @save-weapon="handleSaveWeapon"
          @undo-add-weapon="undoAddWeapon"
          @undo-delete-weapon="undoDeleteWeapon"
        />
      </v-card-text>
    </v-form>

    <FormButtonContainer
      @continue="handleContinue"
      @save="handleSave"
      valid
      v-on="$listeners"
    />
  </div>
</template>

<script lang="ts" setup>
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { computed, ref, watch, set } from 'vue'

const valid = ref(false)
const applicationStore = useCompleteApplicationStore()

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
  let itemArray: Array<WeaponInfoType> = []

  for (const weapon of applicationStore.completeApplication.application
    .weapons) {
    itemArray.push({ ...weapon })
  }

  for (const weapon of applicationStore.completeApplication.application
    .modifyAddWeapons) {
    const item = { ...weapon, added: true }

    itemArray.push(item)
  }

  for (const weapon of applicationStore.completeApplication.application
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
  const originalSerialNumber = items.value[data.index]?.serialNumber

  if (data.value.added) {
    const index =
      applicationStore.completeApplication.application.modifyAddWeapons.findIndex(
        weapon => weapon.serialNumber === originalSerialNumber
      )

    if (index !== -1) {
      set(
        applicationStore.completeApplication.application.modifyAddWeapons,
        index,
        {
          ...data.value,
        }
      )
    }
  } else {
    set(applicationStore.completeApplication.application.weapons, data.index, {
      ...data.value,
    })
  }
}

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
