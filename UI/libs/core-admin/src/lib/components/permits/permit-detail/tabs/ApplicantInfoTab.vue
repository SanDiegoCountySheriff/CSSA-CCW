<!-- eslint-disable vue/max-attributes-per-line -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card flat>
    <v-form v-model="state.valid">
      <v-card-title>
        {{ $t('Personal Information') }}
        <v-spacer></v-spacer>
        <SaveButton
          :disabled="!state.valid"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.personalInfo.lastName
              "
              :label="$t('Last name')"
              :rules="[v => !!v || 'Last name is required']"
              required
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.personalInfo
                      .lastName
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.personalInfo.firstName
              "
              :label="$t('First name')"
              :rules="[v => !!v || 'First name is required']"
              required
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.personalInfo
                      .firstName
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-text-field
              :label="$t('Middle name')"
              v-model="
                permitStore.getPermitDetail.application.personalInfo.middleName
              "
            >
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              :label="$t('Maiden name')"
              v-model="
                permitStore.getPermitDetail.application.personalInfo.maidenName
              "
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-text-field
              :label="$t('Suffix')"
              v-model="
                permitStore.getPermitDetail.application.personalInfo.suffix
              "
            >
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              v-if="!state.ssn"
              :label="$t('Partial Social Security Number')"
              readonly
              type="text"
              v-model="permitStore.getPermitDetail.application.personalInfo.ssn"
              required
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.personalInfo.ssn
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
            <v-text-field
              v-else
              :label="$t('Full Social Security Number')"
              readonly
              type="text"
              v-model="state.ssn"
              required
            >
              <template #append>
                <v-icon
                  color="error"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.personalInfo.ssn
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-select
              v-model="
                permitStore.getPermitDetail.application.personalInfo
                  .maritalStatus
              "
              :label="'Marital status'"
              :rules="[v => !!v || $t('Marital status is required')]"
              :items="['Married', 'Single']"
            >
              <template #append>
                <v-icon
                  color="error"
                  class="mr-3"
                  medium
                  v-if="
                    !permitStore.getPermitDetail.application.personalInfo
                      .maritalStatus
                  "
                >
                  mdi-alert-octagon
                </v-icon>
              </template>
            </v-select>
          </v-col>
          <v-col cols="6">
            <v-btn
              v-if="!state.ssn"
              color="primary"
              :loading="isLoading"
              @click="getSSN"
            >
              Request Social
            </v-btn>
            <v-btn
              v-else
              color="error"
              @click="hideSsn"
            >
              Finished With SSN
            </v-btn>
          </v-col>
        </v-row>

        <v-divider
          v-if="
            permitStore.getPermitDetail.application.personalInfo
              .maritalStatus === 'Married'
          "
        ></v-divider>

        <v-card-title
          v-if="
            permitStore.getPermitDetail.application.personalInfo
              .maritalStatus === 'Married'
          "
        >
          {{ $t('Spouse Information:') }}
        </v-card-title>
        <v-row
          class="ml-5"
          v-if="
            permitStore.getPermitDetail.application.personalInfo
              .maritalStatus === 'Married'
          "
        >
          <v-col
            cols="12"
            md="5"
            sm="12"
          >
            <v-text-field
              :label="$t('Spouse Last Name')"
              :rules="[v => !!v || $t('Spouse Last name cannot be blank')]"
              required
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .lastName
              "
              dense
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="12"
            class="pl-8"
          >
            <v-text-field
              :label="$t('Spouse Middle Name')"
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .middleName
              "
              dense
              outlined
            />
          </v-col>
          <v-col
            cols="12"
            md="5"
            sm="12"
          >
            <v-text-field
              :label="$t('Spouse First Name')"
              :rules="[v => !!v || $t('Spouse First name cannot be blank')]"
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .firstName
              "
              dense
              outlined
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
            md="5"
            sm="12"
            class="pl-8"
          >
            <v-text-field
              :label="$t('Spouse Maiden Name')"
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .maidenName
              "
              dense
              outlined
            />
          </v-col>
          <v-snackbar
            :value="isError"
            :timeout="3000"
            bottom
            color="error"
            outlined
          >
            {{ $t('Failed to retrive SSN') }}
          </v-snackbar>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue';
import { reactive } from 'vue';
import { useMutation } from '@tanstack/vue-query';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();
const emit = defineEmits(['on-save']);
const state = reactive({
  ssn: '',
  valid: false,
});

const {
  isError,
  isLoading,
  mutate: ssnMutation,
} = useMutation(
  ['ssnMutation'],
  () => permitStore.getPermitSsn(permitStore.getPermitDetail.userId),
  {
    onSuccess: res => {
      state.ssn = res;
    },
  }
);

function getSSN() {
  ssnMutation();
}

function hideSsn() {
  state.ssn = '';
}

function handleSave() {
  emit('on-save', 'Application Info');
}
</script>
