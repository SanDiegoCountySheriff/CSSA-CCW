<template>
  <div>
    <v-card elevation="0">
      <v-card-title class="subtitle-2">
        {{ $t('Telephone Numbers:') }}
        <small class="text-caption grey--text text--darken-1"
          >{{ $t('&nbsp;(###-###-####)') }}
        </small>
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            :label="$t('Primary phone number')"
            :rules="phoneRuleSet"
            v-model="
              permitStore.getPermitDetail.application.contact.primaryPhoneNumber
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
            <template #append>
              <v-icon> mdi-cellphone-basic </v-icon>
              <v-icon
                color="error"
                medium
                v-if="
                  !permitStore.getPermitDetail.application.contact
                    .primaryPhoneNumber
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
            dense
            outlined
            :label="$t('Cell phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            maxlength="10"
            :rules="[
              v =>
                v.length === 10 ||
                v === '' ||
                $t('Cell phone number must be ten digits long'),
            ]"
            v-model="
              permitStore.getPermitDetail.application.contact.cellPhoneNumber
            "
          >
            <template #append>
              <v-icon> mdi-cellphone-iphone </v-icon>
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
            dense
            outlined
            :label="$t('Work phone number')"
            :hint="$t('Only numbers no spaces or dashes')"
            maxlength="10"
            :rules="[
              v =>
                v.length === 10 ||
                v === '' ||
                $t('Work phone number must be ten digits long'),
            ]"
            v-model="
              permitStore.getPermitDetail.application.contact.workPhoneNumber
            "
          >
            <template #append>
              <v-icon> mdi-cellphone-link </v-icon>
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
            dense
            outlined
            :label="$t('Fax number')"
            :hint="$t('Only numbers no spaces or dashes')"
            maxlength="10"
            :rules="[
              v =>
                v.length === 10 ||
                v === '' ||
                $t('Fax number must be ten digits long'),
            ]"
            v-model="
              permitStore.getPermitDetail.application.contact.faxPhoneNumber
            "
          >
            <template #append>
              <v-icon> mdi-fax </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
      <v-row>
        <v-col
          cols="12"
          md="5"
          sm="12"
          class="pl-8"
        >
          <CheckboxInput
            :label="'Text message updates'"
            :target="'textMessageUpdates'"
            @input="
              v => {
                permitStore.getPermitDetail.application.contact.textMessageUpdates =
                  v;
              }
            "
          />
        </v-col>
      </v-row>
    </v-card>
    <v-divider class="mt-5 mb-5"></v-divider>
    <v-card elevation="0">
      <v-card-title class="subtitle-2">
        {{ $t('Email:') }}
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            :label="$t('Email address')"
            :hint="$t('user email address (Read Only)')"
            persistent-hint
            readonly
            v-model="permitStore.getPermitDetail.application.userEmail"
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
              <v-icon> mdi-email-variant </v-icon>
              <v-icon
                color="error"
                medium
                v-if="!permitStore.getPermitDetail.application.userEmail"
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
    </v-card>
    <v-divider class="mt-5 mb-5"></v-divider>
    <v-card elevation="0">
      <v-card-title class="subtitle-2">
        {{ $t("Driver's License:") }}
        <small class="text-caption grey--text text--darken-1"
          >{{ $t('&nbsp;(Or other State Issued ID)') }}
        </small>
      </v-card-title>
      <v-row class="ml-5">
        <v-col
          cols="12"
          md="5"
          sm="12"
        >
          <v-text-field
            dense
            outlined
            v-model="permitStore.getPermitDetail.application.idInfo.idNumber"
            :label="$t('Id number')"
            :rules="[v => !!v || $t('Id  number is required')]"
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
                v-if="!permitStore.getPermitDetail.application.idInfo.idNumber"
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
            dense
            outlined
            :label="$t('Issuing State')"
            :rules="[v => !!v || $t('Issuing state is required')]"
            v-model="
              permitStore.getPermitDetail.application.idInfo.issuingState
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
            <template #append>
              <v-icon
                color="error"
                medium
                v-if="
                  !permitStore.getPermitDetail.application.idInfo.issuingState
                "
              >
                mdi-alert-octagon
              </v-icon>
            </template>
          </v-text-field>
        </v-col>
      </v-row>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();
</script>
