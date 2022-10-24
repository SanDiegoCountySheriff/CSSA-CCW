<template>
  <div>
    <v-container v-if="!state.loading">
      <v-row class="mb-5">
        <v-col
          cols="10"
          md="5"
          sm="4"
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
          cols="10"
          md="5"
          sm="4"
        >
          <div class="signature-preview">
            <canvas
              id="signature-canvas"
              height="100"
              width="300"
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
      />
    </v-container>
    <template v-if="state.loading">
      <div>
        {{ $t('loading') }}
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { reactive, watch } from 'vue';
import { sendPostImage } from '@core-public/senders/documentSenders';
import { useAppConfigStore } from '@shared-ui/stores/appConfig';
import { useAuthStore } from '@shared-ui/stores/auth';

interface ISecondFormStepFourProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepFourProps>();

const appConfigStore = useAppConfigStore();
const authStore = useAuthStore();
const baseApiUrl = `${appConfigStore.getAppConfig.apiBaseUrl}/Document`;

const state = reactive({
  valid: false,
  loading: false,
  signature: '',
});

async function handleSubmit() {
  const canvas = document.getElementById('signature-canvas');
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const image = canvas?.toDataURL('image/jpeg', 0.5);
  const file = new File(
    [image],
    `${authStore.getAuthState.userEmail}-signature`,
    { type: 'image/jpeg' }
  );
  state.loading = true;
  sendPostImage(file, baseApiUrl)
    .then(() => {
      state.loading = false;
      props.handleNextSection();
    })
    .catch(e => {
      console.warn(e);
    });
}

function handleCanvasClear() {
  const canvas = document.getElementById('signature-canvas');
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas?.getContext('2d');
  ctx.clearRect(0, 0, 300, 100);
  state.signature = '';
}

watch(state, () => {
  handleCanvasUpdate();
});

function handleCanvasUpdate() {
  const canvas = document.getElementById('signature-canvas');
  // eslint-disable-next-line @typescript-eslint/ban-ts-comment
  //@ts-ignore
  const ctx = canvas?.getContext('2d');
  ctx.font = '30px Brush Script MT';
  ctx.clearRect(0, 0, 300, 100);
  ctx.fillText(state.signature, 10, 50);
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
