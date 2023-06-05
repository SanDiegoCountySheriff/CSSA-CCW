<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <div>
    <v-list>
      <div
        v-for="item in checklistItems"
        :key="item.value"
      >
        <v-list-item>
          <template #default="{ active }">
            <v-list-item-content>
              <v-row align="center">
                <v-col cols="1">
                  <v-tooltip bottom>
                    <template #activator="{ on, attrs }">
                      <v-btn
                        icon
                        small
                        color="blue"
                        class="white--text"
                        :input-value="active"
                        @click="handlePass(item.value, item.label)"
                        v-bind="attrs"
                        v-on="on"
                      >
                        <v-icon
                          v-if="
                            permitStore.getPermitDetail.application
                              .backgroundCheck[item.value].value
                          "
                          color="success"
                        >
                          mdi-check-circle
                        </v-icon>
                        <v-icon
                          v-if="
                            !permitStore.getPermitDetail.application
                              .backgroundCheck[item.value].value
                          "
                          color="success"
                        >
                          mdi-check-circle-outline
                        </v-icon>
                      </v-btn>
                    </template>
                    {{ $t('Pass') }}
                  </v-tooltip>
                </v-col>
                <v-col cols="1">
                  <v-tooltip bottom>
                    <template #activator="{ on, attrs }">
                      <v-btn
                        icon
                        small
                        color="error"
                        :input-value="active"
                        @click="handleFail(item.value)"
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
                    {{ $t('Fail') }}
                  </v-tooltip>
                </v-col>
                <v-col cols="6">
                  <v-list-item-subtitle>
                    {{ $t(item.label) }}
                  </v-list-item-subtitle>
                </v-col>
                <v-col cols="4">
                  <v-dialog
                    v-if="
                      permitStore.getPermitDetail.application.backgroundCheck[
                        item.value
                      ].value !== null
                    "
                    :retain-focus="false"
                    width="800"
                  >
                    <template #activator="{ on, attrs }">
                      <v-list-item-avatar
                        color="primary"
                        class="float-right"
                        v-bind="attrs"
                        v-on="on"
                      >
                        <span class="white--text">
                          {{
                            permitStore.getPermitDetail.application
                              .backgroundCheck[item.value]
                              ? formatInitials(
                                  permitStore.getPermitDetail.application.backgroundCheck[
                                    item.value
                                  ].changeMadeBy.split(', ')[1],
                                  permitStore.getPermitDetail.application
                                    .backgroundCheck[item.value].changeMadeBy
                                )
                              : ''
                          }}
                        </span>
                      </v-list-item-avatar>
                    </template>

                    <v-card min-height="135">
                      <v-card-title> Change made by: </v-card-title>
                      <v-card-text>
                        <v-divider></v-divider>
                        <v-row>
                          <v-col></v-col>
                        </v-row>
                        <v-row
                          align="center"
                          justify="center"
                          class="text-center"
                        >
                          <v-col>
                            {{
                              permitStore.getPermitDetail.application
                                .backgroundCheck[item.value].changeMadeBy
                            }}
                          </v-col>
                          <v-col>
                            {{
                              formatDate(
                                permitStore.getPermitDetail.application
                                  .backgroundCheck[item.value].changeDateTimeUtc
                              )
                            }}
                          </v-col>
                          <v-col>
                            {{
                              formatTime(
                                permitStore.getPermitDetail.application
                                  .backgroundCheck[item.value].changeDateTimeUtc
                              )
                            }}
                          </v-col>
                        </v-row>
                      </v-card-text>
                    </v-card>
                  </v-dialog>
                </v-col>
              </v-row>
            </v-list-item-content>
          </template>
        </v-list-item>
        <v-divider></v-divider>
      </div>
    </v-list>

    <v-dialog
      v-model="ciiDialog"
      persistent
      max-width="400"
    >
      <v-card>
        <v-card-title>Input CII Number</v-card-title>
        <v-card-text>
          <v-row>
            <v-col>
              <v-text-field
                v-model="permitStore.getPermitDetail.application.ciiNumber"
                label="CII Number"
                color="primary"
                outlined
              ></v-text-field>
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="error"
            text
            @click="handleCancelCiiNumber"
          >
            Cancel
          </v-btn>
          <v-btn
            color="primary"
            text
            @click="handleInputCiiNumber"
          >
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@shared-ui/stores/auth'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { useQuery } from '@tanstack/vue-query'
import {
  formatDate,
  formatInitials,
  formatTime,
} from '@shared-utils/formatters/defaultFormatters'

const permitStore = usePermitsStore()
const authStore = useAuthStore()
const changed = ref('')
const currentBackgroundCheckItem = ref('')
const ciiDialog = ref(false)

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
    label: "AKA's Checked",
    value: 'akAsChecked',
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
    label: 'Firearms',
    value: 'firearms',
  },
  {
    label: "SID LTR's Received",
    value: 'sidLettersReceived',
  },
  {
    label: 'Safety Certificate',
    value: 'safetyCertificate',
  },
  {
    label: 'Restrictions',
    value: 'restrictions',
  },
]

const { refetch: updatePermitDetails } = useQuery(
  ['setPermitsDetails'],
  () => permitStore.updatePermitDetailApi(`Updated ${changed.value}`),
  {
    enabled: false,
  }
)

function handlePass(itemValue: string, itemLabel: string) {
  currentBackgroundCheckItem.value = itemValue
  permitStore.getPermitDetail.application.backgroundCheck[itemValue].value =
    true
  changed.value = itemLabel
  permitStore.getPermitDetail.application.backgroundCheck[
    itemValue
  ].changeMadeBy = authStore.getAuthState.userName
  permitStore.getPermitDetail.application.backgroundCheck[
    itemValue
  ].changeDateTimeUtc = new Date()

  if (itemValue === 'ciiNumber') {
    ciiDialog.value = true
  } else {
    updatePermitDetails()
  }
}

function handleFail(itemValue: string) {
  permitStore.getPermitDetail.application.backgroundCheck[itemValue].value =
    false
  permitStore.getPermitDetail.application.backgroundCheck[
    itemValue
  ].changeMadeBy = authStore.getAuthState.userName
  permitStore.getPermitDetail.application.backgroundCheck[
    itemValue
  ].changeDateTimeUtc = new Date()
  updatePermitDetails()
}

function handleInputCiiNumber() {
  ciiDialog.value = false
  updatePermitDetails()
}

function handleCancelCiiNumber() {
  permitStore.getPermitDetail.application.backgroundCheck.ciiNumber.value = null
  permitStore.getPermitDetail.application.ciiNumber = ''
  ciiDialog.value = false
}
</script>
