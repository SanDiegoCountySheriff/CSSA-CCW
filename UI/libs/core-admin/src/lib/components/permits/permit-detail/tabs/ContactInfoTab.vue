<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        {{ $t('Telephone Numbers') }}
        <v-spacer></v-spacer>
        <SaveButton
          :disabled="!valid"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <v-form v-model="valid">
          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.contact
                    .primaryPhoneNumber
                "
                :label="$t('Primary phone number')"
                :rules="phoneRuleSet"
              >
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
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.contact
                    .cellPhoneNumber
                "
                :label="$t('Cell phone number')"
                :hint="$t('Only numbers no spaces or dashes')"
                :rules="[
                  v =>
                    v.length === 10 ||
                    v === '' ||
                    $t('Cell phone number must be ten digits long'),
                ]"
                maxlength="10"
              >
                <template #append>
                  <v-icon> mdi-cellphone-iphone </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.contact
                    .workPhoneNumber
                "
                :label="$t('Work phone number')"
                :hint="$t('Only numbers no spaces or dashes')"
                :rules="[
                  v =>
                    v.length === 10 ||
                    v === '' ||
                    $t('Work phone number must be ten digits long'),
                ]"
                maxlength="10"
              >
                <template #append>
                  <v-icon> mdi-cellphone-link </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.contact.faxPhoneNumber
                "
                :label="$t('Fax number')"
                :hint="$t('Only numbers no spaces or dashes')"
                :rules="[
                  v =>
                    v.length === 10 ||
                    v === '' ||
                    $t('Fax number must be ten digits long'),
                ]"
                maxlength="10"
              >
                <template #append>
                  <v-icon> mdi-fax </v-icon>
                </template>
              </v-text-field>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-text-field
                v-model="permitStore.getPermitDetail.application.userEmail"
                :label="$t('Email address')"
                :hint="$t('User email address (Read Only)')"
                persistent-hint
                readonly
              >
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
          <v-row>
            <v-col>
              <v-text-field
                counter
                maxlength="25"
                v-model="
                  permitStore.getPermitDetail.application.idInfo.idNumber
                "
                :label="$t('State-issued ID number')"
                :rules="[v => !!v || $t('ID number is required')]"
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.idInfo.idNumber
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-text-field>
            </v-col>
            <v-col>
              <v-autocomplete
                :items="states"
                :label="$t('ID Issuing State')"
                :rules="[v => !!v || $t('Issuing state is required')]"
                v-model="
                  permitStore.getPermitDetail.application.idInfo.issuingState
                "
              >
                <template #append>
                  <v-icon
                    color="error"
                    medium
                    v-if="
                      !permitStore.getPermitDetail.application.idInfo
                        .issuingState
                    "
                  >
                    mdi-alert-octagon
                  </v-icon>
                </template>
              </v-autocomplete>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>
<script setup lang="ts">
import SaveButton from './SaveButton.vue';
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets';
import { ref } from 'vue';
import { states } from '@shared-utils/lists/defaultConstants';
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const emit = defineEmits(['on-save']);

const permitStore = usePermitsStore();
const valid = ref(false);

function handleSave() {
  emit('on-save', 'Contact Details');
}
</script>
