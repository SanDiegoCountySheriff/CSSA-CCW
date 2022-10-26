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
            id="last-name-field"
            :label="$t('Last name')"
            :rules="[v => !!v || $t('Last name is required')]"
            v-model="
              completeApplicationStore.completeApplication.personalInfo.lastName
            "
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('First name')"
            :rules="[v => !!v || $t('First name is required')]"
            v-model="
              completeApplicationStore.completeApplication.personalInfo
                .firstName
            "
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            v-if="
              !completeApplicationStore.completeApplication.personalInfo
                .noMiddleName
            "
            :label="$t('Middle name')"
            :rules="[
              v =>
                (!!v &&
                  !completeApplicationStore.completeApplication.personalInfo
                    .noMiddleName) ||
                $t('Middle name is required or you must select no middle name'),
            ]"
            v-model="
              completeApplicationStore.completeApplication.personalInfo
                .middleName
            "
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

        <v-col
          cols="6"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Maiden name')"
            v-model="
              completeApplicationStore.completeApplication.personalInfo
                .maidenName
            "
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
                completeApplicationStore.completeApplication.personalInfo.noMiddleName =
                  v;
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
            v-model="
              completeApplicationStore.completeApplication.personalInfo.suffix
            "
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
          <v-text-field
            :label="$t('Social Security Number')"
            :rules="ssnRuleSet"
            :type="show1 ? 'text' : 'password'"
            :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
            v-model="
              completeApplicationStore.completeApplication.personalInfo.ssn
            "
            @click:append="show1 = !show1"
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

        <v-col
          cols="7"
          md="5"
          sm="3"
        >
          <v-text-field
            :label="$t('Confirm SSN')"
            :type="show2 ? 'text' : 'password'"
            :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="[
              ...ssnRuleSet,
              v =>
                v ===
                  completeApplicationStore.completeApplication.personalInfo
                    .ssn || $t('Social Security Numbers must match'),
            ]"
            v-model="ssnConfirm"
            @click:append="show2 = !show2"
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

      <v-divider class="my-3" />
      <v-subheader class="subHeader font-weight-bold">
        {{ $t('marital status') }}
      </v-subheader>
      <v-row class="ml-1">
        <v-col
          cols="10"
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
                completeApplicationStore.completeApplication.personalInfo.maritalStatus =
                  v;
              }
            "
          />
        </v-col>
        <v-col
          cols="20"
          md="5"
          sm="3"
          v-if="
            completeApplicationStore.completeApplication.personalInfo
              .maritalStatus === 'married'
          "
        >
          <v-subheader class="subHeader font-weight-bold">
            {{ $t('Spouse Information') }}
          </v-subheader>
          <v-row class="ml-5">
            <v-col
              cols="10"
              md="6"
              sm="4"
            >
              <v-text-field
                :label="$t('Last Name')"
                :rules="[v => !!v || $t('Last name cannot be blank')]"
                v-model="
                  completeApplicationStore.completeApplication.spouseInformation
                    .lastName
                "
              >
                <template #prepend>
                  <v-icon
                    small
                    color="error"
                  >
                    mdi-asterisk
                  </v-icon>
                </template>
              </v-text-field>

              <v-text-field
                :label="$t('Middle Name')"
                v-model="
                  completeApplicationStore.completeApplication.spouseInformation
                    .middleName
                "
              />
            </v-col>
            <v-col
              cols="10"
              md="6"
              sm="4"
            >
              <v-text-field
                :label="$t('First Name')"
                :rules="[v => !!v || $t('First name cannot be blank')]"
                v-model="
                  completeApplicationStore.completeApplication.spouseInformation
                    .firstName
                "
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
              <v-text-field
                :label="$t('Maiden Name')"
                v-model="
                  completeApplicationStore.completeApplication.spouseInformation
                    .maidenName
                "
              />
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-form>

    <v-divider />
    <v-subheader class="sub-header font-weight-bold">
      {{ $t('aliases') }}
    </v-subheader>
    <div class="alias-components-container">
      <AliasTable
        :aliases="completeApplicationStore.completeApplication.aliases"
      />
      <AliasDialog :save-alias="getAliasFromDialog" />
    </div>
    <FormButtonContainer
      :valid="valid"
      @submit="handleSubmit"
      @save="handleSave"
    />
    <FormErrorAlert
      v-if="errors.length > 0"
      :errors="errors"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { ssnRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { formatSSN } from '@shared-utils/formatters/defaultFormatters';
import { useRouter } from 'vue-router/composables';
import AliasDialog from '@core-public/components/dialogs/AliasDialog.vue';
import AliasTable from '@shared-ui/components/tables/AliasTable.vue';
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

const errors = ref([] as Array<string>);
const valid = ref(false);
const show1 = ref(false);
const show2 = ref(false);
let ssnConfirm = ref('');

const completeApplicationStore = useCompleteApplicationStore();

const router = useRouter();

function handleSubmit() {
  if (
    !completeApplicationStore.completeApplication.personalInfo.maritalStatus
  ) {
    errors.value.push('Marital Status');
  } else {
    runFormatters();
    props.handleNextSection();
  }
}

function handleSave() {
  completeApplicationStore.postCompleteApplicationFromApi;
  router.push('/');
}

function getAliasFromDialog(alias) {
  completeApplicationStore.completeApplication.aliases.unshift(alias);
}

function runFormatters() {
  completeApplicationStore.completeApplication.personalInfo.ssn = formatSSN(
    completeApplicationStore.completeApplication.personalInfo.ssn
  );
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
  width: 100%;
  justify-content: flex-start;
  align-items: flex-start;
}
</style>
