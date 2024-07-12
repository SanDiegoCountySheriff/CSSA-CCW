<!-- eslint-disable vue/max-attributes-per-line -->
<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card flat>
    <v-form v-model="state.valid">
      <v-card-title>
        {{ $t('Personal Information') }}
        <v-spacer></v-spacer>
        <SaveButton
          :disabled="!state.valid || readonly"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-spacer></v-spacer>
      <template
        v-if="
          permitStore.getPermitDetail.application.modifiedNameComplete !== null
        "
      >
        <v-row>
          <v-col
            cols="12"
            class="pr-7"
          >
            <v-alert
              class="ml-4"
              border="left"
              color="blue"
              text
              type="info"
            >
              Please review the following name modification(s).
            </v-alert>
          </v-col>
        </v-row>
      </template>
      <template
        v-if="
          permitStore.getPermitDetail.application.modifiedNameComplete !== null
        "
      >
        <v-card-text>
          <v-row>
            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.personalInfo
                    .modifiedFirstName
                "
                :readonly="readonly"
                label="Modified First Name"
                color="primary"
                outlined
                dense
              />
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.personalInfo
                    .modifiedMiddleName
                "
                :readonly="readonly"
                label="Modified Middle Name"
                color="primary"
                outlined
                dense
              />
            </v-col>

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.personalInfo
                    .modifiedLastName
                "
                :readonly="readonly"
                label="Modified Last Name"
                color="primary"
                outlined
                dense
              />
            </v-col>
          </v-row>

          <v-row>
            <v-col cols="3">
              <v-btn
                @click="handleOpenPdf"
                color="primary"
                outlined
              >
                <v-icon left>mdi-file-document-check</v-icon>
                Check Document
              </v-btn>
            </v-col>

            <v-col
              cols="3"
              class="mb-5"
              offset-md="6"
            >
              <v-btn
                v-if="
                  !permitStore.getPermitDetail.application.modifiedNameComplete
                "
                @click="onApproveNameChange"
                color="primary"
                style="float: right"
              >
                <v-icon
                  left
                  color="green"
                >
                  mdi-check
                </v-icon>
                Approve
              </v-btn>

              <v-btn
                v-if="
                  permitStore.getPermitDetail.application.modifiedNameComplete
                "
                @click="onUndoApproveNameChange"
                color="primary"
                style="float: right"
              >
                <v-icon left>mdi-undo</v-icon>
                Undo Approve
              </v-btn>
            </v-col>
            <v-spacer></v-spacer>
          </v-row>
        </v-card-text>
      </template>

      <v-divider></v-divider>

      <v-card-text>
        <v-row>
          <v-col></v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.personalInfo.lastName
              "
              :readonly="readonly"
              :label="$t('Last name')"
              :rules="[v => !!v || 'Last name is required']"
              required
              outlined
              dense
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
              :readonly="readonly"
              :label="$t('First name')"
              :rules="[v => !!v || 'First name is required']"
              required
              outlined
              dense
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
              v-model="
                permitStore.getPermitDetail.application.personalInfo.middleName
              "
              :label="$t('Middle name')"
              :readonly="readonly"
              outlined
              dense
            >
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.personalInfo.maidenName
              "
              :readonly="readonly"
              :label="$t('Maiden name')"
              outlined
              dense
            >
            </v-text-field>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.personalInfo.suffix
              "
              :readonly="readonly"
              :label="$t('Suffix')"
              outlined
              dense
            >
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              v-if="!state.ssn"
              v-model="permitStore.getPermitDetail.application.personalInfo.ssn"
              :label="$t('Partial Social Security Number')"
              readonly
              type="text"
              required
              outlined
              dense
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
              outlined
              dense
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
              :items="['Married', 'Single', 'Widowed', 'Divorced']"
              :menu-props="{ bottom: true, offsetY: true }"
              :readonly="readonly"
              outlined
              dense
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
          class="pl-0"
        >
          {{ $t('Spouse Information:') }}
        </v-card-title>
        <v-row
          v-if="
            permitStore.getPermitDetail.application.personalInfo
              .maritalStatus === 'Married'
          "
        >
          <v-col cols="6">
            <v-text-field
              :label="$t('Spouse Last Name')"
              :rules="[v => !!v || $t('Spouse Last name cannot be blank')]"
              :readonly="readonly"
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
          <v-col cols="6">
            <v-text-field
              :label="$t('Spouse First Name')"
              :rules="[v => !!v || $t('Spouse First name cannot be blank')]"
              :readonly="readonly"
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .firstName
              "
              dense
              outlined
            >
            </v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              :label="$t('Spouse Middle Name')"
              :readonly="readonly"
              v-model="
                permitStore.getPermitDetail.application.spouseInformation
                  .middleName
              "
              dense
              outlined
            />
          </v-col>
          <v-col cols="6">
            <v-text-field
              :label="$t('Spouse Maiden Name')"
              :readonly="readonly"
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
import SaveButton from './SaveButton.vue'
import { openPdf } from '@core-admin/components/composables/openDocuments'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { inject, reactive } from 'vue'

const permitStore = usePermitsStore()
const emit = defineEmits(['on-save'])
const state = reactive({
  ssn: '',
  valid: false,
})
const readonly = inject<boolean>('readonly')

const {
  isError,
  isLoading,
  mutate: ssnMutation,
} = useMutation(
  ['ssnMutation'],
  () => permitStore.getPermitSsn(permitStore.getPermitDetail.userId),
  {
    onSuccess: res => {
      state.ssn = res
    },
  }
)

function getSSN() {
  ssnMutation()
}

function hideSsn() {
  state.ssn = ''
}

function handleSave() {
  emit('on-save', 'Application Info')
}

function onApproveNameChange() {
  permitStore.getPermitDetail.application.modifiedNameComplete = true
  emit('on-save', 'Approved name change')
}

function onUndoApproveNameChange() {
  permitStore.getPermitDetail.application.modifiedNameComplete = false
  emit('on-save', 'Undo approved name change')
}

async function handleOpenPdf() {
  const modifyNameDocument =
    permitStore.getPermitDetail.application.uploadedDocuments.find(d => {
      if (
        d.name.indexOf(
          `ModifyName-${permitStore.getPermitDetail.application.modificationNumber}`
        ) >= 0
      ) {
        return d
      }

      return null
    })

  if (modifyNameDocument) {
    await openPdf(modifyNameDocument)
  }
}
</script>
