<template>
  <div>
    <v-container>
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
              :label="$t('Signature')"
              :rules="[v => !!v || $t(' Signature cannot be blank ')]"
              v-model="state.signature"
            />
          </v-form>
        </v-col>
        <v-col
          cols="12"
          lg="6"
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
        :valid="state.valid"
        @submit="handleSubmit"
        @save="router.push('/')"
        @cancel="router.push('/')"
        @back="handlePreviousSection"
      />
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
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import Routes from '@core-public/router/routes';
import axios from 'axios';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { reactive, ref, watch } from 'vue';

interface ISecondFormStepFourProps {
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
  snackbar: false,
});

const fileMutation = useMutation({
  mutationFn: handleFileUpload,
  onSuccess: () => {
    applicationStore.completeApplication.application.currentStep = 10;
    applicationStore.updateApplication('Signature complete');
    router.push(Routes.QUALIFYING_QUESTIONS_ROUTE_PATH);
  },
  onError: () => {
    state.snackbar = true;
  },
});

async function handleSubmit() {
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const image = signatureCanvas.value.toDataURL('image/jpeg', 0.5);
  const file = new File([image], 'file');
  const form = new FormData();

  form.append('fileToPersist', file);

  state.file = form;

  fileMutation.mutate();
}

async function handleFileUpload() {
  const newFileName = `${applicationStore.completeApplication.application.orderId}_${applicationStore.completeApplication.application.personalInfo.lastName}_${applicationStore.completeApplication.application.personalInfo.firstName}_signature`;

  await axios
    .post(
      `${Endpoints.POST_DOCUMENT_IMAGE_ENDPOINT}?saveAsFileName=${newFileName}`,
      state.file
    )
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
</script>

<style scoped lang="scss">
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
