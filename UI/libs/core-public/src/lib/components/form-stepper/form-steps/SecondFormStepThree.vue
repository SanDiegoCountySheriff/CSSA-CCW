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
              completeApplicationStore.completeApplication.applicationType = v;
              state.valid = true;
            }
          "
        />
      </div>
      <v-alert
        dense
        outlined
        type="warning"
        v-if="
          completeApplicationStore.completeApplication.applicationType ===
          'judicial'
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
          completeApplicationStore.completeApplication.applicationType ===
          'reserve'
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
      @submit="handleSubmit"
      @save="handleSave"
    />
  </div>
</template>

<script setup lang="ts">
import ApplicationInfoSection from '@shared-ui/components/info-sections/ApplicationInfoSection';
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { reactive } from 'vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';

interface ISecondFormStepThreeProps {
  handleNextSection: CallableFunction;
}

const props = defineProps<ISecondFormStepThreeProps>();
const completeApplicationStore = useCompleteApplicationStore();

const state = reactive({
  valid: false,
});

function handleSubmit() {
  props.handleNextSection();
}

function handleSave() {
  completeApplicationStore.postCompleteApplicationFromApi;
}
</script>

<style lang="scss" scoped></style>
