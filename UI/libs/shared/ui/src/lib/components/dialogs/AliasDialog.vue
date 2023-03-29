<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-dialog
    v-model="dialog"
    max-width="600"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        small
        id="add-alias-btn"
        color="primary"
        v-bind="attrs"
        v-on="on"
      >
        {{ $t('Add Alias') }}
      </v-btn>
    </template>

    <v-container>
      <v-card>
        <v-form
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                dense
                id="last-name"
                maxlength="50"
                counter
                v-model="state.alias.prevLastName"
                label="Previous Last Name"
                :rules="requireNameRuleSet"
                required
              >
              </v-text-field>
            </v-col>
            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                dense
                maxlength="50"
                counter
                id="first-name"
                v-model="state.alias.prevFirstName"
                label="Previous First name"
                :rules="requireNameRuleSet"
                required
              >
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                dense
                maxlength="50"
                counter
                :rules="notRequiredNameRuleSet"
                v-model="state.alias.prevMiddleName"
                label="Previous Middle name"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                maxlength="50"
                counter
                dense
                v-model="state.alias.cityWhereChanged"
                label="City Where Changed"
              />
            </v-col>

            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                maxlength="50"
                counter
                dense
                v-model="state.alias.stateWhereChanged"
                label="State or Region where changed"
              />
            </v-col>

            <v-col
              cols="12"
              lg="6"
              md="6"
            >
              <v-text-field
                outlined
                dense
                maxlength="50"
                counter
                v-model="state.alias.courtFileNumber"
                label="Court File number"
              />
            </v-col>
          </v-row>
        </v-form>
        <v-btn
          small
          id="submit-btn"
          color="success"
          @click="handleSubmit"
          :disabled="!valid"
        >
          {{ $t('Submit') }}
        </v-btn>
        <v-btn
          color="error"
          small
          @click="dialog = false"
        >
          {{ $t('Close') }}
        </v-btn>
      </v-card>
    </v-container>
  </v-dialog>
</template>

<script setup lang="ts">
import { AliasType } from '@shared-utils/types/defaultTypes';
import {
  notRequiredNameRuleSet,
  requireNameRuleSet,
} from '@shared-ui/rule-sets/ruleSets';
import { reactive, ref } from 'vue';

const emit = defineEmits(['save-alias']);

const state = reactive({
  alias: {} as AliasType,
});

const dialog = ref(false);
const valid = ref(false);

function handleSubmit() {
  emit('save-alias', state.alias);
  state.alias = {} as AliasType;
  dialog.value = false;
}
</script>
