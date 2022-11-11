<template>
  <div>
    <v-container v-if="isLoading">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type=" list-item"
      />
    </v-container>
    <v-card
      class="rounded elevation-2 form-card"
      :class="{ 'dark-card': $vuetify.theme.dark }"
    >
      <v-stepper
        alt-labels
        v-model="stepIndex.step"
      >
        <FormStepHeader
          :starting-step="1"
          :previous-index="stepIndex.previousStep"
          :step-index="stepIndex.step"
          :step-names="formOneStepNames"
        />
        <RenewFormStepItems
          :step-index="stepIndex.step"
          :handle-next-section="handleNextSection"
          :handle-previous-section="handlePreviousSection"
        />
      </v-stepper>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import FormStepHeader from '@core-public/components/form-stepper/FormStepHeader.vue';
import RenewFormStepItems from '@core-public/components/form-stepper/RenewFormStepItems.vue';
import { formOneStepNames } from '@shared-utils/lists/defaultConstants';
import { reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';
import { unformatNumber } from '@shared-utils/formatters/defaultFormatters';
import { useRouter } from 'vue-router/composables';

const stepIndex = reactive({
  step: 1,
  previousStep: 0,
});

const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();
const router = useRouter();

const { isLoading } = useQuery(['getIncompleteApplications'], () => {
  // TODO: once the is current step is added to the object change this to route to the current step is
  if (!applicationStore.completeApplication.id) {
    stepIndex.step = 0;
    const res = applicationStore.getCompleteApplicationFromApi(
      authStore.auth.userEmail,
      false
    );

    res.then(data => {
      data.application.contact.primaryPhoneNumber = unformatNumber(
        data.application.contact.primaryPhoneNumber
      );
      data.application.contact.cellPhoneNumber = unformatNumber(
        data.application.contact.cellPhoneNumber
      );
      data.application.contact.workPhoneNumber = unformatNumber(
        data.application.contact.workPhoneNumber
      );
      data.application.contact.faxPhoneNumber = unformatNumber(
        data.application.contact.faxPhoneNumber
      );
      data.application.personalInfo.ssn = unformatNumber(
        data.application.personalInfo.ssn
      );
      applicationStore.setCompleteApplication(data);
      checkForCorrectForm();
      stepIndex.step = 1;
    });
  }

  function checkForCorrectForm() {
    if (applicationStore.completeApplication.application.currentStep > 5) {
      router.push('/renew-form-2');
    }
  }
});

function handleNextSection() {
  stepIndex.previousStep = stepIndex.step;
  stepIndex.step += 1;
}

function handlePreviousSection() {
  stepIndex.previousStep = stepIndex.step - 2;
  stepIndex.step -= 1;
}
</script>

<style lang="scss">
.form-card {
  height: auto;
  min-height: 45vh;
}

.dark-card {
  background: #455a64;
}
</style>
