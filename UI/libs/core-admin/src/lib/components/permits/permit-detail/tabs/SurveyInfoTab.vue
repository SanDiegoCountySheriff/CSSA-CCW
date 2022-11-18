<!-- eslint-disable @intlify/vue-i18n/no-raw-text -->
<template>
  <v-card elevation="3">
    <v-card-title class="text-h5">
      {{ $t('Survey Questions:') }}
    </v-card-title>
    <div
      v-for="q in questionsArray"
      :key="q.question"
    >
      <v-divider></v-divider>
      <v-row class="ml-5 grey--text text--darken-2 mt-1 mb-1">
        <v-col
          cols="12"
          md="5"
          sm="3"
          class="text-left"
        >
          <v-icon
            x-small
            color="error"
          >
            mdi-star
          </v-icon>
          {{ $t(q.question) }}
        </v-col>
        <v-col
          cols="12"
          md="5"
          sm="3"
          offset="1"
          class="text-left"
        >
          <div
            class="d-flex flex-column"
            style="gap: 10px"
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions[
                q.explanation
              ]
            "
          >
            <v-chip
              color="error lighten-1"
              label
            >
              {{ $t('Yes') }}
            </v-chip>
            <span> {{ $t('Explanation:') }} </span>
            <v-card color="error lighten-1">
              <v-card-text color="white">
                {{
                  permitStore.getPermitDetail.application.qualifyingQuestions[
                    q.explanation
                  ]
                }}
              </v-card-text>
            </v-card>
          </div>
          <v-chip
            color="green lighten-1"
            label
            v-else
          >
            {{ $t('No') }}
          </v-chip>
        </v-col>
      </v-row>
    </div>
  </v-card>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore';

const permitStore = usePermitsStore();

const questionsArray = [
  { question: 'QUESTION-ONE', explanation: 'questionOneExp' },
  { question: 'QUESTION-TWO', explanation: 'questionTwoExp' },
  { question: 'QUESTION-THREE', explanation: 'questionThreeExp' },
  { question: 'QUESTION-FOUR', explanation: 'questionFourExp' },
  { question: 'QUESTION-FIVE', explanation: 'questionFiveExp' },
  { question: 'QUESTION-SIX', explanation: 'questionSixExp' },
  { question: 'QUESTION-SEVEN', explanation: 'questionSevenExp' },
  { question: 'QUESTION-EIGHT', explanation: 'questionEightExp' },
  { question: 'QUESTION-NINE', explanation: 'questionNineExp' },
  { question: 'QUESTION-TEN', explanation: 'questionTenExp' },
  { question: 'QUESTION-ELEVEN', explanation: 'questionElevenExp' },
  { question: 'QUESTION-TWELVE', explanation: 'questionTwelveExp' },
  { question: 'QUESTION-THIRTEEN', explanation: 'questionThirteenExp' },
  { question: 'QUESTION-FOURTEEN', explanation: 'questionFourteenExp' },
  { question: 'QUESTION-FIFTEEN', explanation: 'questionFifteenExp' },
  { question: 'QUESTION-SIXTEEN', explanation: 'questionSixteenExp' },
  { question: 'QUESTION-SEVENTEEN', explanation: 'questionSeventeenExp' },
];
</script>
<style lang="scss" scoped>
.v-chip {
  max-width: 10% !important;
}

.theme--light.v-card > .v-card__text {
  color: #fff !important;
}
</style>
