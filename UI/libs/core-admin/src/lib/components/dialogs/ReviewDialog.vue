<template>
  <v-dialog
    v-model="dialog"
    max-width="800"
  >
    <template #activator="{ on, attrs }">
      <v-btn
        v-if="permitStore.getPermitDetail.application.flaggedForLicensingReview"
        @click="handleInfoClick"
        color="error"
        class="ml-8"
        v-bind="attrs"
        v-on="on"
      >
        {{ $t('Review Required') }}
      </v-btn>
    </template>
    <v-card>
      <v-card-title>
        <v-icon
          large
          class="mr-3"
        >
          mdi-information-outline
        </v-icon>
        {{ $t('Review Required') }}
      </v-card-title>

      <v-card-text>
        <div class="text-h6 font-weight-bold dark-grey--text mt-5 mb-5">
          The applicant has approved the changes. Please confirm if you would
          like to overwrite their previous response with the revised changes.
        </div>

        <v-textarea
          v-if="flaggedQuestionText"
          class="mt-7"
          outlined
          rows="6"
          auto-grow
          :value="flaggedQuestionText"
          readonly
          style="font-size: 18px"
        ></v-textarea>
      </v-card-text>

      <v-card-actions>
        <v-btn
          text
          color="error"
          @click="dialog = false"
        >
          Cancel
        </v-btn>

        <v-spacer></v-spacer>

        <v-btn
          text
          color="primary"
          @click="handleAcceptChanges"
        >
          Accept
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { QualifyingQuestionStandard } from '@shared-utils/types/defaultTypes'
import { i18n } from '@shared-ui/plugins'
import { ref } from 'vue'
import { useMutation } from '@tanstack/vue-query'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const permitStore = usePermitsStore()
const dialog = ref(false)
const flaggedQuestionText = ref('')
const historyMessage = ref('')

const { mutate: updatePermitDetails } = useMutation({
  mutationFn: () => permitStore.updateUserApplication(historyMessage.value),
})

function acceptChanges() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.qualifyingQuestions

  if (qualifyingQuestions) {
    if (qualifyingQuestions.questionOne.temporaryAgency) {
      qualifyingQuestions.questionOne.agency =
        qualifyingQuestions.questionOne.temporaryAgency
      qualifyingQuestions.questionOne.temporaryAgency = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryIssueDate) {
      qualifyingQuestions.questionOne.issueDate =
        qualifyingQuestions.questionOne.temporaryIssueDate
      qualifyingQuestions.questionOne.temporaryIssueDate = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryNumber) {
      qualifyingQuestions.questionOne.number =
        qualifyingQuestions.questionOne.temporaryNumber
      qualifyingQuestions.questionOne.temporaryNumber = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryIssuingState) {
      qualifyingQuestions.questionOne.issuingState =
        qualifyingQuestions.questionOne.temporaryIssuingState
      qualifyingQuestions.questionOne.temporaryIssuingState = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryAgency) {
      qualifyingQuestions.questionTwo.agency =
        qualifyingQuestions.questionTwo.temporaryAgency
      qualifyingQuestions.questionTwo.temporaryAgency = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryDenialDate) {
      qualifyingQuestions.questionTwo.denialDate =
        qualifyingQuestions.questionTwo.temporaryDenialDate
      qualifyingQuestions.questionTwo.temporaryDenialDate = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryDenialReason) {
      qualifyingQuestions.questionTwo.denialReason =
        qualifyingQuestions.questionTwo.temporaryDenialReason
      qualifyingQuestions.questionTwo.temporaryDenialReason = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    for (const trafficViolation of qualifyingQuestions.questionTwelve
      .temporaryTrafficViolations) {
      qualifyingQuestions.questionTwelve.trafficViolations.push(
        trafficViolation
      )
      qualifyingQuestions.questionTwelve.selected = true
    }

    qualifyingQuestions.questionTwelve.temporaryTrafficViolations = []

    for (const [key, value] of Object.entries(qualifyingQuestions)) {
      if (
        key !== 'questionOne' &&
        key !== 'questionTwo' &&
        key !== 'questionTwelve' &&
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      ) {
        convertToQualifyingQuestionStandard(value).selected = true
        convertToQualifyingQuestionStandard(value).explanation =
          convertToQualifyingQuestionStandard(value).temporaryExplanation
        convertToQualifyingQuestionStandard(value).temporaryExplanation = ''
      }
    }
  }

  permitStore.getPermitDetail.application.flaggedForLicensingReview = false
  permitStore.getPermitDetail.application.flaggedForCustomerReview = false

  if (permitStore.getPermitDetail.application.originalStatus) {
    permitStore.getPermitDetail.application.status =
      permitStore.getPermitDetail.application.originalStatus
  }

  historyMessage.value = `Updated Qualifying Questions`

  updatePermitDetails()

  dialog.value = false
}

