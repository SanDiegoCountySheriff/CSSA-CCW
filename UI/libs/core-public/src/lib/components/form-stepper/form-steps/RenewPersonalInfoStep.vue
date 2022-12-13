<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-subheader class="subHeader font-weight-bold">
        {{ $t('Personal Info') }}
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
                id="last-name-field"
                :label="$t('Last name')"
                :rules="[v => !!v || $t('Last name is required')]"
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
            :label="$t('First name')"
            :rules="[v => !!v || $t('First name is required')]"
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
            class="pl-6"
            v-if="!completeApplication.personalInfo.noMiddleName"
            :label="$t('Middle name')"
            v-model="completeApplication.personalInfo.middleName"
          >
          </v-text-field>
        </v-col>

        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            class="pl-6"
            :label="$t('Maiden name')"
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
            class="pl-6"
            :label="$t('Suffix')"
            v-model="completeApplication.personalInfo.suffix"
          />
        </v-col>
      </v-row>
      <v-divider class="my-3" />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Permit Information') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-text-field
            outlined
            dense
            :label="$t(' Permit Number')"
            :rules="[v => !!v || $t(' Permit number cannot be blank')]"
            v-model="completeApplication.license.permitNumber"
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
            :label="$t('Issuing county')"
            :rules="[v => !!v || $t('Issuing county cannot be blank')]"
            v-model="completeApplication.license.issuingCounty"
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
          <v-menu
            v-model="menu"
            :close-on-content-click="true"
            transition="scale-transition"
            offset-y
            min-width="auto"
          >
            <template #activator="{ on, attrs }">
              <v-combobox
                outlined
                dense
                v-model="completeApplication.license.expirationDate"
                :label="$t('Expiration Date')"
                prepend-icon="mdi-calendar"
                :rules="[v => !!v || $t('Expiration date is required')]"
                readonly
                v-bind="attrs"
                v-on="on"
              ></v-combobox>
            </template>
            <v-date-picker
              v-model="completeApplication.license.expirationDate"
              no-title
              scrollable
            >
            </v-date-picker>
          </v-menu>
        </v-col>
      </v-row>
      <v-divider />
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
            :rules="ssnRuleSet"
            :type="show1 ? 'text' : 'password'"
            :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
            @click:append="show1 = !show1"
            v-model="completeApplication.personalInfo.ssn"
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
              ...ssnRuleSet,
              v =>
                v === completeApplication.personalInfo.ssn ||
                $t('SSN\'s do not match'),
            ]"
            :type="show2 ? 'text' : 'password'"
            :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'"
            v-model="ssnConfirm"
            @click:append="show2 = !show2"
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

      <v-subheader class="subHeader font-weight-bold">
        {{ $t('Marital Status') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-radio-group
            v-model="completeApplication.personalInfo.maritalStatus"
            :label="'Marital status'"
            :value="completeApplication.personalInfo.maritalStatus"
            :options="[
              { label: 'Married', value: 'married' },
              { label: 'Single', value: 'single' },
            ]"
            :hint="'Marital Status is required'"
            :layout="'row'"
          >
            <v-radio
              :label="'Married'"
              :value="'married'"
            />
            <v-radio
              :label="'Single'"
              :value="'single'"
            />
          </v-radio-group>
        </v-col>
        <v-col
          cols="12"
          lg="6"
          v-if="completeApplication.personalInfo.maritalStatus === 'married'"
        >
          <v-subheader class="subHeader font-weight-bold">
            {{ $t('Spouse Information') }}
          </v-subheader>
          <v-row>
            <v-col
              cols="12"
              lg="6"
            >
              <v-text-field
                outlined
                dense
                :label="$t('Last Name')"
                :rules="[v => !!v || $t('Last name cannot be blank')]"
                v-model="completeApplication.spouseInformation.lastName"
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
                :label="$t('First Name')"
                :rules="[v => !!v || $t('First name cannot be blank')]"
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
                outlined
                dense
                class="pl-6"
                :label="$t('Middle Name')"
                v-model="completeApplication.spouseInformation.middleName"
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
                :label="$t('Maiden Name')"
                v-model="completeApplication.spouseInformation.maidenName"
              />
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
        class="ml-6"
        :label="$t('Yes')"
        :value="true"
      />
      <v-radio
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
          @delete="deleteAlias"
        />
        <AliasDialog :save-alias="getAliasFromDialog" />
      </div>
    </v-container>
    <FormButtonContainer
      :valid="valid"
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
import { ref } from 'vue';
import { ssnRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

interface FormStepOneProps {
  handleNextSection: () => void;
}

const props = withDefaults(defineProps<FormStepOneProps>(), {
  handleNextSection: () => null,
});

const errors = ref([] as Array<string>);
const valid = ref(false);
const menu = ref(false);
const show1 = ref(false);
const show2 = ref(false);
const showAlias = ref(false);
const snackbar = ref(false);
let ssnConfirm = ref('');

const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;

const router = useRouter();

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 2;
    props.handleNextSection();
  },
  onError: () => {
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

function handleSubmit() {
  if (!completeApplication.personalInfo.maritalStatus) {
    errors.value.push('Marital Status');
  } else {
    updateMutation.mutate();
  }
}

function getAliasFromDialog(alias) {
  completeApplicationStore.completeApplication.application.aliases.unshift(
    alias
  );
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
