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
          :step-index="stepIndex.step"
          :step-names="formOneStepNames"
        />
        <RenewFormStepItems
          :step-index="stepIndex.step"
          :handle-next-section="handleNextSection"
          :handle-reset="handleResetForm"
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

const stepIndex = reactive({
  step: 1,
});

const applicationStore = useCompleteApplicationStore();
const authStore = useAuthStore();

const { isLoading } = useQuery(['getIncompleteApplications'], () => {
  const res = applicationStore.getCompleteApplicationFromApi(
    authStore.auth.userEmail,
    false
  );

  res.then(data => (applicationStore.completeApplication = data.application));
});

function handleNextSection() {
  stepIndex.step += 1;
}

function handleResetForm() {
  stepIndex.step = 1;
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
