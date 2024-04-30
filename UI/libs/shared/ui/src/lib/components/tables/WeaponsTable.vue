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
            v-if="editEnable"
            @click="openAddWeaponDialog"
            color="primary"
            small
          >
            {{ $t('Add Weapon') }}
          </v-btn>
        </v-toolbar>
      </template>
      <template
        v-if="editEnable"
        #[`item.actions`]="{ item, index }"
      >
        <v-tooltip
          top
          open-delay="500"
        >
          <template #activator="{ on, attrs }">
            <v-icon
              v-bind="attrs"
              @click="handleDelete(index)"
              color="error"
              v-on="on"
              default
            >
              mdi-delete
            </v-icon>
          </template>
          <span>{{ $t('Delete item') }}</span>
        </v-tooltip>
        <v-icon
          class="mx-3"
          @click="editWeapon(item)"
          color="primary"
          default
        >
          mdi-pencil
        </v-icon>
      </template>
    </v-data-table>
    <WeaponsDialog
      v-model="weaponDialog"
      :item="currentWeapon"
      :editing="isEditing"
      @update-weapon="handleUpdateWeapon"
      @edit-weapon="handleEditWeapon"
      v-on="$listeners"
    />
  </v-container>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import WeaponsDialog from '@shared-ui/components/dialogs/WeaponsDialog.vue'
import { computed, ref } from 'vue'

interface IWeaponTableProps {
  editEnable?: boolean
  weapons: Array<WeaponInfoType>
}

const emit = defineEmits(['delete-weapon', 'handle-edit-weapon'])
const editedWeaponIndex = ref(-1)
const isEditing = ref(false)
const editedWeapon = ref({
  make: '',
  model: '',
  caliber: '',
  serialNumber: '',
})
const weaponDialog = ref(false)

const props = withDefaults(defineProps<IWeaponTableProps>(), {
  editEnable: true,
})

const headersWithActions = [
  { text: 'Make', value: 'make' },
  { text: 'Model', value: 'model' },
  { text: 'Caliber', value: 'caliber' },
  { text: 'Serial Number', value: 'serialNumber' },
  { text: 'Actions', value: 'actions' },
]

const headersWithoutActions = [
  { text: 'Make', value: 'make' },
  { text: 'Model', value: 'model' },
  { text: 'Caliber', value: 'caliber' },
  { text: 'Serial Number', value: 'serialNumber' },
]

const headers = computed(() => {
  return props.editEnable ? headersWithActions : headersWithoutActions
})

const currentWeapon = ref<WeaponInfoType>({
  make: '',
  model: '',
  caliber: '',
  serialNumber: '',
})

function openAddWeaponDialog() {
  currentWeapon.value = {
    make: '',
    model: '',
    caliber: '',
    serialNumber: '',
  }
  isEditing.value = false
  editedWeaponIndex.value = -1
  weaponDialog.value = true
}

function handleDelete(index: number) {
  emit('delete-weapon', index)
}

function editWeapon(item: WeaponInfoType) {
  editedWeaponIndex.value = props.weapons.indexOf(item)
  currentWeapon.value = { ...item }
  isEditing.value = true
  weaponDialog.value = true
}

function handleUpdateWeapon(item: WeaponInfoType) {
  editedWeapon.value = { ...item }
}

function handleEditWeapon(item: WeaponInfoType) {
  isEditing.value = false
  emit('handle-edit-weapon', { index: editedWeaponIndex.value, value: item })
  weaponDialog.value = false
  currentWeapon.value = {
    make: '',
    model: '',
    caliber: '',
    serialNumber: '',
  }
}
</script>

<style lang="scss" scoped>
.theme--dark.v-data-table {
  background: #303030;
}
</style>
