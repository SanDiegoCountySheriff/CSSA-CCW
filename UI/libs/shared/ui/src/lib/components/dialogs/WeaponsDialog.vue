<template>
  <div>
    <v-dialog v-model="state.dialog">
      <template #activator="{ on, attrs }">
        <v-btn
          small
          class="mb-5"
          id="add-weapon-btn"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          v-bind="attrs"
          v-on="on"
        >
          {{ $t('Add Weapon') }}
        </v-btn>
      </template>
      <div
        class="weapon-container"
        :style="{ background: $vuetify.theme.dark ? '#222' : '#EEE' }"
      >
        <v-form
          ref="form"
          v-model="state.valid"
          class="form-container"
        >
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-combobox
                dense
                outlined
                max-length="25"
                counter
                :items="weaponMake"
                :label="$t('Make')"
                :rules="[v => !!v || 'Make is required']"
                v-model="state.weapon.make"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-combobox>
            </v-col>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                max-length="25"
                counter
                :label="$t('Model')"
                :rules="[v => !!v || 'Model is required']"
                v-model="state.weapon.model"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              lg="6"
            >
              <v-combobox
                dense
                outlined
                max-length="25"
                counter
                :items="calibers"
                :label="$t('Caliber')"
                :rules="[v => !!v || 'Caliber is required']"
                v-model="state.weapon.caliber"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-combobox>
            </v-col>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                :label="$t('Serial number')"
                :rules="[v => !!v || 'Serial number is required']"
                v-model="state.weapon.serialNumber"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-star
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
        <v-divider />
        <div class="btn-container">
          <v-btn
            small
            id="submit-btn"
            color="primary"
            @click="handleSubmit"
            class="mr-2"
            :disabled="!state.valid"
          >
            {{ $t('Submit') }}
          </v-btn>
          <v-btn
            small
            color="error"
            @click="state.dialog = false"
          >
            {{ $t('Close') }}
          </v-btn>
        </div>
      </div>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';
import { reactive } from 'vue';
import { calibers, weaponMake } from '@shared-utils/lists/defaultConstants';

interface IWeaponsDialogProps {
  saveWeapon: (weapon) => void;
}

const props = defineProps<IWeaponsDialogProps>();

const state = reactive({
  weapon: {} as WeaponInfoType,
  dialog: false,
  valid: false,
});

function handleSubmit() {
  props.saveWeapon(state.weapon);
  state.weapon = {} as WeaponInfoType;
  state.dialog = false;
}
</script>

<style lang="scss" scoped>
.weapon-container {
  margin-left: 5%;
  display: flex;
  flex-direction: column;
  height: auto;
  width: 90%;
  justify-content: center;
  align-items: center;
  border-radius: 12px;
}
.btn-container {
  display: flex;
  width: 75%;
  justify-content: flex-end;
  margin: 2em 0;
}
.form-container {
  width: 90%;
  margin: 2em;
}
</style>
