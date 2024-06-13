<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        {{ $t('Telephone Numbers') }}

        <v-spacer></v-spacer>

        <SaveButton
          :disabled="!valid || readonly"
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
                @input="formatPhone('contact', 'primaryPhoneNumber')"
                :readonly="readonly"
                :label="$t('Primary phone number')"
                :rules="phoneRuleSet"
                outlined
                dense
              >
                <template #append>
                  <v-icon> mdi-phone </v-icon>

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
                :readonly="readonly"
                :label="$t('Cell phone number')"
                :hint="$t('Only numbers no spaces or dashes')"
                :rules="[
                  v =>
                    (!!v && v.length === 10) ||
                    v === '' ||
                    $t('Cell phone number must be ten digits long'),
                ]"
                maxlength="10"
                outlined
                dense
              >
                <template #append>
                  <v-icon> mdi-cellphone-basic </v-icon>
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
                :readonly="readonly"
                :label="$t('Work phone number')"
                :hint="$t('Only numbers no spaces or dashes')"
                :rules="[
                  v =>
                    (!!v && v.length === 10) ||
                    v === '' ||
                    $t('Work phone number must be ten digits long'),
                ]"
                maxlength="10"
                outlined
                dense
              >
                <template #append>
                  <v-icon> mdi-cellphone-link </v-icon>
                </template>
              </v-text-field>
            </v-col>

            <v-col>
              <v-text-field
                v-model="permitStore.getPermitDetail.application.userEmail"
                :label="$t('Email address')"
                :hint="$t('User email address (Read Only)')"
                persistent-hint
                readonly
                outlined
                dense
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
                v-model="
                  permitStore.getPermitDetail.application.idInfo.idNumber
                "
                :label="$t('State-issued ID number')"
                :rules="[v => !!v || $t('ID number is required')]"
                :readonly="readonly"
                maxlength="25"
                counter
                outlined
                dense
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
                v-model="
                  permitStore.getPermitDetail.application.idInfo.issuingState
                "
                :items="states"
                :label="$t('ID Issuing State')"
                :rules="[v => !!v || $t('Issuing state is required')]"
                :readonly="readonly"
                outlined
                dense
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

            <v-col>
              <v-text-field
                v-model="
                  permitStore.getPermitDetail.application.idInfo.restrictions
                "
                :readonly="readonly"
                :label="$t('Restrictions')"
                maxlength="100"
                counter
                outlined
                dense
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-title>Character References</v-card-title>

      <v-card-text>
        <v-form v-model="valid">
          <v-row
            v-for="(reference, index) in permitStore.getPermitDetail.application
              .characterReferences"
            :key="index"
          >
            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="reference.name"
                :label="
                  $t('Reference') + ' ' + (index + 1) + ' - ' + $t('Name')
                "
                :rules="[v => !!v || $t('Name cannot be blank')]"
                :readonly="readonly"
                dense
                outlined
              >
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="reference.relationship"
                :label="
                  $t('Reference') +
                  ' ' +
                  (index + 1) +
                  ' - ' +
                  $t('Relationship')
                "
                :readonly="readonly"
                :rules="[v => !!v || $t('Relationship cannot be blank')]"
                dense
                outlined
              >
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="reference.phoneNumber"
                :readonly="readonly"
                :label="
                  $t('Reference') +
                  ' ' +
                  (index + 1) +
                  ' - ' +
                  $t('Phone Number')
                "
                :rules="phoneRuleSet"
                @input="formatReferencePhone(reference)"
                dense
                outlined
              >
              </v-text-field>
            </v-col>

            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="reference.email"
                :readonly="readonly"
                :label="
                  $t('Reference') +
                  ' ' +
                  (index + 1) +
                  ' - ' +
                  $t('Email Address')
                "
                :rules="[
                  v => !!v || $t('Email address cannot be blank'),
                  v => /.+@.+\..+/.test(v) || $t('Email address must be valid'),
                ]"
                dense
                outlined
              >
              </v-text-field>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>

      <v-card-title>Reference Notes</v-card-title>

      <v-card-text>
        <v-form v-model="valid">
          <v-row>
            <v-col cols="12">
              <v-textarea
                v-model="permitStore.getPermitDetail.application.referenceNotes"
                :readonly="readonly"
                label="Character reference notes, not seen by applicant"
                color="primary"
                outlined
              ></v-textarea>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import { CharacterReferenceType } from '@shared-utils/types/defaultTypes'
import SaveButton from './SaveButton.vue'
import { phoneRuleSet } from '@shared-ui/rule-sets/ruleSets'
import { states } from '@shared-utils/lists/defaultConstants'
import { usePermitsStore } from '@core-admin/stores/permitsStore'
import { inject, ref } from 'vue'

const emit = defineEmits(['on-save'])

const permitStore = usePermitsStore()
const valid = ref(false)
const readonly = inject<boolean>('readonly')

function handleSave() {
  emit('on-save', 'Contact Details')
}

function formatReferencePhone(reference: CharacterReferenceType) {
  let formattedNumber = reference.phoneNumber.replace(/\D/g, '')

  const match = formattedNumber.match(/^(\d{1,3})(\d{0,3})(\d{0,4})$/)

  if (match) {
    reference.phoneNumber = `(${match[1]})${match[2] ? ' ' : ''}${match[2]}${
      match[3] ? '-' : ''
    }${match[3]}`
  }
}

function formatPhone(modelName1: string, modelName2: string) {
  const phoneNumber =
    permitStore.getPermitDetail.application[modelName1][modelName2]

  if (phoneNumber) {
    let validInput = phoneNumber.replace(/\D/g, '')
    const match = validInput.match(/^(\d{1,3})(\d{0,3})(\d{0,4})$/)

    if (match) {
      permitStore.getPermitDetail.application[modelName1][modelName2] = `(${
        match[1]
      })${match[2] ? ' ' : ''}${match[2]}${match[3] ? '-' : ''}${match[3]}`
    }
  }
}
</script>
