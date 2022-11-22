<!-- eslint-disable vue/max-attributes-per-line -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('Applicant Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          :label="$t('Last name')"
          :rules="[v => !!v || 'Last name is required']"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.lastName
          "
          dense
          filled
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
          <template #append>
            <v-icon
              color="error"
              medium
              v-if="
                !permitStore.getPermitDetail.application.personalInfo.lastName
              "
            >
              mdi-alert-octagon
            </v-icon>
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          :label="$t('First name')"
          :rules="[v => !!v || 'First name is required']"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.firstName
          "
          dense
          filled
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
          <template #append>
            <v-icon
              color="error"
              medium
              v-if="
                !permitStore.getPermitDetail.application.personalInfo.firstName
              "
            >
              mdi-alert-octagon
            </v-icon>
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
        class="pl-8"
      >
        <v-text-field
          :label="$t('Middle name')"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.middleName
          "
          filled
        >
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
        class="pl-8"
      >
        <v-text-field
          :label="$t('Maiden name')"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.maidenName
          "
          filled
        >
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
        class="pl-8"
      >
        <v-text-field
          :label="$t('Suffix')"
          v-model="permitStore.getPermitDetail.application.personalInfo.suffix"
          filled
        >
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          :label="$t('Social Security Number')"
          :rules="ssnRuleSet"
          :type="show1 ? 'text' : 'password'"
          v-model="permitStore.getPermitDetail.application.personalInfo.ssn"
          filled
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
          <template #append>
            <v-icon
              @click="show1 = !show1"
              medium
            >
              {{ show1 ? 'mdi-eye' : 'mdi-eye-off' }}
            </v-icon>
            <v-icon
              color="error"
              medium
              v-if="!permitStore.getPermitDetail.application.personalInfo.ssn"
            >
              mdi-alert-octagon
            </v-icon>
            <v-icon
              v-else
              medium
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-text-field>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-radio-group
          row
          v-model="
            permitStore.getPermitDetail.application.personalInfo.maritalStatus
          "
        >
          <template #label>
            <div>{{ $t('Marital status') }}</div>
          </template>
          <v-radio value="married">
            <template #label>
              <div>{{ $t('Married') }}</div>
            </template>
          </v-radio>
          <v-radio value="single">
            <template #label>
              <div>{{ $t('Single') }}</div>
            </template>
          </v-radio>
          <template #prepend>
            <v-icon
              x-small
              color="error"
            >
              mdi-star
            </v-icon>
          </template>
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
            <v-icon
              v-else
              medium
              class="mr-3"
              color="primary"
            >
              mdi-checkbox-marked-circle
            </v-icon>
          </template>
        </v-radio-group>
      </v-col>
    </v-row>
    <v-divider></v-divider>
    <v-card-title
      v-if="
        permitStore.getPermitDetail.application.personalInfo.maritalStatus ===
        'married'
      "
      class="text-h5"
    >
      {{ $t('Spouse Information:') }}
    </v-card-title>
    <v-row
      class="ml-5"
      v-if="
        permitStore.getPermitDetail.application.personalInfo.maritalStatus ===
        'married'
      "
    >
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          :label="$t('Spouse Last Name')"
          :rules="[v => !!v || $t('Spouse Last name cannot be blank')]"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.lastName
          "
          filled
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
        sm="3"
        class="pl-8"
      >
        <v-text-field
          :label="$t('Spouse Middle Name')"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.middleName
          "
          filled
        />
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="3"
      >
        <v-text-field
          :label="$t('Spouse First Name')"
          :rules="[v => !!v || $t('Spouse First name cannot be blank')]"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.firstName
          "
          filled
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
        sm="3"
        class="pl-8"
      >
        <v-text-field
          :label="$t('Spouse Maiden Name')"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.maidenName
          "
          filled
        />
      </v-col>
    </v-row>
    <v-divider></v-divider>
  </v-card>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { ssnRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const show1 = ref(false);
const permitStore = usePermitsStore();
</script>
<style lang="scss" scoped>
.subHeader {
  font-size: 1.5rem;
}
</style>
