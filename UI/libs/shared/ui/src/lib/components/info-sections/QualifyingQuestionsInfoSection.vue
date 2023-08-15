<template>
  <v-container class="info-section-container rounded">
    <v-banner class="sub-header font-weight-bold text-left my-5">
      {{ $t('Qualifying Questions: ') }}
      <template #actions>
        <v-tooltip bottom>
          <template #activator="{ on, attrs }">
            <v-btn
              v-if="
                applicationStore.completeApplication.application.status ==
                ApplicationStatus.Incomplete
              "
              icon
              @click="handleEditRequest"
              v-bind="attrs"
              v-on="on"
            >
              <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
                mdi-square-edit-outline
              </v-icon>
            </v-btn>
          </template>
          {{ $t('Edit Section') }}
        </v-tooltip>
      </template>
    </v-banner>
    <v-row
      class="text-left ml-3"
      v-for="(value, key, index) in removeTempQualifyingQuestions(
        props.qualifyingQuestionsInfo
      )"
      :key="index"
    >
      <v-container v-if="index % 2 === 0 && index != 2">
        <v-row>
          <v-col
            cols="12"
            lg="3"
            class="pr-0"
          >
            <v-icon
              left
              color="primary"
            >
              {{ 'mdi-chat-question' }}
            </v-icon>
            <strong class="mr-3">{{ `${getQuestion(key)}: ` }}</strong>
          </v-col>
          <v-col
            cols="12"
            lg="7"
            class="pl-0"
          >
            {{ $t(getText(index)) }}
          </v-col>
          <v-col
            cols="12"
            lg="2"
          >
            <span>
              <strong>{{ $t('Answered') }}:</strong>
              {{ value ? 'Yes' : 'No' }}</span
            >
          </v-col>
        </v-row>
      </v-container>
      <v-container v-else>
        <v-row v-if="value !== ''">
          <v-col
            cols="12"
            lg="3"
            class="pl-12"
          >
            <v-icon
              left
              color="primary"
            >
              {{ 'mdi-chat-question-outline' }}
            </v-icon>
            <strong class="mr-3">{{ `${getExplanation(key)}: ` }}</strong>
          </v-col>
          <v-col
            cols="12"
            lg="7"
          >
            <span>{{ value }}</span>
          </v-col>
        </v-row>
        <v-divider v-if="index >= 3"></v-divider>
      </v-container>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { ApplicationStatus } from '@shared-utils/types/defaultTypes'
import { QualifyingQuestions } from '@shared-utils/types/defaultTypes'
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

interface IQualifyingQuestionsProps {
  qualifyingQuestionsInfo: QualifyingQuestions
  color: string
}

const applicationStore = useCompleteApplicationStore()
const props = defineProps<IQualifyingQuestionsProps>()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 9
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete.toString(),
    },
  })
}

function getQuestion(key: string) {
  return `${capitalize(key.substring(0, 8))} ${capitalize(
    key.substring(8, key.length)
  )}`
}

function getExplanation(key: string) {
  if (key.includes('Agency')) {
    return 'Agency'
  }

  if (key.includes('Issue')) {
    return 'Issued Date'
  }

  if (key.includes('Number')) {
    return 'CCW Number'
  }

  if (key.includes('Exp')) {
    return 'Explanation'
  }

  return ''
}

function getText(index: number) {
  switch (index) {
    case 0:
      return 'QUESTION-ONE'
    case 4:
      return 'QUESTION-TWO'
    case 6:
      return 'QUESTION-THREE'
    case 8:
      return 'QUESTION-FOUR'
    case 10:
      return 'QUESTION-FIVE'
    case 12:
      return 'QUESTION-SIX'
    case 14:
      return 'QUESTION-SEVEN'
    case 16:
      return 'QUESTION-EIGHT'
    case 18:
      return 'QUESTION-NINE'
    case 20:
      return 'QUESTION-TEN'
    case 22:
      return 'QUESTION-ELEVEN'
    case 24:
      return 'QUESTION-TWELVE'
    case 26:
      return 'QUESTION-THIRTEEN'
    case 28:
      return 'QUESTION-FOURTEEN'
    case 30:
      return 'QUESTION-FIFTEEN'
    case 32:
      return 'QUESTION-SIXTEEN'
    case 34:
      return 'QUESTION-SEVENTEEN'
    default:
      return ''
  }
}

function removeTempQualifyingQuestions(qualifyingQuestions) {
  const newQualifyingQuestions = {}

  for (const [key, value] of Object.entries(qualifyingQuestions)) {
    if (!key.includes('Temp')) {
      newQualifyingQuestions[key] = value
    }
  }

  return newQualifyingQuestions
}
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 100%;
  height: 100%;
  margin: 0;
  padding: 0;
}
.info-row {
  display: flex;
  flex-direction: row;
  max-height: 2vh;
  min-height: 1vh;
  margin-left: 0.5rem;
}

.info-text {
  margin-left: 0.5rem;
  text-align: start;
  height: 1.8em;
  width: 50%;
  margin-bottom: 0.5rem;
  padding-bottom: 0.5rem;
  padding-left: 1rem;
  background-color: rgba(211, 241, 241, 0.3);
  border-bottom: 1px solid #666;
  border-radius: 5px;
  font-size: 1.2em;
  font-weight: bold;
}
</style>
