<!-- eslint-disable vue/max-attributes-per-line -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="0">
    <v-card-title class="subtitle-2">
      {{ $t('Personal Information:') }}
    </v-card-title>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          :label="$t('Last name')"
          :rules="[v => !!v || 'Last name is required']"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.lastName
          "
          dense
          outlined
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
          </template>
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          :label="$t('First name')"
          :rules="[v => !!v || 'First name is required']"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.firstName
          "
          dense
          outlined
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
          :label="$t('Middle name')"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.middleName
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
          :label="$t('Maiden name')"
          v-model="
            permitStore.getPermitDetail.application.personalInfo.maidenName
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
          :label="$t('Suffix')"
          v-model="permitStore.getPermitDetail.application.personalInfo.suffix"
          dense
          outlined
        >
        </v-text-field>
      </v-col>
      <v-col
        cols="12"
        md="5"
        sm="12"
      >
        <v-text-field
          :label="$t('Social Security Number')"
          :rules="ssnRuleSet"
          :type="show1 ? 'text' : 'password'"
          v-model="permitStore.getPermitDetail.application.personalInfo.ssn"
          dense
          outlined
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
          </template>
        </v-text-field>
      </v-col>
    </v-row>
    <v-row class="ml-5">
      <v-col
        cols="12"
        md="5"
        sm="12"
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
          </template>
        </v-radio-group>
      </v-col>
    </v-row>
    <v-divider
      v-if="
        permitStore.getPermitDetail.application.personalInfo.maritalStatus ===
        'married'
      "
    ></v-divider>
    <v-card-title
      v-if="
        permitStore.getPermitDetail.application.personalInfo.maritalStatus ===
        'married'
      "
      class="subtitle-2"
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
        sm="12"
      >
        <v-text-field
          :label="$t('Spouse Last Name')"
          :rules="[v => !!v || $t('Spouse Last name cannot be blank')]"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.lastName
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
          :label="$t('Spouse Middle Name')"
          v-model="
            permitStore.getPermitDetail.application.spouseInformation.middleName
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
            permitStore.getPermitDetail.application.spouseInformation.firstName
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
            permitStore.getPermitDetail.application.spouseInformation.maidenName
          "
          dense
          outlined
        />
      </v-col>
    </v-row>
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
