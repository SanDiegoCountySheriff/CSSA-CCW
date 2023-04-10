<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Personal Information') }}
      </v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-tooltip bottom>
            <template #activator="{ on, attrs }">
              <v-text-field
                outlined
                dense
                maxlength="50"
                counter
                id="last-name-field"
                :color="$vuetify.theme.dark ? 'text' : 'text'"
                :label="$t('Last name')"
                :rules="requireNameRuleSet"
                v-model="completeApplication.personalInfo.lastName"
                v-bind="attrs"
                v-on="on"
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
            </template>
            {{
              $t(
                ' IMPORTANT! Must exactly match the information on your drivers license or id.'
              )
            }}
          </v-tooltip>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            maxlength="50"
            counter
            :label="$t('First name')"
            :rules="requireNameRuleSet"
            v-model="completeApplication.personalInfo.firstName"
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
          <v-text-field
            outlined
            dense
            maxlength="50"
            counter
            :label="$t('Middle name')"
            class="pl-6"
            :rules="notRequiredNameRuleSet"
            v-model="completeApplication.personalInfo.middleName"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            class="pl-6"
            maxlength="50"
            counter
            :label="$t('Maiden name')"
            :rules="notRequiredNameRuleSet"
            v-model="completeApplication.personalInfo.maidenName"
          />
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            maxlength="10"
            counter
            class="pl-6"
            :label="$t('Suffix')"
            v-model="completeApplication.personalInfo.suffix"
          />
        </v-col>
      </v-row>
      <v-divider class="my-3" />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Social Security Information') }}
      </v-subheader>

      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            :label="$t('Social Security Number')"
            :error-messages="errors"
            :value="hidden1"
            :rules="[
              v => !!v || $t('SSN cannot be blank'),
              v => v.length === 9 || $t('SSN must be 9 characters in length'),
            ]"
            @input="
              event => {
                handleInput(event);
              }
            "
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
          <v-text-field
            outlined
            dense
            :label="$t('Confirm SSN')"
            :rules="[
              v => !!v || $t('SSN cannot be blank'),
              v => v.length === 9 || $t('SSN must be 9 characters in length'),
            ]"
            :value="hidden2"
            @input="
              event => {
                handleConfirmInput(event);
              }
            "
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

      <v-divider class="my-3" />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Marital Status') }}
      </v-subheader>
      <v-row class="ml-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-select
            dense
            outlined
            v-model="completeApplication.personalInfo.maritalStatus"
            :label="'Marital status'"
            :hint="'Marital Status is required'"
            :rules="[v => !!v || $t('Marital status is required')]"
            :items="['Married', 'Single']"
          >
            <template #prepend>
              <v-icon
                x-small
                color="error"
              >
                mdi-star
              </v-icon>
            </template>
          </v-select>
        </v-col>
        <v-col
          cols="12"
          v-if="
            completeApplication.personalInfo.maritalStatus.toLowerCase() ===
            'married'
          "
        >
          <v-subheader class="sub-header font-weight-bold">
            {{ $t('Spouse Information') }}
          </v-subheader>
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                maxlength="50"
                counter
                :label="$t('Last Name')"
                :rules="requireNameRuleSet"
                v-model="completeApplication.spouseInformation.lastName"
              >
                <template #prepend>
                  <v-icon
                    small
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
              <v-text-field
                dense
                outlined
                maxlength="50"
                counter
                :label="$t('First Name')"
                :rules="requireNameRuleSet"
                v-model="completeApplication.spouseInformation.firstName"
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
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                class="pl-6"
                maxlength="50"
                counter
                :label="$t('Middle Name')"
                :rules="notRequiredNameRuleSet"
                v-model="completeApplication.spouseInformation.middleName"
              />
            </v-col>

            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                maxlength="50"
                counter
                class="pl-6"
                :label="$t('Maiden Name')"
                :rules="notRequiredNameRuleSet"
                v-model="completeApplication.spouseInformation.maidenName"
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                dense
                outlined
                maxlength="10"
                counter
                :label="$t('Phone number')"
                :rules="phoneRuleSet"
                v-model="completeApplication.spouseInformation.phoneNumber"
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
        </v-col>
      </v-row>
    </v-form>

    <v-divider />
    <v-subheader class="sub-header font-weight-bold">
      {{ $t('Aliases') }}
    </v-subheader>
    <v-radio-group
      class="ml-6"
      v-model="showAlias"
      :label="$t('In the past have you ever gone by a different name?')"
      row
    >
      <v-radio
        :color="$vuetify.theme.dark ? 'info' : 'primary'"
        class="ml-6"
        :label="$t('Yes')"
        :value="true"
      />
      <v-radio
        :color="$vuetify.theme.dark ? 'info' : 'primary'"
        class="ml-6"
        :label="$t('No')"
        :value="false"
      />
    </v-radio-group>
    <v-container
      fluid
      v-if="showAlias"
    >
      <div class="alias-components-container">
        <AliasTable
          :aliases="completeApplication.aliases"
          :enable-delete="true"
          @delete="deleteAlias"
        />
        <AliasDialog @save-alias="getAliasFromDialog" />
      </div>
    </v-container>
    <v-divider class="my-5" />
    <FormButtonContainer
      :valid="valid"
      :submitting="submited"
      @submit="handleSubmit"
      @save="saveMutation.mutate"
      @back="router.push('/')"
      @cancel="router.push('/')"
    />
    <FormErrorAlert
      v-if="errors.length > 0"
      :errors="errors"
    />
    <v-snackbar
      :value="snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import FormErrorAlert from '@shared-ui/components/alerts/FormErrorAlert.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { onMounted, ref } from 'vue';
