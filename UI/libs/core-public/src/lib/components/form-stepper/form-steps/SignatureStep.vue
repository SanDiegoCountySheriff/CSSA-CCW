<template>
  <div class="signature-container">
    <v-container v-if="!state.previousSignature">
      <v-row class="mb-5">
        <v-col
          cols="12"
          lg="6"
        >
          <v-form
            ref="form"
            v-model="state.valid"
          >
            <v-text-field
              outlined
              persistent-hint
              dense
              :label="$t('Signature')"
              :rules="[v => !!v || $t(' Signature cannot be blank ')]"
              v-model="state.signature"
              @keydown.enter.prevent
            />
          </v-form>
        </v-col>
        <v-col
          cols="12"
          lg="5"
        >
          <div class="signature-preview">
            <canvas
              ref="signatureCanvas"
              height="100"
              width="250"
            ></canvas>
            <v-btn
              text
              class="m-5"
              color="error"
              @click="handleCanvasClear"
            >
              {{ $t('clear') }}
            </v-btn>
          </div>
        </v-col>
      </v-row>
      <v-divider class="mb-5" />
      <FormButtonContainer
        v-if="!state.previousSignature"
        :valid="state.valid"
        @submit="handleSubmit"
        @save="router.push('/')"
        @cancel="router.push('/')"
        @back="handlePreviousSection"
      />
    </v-container>
    <v-container
      fluid
      v-else
    >
      <v-row :style="{ width: '100%' }">
        <v-icon
          color="success"
          x-large
        >
          mdi-check-circle-outline
        </v-icon>
        <v-subheader class="pt-2">
          {{
            $t(
              'Signature has already been submitted. Press continue to move forward.'
            )
          }}
        </v-subheader>
      </v-row>
      <v-row>
        <FormButtonContainer
          :style="{ width: '100%' }"
          v-if="state.previousSignature"
          :valid="true"
          @submit="handleSkipSubmit"
          @save="router.push('/')"
          @cancel="router.push('/')"
          @back="handlePreviousSection"
        />
      </v-row>
    </v-container>
    <v-snackbar
      :value="state.snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('File upload unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import Endpoints from '@shared-ui/api/endpoints';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { UploadedDocType } from '@shared-utils/types/defaultTypes';
import axios from 'axios';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { onMounted, reactive, ref, watch } from 'vue';

interface ISecondFormStepFourProps {
  routes: unknown;
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = defineProps<ISecondFormStepFourProps>();
const signatureCanvas = ref<HTMLCanvasElement | null>(null);
const applicationStore = useCompleteApplicationStore();
const router = useRouter();

const state = reactive({
  valid: false,
  file: {},
  signature: '',
  previousSignature: false,
  snackbar: false,
});

onMounted(() => {
  for (let item of applicationStore.completeApplication.application
    .uploadedDocuments) {
    if (item.documentType === 'signature') {
      state.previousSignature = true;
    }
  }
});

const fileMutation = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    state.valid = false;
    applicationStore.completeApplication.application.currentStep = 10;
    applicationStore.updateApplication();
    router.push({
      path: props.routes.QUALIFYING_QUESTIONS_ROUTE_PATH,
      query: {
        orderId: applicationStore.completeApplication.application.orderId,
        isComplete: applicationStore.completeApplication.application.isComplete,
      },
    });
  },
  onError: () => {
    state.snackbar = true;
  },
});

async function handleSubmit() {
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const image = signatureCanvas.value.toDataURL('image/jpeg', 0.5);
  const file = new File([image], 'file', { type: 'image/jpeg' });
  const form = new FormData();

  form.append('fileToUpload', file);

  state.file = form;

  fileMutation.mutate();
}

async function handleFileUpload() {
  const newFileName = `${applicationStore.completeApplication.application.orderId}_${applicationStore.completeApplication.application.personalInfo.lastName}_${applicationStore.completeApplication.application.personalInfo.firstName}_signature`;

  const uploadDoc: UploadedDocType = {
    documentType: 'signature',
    name: newFileName,
    uploadedBy: applicationStore.completeApplication.application.userEmail,
    uploadedDateTimeUtc: new Date(Date.now()).toISOString(),
  };

  await axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
      state.file
    )
    .then(() => {
      applicationStore.completeApplication.application.uploadedDocuments.push(
        uploadDoc
      );
    })
    .catch(e => {
      window.console.warn(e);
      Promise.reject();
    });
}

function handleCanvasClear() {
  const canvas = signatureCanvas.value;
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas?.getContext('2d');

  ctx?.clearRect(0, 0, 300, 100);
  state.signature = '';
}

watch(state, () => {
  handleCanvasUpdate();
});

function handleCanvasUpdate() {
  const canvas = signatureCanvas.value;
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas.getContext('2d');

  if (ctx) {
    ctx.font = '30px Brush Script MT';
    ctx.clearRect(0, 0, 300, 100);
    ctx.fillText(state.signature, 10, 50);
  }
}

function handleSkipSubmit() {
  state.valid = false;
  applicationStore.completeApplication.application.currentStep = 10;
  applicationStore.updateApplication();
  router.push({
    path: props.routes.QUALIFYING_QUESTIONS_ROUTE_PATH,
    query: {
      orderId: applicationStore.completeApplication.application.orderId,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  });
}
</script>

<style scoped lang="scss">
.signature-container {
  width: 100%;
}
.signature-preview {
  border: 1px solid #333;
  border-radius: 10px;
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: flex-end;
  margin-left: 1rem;
}
</style>
