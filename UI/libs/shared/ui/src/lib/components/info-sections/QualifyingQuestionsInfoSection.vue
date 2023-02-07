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
      <v-col
        cols="12"
        lg="3"
      >
        <v-icon
          left
          color="accent"
        >
          {{
            index % 2 === 0 ? 'mdi-chat-question' : 'mdi-chat-question-outline'
          }}
        </v-icon>
        <strong class="mr-3">{{ `${key}: ` }}</strong>
      </v-col>
      <v-col
        cols="12"
        lg="9"
      >
        <span>{{ value }}</span>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useRouter } from 'vue-router/composables';

const applicationStore = useCompleteApplicationStore();
const router = useRouter();

function handleEditRequest() {
  applicationStore.completeApplication.application.currentStep = 9;
  router.push({
    path: '/form',
    query: {
      applicationId: applicationStore.completeApplication.id,
      isComplete: applicationStore.completeApplication.application.isComplete,
    },
  });
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
