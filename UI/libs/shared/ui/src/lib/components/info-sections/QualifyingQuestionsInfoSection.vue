<template>
  <v-container fluid>
    <v-banner class="sub-header font-weight-bold text-left my-5">
      {{ $t('Qualifying Questions: ') }}
    </v-banner>

    <v-row
      v-for="(value, key, index) in applicationStore.completeApplication
        .application.qualifyingQuestions"
      :key="index"
    >
      <v-col>
        <v-banner>
          <v-row>
            <v-col cols="2">
              <v-icon
                left
                color="primary"
              >
                mdi-chat-question
              </v-icon>
              <strong>{{ `${getQuestion(key)}: ` }}</strong>
            </v-col>
            <v-col cols="8"> {{ $t(getText(index)) }}</v-col>
            <v-col cols="2">
              <span>
                <strong>{{ $t('Answered') }}:</strong>
                {{ value.selected ? 'Yes' : 'No' }}</span
              >
            </v-col>
          </v-row>

          <v-row v-if="value.selected">
            <template v-if="index !== 0 && index !== 1 && index !== 11">
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Answer:</strong>
              </v-col>
              <v-col cols="8">
                {{ castToQualifyingQuestionStandardType(value).explanation }}
              </v-col>
            </template>

            <template v-else-if="index === 0">
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Agency:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionOne(value).agency }}
              </v-col>
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Issuing State:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionOne(value).issuingState }}
              </v-col>
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Issue Date:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionOne(value).issueDate }}
              </v-col>
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Number:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionOne(value).number }}
              </v-col>
            </template>

            <template v-else-if="index === 1">
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Agency:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionTwo(value).agency }}
              </v-col>
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Denial Date:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionTwo(value).denialDate }}
              </v-col>
              <v-col cols="2">
                <v-icon
                  left
                  color="primary"
                >
                  mdi-chat-question-outline
                </v-icon>
                <strong>Denial Reason:</strong>
              </v-col>
              <v-col cols="10">
                {{ castToQualifyingQuestionTwo(value).denialReason }}
              </v-col>
            </template>

            <template v-else-if="index === 11">
              <template
                v-for="(
                  violation, violationIndex
                ) of castToQualifyingQuestionTwelve(value).trafficViolations"
              >
                <v-col
                  :key="violationIndex"
                  cols="12"
                >
                  <v-row>
                    <v-col> <strong>Date:</strong> {{ violation.date }} </v-col>
                    <v-col>
                      <strong>Agency:</strong>
                      {{ violation.agency }}
                    </v-col>
                    <v-col>
                      <strong>Violation:</strong> {{ violation.violation }}
                    </v-col>
                    <v-col>
                      <strong>Citation Number:</strong>
                      {{ violation.citationNumber }}
                    </v-col>
                  </v-row>
                </v-col>
              </template>
            </template>
          </v-row>
        </v-banner>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { capitalize } from '@shared-utils/formatters/defaultFormatters'
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import {
  QualifyingQuestionOne,
  QualifyingQuestionStandard,
  QualifyingQuestionTwelve,
  QualifyingQuestionTwo,
} from '@shared-utils/types/defaultTypes'

const applicationStore = useCompleteApplicationStore()

function castToQualifyingQuestionStandardType(item) {
  return item as QualifyingQuestionStandard
}

function castToQualifyingQuestionOne(item) {
  return item as QualifyingQuestionOne
}

function castToQualifyingQuestionTwo(item) {
  return item as QualifyingQuestionTwo
}

function castToQualifyingQuestionTwelve(item) {
  return item as QualifyingQuestionTwelve
}

function getQuestion(key: string) {
  return `${capitalize(key.substring(0, 8))} ${capitalize(
    key.substring(8, key.length)
  )}`
}

function getText(index: number) {
  switch (index) {
    case 0:
      return 'QUESTION-ONE'
    case 1:
      return 'QUESTION-TWO'
    case 2:
      return 'QUESTION-THREE'
    case 3:
      return 'QUESTION-FOUR'
    case 4:
      return 'QUESTION-FIVE'
    case 5:
      return 'QUESTION-SIX'
    case 6:
      return 'QUESTION-SEVEN'
    case 7:
      return 'QUESTION-EIGHT'
    case 8:
      return 'QUESTION-NINE'
    case 9:
      return 'QUESTION-TEN'
    case 10:
      return 'QUESTION-ELEVEN'
    case 11:
      return 'QUESTION-TWELVE'
    case 12:
      return 'QUESTION-THIRTEEN'
    case 13:
      return 'QUESTION-FOURTEEN'
    case 14:
      return 'QUESTION-FIFTEEN'
    case 15:
      return 'QUESTION-SIXTEEN'
    case 16:
      return 'QUESTION-SEVENTEEN'
    case 17:
      return 'QUESTION-EIGHTEEN'
    case 18:
      return 'QUESTION-NINETEEN'
    case 19:
      return 'QUESTION-TWENTY'
    case 20:
      return 'QUESTION-TWENTYONE'
    default:
      return ''
  }
}
</script>
