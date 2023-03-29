<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-list>
    <v-list-item-group
      v-model="settings"
      multiple
      active-class=""
      aria-label="Background Checklist"
    >
      <div class="text-left">
        <div
          v-for="item in checklistItems"
          :key="item.value"
        >
          <v-list-item class="px-0">
            <template #default="{ active }">
              <v-list-item-content>
                <v-row>
                  <v-col
                    cols="12"
                    md="1"
                    lg="1"
                    class="pl-3"
                  >
                    <v-tooltip bottom>
                      <template #activator="{ on, attrs }">
                        <v-btn
                          icon
                          small
                          color="blue"
                          class="white--text"
                          :input-value="active"
                          @click="
                            permitStore.getPermitDetail.application.backgroundCheck[
                              item.value
                            ].value = true;
                            changed = item.label;
                            updatePermitDetails();
                            permitStore.getPermitDetail.application.backgroundCheck[
                              item.value
                            ].changeMadeBy = authStore.getAuthState.userEmail;
                          "
                          v-bind="attrs"
                          v-on="on"
                        >
                          <v-icon
                            v-if="
                              permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value
                            "
                          >
                            mdi-check-circle
                          </v-icon>
                          <v-icon
                            v-if="
                              !permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value
                            "
                          >
                            mdi-check-circle-outline
                          </v-icon>
                        </v-btn>
                      </template>
                      {{ $t('pass') }}
                    </v-tooltip>
                  </v-col>
                  <v-col
                    cols="12"
                    md="1"
                    lg="1"
                    class="pl-1"
                  >
                    <v-tooltip bottom>
                      <template #activator="{ on, attrs }">
                        <v-btn
                          icon
                          small
                          color="error"
                          :input-value="active"
                          @click="
                            permitStore.getPermitDetail.application.backgroundCheck[
                              item.value
                            ].value = false;
                            updatePermitDetails();
                            permitStore.getPermitDetail.application.backgroundCheck[
                              item.value
                            ].changeMadeBy = authStore.getAuthState.userEmail;
                          "
                          v-bind="attrs"
                          v-on="on"
                        >
                          <v-icon
                            v-if="
                              !permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value &&
                              permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value !== null
                            "
                          >
                            mdi-close-circle
                          </v-icon>
                          <v-icon
                            v-if="
                              permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value ||
                              permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].value === null
                            "
                          >
                            mdi-close-circle-outline
                          </v-icon>
                        </v-btn>
                      </template>
                      {{ $t(' Fail') }}
                    </v-tooltip>
                  </v-col>
                  <v-col
                    cols="12"
                    md="4"
                    lg="4"
                    class="px-1 pt-4"
                  >
                    <v-list-item-subtitle>
                      {{ $t(item.label) }}
                    </v-list-item-subtitle>
                  </v-col>
                  <v-col
                    cols="12"
                    md="6"
                    lg="6"
                    sm="12"
                    class="px-1 pt-4"
                  >
                    <v-list-item-subtitle>
                      {{
                        permitStore.getPermitDetail.application.backgroundCheck[
                          item.value
                        ]
                          ? permitStore.getPermitDetail.application
                              .backgroundCheck[item.value].changeMadeBy
                          : ''
                      }}
                    </v-list-item-subtitle>
                  </v-col>
                </v-row>
              </v-list-item-content>
            </template>
          </v-list-item>
          <v-divider></v-divider>
        </div>
      </div>
    </v-list-item-group>
  </v-list>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const permitStore = usePermitsStore();
const authStore = useAuthStore();
const changed = ref('');

const checklistItems = [
  {
    label: 'Proof of ID',
    value: 'proofOfID',
  },
  {
    label: 'Residency',
    value: 'proofOfResidency',
  },
  {
    label: 'NCIC Warrants',
    value: 'ncicWantsWarrants',
  },
  {
    label: 'Locals',
    value: 'locals',
  },
  {
    label: 'Probations',
    value: 'probations',
  },
  {
    label: 'DMV Record',
    value: 'dmvRecord',
  },
  {
    label: "AKS's Checked",
    value: 'akSsChecked',
  },
  {
    label: 'Coplink',
    value: 'coplink',
  },
  {
    label: 'Traffic Portal',
    value: 'trafficCourtPortal',
  },
  {
    label: 'Property Assessor',
    value: 'propertyAssesor',
  },
  {
    label: 'Voter Registration',
    value: 'voterRegistration',
  },
  {
    label: 'DOJ Approval',
    value: 'dojApprovalLetter',
  },
  {
    label: 'CII Number',
    value: 'ciiNumber',
  },
  {
    label: 'DOJ',
    value: 'doj',
  },
  {
    label: 'FBI',
    value: 'fbi',
  },
  {
    label: 'SR14',
    value: 'sR14',
  },
  {
    label: 'Firearms Reg',
    value: 'firearmsReg',
  },
  {
    label: "Chief LTR's RCRD",
    value: 'allDearChiefLTRsRCRD',
  },
  {
    label: 'Safety Certificate',
    value: 'safetyCertificate',
  },
  {
    label: 'Restrictions',
    value: 'restrictions',
  },
];

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(`Updated ${changed.value}`),
  {
    enabled: false,
  }
);

const settings = ref([]);
</script>
