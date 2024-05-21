<template>
  <div>
    <FormButtonContainer
      v-if="$vuetify.breakpoint.lgAndUp"
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />

    <v-container v-if="model.application.legacyQualifyingQuestions">
      <v-card-title>
        {{ $t('Qualifying Questions') }}
      </v-card-title>

      <v-form
        ref="form"
        v-model="valid"
      >
        <PriorLicenseQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionOne"
          :title="$t('LEGACY-QUESTION-ONE')"
        />

        <DeniedLicenseQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionTwo"
          :title="$t('LEGACY-QUESTION-TWO')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionThree"
          :max-length="config.appConfig.questions.three"
          :title="$t('LEGACY-QUESTION-THREE')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionFour"
          :max-length="config.appConfig.questions.four"
          :title="$t('LEGACY-QUESTION-FOUR')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionFive"
          :max-length="config.appConfig.questions.five"
          :title="$t('LEGACY-QUESTION-FIVE')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionSix"
          :max-length="config.appConfig.questions.six"
          :title="$t('LEGACY-QUESTION-SIX')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionSeven"
          :max-length="config.appConfig.questions.seven"
          :title="$t('LEGACY-QUESTION-SEVEN')"
        />

        <TrafficQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionEight"
          :title="$t('LEGACY-QUESTION-EIGHT')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionNine"
          :max-length="config.appConfig.questions.nine"
          :title="$t('LEGACY-QUESTION-NINE')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionTen"
          :max-length="config.appConfig.questions.ten"
          :title="$t('LEGACY-QUESTION-TEN')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionEleven"
          :max-length="config.appConfig.questions.eleven"
          :title="$t('LEGACY-QUESTION-ELEVEN')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionTwelve"
          :max-length="config.appConfig.questions.twelve"
          :title="$t('LEGACY-QUESTION-TWELVE')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionThirteen"
          :max-length="config.appConfig.questions.thirteen"
          :title="$t('LEGACY-QUESTION-THIRTEEN')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionFourteen"
          :max-length="config.appConfig.questions.fourteen"
          :title="$t('LEGACY-QUESTION-FOURTEEN')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionFifteen"
          :max-length="config.appConfig.questions.fifteen"
          :title="$t('LEGACY-QUESTION-FIFTEEN')"
        />

        <StandardQualifyingQuestion
          v-model="model.application.legacyQualifyingQuestions.questionSixteen"
          :max-length="config.appConfig.questions.sixteen"
          :title="$t('LEGACY-QUESTION-SIXTEEN')"
        />

        <StandardQualifyingQuestion
          v-model="
            model.application.legacyQualifyingQuestions.questionSeventeen
          "
          :max-length="config.appConfig.questions.seventeen"
          :title="$t('LEGACY-QUESTION-SEVENTEEN')"
        />
      </v-form>
    </v-container>

    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />

    <v-snackbar
      :value="snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </div>
</template>

<script setup lang="ts">
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import DeniedLicenseQualifyingQuestion from '@core-public/components/qualifying-questions/DeniedLicenseQualifyingQuestion.vue'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import PriorLicenseQualifyingQuestion from '@core-public/components/qualifying-questions/PriorLicenseQualifyingQuestion.vue'
import StandardQualifyingQuestion from '@core-public/components/qualifying-questions/StandardQualifyingQuestion.vue'
import TrafficQualifyingQuestion from '@core-public/components/qualifying-questions/TrafficQualifyingQuestion.vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { computed, onMounted, ref, watch } from 'vue'

interface IProps {
  value: CompleteApplication
}

const props = defineProps<IProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-seven-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const form = ref()
const snackbar = ref(false)
const valid = ref(false)
const config = useAppConfigStore()

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

function handleContinue() {
  emit('update-step-seven-valid', true)
  emit('handle-continue')
}

function handleSave() {
  emit('update-step-seven-valid', true)
  emit('handle-save')
}

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-seven-valid', newValue)
  }
})
</script>
