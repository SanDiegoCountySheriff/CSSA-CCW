<template>
  <v-container class="mb-10">
    <v-sheet class="rounded p-4">
      <v-subheader class="sub-header font-weight-bold mb-2">
        {{ $t('Qualifying Questions') }}
      </v-subheader>
      <v-form class="ml-5">
        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-ONE') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionOne"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionOne"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionOneExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTwo"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionTwo"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionTwoExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionThree"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionThree"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionThreeExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-FOUR') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFour"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionFour"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionFourExp"
            >
            </v-textarea>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-FIVE') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFive"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionFive"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionFiveExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-SIX') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionSix"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionSix"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionSixExp"
            >
            </v-textarea>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionSeven"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionSeven"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionSevenExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionEight"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionEight"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionEightExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionNine"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionNine"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionNineExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTen"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionTen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionTenExp"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionEleven"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionEleven"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionElevenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTwelve"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionTwelve"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionTwelveExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-THIRTEEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionThirteen"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionThirteen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionThirteenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-FOURTEEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFourteen"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionFourteen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionFourteenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-FIFTEEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFifteen"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionFifteen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionFifteenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-SIXTEEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionSixteen"
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionSixteen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionSixteenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="5"
            lg="4"
            sm="1"
          >
            {{ $t('QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col
            cols="3"
            lg="2"
            sm="1"
          >
            <v-radio-group
              v-model="
                completeApplication.qualifyingQuestions.questionSeventeen
              "
              row
            >
              <v-radio
                :label="$t('true')"
                :value="true"
              />
              <v-radio
                :label="$t('false')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="6"
            lg="5"
            sm="1"
            v-if="completeApplication.qualifyingQuestions.questionSeventeen"
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionSeventeenExp
              "
            >
            </v-textarea>
          </v-col>
        </v-row>
      </v-form>
      <FormButtonContainer
        :valid="true"
        @submit="updateMutation.mutate"
        @save="saveMutation.mutate"
      />
    </v-sheet>
  </v-container>
</template>

<script setup lang="ts">
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import { useCompleteApplicationStore } from '@core-public/stores/completeApplication';
import { useRouter } from 'vue-router/composables';
import { useMutation } from '@tanstack/vue-query';
import { i18n } from '@shared-ui/plugins';

const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.completeApplication.application;
const router = useRouter();

const updateMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication('Qualifying question complete.');
  },
  onSuccess: () => {
    router.push('/finalize');
  },
  onError: () => {
    alert(' Api error');
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication('Save and quit');
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    alert(i18n.t('Save unsuccessful, please try again'));
  },
});
</script>

<style lang="scss" scoped></style>
