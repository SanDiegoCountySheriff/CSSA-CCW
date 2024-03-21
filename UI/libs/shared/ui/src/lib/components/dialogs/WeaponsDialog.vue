<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <v-card outlined>
      <v-card-title>{{ $t('Weapon Information') }}</v-card-title>

      <v-card-text>
        <v-form
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-combobox
                max-length="25"
                :items="weaponMake"
                :label="$t('Make')"
                :rules="[v => !!v || 'Make is required']"
                v-model="weapon.make"
                outlined
                dense
              >
              </v-combobox>
            </v-col>
          </v-row>
          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-text-field
                max-length="25"
                :label="$t('Model')"
                :rules="[v => !!v || 'Model is required']"
                v-model="weapon.model"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-combobox
                max-length="25"
                :items="calibers"
                :label="$t('Caliber')"
                :rules="[v => !!v || 'Caliber is required']"
                v-model="weapon.caliber"
                outlined
                dense
              >
              </v-combobox>
            </v-col>
          </v-row>
          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-text-field
                :label="$t('Serial number')"
                :rules="[v => !!v || 'Serial number is required']"
                v-model="weapon.serialNumber"
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-btn
          text
          color="error"
          @click="dialog = false"
        >
          {{ $t('Cancel') }}
        </v-btn>
        <v-spacer></v-spacer>
        <v-btn
          text
          color="primary"
          @click="handleSubmit"
          :disabled="!valid"
        >
          {{ $t('Submit') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import { calibers, weaponMake } from '@shared-utils/lists/defaultConstants'
import { computed, ref } from 'vue'

interface WeaponsDialogProps {
  value: boolean
  item: WeaponInfoType
  editing: boolean
}
const props = defineProps<WeaponsDialogProps>()

const emit = defineEmits([
  'save-weapon',
  'input',
  'edit-weapon',
  'update-weapon',
])
const valid = ref(false)
const dialog = computed({
  get() {
    return props.value
  },
  set(value) {
    emit('input', value)
  },
})

const weapon = computed({
  get() {
    return props.item
  },
  set(value) {
    emit('update-weapon', value)
  },
})

function handleSubmit() {
  if (!props.editing) {
    emit('save-weapon', weapon.value)
    dialog.value = false
  } else {
    emit('edit-weapon', weapon.value)
  }
}
</script>
