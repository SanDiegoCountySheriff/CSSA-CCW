<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div data-app>
    <v-dialog v-model="dialog.state">
      <template #activator="{ on, attrs }">
        <v-btn
          id="add-alias-btn"
          color="primary my-5"
          v-bind="attrs"
          v-on="on"
        >
          {{ $t('Add Alias') }}
        </v-btn>
      </template>

      <div class="alias-container">
        <v-form
          ref="form"
          v-model="valid"
          class="form-container"
        >
          <v-row>
            <v-col
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="last-name"
                v-model="alias.prevLastName"
                label="Previous Last Name"
                :rules="[v => !!v || 'Last name is required']"
                required
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
              cols="6"
              md="5"
              sm="3"
            >
              <v-text-field
                id="first-name"
                v-model="alias.prevFirstName"
                label="Previous First name"
                :rules="[v => !!v || 'First name is required']"
                required
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
              cols="6"
              md="5"
              sm="3"
              class="pl-8"
            >
              <v-text-field
                v-model="alias.prevMiddleName"
                label="Previous Middle name"
              />
            </v-col>
            <v-col
              cols="6"
              md="5"
              sm="3"
              class="pl-8"
            >
              <v-text-field
                v-model="alias.cityWhereChanged"
                label="City Where Changed"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="6"
              md="5"
              sm="3"
              class="pl-8"
            >
              <v-text-field
                v-model="alias.stateWhereChanged"
                label="State or Region where changed"
              />
            </v-col>

            <v-col
              cols="6"
              md="5"
              sm="3"
              class="pl-8"
            >
              <v-text-field
                v-model="alias.courtFileNumber"
                label="Court File number"
              />
            </v-col>
          </v-row>
        </v-form>
        <div class="mt-2 btn-container">
          <v-btn
            id="submit-btn"
            color="primary"
            @click="handleSubmit"
            class="mr-2"
            :disabled="!valid"
          >
            {{ $t('Submit') }}
          </v-btn>
          <v-btn
            color="error"
            @click="dialog.state = false"
          >
            {{ $t('Close') }}
          </v-btn>
        </div>
      </div>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { AliasType } from '@shared-utils/types/defaultTypes';
import { reactive, ref } from 'vue';

interface AliasDialogProps {
  saveAlias?: (alias: AliasType) => void;
}

const props = withDefaults(defineProps<AliasDialogProps>(), {
  // eslint-disable-next-line @typescript-eslint/no-empty-function
  saveAlias: () => {},
});

const alias = reactive({
  prevLastName: '',
  prevFirstName: '',
  prevMiddleName: '',
  cityWhereChanged: '',
  stateWhereChanged: '',
  courtFileNumber: '',
} as AliasType);

let dialog = reactive({ state: false });
const valid = ref(false);

function handleSubmit() {
  props.saveAlias(alias);
  dialog.state = false;
}
</script>

<style lang="scss" scoped>
.alias-container {
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
}

.form-container {
  width: 90%;
}
</style>