function acceptLegacyChanges() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.legacyQualifyingQuestions

  if (qualifyingQuestions) {
    if (qualifyingQuestions.questionOne.temporaryAgency) {
      qualifyingQuestions.questionOne.agency =
        qualifyingQuestions.questionOne.temporaryAgency
      qualifyingQuestions.questionOne.temporaryAgency = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryIssueDate) {
      qualifyingQuestions.questionOne.issueDate =
        qualifyingQuestions.questionOne.temporaryIssueDate
      qualifyingQuestions.questionOne.temporaryIssueDate = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryNumber) {
      qualifyingQuestions.questionOne.number =
        qualifyingQuestions.questionOne.temporaryNumber
      qualifyingQuestions.questionOne.temporaryNumber = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionOne.temporaryIssuingState) {
      qualifyingQuestions.questionOne.issuingState =
        qualifyingQuestions.questionOne.temporaryIssuingState
      qualifyingQuestions.questionOne.temporaryIssuingState = ''
      qualifyingQuestions.questionOne.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryAgency) {
      qualifyingQuestions.questionTwo.agency =
        qualifyingQuestions.questionTwo.temporaryAgency
      qualifyingQuestions.questionTwo.temporaryAgency = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryDenialDate) {
      qualifyingQuestions.questionTwo.denialDate =
        qualifyingQuestions.questionTwo.temporaryDenialDate
      qualifyingQuestions.questionTwo.temporaryDenialDate = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    if (qualifyingQuestions.questionTwo.temporaryDenialReason) {
      qualifyingQuestions.questionTwo.denialReason =
        qualifyingQuestions.questionTwo.temporaryDenialReason
      qualifyingQuestions.questionTwo.temporaryDenialReason = ''
      qualifyingQuestions.questionTwo.selected = true
    }

    for (const trafficViolation of qualifyingQuestions.questionEight
      .temporaryTrafficViolations) {
      qualifyingQuestions.questionEight.trafficViolations.push(trafficViolation)
      qualifyingQuestions.questionEight.selected = true
    }

    qualifyingQuestions.questionEight.temporaryTrafficViolations = []

    for (const [key, value] of Object.entries(qualifyingQuestions)) {
      if (
        key !== 'questionOne' &&
        key !== 'questionTwo' &&
        key !== 'questionEight' &&
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      ) {
        convertToQualifyingQuestionStandard(value).selected = true
        convertToQualifyingQuestionStandard(value).explanation =
          convertToQualifyingQuestionStandard(value).temporaryExplanation
        convertToQualifyingQuestionStandard(value).temporaryExplanation = ''
      }
    }
  }

  permitStore.getPermitDetail.application.flaggedForLicensingReview = false
  permitStore.getPermitDetail.application.flaggedForCustomerReview = false

  if (permitStore.getPermitDetail.application.originalStatus) {
    permitStore.getPermitDetail.application.status =
      permitStore.getPermitDetail.application.originalStatus
  }

  historyMessage.value = `Updated Qualifying Questions`

  updatePermitDetails()

  dialog.value = false
}

