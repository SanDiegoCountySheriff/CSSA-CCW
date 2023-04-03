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
        {{ $t('Add Alias') }}
      </v-btn>
    </template>

    <v-card outlined>
      <v-card-title>{{ $t('Alias Information') }}</v-card-title>

      <v-card-text>
        <v-form
          ref="form"
          v-model="valid"
        >
          <v-row>
            <v-col>
              <v-text-field
                v-model="state.alias.prevLastName"
                :rules="requireNameRuleSet"
                maxlength="50"
                counter
                label="Previous Last Name"
                required
              ></v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                v-model="state.alias.prevFirstName"
                :rules="requireNameRuleSet"
                maxlength="50"
                counter
                label="Previous First name"
                required
              ></v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="state.alias.prevMiddleName"
                :rules="notRequiredNameRuleSet"
                maxlength="50"
                counter
                label="Previous Middle name"
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="state.alias.cityWhereChanged"
                maxlength="50"
                counter
                label="City Where Changed"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="state.alias.stateWhereChanged"
                maxlength="50"
                counter
                label="State or Region where changed"
              />
            </v-col>
            <v-col>
              <v-text-field
                v-model="state.alias.courtFileNumber"
                maxlength="50"
                counter
                label="Court File number"
              />
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
          color="primary"
          small
          @click="dialog = false"
        >
          {{ $t('Close') }}
        </v-btn>
      </v-card-actions>
    </v-card>
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
