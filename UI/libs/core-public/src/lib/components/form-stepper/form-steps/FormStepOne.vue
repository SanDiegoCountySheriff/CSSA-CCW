<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="subHeader font-weight-bold">
        {{ $t('personal info') }}
      </v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Last name')"
            :rules="[v => !!v || 'Last name is required']"
            v-model="personalInfo.lastName"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('First name')"
            :rules="[v => !!v || 'First name is required']"
            v-model="personalInfo.firstName"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Middle name')"
            :rules="[
              v =>
                (!!v && !personalInfo.noMiddleName) ||
                'Middle name is required or you must select no middle name',
            ]"
            v-model="personalInfo.middleName"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Maiden name')"
            v-model="personalInfo.maidenName"
          />
        </v-col>

        <v-col
          cols="6"
          md="5"
        >
          <CheckboxInput
            :target="'noMiddleName'"
            :label="'No middle name'"
            @input="
              v => {
                personalInfo.noMiddleName = v;
              }
            "
          />
        </v-col>
        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Suffix')"
            v-model="personalInfo.suffix"
          />
        </v-col>
      </v-row>
      <v-divider class="my-3" />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Social Security Information') }}
      </v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="7"
          md="5"
          m="3"
        >
          <!-- TODO: Add further validation to this once we decide of SSN formatting -->
          <v-text-field
            :label="$t('Social Security Number')"
            :rules="[v => !!v || $t('Social Security Number cannot be blank')]"
            v-model="personalInfo.ssn"
          />
        </v-col>

        <v-col
          cols="7"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Confirm SSN')"
            :rules="[
              v => !!v || 'Confirm ssn cannot be blank',
              v => v === personalInfo.ssn || 'SSN\'s do not match',
            ]"
            v-model="ssnConfirm"
          />
        </v-col>
      </v-row>

      <v-divider class="my-3" />
      <v-row class="ml-1">
        <v-subheader class="subHeader font-weight-bold">
          {{ $t('marital status') }}
        </v-subheader>
        <v-col
          cols="16"
          md="5"
          sm="3"
        >
          <RadioGroupInput
            :label="'Marital status'"
            :options="[
              { label: 'Married', value: 'married' },
              { label: 'Single', value: 'single' },
            ]"
            :hint="'Marital Status is required'"
            :layout="'row'"
            @input="
              v => {
                personalInfo.maritalStatus = v;
              }
            "
          />
        </v-col>
      </v-row>
    </v-form>

    <v-divider />
    <v-subheader class="sub-header font-weight-bold">
      {{ $t('aliases') }}
    </v-subheader>
    <div class="alias-components-container">
      <AliasTable :aliases="aliases" />
      <AliasDialog :save-alias="getAliasFromDialog" />
    </div>
    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
    />
    <FormErrorAlert
      v-if="errors.length > 0"
      :errors="errors"
    />
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { useAliasStore } from '@shared-ui/stores/alias';
import { usePersonalInfoStore } from '@shared-ui/stores/personalInfo';
import AliasDialog from '@core-public/components/dialogs/AliasDialog.vue';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
import { AliasType, PersonalInfoType } from '@shared-utils/types/defaultTypes';
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import FormErrorAlert from '@shared-ui/components/alerts/FormErrorAlert.vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface FormStepOneProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepOneProps>(), {
  handleNextSection: () => null,
});

const personalInfo = reactive({} as PersonalInfoType);
const aliases = ref([] as Array<AliasType>);
const errors = ref([] as Array<string>);
const valid = ref(false);
let ssnConfirm = ref('');

const aliasStore = useAliasStore();
const personalInfoStore = usePersonalInfoStore();

function handleSubmit() {
  if (!personalInfo.maritalStatus) {
    errors.value.push('Marital Status');
  } else {
    personalInfoStore.setPersonalInfo(personalInfo);
    aliasStore.addAlias(aliases.value);
    props.handleNextSection();
  }
}

function getAliasFromDialog(alias) {
  aliases.value.unshift(alias);
}
</script>

<style lang="scss" scoped>
.subHeader {
  font-size: 1.5rem;
}

.form-btn-container {
  display: flex;
  width: 90%;
  justify-content: flex-end;
}
.alias-components-container {
  display: flex;
  flex-direction: column;
  width: 90%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
