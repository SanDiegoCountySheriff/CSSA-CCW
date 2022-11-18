<template>
  <div>
    <v-container v-if="isLoading">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type=" list-item"
      />
    </v-container>
    <v-container fluid>
      <v-stepper
        vertical
        v-model="stepIndex.step"
      >
        <v-stepper-step
          step="6"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 6"
        >
          {{ $t(' Employment & Weapons') }}
        </v-stepper-step>
        <v-stepper-content step="6">
          <SecondFormStepOne
            v-if="stepIndex.step === 6"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="7"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 7"
        >
          {{ $t('Application Type') }}
        </v-stepper-step>
        <v-stepper-content step="7">
          <RenewSecondFormStepThree
            v-if="stepIndex.step === 7"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="8"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 8"
        >
          {{ $t(' Upload Files') }}
        </v-stepper-step>
        <v-stepper-content step="8">
          <SecondFormStepTwo
            v-if="stepIndex.step === 8"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>

        <v-stepper-step
          step="9"
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          :complete="stepIndex.step > 9"
        >
          {{ $t('Signature') }}
        </v-stepper-step>
        <v-stepper-content step="9">
          <SecondFormStepFour
            v-if="stepIndex.step === 9"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
      </v-stepper>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/SecondFormStepOne.vue';
import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/SecondFormStepTwo.vue';
import RenewSecondFormStepThree from '@core-public/components/form-stepper/form-steps/RenewSecondFormStepThree.vue';
import SecondFormStepFour from '@core-public/components/form-stepper/form-steps/SecondFormStepFour.vue';
import { onMounted, reactive } from 'vue';
import { useAuthStore } from '@shared-ui/stores/auth';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useQuery } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';
import Routes from '@core-public/router/routes';

const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();
const router = useRouter();

const stepIndex = reactive({
  previousStep: 5,
  step: 6,
});

onMounted(() => {
  stepIndex.step = applicationStore.completeApplication.application.currentStep;

  if (applicationStore.completeApplication.application.currentStep > 9) {
    router.push(Routes.QUALIFYING_QUESTIONS_ROUTE_PATH);
  }
});

const { isLoading } = useQuery(['getIncompleteApplications'], () => {
  if (!applicationStore.completeApplication.id) {
    stepIndex.step = 5;
    const res = applicationStore.getCompleteApplicationFromApi(
      authStore.auth.userEmail,
      false
    );

    res.then(data => {
      applicationStore.setCompleteApplication(data);
      stepIndex.step = 6;
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
</script>