function showReviewDialog() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.qualifyingQuestions

  flaggedQuestionText.value = ''

  if (qualifyingQuestions) {
    const questionOneAgencyTempValue =
      qualifyingQuestions.questionOne.temporaryAgency || ''
    const questionOneIssueDateTempValue =
      qualifyingQuestions.questionOne.temporaryIssueDate || ''
    const questionOneNumberTempValue =
      qualifyingQuestions.questionOne.temporaryNumber || ''
    const questionOneTemporaryIssuingStateValue =
      qualifyingQuestions.questionOne.temporaryIssuingState || ''

    const questionTwoAgencyTempValue =
      qualifyingQuestions.questionTwo.temporaryAgency || ''
    const questionTwoDenialDateTempValue =
      qualifyingQuestions.questionTwo.temporaryDenialDate || ''
    const questionTwoDenialReasonTempValue =
      qualifyingQuestions.questionTwo.temporaryDenialReason || ''

    if (
      questionOneAgencyTempValue ||
      questionOneIssueDateTempValue ||
      questionOneNumberTempValue ||
      questionOneTemporaryIssuingStateValue
    ) {
      flaggedQuestionText.value += `${i18n.t('QUESTION-ONE')}\n\n`

      flaggedQuestionText.value += `Original Response:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionOne.agency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issuing State: ${
        qualifyingQuestions.questionOne.issuingState || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionOne.issueDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionOne.number || 'N/A'
      }\n\n`

      flaggedQuestionText.value += `Revised Changes:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionOne.temporaryAgency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issuing State: ${
        qualifyingQuestions.questionOne.temporaryIssuingState || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionOne.temporaryIssueDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionOne.temporaryNumber || 'N/A'
      }\n\n`
    }

    if (
      questionTwoAgencyTempValue ||
      questionTwoDenialDateTempValue ||
      questionTwoDenialReasonTempValue
    ) {
      flaggedQuestionText.value += `${i18n.t('QUESTION-TWO')}\n\n`

      flaggedQuestionText.value += `Original Response:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionTwo.agency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Denial Date: ${
        qualifyingQuestions.questionTwo.denialDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `Denial Reason Number: ${
        qualifyingQuestions.questionTwo.denialReason || 'N/A'
      }\n\n`

      flaggedQuestionText.value += `Revised Changes:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionTwo.temporaryAgency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionTwo.temporaryDenialDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionTwo.temporaryDenialReason || 'N/A'
      }\n\n`
    }

    if (
      qualifyingQuestions.questionTwelve.temporaryTrafficViolations.length > 0
    ) {
      flaggedQuestionText.value += `${i18n.t('QUESTION-TWELVE')}\n\n`

      for (const trafficViolation of qualifyingQuestions.questionTwelve
        .temporaryTrafficViolations) {
        flaggedQuestionText.value += `Additional Citations Found: \n`
        flaggedQuestionText.value += `Date: ${trafficViolation.date}\n`
        flaggedQuestionText.value += `Agency: ${trafficViolation.agency}\n`
        flaggedQuestionText.value += `Violation: ${trafficViolation.violation}\n`
        flaggedQuestionText.value += `Citation Number: ${trafficViolation.citationNumber}\n\n`
      }
    }

    for (const [key, value] of Object.entries(qualifyingQuestions)) {
      if (
        key !== 'questionOne' &&
        key !== 'questionTwo' &&
        key !== 'questionTwelve' &&
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      ) {
        const questionNumber = key.slice(8)

        flaggedQuestionText.value += `Question ${i18n.t(
          `QUESTION-${questionNumber.toUpperCase()}`
        )}\n\n`
        flaggedQuestionText.value += `Original Response: ${
          convertToQualifyingQuestionStandard(value).explanation
        }\n\n`
        flaggedQuestionText.value += `Revised Changes: ${
          convertToQualifyingQuestionStandard(value).temporaryExplanation
        }\n\n`
      }
    }
  }

  if (flaggedQuestionText.value !== '') {
    dialog.value = true
  }
}

function showLegacyReviewDialog() {
  const qualifyingQuestions =
    permitStore.getPermitDetail.application.legacyQualifyingQuestions

  flaggedQuestionText.value = ''

  if (qualifyingQuestions) {
    const questionOneAgencyTempValue =
      qualifyingQuestions.questionOne.temporaryAgency || ''
    const questionOneIssueDateTempValue =
      qualifyingQuestions.questionOne.temporaryIssueDate || ''
    const questionOneNumberTempValue =
      qualifyingQuestions.questionOne.temporaryNumber || ''
    const questionOneTemporaryIssuingStateValue =
      qualifyingQuestions.questionOne.temporaryIssuingState || ''

    const questionTwoAgencyTempValue =
      qualifyingQuestions.questionTwo.temporaryAgency || ''
    const questionTwoDenialDateTempValue =
      qualifyingQuestions.questionTwo.temporaryDenialDate || ''
    const questionTwoDenialReasonTempValue =
      qualifyingQuestions.questionTwo.temporaryDenialReason || ''

    if (
      questionOneAgencyTempValue ||
      questionOneIssueDateTempValue ||
      questionOneNumberTempValue ||
      questionOneTemporaryIssuingStateValue
    ) {
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-ONE')}\n\n`

      flaggedQuestionText.value += `Original Response:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionOne.agency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issuing State: ${
        qualifyingQuestions.questionOne.issuingState || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionOne.issueDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionOne.number || 'N/A'
      }\n\n`

      flaggedQuestionText.value += `Revised Changes:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionOne.temporaryAgency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issuing State: ${
        qualifyingQuestions.questionOne.temporaryIssuingState || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionOne.temporaryIssueDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionOne.temporaryNumber || 'N/A'
      }\n\n`
    }

    if (
      questionTwoAgencyTempValue ||
      questionTwoDenialDateTempValue ||
      questionTwoDenialReasonTempValue
    ) {
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-TWO')}\n\n`

      flaggedQuestionText.value += `Original Response:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionTwo.agency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Denial Date: ${
        qualifyingQuestions.questionTwo.denialDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `Denial Reason Number: ${
        qualifyingQuestions.questionTwo.denialReason || 'N/A'
      }\n\n`

      flaggedQuestionText.value += `Revised Changes:\n`
      flaggedQuestionText.value += `Agency: ${
        qualifyingQuestions.questionTwo.temporaryAgency || 'N/A'
      }\n`
      flaggedQuestionText.value += `Issue Date: ${
        qualifyingQuestions.questionTwo.temporaryDenialDate || 'N/A'
      }\n`
      flaggedQuestionText.value += `License Number: ${
        qualifyingQuestions.questionTwo.temporaryDenialReason || 'N/A'
      }\n\n`
    }

    if (
      qualifyingQuestions.questionEight.temporaryTrafficViolations.length > 0
    ) {
      flaggedQuestionText.value += `${i18n.t('LEGACY-QUESTION-EIGHT')}\n\n`

      for (const trafficViolation of qualifyingQuestions.questionEight
        .temporaryTrafficViolations) {
        flaggedQuestionText.value += `Additional Citations Found: \n`
        flaggedQuestionText.value += `Date: ${trafficViolation.date}\n`
        flaggedQuestionText.value += `Agency: ${trafficViolation.agency}\n`
        flaggedQuestionText.value += `Violation: ${trafficViolation.violation}\n`
        flaggedQuestionText.value += `Citation Number: ${trafficViolation.citationNumber}\n\n`
      }
    }

    for (const [key, value] of Object.entries(qualifyingQuestions)) {
      if (
        key !== 'questionOne' &&
        key !== 'questionTwo' &&
        key !== 'questionEight' &&
        convertToQualifyingQuestionStandard(value).temporaryExplanation
      ) {
        const questionNumber = key.slice(8)

        flaggedQuestionText.value += `Question ${i18n.t(
          `LEGACY-QUESTION-${questionNumber.toUpperCase()}`
        )}\n\n`
        flaggedQuestionText.value += `Original Response: ${
          convertToQualifyingQuestionStandard(value).explanation
        }\n\n`
        flaggedQuestionText.value += `Revised Changes: ${
          convertToQualifyingQuestionStandard(value).temporaryExplanation
        }\n\n`
      }
    }
  }

  if (flaggedQuestionText.value !== '') {
    dialog.value = true
  }
}

function handleInfoClick() {
  if (permitStore.getPermitDetail.application.legacyQualifyingQuestions) {
    showLegacyReviewDialog()
  } else {
    showReviewDialog()
  }
}

function handleAcceptChanges() {
  if (permitStore.getPermitDetail.application.legacyQualifyingQuestions) {
    acceptLegacyChanges()
  } else {
    acceptChanges()
  }
}

function convertToQualifyingQuestionStandard(item) {
  return item as QualifyingQuestionStandard
}
</script>
