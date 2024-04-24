<template>
  <div>
    <v-form v-model="valid">
      <v-card-title>Modify Weapons</v-card-title>

      <v-card-text>
        <WeaponsTable
          :weapons="items"
          :modifying="true"
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
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import {
  CompleteApplication,
  WeaponInfoType,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, ref, watch, nextTick } from 'vue'

interface ModifyWeaponProps {
  application: CompleteApplication
}

const valid = ref(false)
const applicationStore = useCompleteApplicationStore()
// const items = ref<Array<WeaponInfoType>>([])

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

// onMounted(() => {
//   getItems()
// })

// function getItems() {
//   // eslint-disable-next-line @typescript-eslint/no-explicit-any
//   let itemArray: Array<WeaponInfoType> = []

//   for (const weapon of applicationStore.completeApplication.application
//     .weapons) {
//     itemArray.push({ ...weapon })
//   }

//   for (const weapon of applicationStore.completeApplication.application
//     .modifyAddWeapons) {
//     const item = { ...weapon, added: true }

//     itemArray.push(item)
//   }

//   for (const weapon of applicationStore.completeApplication.application
//     .modifyDeleteWeapons) {
//     const index = itemArray.findIndex(
//       item => item.serialNumber === weapon.serialNumber
//     )

//     if (index !== -1) {
//       const deletedWeapon = itemArray[index]

//       deletedWeapon.deleted = true
//       itemArray[index] = deletedWeapon
//     }
//   }

//   items.value = itemArray

//   // return itemArray
// }

const items = computed(() => {
  window.console.log('getting items again')
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

watch(
  items,
  () => {
    window.console.log('changing from modify weapon step')
  },
  {
    deep: true,
  }
)

// watch(
//   applicationStore.completeApplication.application,
//   () => {
//     window.console.log('getting items')
//     nextTick(() => {
//       getItems()
//     })
//   },
//   {
//     deep: true,
//   }
// )
</script>
