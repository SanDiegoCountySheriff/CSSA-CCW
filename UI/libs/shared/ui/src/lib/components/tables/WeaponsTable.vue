<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-container fluid>
    <v-data-table
      hide-default-footer
      :headers="headers"
      :items="weapons"
      mobile-breakpoint="800"
    >
      <template #top>
        <v-toolbar flat>
          <v-toolbar-title> Weapon Information </v-toolbar-title>
          <v-spacer></v-spacer>
          <v-btn
            @click="weaponDialog = true"
            color="primary"
            small
          >
            {{ $t('Add Weapon') }}
          </v-btn>
        </v-toolbar>
      </template>
      <template #[`item.actions`]="{ item }">
        <v-tooltip
          top
          open-delay="500"
        >
          <template #activator="{ on, attrs }">
            <v-icon
              v-bind="attrs"
              @click="handleDelete(item)"
              v-on="on"
            >
              mdi-delete
            </v-icon>
          </template>
          <span>{{ $t('Delete item') }}</span>
        </v-tooltip>
        <v-icon
          @click="editWeapon(item)"
          color="primary"
          small
        >
          mdi-pencil
        </v-icon>
      </template>
    </v-data-table>
    <!-- ADD ISEDITING -->
    <WeaponsDialog
      v-model="weaponDialog"
      :item="editedWeapon"
      @update-weapon="handleUpdateWeapon"
      v-on="$listeners"
    />
  </v-container>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue'
import { ref } from 'vue'

interface IWeaponTableProps {
  weapons: Array<WeaponInfoType>
}

const emit = defineEmits(['delete-weapon', 'edit-weapon'])
const editedWeaponIndex = ref(-1)
const isEditing = ref(false)
const editedWeapon = ref({
  make: '',
  model: '',
  caliber: '',
  serialNumber: '',
})
const weaponDialog = ref(false)

const props = defineProps<IWeaponTableProps>()

const headers = [
  { text: 'Make', value: 'make' },
  { text: 'Model', value: 'model' },
  { text: 'Caliber', value: 'caliber' },
  { text: 'Serial Number', value: 'serialNumber' },
  { text: 'Actions', value: 'actions' },
]

function handleDelete(index) {
  emit('delete-weapon', index)
}

function editWeapon(item) {
  // const item = props.weapons

  // like hair colors, get indexOf from the weapons array
  editedWeaponIndex.value = props.weapons.indexOf(item)
  editedWeapon.value.model = item.model
  editedWeapon.value.caliber = item.caliber
  editedWeapon.value.serialNumber = item.serialNumber
  editedWeapon.value.make = item.make
  // set isEditing.value = true

  weaponDialog.value = true
}

function handleUpdateWeapon(item: WeaponInfoType) {
  editedWeapon.value = { ...item }
}

// catch @edit-weapon here
// function handleEditWeapon
// set isEditing.value = false
// re-emit 'edit-weapon' with INDEX and EDITEDWEAPON
</script>

<style lang="scss" scoped>
.theme--dark.v-data-table {
  background: #303030;
}
</style>
