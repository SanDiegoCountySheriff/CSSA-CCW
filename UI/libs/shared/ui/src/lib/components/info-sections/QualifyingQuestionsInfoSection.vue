<template>
  <v-container class="info-section-container rounded mt-3">
    <v-banner class="sub-header font-weight-bold text-left my-5 pl-0">
      {{ $t('Qualifiying Questions') }}
      <template #actions>
        <v-btn
          icon
          @click="handleEditRequest"
        >
          <v-icon :color="$vuetify.theme.dark ? 'info' : 'info'">
            mdi-square-edit-outline
          </v-icon>
        </v-btn>
      </template>
    </v-banner>
    <v-row
      class="text-left ml-3"
      v-for="(value, key, index) in applicationStore.completeApplication
        .application.qualifyingQuestions"
      :key="index"
    >
      <v-container v-if="index % 2 === 0">
        <v-row>
          <v-col
            cols="12"
            lg="3"
            class="pr-0"
          >
            <v-icon
              left
              color="accent"
            >
              {{ 'mdi-chat-question' }}
            </v-icon>
            <strong class="mr-3">{{ `${key}: ` }}</strong>
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
        <v-row>
          <v-col
            cols="12"
            lg="4"
            class="pr-0"
          >
            <v-icon
              left
              color="accent"
            >
              {{ 'mdi-chat-question-outline' }}
            </v-icon>
            <strong class="mr-3">{{ `${key}: ` }}</strong>
          </v-col>
          <v-col
            cols="12"
            lg="8"
          >
            <span>{{ value }}</span>
          </v-col>
        </v-row>
      </v-container>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication'
import { useRouter } from 'vue-router/composables'

const applicationStore = useCompleteApplicationStore()
const router = useRouter()

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 9
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  })
}

function getText(index: number) {
  switch (index) {
    case 0:
      return 'QUESTION-ONE'
    case 2:
      return 'QUESTION-TWO'
    case 4:
      return 'QUESTION-THREE'
    case 6:
      return 'QUESTION-FOUR'
    case 8:
      return 'QUESTION-FIVE'
    case 10:
      return 'QUESTION-SIX'
    case 12:
      return 'QUESTION-SEVEN'
    case 14:
      return 'QUESTION-EIGHT'
    case 16:
      return 'QUESTION-NINE'
    case 18:
      return 'QUESTION-TEN'
    case 20:
      return 'QUESTION-ELEVEN'
    case 22:
      return 'QUESTION-TWELVE'
    case 24:
      return 'QUESTION-THIRTEEN'
    case 26:
      return 'QUESTION-FOURTEEN'
    case 28:
      return 'QUESTION-FIFTEEN'
    case 30:
      return 'QUESTION-SIXTEEN'
    case 32:
      return 'QUESTION-SEVENTEEN'
    default:
      break
  }
}
</script>

<style lang="scss" scoped>
.info-section-container {
  width: 80%;
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
