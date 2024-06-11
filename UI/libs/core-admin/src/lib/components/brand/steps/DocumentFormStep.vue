<template>
  <v-card :loading="isLoading || isValidationLoading">
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title>
        Agency Documents
        <v-spacer />

        <v-btn
          :disabled="!valid"
          @click="save"
          color="primary"
        >
          <v-icon left>mdi-content-save</v-icon>
          Save
        </v-btn>
      </v-card-title>

      <v-card-text>
        Please upload all the necessary PDF forms for the application.
      </v-card-text>

      <v-card-text>
        <v-row>
          <v-col>
            <v-file-input
              v-model="file"
              :rules="fileRules"
              label="Upload Document"
              outlined
              required
            />
          </v-col>

          <v-col>
            <v-select
              v-model="selectedDocument"
              :items="documents"
              item-value="name"
              item-text="display"
              :rules="[v => !!v || 'Document type must be selected']"
              label="Document Type"
              outlined
              clearable
            />
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            <v-list>
              <v-list-item
                v-for="(document, index) in documents"
                :key="index"
              >
                <v-icon
                  :color="data?.[document.name] ? 'success' : 'error'"
                  class="mr-2"
                >
                  {{
                    data?.[document.name] ? 'mdi-check-bold' : 'mdi-close-thick'
                  }}
                </v-icon>
                {{ document.text }}
              </v-list-item>
            </v-list>
          </v-col>
        </v-row>
      </v-card-text>
    </v-form>
  </v-card>
</template>

<script setup lang="ts">
import { useDocumentsStore } from '@core-admin/stores/documentsStore'
import { computed, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const documentStore = useDocumentsStore()
const file = ref<File>()
const valid = ref(false)
const selectedDocument = ref('')
const documents = [
  {
    name: 'BOF_4012_rev_01_2024',
    display: 'BOF 4012 (rev. 01/2024)',
    text: 'BOF 4012 (rev. 01/2024) Standard initial and renewal application for license to carry a weapon capable of being concealed',
  },
  {
    name: 'BOF_4012_rev_08_2022',
    display: 'BOF 4012 (rev. 08/2022)',
    text: 'BOF 4012 (rev. 08/2022) Standard initial and renewal application for license to carry a weapon capable of being concealed',
  },
  {
    name: 'BOF_4502_rev_09_2011',
    display: 'BOF 4502 (rev. 09/2011)',
    text: 'BOF 4502 (rev. 09/2011) Carry Concealed Weapon License Amendment',
  },
  {
    name: 'BOF_1032_rev_01_2024',
    display: 'BOF 1032 (rev. 01/2024)',
    text: 'BOF 1032 (rev. 01/2024) License to carry concealed pistol, revolver, or other firearm notification of denial or revocation',
  },
  {
    name: 'BOF_1031_orig_01_2024',
    display: 'BOF 1031 (orig. 01/2024)',
    text: 'BOF 1031 (orig. 01/2024) Request for hearing to challenge disqualified person determination',
  },
  {
    name: 'BOF_8018_rev_01_2024',
    display: 'BOF 8018 (rev. 01/2024)',
    text: 'BOF 8018 (rev. 01/2024) Firearms Prohibiting Categories',
  },
  {
    name: 'BCIA_8016_rev_04_2020',
    display: 'BCIA 8016 (rev. 04/2020)',
    text: 'BCIA 8016 (rev. 04/2020) Request for Live Scan Service',
  },
  {
    name: 'BOF_1027_rev_01_2024',
    display: 'BOF 1027 (rev. 01/2024)',
    text: 'BOF 1027 (rev. 01/2024) Reserve/Auxiliary Peace Officer and Judicial Carry Concealed Weapon (CCW) License Annual Survey',
  },
  {
    name: 'BOF_1034_orig_01_2024',
    display: 'BOF 1034 (orig. 01/2024)',
    text: 'BOF 1034 (orig. 01/2024) Carry Concealed Weapon Program DOJ Certified Instructor Application',
  },
  {
    name: 'BCIA_8020_rev_01_2014',
    display: 'BCIA 8020 (rev. 01/2014)',
    text: 'BCIA 8020 (rev. 01/2014) Request For Applicant Name Check By The Federal Bureau of Investigation (FBI)',
  },
  {
    name: 'Official_License',
    display: 'Official License',
    text: 'Official License',
  },
  {
    name: 'Unofficial_License',
    display: 'Unofficial License',
    text: 'Unofficial License',
  },
  {
    name: 'Conditions_for_Issuance',
    display: 'Conditions for Issuance',
    text: 'Conditions for Issuance (rev. 05/04/2023)',
  },
  {
    name: 'False_Info',
    display: 'False Info',
    text: 'False Info Penal Code Section 26180',
  },
]
const fileRules = computed(() => {
  return [Boolean(file.value) || 'A file is required.']
})

const {
  data,
  refetch,
  isLoading: isValidationLoading,
} = useQuery(['getPdfFormValidation'], documentStore.getPdfFormValidation)

const { mutate: uploadFile, isLoading } = useMutation({
  mutationFn: (formData: FormData) =>
    documentStore
      .uploadAgencyFile(formData, selectedDocument.value)
      .then(() => {
        refetch()
      }),
})

function save() {
  if (file.value) {
    const formData = new FormData()

    formData.append('fileToUpload', file.value)
    uploadFile(formData)
  }
}
</script>
