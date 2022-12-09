<template>
  <div>
    <DocumentInfoSection />
    <v-form
      ref="form"
      v-model="state.valid"
    >
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
            :value="state.driverLicense"
            small-chips
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Driver License')"
            @change="
              event => {
                state.driverLicense = event.name;
                handleFileInput(event, 'DriverLicense');
              }
            "
          >
            <template #selection="{}">
              <v-chip
                color="info"
                v-if="state.driverLicense"
                label
                small
              >
                {{ state.driverLicense }}
              </v-chip>
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
            :value="state.proofResidence"
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Proof of Residence 1')"
            @change="
              event => {
                state.proofResidence = event.name;
                handleFileInput(event, 'ProofResidency');
              }
            "
          >
            <template #selection="{}">
              <v-chip
                v-if="state.proofResidence"
                color="info"
                label
                small
              >
                {{ state.proofResidence }}
              </v-chip>
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
            :value="state.proofResidence2"
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Proof of Residence 2')"
            @change="
              event => {
                state.proofResidence2 = event.name;
                handleFileInput(event, 'ProofResidency2');
              }
            "
          >
            <template #selection="{}">
              <v-chip
                v-if="state.proofResidence2"
                color="info"
                label
                small
              >
                {{ state.proofResidence2 }}
              </v-chip>
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
            :value="state.military"
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Military Document')"
            @change="
              event => {
                state.military = event.name;
                handleFileInput(event, 'MilitaryDoc');
              }
            "
          >
            <template #selection="{}">
              <v-chip
                v-if="state.military"
                color="info"
                label
                small
              >
                {{ state.military }}
              </v-chip>
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
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Citizenship Documents')"
            @change="
              event => {
                state.citizenship = event.name;
                handleFileInput(event, 'Citizenship');
              }
            "
            :value="state.citizenship"
          >
            <template #selection="{}">
              <v-chip
                v-if="state.citizenship"
                color="info"
                label
                small
              >
                {{ state.citizenship }}
              </v-chip>
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
            :label="$t('Supporting Documents')"
            @change="
              event => {
                state.supporting = [];
                handleMultiInput(event, 'Supporting');
              }
            "
            :value="state.supporting"
          >
            <template
              #selection="{}"
              v-if="state.supporting"
            >
              <v-chip
                v-for="(item, index) in state.supporting"
                :key="index"
                color="info"
                label
                small
              >
                {{ item }}
              </v-chip>
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
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Name change documents')"
            @change="
              event => {
                state.nameChange = event.name;
                handleFileInput(event, 'NameChange');
              }
            "
            :value="state.nameChange"
          >
            <template #selection="{}">
              <v-chip
                v-if="state.nameChange"
                color="info"
                label
                small
              >
                {{ state.nameChange }}
              </v-chip>
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
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Judicial documents')"
            @change="
              event => {
                state.judicial = event.name;
                handleFileInput(event, 'Judicial');
              }
            "
            :value="state.judicial"
          >
            <template #selection="{}">
              <v-chip
                v-if="state.judicial"
                color="info"
                label
                small
              >
                {{ state.nameChange }}
              </v-chip>
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
            accept="image/png, image/jpeg, .pdf"
            :label="$t('Reserve documents')"
            @change="
              event => {
                state.reserve = event.name;
                handleFileInput(event, 'Reserve');
              }
            "
            :value="state.reserve"
          >
            <template #selection="{}">
              <v-chip
                v-if="state.reserve"
                color="info"
                label
                small
              >
                {{ state.reserve }}
              </v-chip>
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
    state.snackbar = true;
  },
});

function handleFileInput(event: File, target: string) {
  window.console.log(event);
  const form = new FormData();

  form.append('fileToPersist', event);

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

    form.append('fileToPersist', file);
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
    const newFileName = `${applicationStore.completeApplication.application.orderId}_${completeApplication.personalInfo.lastName}_${completeApplication.personalInfo.firstName}_${file.target}`;

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
