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
            :label="'Renew Standard'"
            :value="'renew-standard'"
          />
          <v-radio
            :label="'Renew Judicial'"
            :value="'renew-judicial'"
            color="warning"
          />
          <v-radio
            :label="'Renew Reserve'"
            :value="'renew-reserve'"
            color="warning"
          />
          <v-radio
            :label="'Modify Standard'"
            :value="'modify-standard'"
          />
          <v-radio
            :label="'Modify Judicial'"
            :value="'modify-judicial'"
            color="warning"
          />
          <v-radio
            :label="'Modify Reserve'"
            :value="'modify-reserve'"
            color="warning"
          />
          <v-radio
            :label="'Duplicate Standard'"
            :value="'duplicate-standard'"
          />
          <v-radio
            :label="'Duplicate Judicial'"
            :value="'duplicate-judicial'"
            color="warning"
          />
          <v-radio
            :label="'Duplicate Reserve'"
            :value="'duplicate-reserve'"
            color="warning"
          />
        </v-radio-group>
      </div>
      <v-alert
        dense
        outlined
        type="warning"
        v-if="
          completeApplicationStore.completeApplication.application
            .applicationType === 'renew-judicial' ||
          completeApplicationStore.completeApplication.application
            .applicationType === 'modify-judicial'
        "
      >
        <strong>
          {{ $t('Judicial-warning') }}
        </strong>
      </v-alert>
      <v-alert
        dense
        outlined
        type="warning"
        v-if="
          completeApplicationStore.completeApplication.application
            .applicationType === 'renew-reserve' ||
          completeApplicationStore.completeApplication.application
            .applicationType === 'modify-reserve'
        "
      >
        <strong>
          {{ $t('Judicial-reserve') }}
        </strong>
      </v-alert>
    </v-form>
    <v-divider />
    <FormButtonContainer
      :valid="state.valid"
      @submit="updateMutation.mutate"
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
});
const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step one complete');
  },
  onSuccess: () => {
    completeApplication.currentStep = 8;
    props.handleNextSection();
  },
  onError: () => {
    state.snackbar = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Save and quit');
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    state.snackbar = true;
  },
});
</script>
