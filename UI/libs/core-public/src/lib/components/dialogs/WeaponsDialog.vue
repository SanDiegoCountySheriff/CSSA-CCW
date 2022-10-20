<template>
  <div>
    <v-dialog v-model="state.dialog">
      <template #activator="{ on, attrs }">
        <v-btn
          id="add-weapon-btn"
          color="primary my-5"
          v-bind="attrs"
          v-on="on"
        >
          {{ $t('Add Weapon') }}
        </v-btn>
      </template>
      <div class="weapon-container">
        <v-form
          ref="form"
          v-model="state.valid"
          class="form-container"
        >
          <v-row>
            <v-col>
              <v-text-field
                :label="$t('Make')"
                :rule="[v => !!v || 'Make is required']"
                v-model="state.weapon.make"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Model')"
                :rule="[v => !!v || 'Model is required']"
                v-model="state.weapon.model"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Caliber')"
                :rule="[v => !!v || 'Caliber is required']"
                v-model="state.weapon.caliber"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                :label="$t('Serial number')"
                :rule="[v => !!v || 'Serial number is required']"
                v-model="state.weapon.serialNumber"
              >
                <template #prepend>
                  <v-icon
                    x-small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
        <v-divider />
        <div class="btn-container">
          <v-btn
            id="submit-btn"
            color="primary"
            @click="handleSubmit"
            class="mr-2"
            :disabled="!state.valid"
          >
            {{ $t('Submit') }}
          </v-btn>
          <v-btn
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
import { reactive } from 'vue';
import { WeaponInfoType } from '@shared-utils/types/defaultTypes';

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
  state.dialog = false;
}
</script>

<style lang="scss" scoped>
.weapon-container {
  margin-left: 5%;
  display: flex;
  flex-direction: column;
  height: 50vh;
  width: 90%;
  justify-content: center;
  align-items: center;
  background: aliceblue;
  border-radius: 12px;
}
.btn-container {
  display: flex;
  width: 75%;
  justify-content: flex-end;
  margin-top: 1rem;
}
.form-container {
  width: 90%;
  margin-bottom: 1rem;
}
</style>
