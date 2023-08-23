<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        small
        color="primary"
        v-bind="attrs"
        v-on="on"
      >
        {{ $t('Add Weapon') }}
      </v-btn>
    </template>

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
                v-model="state.weapon.make"
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
                v-model="state.weapon.model"
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
                v-model="state.weapon.caliber"
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
                v-model="state.weapon.serialNumber"
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
          small
          color="primary"
          @click="handleSubmit"
          :disabled="!valid"
        >
          {{ $t('Submit') }}
        </v-btn>
        <v-btn
          small
          color="primary"
          @click="dialog = false"
        >
          {{ $t('Close') }}
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes'
import { calibers, weaponMake } from '@shared-utils/lists/defaultConstants'
import { reactive, ref } from 'vue'

const emit = defineEmits(['save-weapon'])
const valid = ref(false)
const dialog = ref(false)
const state = reactive({
  weapon: {} as WeaponInfoType,
})

function handleSubmit() {
  emit('save-weapon', state.weapon)
  state.weapon = {} as WeaponInfoType
  dialog.value = false
}
</script>
