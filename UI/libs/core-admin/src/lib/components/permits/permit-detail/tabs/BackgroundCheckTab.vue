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
              <v-checkbox
                color="blue"
                name="proofOfID"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .proofOfID
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="proofOfResidency"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .proofOfResidency
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="NCICWantsWarrants"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .NCICWantsWarrants
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="locals"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.locals
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="probations"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .probations
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="DMVRecord"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .DMVRecord
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="AKSsChecked"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .AKSsChecked
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="coplink"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.coplink
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="trafficCourtPortal"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .trafficCourtPortal
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="propertyAssesor"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .propertyAssesor
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="voterRegistration"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .voterRegistration
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="DOJApprovalLetter"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .DOJApprovalLetter
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="CIINumber"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .CIINumber
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="DOJ"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.DOJ
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="FBI"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.FBI
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="SR14"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck.SR14
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="firearmsReg"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .firearmsReg
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="allDearChiefLTRsRCRD"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .allDearChiefLTRsRCRD
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="safetyCertificate"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .safetyCertificate
                "
              ></v-checkbox>
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
              <v-checkbox
                color="blue"
                name="restrictions"
                :input-value="active"
                @click="updatePermitDetails"
                v-model="
                  permitStore.getPermitDetail.application.backgroudCheck
                    .restrictions
                "
              ></v-checkbox>
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
