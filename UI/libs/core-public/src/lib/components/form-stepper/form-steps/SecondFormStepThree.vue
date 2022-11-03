<template>
  <div>
    <ApplicationInfoSection />
    <v-form>
      <v-subheader class="sub-header font-weight-bold">
        {{ $t('Application Type') }}
      </v-subheader>
      <div class="ml-5">
        <RadioGroupInput
          :layout="'column'"
          :options="[
            { label: 'Standard', value: 'standard' },
            { label: 'Judicial', value: 'judicial', color: 'warning' },
            { label: 'Reserve', value: 'reserve', color: 'warning' },
          ]"
          @input="
            v => {
              completeApplication.applicationType = v;
              state.valid = true;
            }
          "
        />
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
      @submit="updateMutation.mutate"
      @save="saveMutation.mutate"
    />
  </div>
</template>

<script setup lang="ts">
import ApplicationInfoSection from '@shared-ui/components/info-sections/ApplicationInfoSection';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import { i18n } from '@shared-ui/plugins';
import { reactive } from 'vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

interface ISecondFormStepThreeProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepThreeProps>();
const completeApplicationStore = useCompleteApplicationStore();
const completeApplication =
  completeApplicationStore.completeApplication.application;
const router = useRouter();

const state = reactive({
  valid: false,
});

const updateMutation = useMutation({
  mutationFn: () => {
    return completeApplicationStore.updateApplication('Step seven complete');
  },
  onSuccess: () => {
    props.handleNextSection();
  },
  onError: error => {
    alert(error);
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
    alert(i18n.t('Save unsuccessful, please try again'));
  },
});
</script>

<style lang="scss" scoped></style>
