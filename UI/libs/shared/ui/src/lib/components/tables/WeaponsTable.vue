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
          v-if="!item.deleted && !item.added"
          top
        >
          <template #activator="{ on, attrs }">
            <v-icon
              v-bind="attrs"
              v-on="on"
              @click="handleDelete(index)"
              color="primary"
              default
            >
              mdi-delete
            </v-icon>
          </template>
          <span>{{ $t('Delete Weapon') }}</span>
        </v-tooltip>

        <v-icon
          v-if="item.added && !item.deleted"
          color="primary"
          @click="undoAddWeapon(item)"
        >
          mdi-undo
        </v-icon>

        <v-icon
          v-if="item.deleted"
          color="primary"
          @click="undoDeleteWeapon(item)"
        >
          mdi-undo
        </v-icon>

        <v-tooltip
          v-if="!props.modifying"
          top
        >
          <template #activator="{ on, attrs }">
            <v-icon
              v-bind="attrs"
              v-on="on"
              @click="editWeapon(item)"
              color="primary"
              class="mx-3"
              default
            >
              mdi-pencil
            </v-icon>
          </template>

          <span>{{ $t('Edit Weapon') }}</span>
        </v-tooltip>
      </template>

      <template #[`item.status`]="{ item }">
        <div v-if="item.deleted">Deleted</div>

        <div v-else-if="item.added">Added</div>

        <div v-else>Existing</div>
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
import { computed, ref, watch } from 'vue'

interface IWeaponTableProps {
  editEnable?: boolean
  modifying: boolean
  weapons: Array<WeaponInfoType>
}

const props = withDefaults(defineProps<IWeaponTableProps>(), {
  editEnable: true,
  modifying: false,
})

const emit = defineEmits([
  'delete-weapon',
  'handle-edit-weapon',
  'modify-delete-weapon',
  'undo-delete-weapon',
  'undo-add-weapon',
])

const editedWeaponIndex = ref(-1)
const isEditing = ref(false)
const editedWeapon = ref({
  make: '',
  model: '',
  caliber: '',
  serialNumber: '',
})
const weaponDialog = ref(false)

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

const modifyingHeaders = [
  { text: 'Make', value: 'make' },
  { text: 'Model', value: 'model' },
  { text: 'Caliber', value: 'caliber' },
  { text: 'Serial Number', value: 'serialNumber' },
  { text: 'Actions', value: 'actions' },
  { text: 'Status', value: 'status' },
]

const headers = computed(() => {
  if (props.editEnable && props.modifying) {
    return modifyingHeaders
  }

  if (props.editEnable) {
    return headersWithActions
  }

  return headersWithoutActions
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
  if (props.modifying) {
    emit('modify-delete-weapon', props.weapons[index])
  } else {
    emit('delete-weapon', index)
  }
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

function undoAddWeapon(item: WeaponInfoType) {
  emit('undo-add-weapon', item)
}

function undoDeleteWeapon(item: WeaponInfoType) {
  emit('undo-delete-weapon', item)
}

watch(
  props.weapons,
  () => {
    window.console.log('changing')
  },
  {
    deep: true,
  }
)
</script>

<style lang="scss" scoped>
.theme--dark.v-data-table {
  background: #303030;
}
</style>
