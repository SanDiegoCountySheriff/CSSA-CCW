<template>
  <div>
    <v-container
      v-if="!$vuetify.breakpoint.mdAndDown"
      fluid
    >
      <v-stepper
        v-model="stepIndex.step"
        non-linear
        class="stepper"
        @change="updateMutation"
        :alt-labels="$vuetify.breakpoint.lgAndDown"
      >
        <v-stepper-header
          :class="$vuetify.theme.dark ? 'sticky-dark' : 'sticky-light'"
        >
          <v-stepper-step
            editable
            :complete="stepOneValid"
            :edit-icon="stepOneValid ? 'mdi-check' : '$edit'"
            :color="stepOneValid ? 'success' : 'primary'"
            :step="1"
          >
            <div class="text-center">{{ $t('Name Change') }}</div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepTwoValid"
            :edit-icon="stepTwoValid ? 'mdi-check' : '$edit'"
            :color="stepTwoValid ? 'success' : 'primary'"
            :step="2"
          >
            <div class="text-center">
              {{ $t('Address Change') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepThreeValid"
            :edit-icon="stepThreeValid ? 'mdi-check' : '$edit'"
            :color="stepThreeValid ? 'success' : 'primary'"
            :step="3"
          >
            <div class="text-center">
              {{ $t('Weapon Change') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepFourValid"
            :edit-icon="stepFourValid ? 'mdi-check' : '$edit'"
            :color="stepFourValid ? 'success' : 'primary'"
            :step="4"
          >
            <div class="text-center">
              {{ $t('Supporting Documents') }}
            </div>
          </v-stepper-step>

          <v-divider></v-divider>

          <v-stepper-step
            editable
            :complete="stepFiveValid"
            :edit-icon="stepFiveValid ? 'mdi-check' : '$edit'"
            :color="stepFiveValid ? 'success' : 'primary'"
            :step="5"
          >
            <div class="text-center">
              {{ $t('Finalize') }}
            </div>
          </v-stepper-step>

          <v-progress-linear
            :active="isGetApplicationLoading || isUpdateApplicationLoading"
            indeterminate
          ></v-progress-linear>
        </v-stepper-header>

        <v-stepper-items>
          <v-stepper-content :step="1">
            <v-card-title>Modify the first thing</v-card-title>
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="2">
            <v-card-title>Modify the second thing</v-card-title>
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="3">
            <v-card-title>Modify the third thing</v-card-title>
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="4">
            <v-card-title>Modify the fourth thing</v-card-title>
          </v-stepper-content>
        </v-stepper-items>

        <v-stepper-items>
          <v-stepper-content :step="5">
            <v-card-title>Modify the fifth thing</v-card-title>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-container>
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { onMounted, reactive, ref } from 'vue'
import { useMutation, useQuery } from '@tanstack/vue-query'

const applicationStore = useCompleteApplicationStore()
const isApplicationValid = ref(false)
const stepIndex = reactive({
  step: 1,
  previousStep: 1,
})
const stepOneValid = ref(false)
const stepTwoValid = ref(false)
const stepThreeValid = ref(false)
const stepFourValid = ref(false)
const stepFiveValid = ref(false)

onMounted(() => {
  window.scrollTo(0, 0)
  isApplicationValid.value = Boolean(applicationStore.completeApplication.id)
})

const { isLoading: isGetApplicationLoading } = useQuery(
  ['getApplicationsByUser'],
  () => applicationStore.getAllUserApplicationsApi(),
  {
    enabled: !isApplicationValid.value,
    onSuccess: data => {
      applicationStore.setCompleteApplication(data[0] as CompleteApplication)
    },
  }
)

const { isLoading: isUpdateApplicationLoading, mutate: updateMutation } =
  useMutation({
    mutationFn: () => {
      return applicationStore.updateApplication()
    },
  })
</script>

<style lang="scss">
.theme--dark.v-expansion-panels .v-expansion-panel {
  background: #303030;
}

.v-expansion-panel-content__wrap {
  padding-left: 5px !important;
  padding-right: 5px !important;
}

.stepper {
  overflow: visible;
}

.sticky-light {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: white;
}

.sticky-dark {
  position: sticky;
  top: 64px;
  z-index: 1;
  background-color: #303030;
}

.progress-circular {
  position: fixed;
  top: 75px;
  left: 50%;
  z-index: 2;
}

.theme--dark.v-label.v-label--active {
  color: white !important;
}
</style>
