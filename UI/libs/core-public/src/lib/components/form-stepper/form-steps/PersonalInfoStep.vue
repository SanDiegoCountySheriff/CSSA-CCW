<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title v-if="!isMobile">
        {{ $t('Personal Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Personal Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="4"
            cols="12"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.firstName"
              :label="$t('First name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.middleName"
              :label="$t('Middle name')"
              :rules="notRequiredNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            />
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.lastName"
              :label="$t('Last name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.suffix"
              :label="$t('Suffix')"
              :dense="isMobile"
              maxlength="10"
              outlined
            />
          </v-col>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.personalInfo.maidenName"
              :label="$t('Maiden name')"
              :rules="notRequiredNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            />
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Social Security Information') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Social Security Information') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              :label="$t('Social Security Number')"
              :error-messages="errors"
              :value="hidden1"
              :rules="[
                v => !!v || $t('SSN cannot be blank'),
                v => v.length === 9 || $t('SSN must be 9 characters in length'),
              ]"
              :dense="isMobile"
              @input="
                event => {
                  handleInput(event);
                }
              "
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              :label="$t('Confirm SSN')"
              :rules="[
                v => !!v || $t('SSN cannot be blank'),
                v => v.length === 9 || $t('SSN must be 9 characters in length'),
              ]"
              :value="hidden2"
              :dense="isMobile"
              @input="
                event => {
                  handleConfirmInput(event);
                }
              "
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title v-if="!isMobile">
        {{ $t('Marital Status') }}
      </v-card-title>

      <v-card-subtitle v-if="isMobile">
        {{ $t('Marital Status') }}
      </v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-select
              v-model="model.application.personalInfo.maritalStatus"
              :label="'Marital status'"
              :hint="'Marital Status is required'"
              :rules="[v => !!v || $t('Marital status is required')]"
              :items="['Married', 'Single']"
              :dense="isMobile"
              @change="handleValidateForm"
              outlined
            >
            </v-select>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-title
        v-if="
          model.application.personalInfo.maritalStatus.toLowerCase() ===
            'married' && !isMobile
        "
      >
        {{ $t('Spouse Information') }}
      </v-card-title>

      <v-card-subtitle
        v-if="
          model.application.personalInfo.maritalStatus.toLowerCase() ===
            'married' && isMobile
        "
      >
        {{ $t('Spouse Information') }}
      </v-card-subtitle>

      <v-card-text
        v-if="
          model.application.personalInfo.maritalStatus.toLowerCase() ===
          'married'
        "
      >
        <v-row>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.lastName"
              :label="$t('Last Name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.firstName"
              :label="$t('First Name')"
              :rules="requireNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.middleName"
              :label="$t('Middle Name')"
              :rules="notRequiredNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            />
          </v-col>
        </v-row>
        <v-row>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.maidenName"
              :label="$t('Maiden Name')"
              :rules="notRequiredNameRuleSet"
              :dense="isMobile"
              maxlength="50"
              outlined
            />
          </v-col>
          <v-col
            cols="12"
            md="6"
            :class="isMobile ? 'pb-0' : ''"
          >
            <v-text-field
              v-model="model.application.spouseInformation.phoneNumber"
              :label="$t('Phone number')"
              :rules="phoneRuleSet"
              :dense="isMobile"
              maxlength="10"
              outlined
            >
            </v-text-field>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>

    <v-card-title v-if="!isMobile">
      {{ $t('Aliases') }}
    </v-card-title>

    <v-card-subtitle v-if="isMobile">
      {{ $t('Aliases') }}
    </v-card-subtitle>

    <v-card-text>
      <v-radio-group
        v-model="showAlias"
        :label="$t('In the past have you ever gone by a different name?')"
        :row="!isMobile"
      >
        <v-radio
          color="primary"
          :label="$t('Yes')"
          :value="true"
        />
        <v-radio
          color="primary"
          :label="$t('No')"
          :value="false"
        />
      </v-radio-group>
      <AliasDialog
        v-if="showAlias"
        @save-alias="getAliasFromDialog"
      />
      <AliasTable
        v-if="showAlias"
        :aliases="model.application.aliases"
        :enable-delete="true"
        @delete="deleteAlias"
      />
    </v-card-text>

    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { useVuetify } from '@shared-ui/composables/useVuetify';
