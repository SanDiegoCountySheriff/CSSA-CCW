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
          <v-list-item>
            <template #default="{ active }">
              <v-list-item-action>
                <v-row
                  align="center"
                  justify="center"
                >
                  <v-col>
                    <v-btn
                      color="blue"
                      class="white--text"
                      width="60"
                      :input-value="active"
                      @click="
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ] = true;
                        updatePermitDetails();
                      "
                    >
                      Pass
                    </v-btn>
                  </v-col>
                  <v-col>
                    <v-btn
                      color="error"
                      width="60"
                      :input-value="active"
                      @click="
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ] = false;
                        updatePermitDetails();
                      "
                    >
                      Fail
                    </v-btn>
                  </v-col>
                  <v-col
                    :style="{
                      visibility:
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ] === null
                          ? 'hidden'
                          : 'visible',
                    }"
                  >
                    <v-icon
                      :color="
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ]
                          ? 'blue'
                          : 'error'
                      "
                    >
                      {{
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ]
                          ? 'mdi-check'
                          : 'mdi-close'
                      }}
                    </v-icon>
                  </v-col>
                </v-row>
              </v-list-item-action>
              <v-list-item-content>
                <v-row
                  align="center"
                  justify="center"
                >
                  <v-col>
                    <v-list-item-subtitle>
                      {{ $t(item.label) }}
                    </v-list-item-subtitle>
                  </v-col>
                  <v-col
                    cols="12"
                    md="5"
                    sm="12"
                  >
                    <v-dialog
                      v-if="
                        permitStore.getPermitDetail.application.backgroudCheck[
                          item.value
                        ] !== null
                      "
                      v-model="dialog"
                      width="800"
                    >
                      <template #activator="{ on, attrs }">
                        <v-list-item-avatar
                          color="blue"
                          height="40"
                          width="40"
                          v-bind="attrs"
                          v-on="on"
                        >
                          <span class="white--text"> SG</span>
                        </v-list-item-avatar>
                      </template>

                      <v-card>
                        <v-card-title class="text-h6 grey lighten-2">
                          Change made by:
                        </v-card-title>
                        <v-card-text>
                          <v-row
                            align="center"
                            justify="center"
                            class="mt-2"
                          >
                            <v-col>Sharath Gaddameedi</v-col>
                            <v-col>sgaddameedi@calsheriffs.com</v-col>
                            <v-col>January 9, 2023</v-col>
                            <v-col>05:31 PM</v-col>
                          </v-row>
                        </v-card-text>
                        <v-divider></v-divider>
                        <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn
                            color="primary"
                            text
                            @click="dialog = false"
                          >
                            Close
                          </v-btn>
                        </v-card-actions>
                      </v-card>
                    </v-dialog>
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
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const permitStore = usePermitsStore();

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
    value: 'NCICWantsWarrants',
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
    value: 'DMVRecord',
  },
  {
    label: "AKS's Checked",
    value: 'AKSsChecked',
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
    value: 'propertyAssessor',
  },
  {
    label: 'Voter Registration',
    value: 'voterRegistration',
  },
  {
    label: 'DOJ Approval',
    value: 'DOJApprovalLetter',
  },
  {
    label: 'CII Number',
    value: 'CIINumber',
  },
  {
    label: 'DOJ',
    value: 'DOJ',
  },
  {
    label: 'FBI',
    value: 'FBI',
  },
  {
    label: 'SR14',
    value: 'SR14',
  },
  {
    label: 'Firearms Reg',
    value: 'Firearms Reg',
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
  permitStore.updatePermitDetailApi,
  {
    enabled: false,
  }
);

const settings = ref([]);
const dialog = ref(false);
</script>
