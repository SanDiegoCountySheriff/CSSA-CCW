<template>
  <div class="text-left">
    <v-container v-if="isLoading">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type=" list-item"
      />
    </v-container>
    <v-container
      v-if="!isLoading"
      fluid
    >
      <v-stepper
        vertical
        v-model="stepIndex.step"
      >
        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="1"
        >
          {{ $t('Personal') }}
        </v-stepper-step>
        <v-stepper-content step="1">
          <FormStepOne
            v-if="stepIndex.step === 1"
            :handle-next-section="handleNextSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="2"
          :complete="stepIndex.step > 2"
        >
          {{ $t('Citizenship') }}
        </v-stepper-step>
        <v-stepper-content step="2">
          <FormStepTwo
            v-if="stepIndex.step === 2"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="3"
          :complete="stepIndex.step > 3"
        >
          {{ $t('Address') }}
        </v-stepper-step>
        <v-stepper-content step="3">
          <FormStepThree
            v-if="stepIndex.step === 3"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="4"
          :complete="stepIndex.step > 4"
        >
          {{ $t('Appearance') }}
        </v-stepper-step>
        <v-stepper-content step="4">
          <FormStepFour
            v-if="stepIndex.step === 4"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="5"
          :complete="stepIndex.step > 5"
        >
          {{ $t('Contact') }}
        </v-stepper-step>
        <v-stepper-content step="5">
          <FormStepFive
            v-if="stepIndex.step === 5"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
      </v-stepper>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import FormStepOne from '@core-public/components/form-stepper/form-steps/FormStepOne.vue';
import FormStepTwo from '@core-public/components/form-stepper/form-steps/FormStepTwo.vue';
import FormStepThree from '@core-public/components/form-stepper/form-steps/FormStepThree.vue';
import FormStepFour from '@core-public/components/form-stepper/form-steps/FormStepFour.vue';
import FormStepFive from '@core-public/components/form-stepper/form-steps/FormStepFive.vue';
import Routes from '@core-public/router/routes';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();
const router = useRouter();

const stepIndex = reactive({
  step: 1,
  previousStep: 0,
});

onMounted(() => {
  stepIndex.step = applicationStore.completeApplication.application.currentStep;
  checkForCorrectForm();
});

const { isLoading } = useQuery(['getIncompleteApplications'], () => {
  if (!applicationStore.completeApplication.id) {
    stepIndex.step = 0;
    const res = applicationStore.getCompleteApplicationFromApi(
      authStore.auth.userEmail,
      false
    );

    res.then(data => {
      applicationStore.setCompleteApplication(data);
      stepIndex.step =
        applicationStore.completeApplication.application.currentStep;
      checkForCorrectForm();
    });
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

function checkForCorrectForm() {
  if (
    applicationStore.completeApplication.application.currentStep > 5 &&
    applicationStore.completeApplication.application.currentStep <= 9
  ) {
    router.push(Routes.FORM_TWO_ROUTE_PATH);
  } else if (applicationStore.completeApplication.application.currentStep > 9) {
    router.push(Routes.QUALIFYING_QUESTIONS_ROUTE_PATH);
  }
}
</script>

<style lang="scss" scoped>
.form-card {
  height: auto;
  min-height: 45vh;
}
</style>