import { computed, nextTick, onMounted, ref } from 'vue';
import {
  notRequiredNameRuleSet,
  phoneRuleSet,
  requireNameRuleSet,
} from '@shared-ui/rule-sets/ruleSets';

interface FormStepOneProps {
  value: CompleteApplication;
}

const props = defineProps<FormStepOneProps>();

const emit = defineEmits(['input', 'handle-save', 'handle-submit']);

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
});

const form = ref();
const errors = ref([] as Array<string>);
const valid = ref(false);
const showAlias = ref(false);
const hidden1 = ref('');
const hidden2 = ref('');
let ssnConfirm = ref('');
const vuetify = useVuetify();
const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
);

onMounted(() => {
  if (model.value.application.personalInfo.ssn) {
    ssnConfirm.value = model.value.application.personalInfo.ssn;
  }

  if (form.value) {
    form.value.validate();
  }
});

function handleSave() {
  emit('handle-save');
}

function handleInput(event: string) {
  if (event.match(/[0-9]/)) {
    if (event.length === 1) {
      model.value.application.personalInfo.ssn = event;
      hidden1.value += '*';
    } else {
      model.value.application.personalInfo.ssn += event.slice(-1);
      hidden1.value += '*';
    }
  } else if (event.match(/[A-Za-z]/)) {
    errors.value.push('SSN must only contain numbers');
  } else {
    if (!model.value.application.personalInfo.ssn.match(/[a-zA-Z\s]+$/)) {
      errors.value = [];
    }

    model.value.application.personalInfo.ssn =
      model.value.application.personalInfo.ssn.slice(0, -1);
    hidden1.value = hidden1.value.slice(0, -1);
  }

  if (
    model.value.application.personalInfo.ssn.length === 9 &&
    model.value.application.personalInfo.ssn.match(/^(\d)\1{8,}/)
  ) {
    errors.value.push('Cannot contain repeating numbers');
  }
}

function handleConfirmInput(event: string) {
  if (event.match(/[0-9]/)) {
    if (event.length === 1) {
      ssnConfirm.value = event;
      hidden2.value += '*';
    } else {
      ssnConfirm.value += event.slice(-1);
      hidden2.value += '*';
    }
  } else if (event.match(/[A-Za-z]/)) {
    errors.value.push('SSN must only contain numbers');
  } else {
    if (!ssnConfirm.value.match(/[a-zA-Z\s]+$/)) {
      errors.value = [];
    }

    ssnConfirm.value = ssnConfirm.value.slice(0, -1);
    hidden2.value = hidden2.value.slice(0, -1);
  }

  if (ssnConfirm.value.length === 9 && ssnConfirm.value.match(/^(\d)\1{8,}/)) {
    errors.value.push('Cannot contain repeating numbers');
  } else if (
    ssnConfirm.value.length === 9 &&
    ssnConfirm.value !== model.value.application.personalInfo.ssn
  ) {
    errors.value.push('SSN must match');
  }
}

async function handleSubmit() {
  // Clear out the hidden fields if information was entered incorrectly.
  if (
    model.value.application.personalInfo.maritalStatus.toLowerCase() ===
    'single'
  ) {
    model.value.application.spouseInformation.lastName = '';
    model.value.application.spouseInformation.firstName = '';
    model.value.application.spouseInformation.maidenName = '';
    model.value.application.spouseInformation.middleName = '';
    model.value.application.spouseInformation.phoneNumber = '';
  }

  emit('handle-submit');
  valid.value = false;
}

function getAliasFromDialog(alias) {
  model.value.application.aliases.push(alias);
}

function deleteAlias(index) {
  model.value.application.aliases.splice(index, 1);
}

function handleValidateForm() {
  if (form.value) {
    nextTick(() => {
      form.value.validate();
    });
  }
}
</script>

<style lang="scss">
.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
