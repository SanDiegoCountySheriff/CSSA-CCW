<template>
  <div>
    <v-row class="ml-5">
      <v-col
        cols="12"
        lg="6"
        class="text-left"
      >
        {{ props.title }}
      </v-col>

      <v-col>
        <v-radio-group
          v-model="viewModel.selected"
          :rules="[viewModel.selected !== null]"
          :disabled="disableRadioGroup"
          row
        >
          <v-radio
            :label="$t('YES')"
            :value="true"
            color="primary"
          />

          <v-radio
            :label="$t('NO')"
            :value="false"
            color="primary"
          />
        </v-radio-group>
      </v-col>

      <v-col>
        <v-btn
          v-if="showUpdateButton"
          :disabled="viewModel.updateInformation"
          @click="toggleUpdateInformation('questionThree')"
          color="primary"
        >
          Update Question {{ questionNumber }}
        </v-btn>
      </v-col>
    </v-row>

    <v-row v-if="viewModel.selected">
      <v-col class="mx-8">
        <v-textarea
          v-if="showRenewalExplanation"
          v-model="viewModel.renewalExplanation"
          :rules="[v => !!v || $t('Field cannot be blank')]"
          :label="$t('Please explain')"
          :maxlength="maxLength"
          outlined
        />

        <v-textarea
          v-if="showOriginalExplanation"
          v-model="viewModel.explanation"
          :rules="[v => !!v || $t('Field cannot be blank')]"
          :label="$t('Please explain')"
          :maxlength="maxLength"
          outlined
        />
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { QualifyingQuestionStandard } from '@shared-utils/types/defaultTypes'
import { TranslateResult } from 'vue-i18n'
import { computed } from 'vue'

const emit = defineEmits(['input', 'toggle-update-information'])

interface StandardQualifyingQuestionsProps {
  value: QualifyingQuestionStandard
  maxLength: number
  title: TranslateResult
  questionNumber: string
  isRenew: boolean
  isRenewingWithLegacyQuestions: boolean
}

const props = withDefaults(defineProps<StandardQualifyingQuestionsProps>(), {
  questionNumber: '',
  isRenew: false,
  isRenewingWithLegacyQuestions: false,
})

const viewModel = computed({
  get() {
    return props.value
  },
  set(newVal) {
    emit('input', newVal)
  },
})

const disableRadioGroup = computed(() => {
  return props.isRenew && viewModel && props.isRenewingWithLegacyQuestions
})

const showRenewalExplanation = computed(() => {
  return (
    props.isRenew &&
    Boolean(viewModel.value.updateInformation) &&
    !props.isRenewingWithLegacyQuestions
  )
})

const showOriginalExplanation = computed(() => {
  return (
    !props.isRenew ||
    Boolean(viewModel.value.explanation) ||
    props.isRenewingWithLegacyQuestions
  )
})

const showUpdateButton = computed(() => {
  return props.isRenew && !props.isRenewingWithLegacyQuestions
})

function toggleUpdateInformation(questionKey: string) {
  emit('toggle-update-information', questionKey)
}
</script>
