<template>
  <div>
    <v-container v-if="state.isLoading && !state.isError">
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>

    <v-container v-else>
      <v-stepper
        v-model="stepIndex.step"
        non-linear
      >
        <v-stepper-header>
          <v-stepper-step
            color="primary"
            editable
            :step="1"
          >
            {{ $t('Personal') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="2"
          >
            {{ $t('Citizenship') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="3"
          >
            {{ $t('Address') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="4"
          >
            {{ $t('Appearance') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="5"
          >
            {{ $t('Contact') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="6"
          >
            {{ $t(' Employment & Weapons') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="7"
          >
            {{ $t('Application Type') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="8"
          >
            {{ $t(' Upload Files') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="9"
          >
            {{ $t('Qualifying Questions') }}
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step
            editable
            color="primary"
            :step="10"
          >
            {{ $t('Signature') }}
          </v-stepper-step>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <PersonalInfoStep
              v-if="stepIndex.step === 1"
              :handle-next-section="handleNextSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="2">
            <IdBirthInfoStep
              v-if="stepIndex.step === 2"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="3">
            <AddressInfoStep
              v-if="stepIndex.step === 3"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="4">
            <PhysicalAppearanceStep
              v-if="stepIndex.step === 4"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="5">
            <ContactStep
              v-if="stepIndex.step === 5"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="6">
            <WorkInfoStep
              v-if="stepIndex.step === 6"
              :routes="routes"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="7">
            <ApplicationTypeStep
              v-if="stepIndex.step === 7"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="8">
            <FileUploadStep
              v-if="stepIndex.step === 8"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="9">
            <QualifyingQuestionsStep
              v-if="stepIndex.step === 9"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="10">
            <SignatureStep
              v-if="stepIndex.step === 10"
              :routes="routes"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import AddressInfoStep from '@core-public/components/form-stepper/form-steps/AddressInfoStep.vue';
import ApplicationTypeStep from '@core-public/components/form-stepper/form-steps/ApplicationTypeStep.vue';
import ContactStep from '@core-public/components/form-stepper/form-steps/ContactStep.vue';
import FileUploadStep from '@core-public/components/form-stepper/form-steps/FileUploadStep.vue';
import IdBirthInfoStep from '@core-public/components/form-stepper/form-steps/IdBirthInfoStep.vue';
import PersonalInfoStep from '@core-public/components/form-stepper/form-steps/PersonalInfoStep.vue';
import PhysicalAppearanceStep from '@core-public/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import QualifyingQuestionsStep from '@core-public/components/form-stepper/form-steps/QualifyingQuestionsStep.vue';
import SignatureStep from '@core-public/components/form-stepper/form-steps/SignatureStep.vue';
import WorkInfoStep from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRoute } from 'vue-router/composables';
import { onMounted, reactive } from 'vue';

interface IWrapperProps {
  admin: boolean;
  routes: unknown;
}

const props = defineProps<IWrapperProps>();

const applicationStore = useCompleteApplicationStore();
const route = useRoute();

const stepIndex = reactive({
  step: 1,
  previousStep: 0,
});

const state = reactive({
  isLoading: false,
  isError: false,
});

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    state.isLoading = true;
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.applicationId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res);
        state.isLoading = false;
        stepIndex.step =
          applicationStore.completeApplication.application.currentStep;
      })
      .catch(() => {
        state.isError = true;
      });
  }

  stepIndex.step = applicationStore.completeApplication.application.currentStep;
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
