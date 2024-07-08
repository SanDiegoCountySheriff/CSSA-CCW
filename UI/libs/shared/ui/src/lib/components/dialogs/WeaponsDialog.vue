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
              <v-text-field
                v-model="weapon.make"
                :label="$t('Make')"
                :rules="[v => !!v || 'Make is required']"
                max-length="25"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-text-field
                v-model="weapon.model"
                :label="$t('Model')"
                :rules="[v => !!v || 'Model is required']"
                max-length="25"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-text-field
                v-model="weapon.caliber"
                :label="$t('Caliber')"
                :rules="[v => !!v || 'Caliber is required']"
                max-length="25"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col :class="$vuetify.breakpoint.smAndDown ? 'pb-0' : ''">
              <v-text-field
                v-model="weapon.serialNumber"
                :label="$t('Serial number')"
                :rules="[v => !!v || 'Serial number is required']"
                max-length="25"
                outlined
                dense
              />
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

        <v-spacer />

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
