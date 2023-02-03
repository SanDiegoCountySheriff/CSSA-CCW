<template>
  <div>
    <ApplicationInfoSection />
    <v-form v-model="state.valid">
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Application Type') }}
      </v-subheader>
      <div class="ml-5">
        <v-radio-group v-model="completeApplication.applicationType">
          <v-radio
            :color="$vuetify.theme.dark ? 'info' : 'primary'"
            :label="'Standard'"
            :value="'standard'"
          />
          <v-radio
            :label="'Judicial'"
            :value="'judicial'"
            color="warning"
          />
          <v-radio
            :label="'Reserve'"
            :value="'reserve'"
            color="warning"
          />
        </v-radio-group>
      </div>
      <v-alert
        dense
        outlined
        type="warning"
        v-if="completeApplication.applicationType === 'judicial'"
      >
        <strong>
          {{ $t('Judicial-warning') }}
        </strong>
      </v-alert>
      <v-alert
        dense
        outlined
        type="warning"
        v-if="completeApplication.applicationType === 'reserve'"
      >
        <strong>
          {{ $t('Judicial-reserve') }}
        </strong>
      </v-alert>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      :submitting="state.submited"
      @submit="handleSubmit"
      @save="saveMutation.mutate"
      @back="handlePreviousSection"
      @cancel="router.push('/')"
    />
    <v-snackbar
      :value="state.snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import ApplicationInfoSection from '@shared-ui/components/info-sections/ApplicationInfoSection.vue';
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

interface ISecondFormStepThreeProps {
  handleNextSection: CallableFunction;
  handlePreviousSection: CallableFunction;
}

const props = defineProps<ISecondFormStepThreeProps>();
const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;
const router = useRouter();

const state = reactive({
  valid: false,
  snackbar: false,
  submited: false,
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    completeApplication.currentStep = 8;
    props.handleNextSection();
    state.valid = false;
  },
  onError: () => {
    state.submited = false;
    state.valid = true;
    state.snackbar = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    state.snackbar = true;
  },
});

function handleSubmit() {
  state.submited = true;
  state.valid = false;
  updateMutation.mutate();
}
</script>
