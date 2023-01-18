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
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .proofOfID
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="proofOfID"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .proofOfID
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .proofOfID
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Proof of ID') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .proofOfResidency
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="proofOfResidency"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .proofOfResidency
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .proofOfResidency
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Proof of Residency') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .NCICWantsWarrants
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="NCICWantsWarrants"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .NCICWantsWarrants
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .NCICWantsWarrants
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('NCIC Wants/Warrants') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.locals
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="locals"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .locals
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .locals
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Locals') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .probations
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="probations"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .probations
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .probations
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Probations') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .DMVRecord
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="DMVRecord"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .DMVRecord
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .DMVRecord
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('DMV Record') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .AKSsChecked
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="AKSsChecked"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .AKSsChecked
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .AKSsChecked
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t("AKS's Checked") }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.coplink
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="coplink"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .coplink
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .coplink
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Coplink') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .trafficCourtPortal
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="trafficCourtPortal"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .trafficCourtPortal
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .trafficCourtPortal
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Traffic/Court Portal') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .propertyAssesor
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="propertyAssesor"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .propertyAssesor
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .propertyAssesor
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Property Assessor') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .voterRegistration
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="voterRegistration"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .voterRegistration
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .voterRegistration
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Voter Registration') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .DOJApprovalLetter
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="DOJApprovalLetter"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .DOJApprovalLetter
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .DOJApprovalLetter
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('DOJ Approval Letter') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .CIINumber
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="CIINumber"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .CIINumber
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .CIINumber
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('CII Number') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.DOJ
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="DOJ"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck.DOJ
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck.DOJ
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('DOJ') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.FBI
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="FBI"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck.FBI
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck.FBI
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('FBI') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.SR14
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="SR14"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .SR14
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .SR14
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('SR14') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .firearmsReg
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="firearmsReg"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .firearmsReg
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .firearmsReg
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Firearms Reg') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .allDearChiefLTRsRCRD
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="allDearChiefLTRsRCRD"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .allDearChiefLTRsRCRD
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .allDearChiefLTRsRCRD
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t("All Dear Chief LTR's RCRD") }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .safetyCertificate
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="safetyCertificate"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .safetyCertificate
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .safetyCertificate
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Safety Certificate') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-action>
              <v-switch
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .restrictions
                "
                @click="updatePermitDetails"
                :input-value="active"
                color="blue"
                name="restrictions"
                inset
              >
                <template #append>
                  <v-icon
                    :color="
                      permitStore.getPermitDetail.application.backgroudCheck
                        .restrictions
                        ? 'blue'
                        : 'error'
                    "
                  >
                    {{
                      permitStore.getPermitDetail.application.backgroudCheck
                        .restrictions
                        ? 'mdi-check'
                        : 'mdi-close'
                    }}
                  </v-icon>
                </template>
              </v-switch>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-subtitle>
                {{ $t('Restrictions') }}
              </v-list-item-subtitle>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
      </div>
    </v-list-item-group>
  </v-list>
</template>
<script setup lang="ts">
import { ref } from 'vue';
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { useQuery } from '@tanstack/vue-query';

const permitStore = usePermitsStore();

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  permitStore.updatePermitDetailApi,
  {
    enabled: false,
  }
);

const settings = ref([]);
</script>
