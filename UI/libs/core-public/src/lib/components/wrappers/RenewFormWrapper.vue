<template>
  <div>
    <v-container
      class="stepper-container"
      fluid
    >
      <v-stepper
        vertical
        v-model="stepIndex.step"
      >
        <v-stepper-step
          :color="$vuetify.theme.dark ? 'info' : 'primary'"
          step="1"
          :complete="stepIndex.step > 1"
        >
          {{ $t('Personal') }}
        </v-stepper-step>
        <v-stepper-content step="1">
          <RenewFormStepOne
            v-if="stepIndex.step === 1"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
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
          <RenewFormStepFive
            v-if="stepIndex.step === 5"
            :routes="routes"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
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
            :routes="routes"
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
            :routes="routes"
            :handle-next-section="handleNextSection"
            :handle-previous-section="handlePreviousSection"
          />
        </v-stepper-content>
      </v-stepper>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import FormStepFour from '@core-public/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import FormStepThree from '@core-public/components/form-stepper/form-steps/AddressInfoStep.vue';
import FormStepTwo from '@core-public/components/form-stepper/form-steps/IdBirthInfoStep.vue';
import RenewFormStepFive from '@core-public/components/form-stepper/form-steps/RenewContactInfoStep.vue';
import RenewFormStepOne from '@core-public/components/form-stepper/form-steps/RenewPersonalInfoStep.vue';
import RenewSecondFormStepThree from '@core-public/components/form-stepper/form-steps/RenewApplicationTypeStep.vue';
import SecondFormStepFour from '@core-public/components/form-stepper/form-steps/SignatureStep.vue';
import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue';
import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

interface IProps {
  admin: boolean;
  routes: unknown;
}
const props = defineProps<IProps>();
const router = useRouter();

const stepIndex = reactive({
  step: 1,
  previousStep: 0,
});

const applicationStore = useCompleteApplicationStore();

onMounted(() => {
  stepIndex.step = applicationStore.completeApplication.application.currentStep;

  if (stepIndex.step > 9) {
    router.push(props.routes.QUALIFYING_QUESTIONS_ROUTE_PATH);
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
<style lang="scss" scoped>
.stepper-container {
  padding: 0;
  margin: 0;
}
</style>
