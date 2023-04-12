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

    <v-container
      v-else-if="!$vuetify.breakpoint.xsOnly"
      fluid
    >
      <v-stepper
        non-linear
        @change="handleStepChange"
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
            <PersonalInfoStep :handle-next-section="handleNextSection" />
          </v-stepper-content>
          <v-stepper-content :step="2">
            <IdBirthInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="3">
            <AddressInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="4">
            <PhysicalAppearanceStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="5">
            <ContactStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="6">
            <WorkInfoStep
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
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="9">
            <QualifyingQuestionsStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
          <v-stepper-content :step="10">
            <SignatureStep
              :routes="routes"
              :handle-previous-section="handlePreviousSection"
            />
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-container>

    <v-container
      v-else
      class="pa-0"
    >
      <v-expansion-panels accordion>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Personal') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <PersonalInfoStep :handle-next-section="handleNextSection" />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Citizenship') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <IdBirthInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Address') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <AddressInfoStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Appearance') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <PhysicalAppearanceStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Contact') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <ContactStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t(' Employment & Weapons') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <WorkInfoStep
              :routes="routes"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Application Type') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <ApplicationTypeStep
              v-if="stepIndex.step === 7"
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t(' Upload Files') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <FileUploadStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Qualifying Questions') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <QualifyingQuestionsStep
              :handle-next-section="handleNextSection"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
          <v-expansion-panel-header>
            {{ $t('Signature') }}
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <SignatureStep
              :routes="routes"
              :handle-previous-section="handlePreviousSection"
            />
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
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

function handleStepChange(step: number) {
  window.console.log('changing to step', step);
  stepIndex.previousStep = stepIndex.step - 1;
  stepIndex.step = step;
}
</script>

<style lang="scss">
@media only screen and (max-width: 1900px) {
  .v-stepper:not(.v-stepper--vertical) .v-stepper__label {
    display: none !important;
  }
}

.theme--dark.v-expansion-panels .v-expansion-panel {
  background: #303030;
}

// .v-expansion-panel-content__wrap {
//   padding-left: 5px !important;
//   padding-right: 5px !important;
// }
</style>
