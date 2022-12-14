<template>
  <div>
    <DocumentInfoSection />
    <v-form
      ref="form"
      v-model="state.valid"
    >
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Currently uploaded files') }}
      </v-subheader>
      <v-row>
        <v-chip-group
          class="ml-5 mb-3"
          column
        >
          <v-chip
            v-for="(item, index) in completeApplication.uploadedDocuments"
            color="info"
            :key="index"
          >
            {{ item.documentType }}
          </v-chip>
        </v-chip-group>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('File Upload') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            ref="driver-license"
            show-size
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Driver License')"
            :hint="
              state.driverLicense
                ? $t('Document has already been submitted')
                : ''
            "
            @change="handleFileInput($event, 'DriverLicense')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.driverLicense"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            show-size
            small-chips
            persistent-hint
            :hint="
              state.proofResidence
                ? $t('Document has already been submitted')
                : ''
            "
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Proof of Residence 1')"
            @change="handleFileInput($event, 'ProofResidency')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.proofResidence"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Proof of Residence 2')"
            :hint="
              state.proofResidence2
                ? $t('Document has already been submitted')
                : ''
            "
            @change="handleFileInput($event, 'ProofResidency2')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.proofResidence2"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Military') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Military Document')"
            :hint="
              state.military ? $t('Document has already been submitted') : ''
            "
            @change="handleFileInput($event, 'MilitaryDoc')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.military"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Not born in US') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Citizenship Documents')"
            :hint="
              state.citizenship ? $t('Document has already been submitted') : ''
            "
            @change="handleFileInput($event, 'Citizenship')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.citizenship"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>

      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Supporting Documents') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            show-size
            small-chips
            multiple
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :hint="
              state.supporting.length > 0
                ? $t('Documents has already been submitted')
                : ''
            "
            :label="$t('Supporting Documents')"
            @change="handleMultiInput($event, 'Supporting')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.supporting.length > 0"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>
      <v-divider />

      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' Legal name change') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            small-chips
            persistent-hint
            accept="image/png, image/jpeg, .pdf"
            :hint="
              state.nameChange ? $t('Document has already been submitted') : ''
            "
            :label="$t('Name change documents')"
            @change="handleFileInput($event, 'NameChange')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.nameChange"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>
      <v-divider />
      <v-subheader class="sub-header font-weight-bold">
        {{ $t(' License Type Documents') }}
      </v-subheader>
      <v-row>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            dense
            show-size
            small-chips
            persistent-hint
            :hint="
              state.judicial ? $t('Document has already been submitted') : ''
            "
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Judicial documents')"
            @change="handleFileInput($event, 'Judicial')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.judicial"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
        <v-col
          cols="12"
          lg="6"
        >
          <v-file-input
            outlined
            show-size
            dense
            small-chips
            persistent-hint
            :hint="
              state.reserve ? $t('Document has already been submitted') : ''
            "
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Reserve documents')"
            @change="handleFileInput($event, 'Reserve')"
          >
            <template #prepend-inner>
              <v-icon
                v-if="state.reserve"
                color="success"
              >
                mdi-check-circle-outline
              </v-icon>
            </template>
          </v-file-input>
        </v-col>
      </v-row>
      <v-divider />
    </v-form>
    <v-progress-circular
      v-if="!state.uploadSuccessful"
      color="primary"
      indeterminate
    />
    <FormButtonContainer
      v-else
      :valid="state.valid"
      :submitting="state.submited"
      @submit="handleSubmit"
      @back="handlePreviousSection"
      @cancel="router.push('/')"
    />
    <v-snackbar
      v-model="state.snackbar"
      :timeout="2000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import DocumentInfoSection from '@shared-ui/components/info-sections/DocumentInfoSection.vue';
import Endpoints from '@shared-ui/api/endpoints';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { UploadedDocType } from '@shared-utils/types/defaultTypes';
import axios from 'axios';
import { onMounted, reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.completeApplication.application;
const router = useRouter();

interface ISecondFormStepTwoProps {
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = defineProps<ISecondFormStepTwoProps>();

const state = reactive({
  driver: {} as File,
  files: [] as Array<{ form; target }>,
  valid: false,
  submited: false,
  driverLicense: '',
  proofResidence: '',
  proofResidence2: '',
  military: '',
  citizenship: '',
  supporting: [] as Array<string>,
  nameChange: '',
  judicial: '',
  reserve: '',
  uploadSuccessful: true,
  snackbar: false,
});

const fileMutation = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    state.uploadSuccessful = true;
    completeApplication.currentStep = 9;
    props.handleNextSection();
  },
  onError: () => {
    state.submited = false;
    state.valid = true;
    state.snackbar = true;
  },
});

function handleFileInput(event: File, target: string) {
  const form = new FormData();

  form.append('fileToUpload', event);
  const fileObject = {
    form,
    target,
  };

  state.files.push(fileObject);
}

function handleMultiInput(event, target: string) {
  let index = 1;

  event.forEach(file => {
    const form = new FormData();

    form.append('fileToUpload', file);
    const fileObject = {
      form,
      target: target + index.toString(),
    };

    state.files.push(fileObject);
    index++;
  });
}

async function handleFileUpload() {
  state.files.forEach(file => {
    const newFileName = `${completeApplication.personalInfo.lastName}_${completeApplication.personalInfo.firstName}_${file.target}`;

    axios
      .post(
        `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
        file.form
      )
      .catch(e => {
        window.console.warn(e);
        Promise.reject();
      });

    const uploadDoc: UploadedDocType = {
      documentType: file.target,
      name: `${newFileName}`,
      uploadedBy: completeApplication.userEmail,
      uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
    };

    completeApplication.uploadedDocuments.push(uploadDoc);
  });
  applicationStore.updateApplication();
}

function handleSubmit() {
  state.submited = false;
  state.uploadSuccessful = false;
  fileMutation.mutate();
}

onMounted(() => {
  for (let item of completeApplication.uploadedDocuments) {
    switch (item.documentType.toLowerCase()) {
      case 'driverlicense':
        state.driverLicense = item.name;
        break;
      case 'proofresidency':
        state.proofResidence = item.name;
        break;
      case 'proofresidency2':
        state.proofResidence2 = item.name;
        break;
      case 'militarydoc':
        state.military = item.name;
        break;
      case 'citizenship':
        state.citizenship = item.name;
        break;
      case 'supporting':
        state.supporting.push(item.name);
        break;
      case 'namechange':
        state.nameChange = item.name;
        break;
      case 'judicial':
        state.judicial = item.name;
        break;
      case 'reserve':
        state.reserve = item.name;
        break;
      case 'signature':
        break;
      default:
        break;
    }
  }
});
</script>

<style lang="scss" scoped>
.text-line {
  display: flex;
  flex-direction: row;
}
</style>