import {
  notRequiredNameRuleSet,
  phoneRuleSet,
  requireNameRuleSet,
} from '@shared-ui/rule-sets/ruleSets';

interface FormStepOneProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepOneProps>(), {
  handleNextSection: () => null,
});

const errors = ref([] as Array<string>);
const valid = ref(false);
const showAlias = ref(false);
const snackbar = ref(false);
const submited = ref(false);
const hidden1 = ref('');
const hidden2 = ref('');
let ssnConfirm = ref('');

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const router = useRouter();

onMounted(() => {
  if (completeApplication.personalInfo.ssn) {
    ssnConfirm.value = completeApplication.personalInfo.ssn;
  }
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 2;
    props.handleNextSection();
  },
  onError: () => {
    submited.value = false;
    valid.value = true;
    snackbar.value = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    snackbar.value = true;
  },
});

function handleInput(event: string) {
  if (event.match(/[0-9]/)) {
    if (event.length === 1) {
      completeApplication.personalInfo.ssn = event;
      hidden1.value += '*';
    } else {
      completeApplication.personalInfo.ssn += event.slice(-1);
      hidden1.value += '*';
    }
  } else if (event.match(/[A-Za-z]/)) {
    errors.value.push('SSN must only contain numbers');
  } else {
    if (!completeApplication.personalInfo.ssn.match(/[a-zA-Z\s]+$/)) {
      errors.value = [];
    }

    completeApplication.personalInfo.ssn =
      completeApplication.personalInfo.ssn.slice(0, -1);
    hidden1.value = hidden1.value.slice(0, -1);
  }

  if (
    completeApplication.personalInfo.ssn.length === 9 &&
    completeApplication.personalInfo.ssn.match(/^(\d)\1{8,}/)
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
    ssnConfirm.value !== completeApplication.personalInfo.ssn
  ) {
    errors.value.push('SSN must match');
  }
}

async function handleSubmit() {
  // Clear out the hidden fields if information was entered incorrectly.
  if (
    completeApplication.personalInfo.maritalStatus.toLowerCase() === 'single'
  ) {
    completeApplication.spouseInformation.lastName = '';
    completeApplication.spouseInformation.firstName = '';
    completeApplication.spouseInformation.maidenName = '';
    completeApplication.spouseInformation.middleName = '';
    completeApplication.spouseInformation.phoneNumber = '';
  }

  updateMutation.mutate();
  valid.value = false;
  submited.value = true;
}

function getAliasFromDialog(alias) {
  completeApplication.aliases.push(alias);
}

function deleteAlias(index) {
  completeApplication.aliases.splice(index, 1);
}
</script>

<style lang="scss" scoped>
.alias-components-container {
  display: flex;
  flex-direction: column;
  width: 100%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
