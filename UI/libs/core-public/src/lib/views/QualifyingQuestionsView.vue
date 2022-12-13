<template>
  <v-container class="mb-10">
    <v-sheet class="rounded p-4">
      <v-subheader class="sub-header font-weight-bold mb-2">
        {{ $t('Qualifying Questions') }}
      </v-subheader>
      <v-form
        class="ml-5"
        ref="form"
        v-model="valid"
      >
        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="12"
            lg="6"
          >
            {{ $t('QUESTION-ONE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionOne"
              :rules="[
                completeApplication.qualifyingQuestions.questionOne !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionOne"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionOneExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTwo"
              :rules="[
                completeApplication.qualifyingQuestions.questionTwo !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionTwo"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionTwoExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionThree"
              :rules="[
                completeApplication.qualifyingQuestions.questionThree !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionThree"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionThreeExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FOUR') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFour"
              :rules="[
                completeApplication.qualifyingQuestions.questionFour !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionFour"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionFourExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FIVE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFive"
              :rules="[
                completeApplication.qualifyingQuestions.questionFive !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionFive"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionFiveExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SIX') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionSix"
              :rules="[
                completeApplication.qualifyingQuestions.questionSix !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionSix"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionSixExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionSeven"
              :rules="[
                completeApplication.qualifyingQuestions.questionSeven !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionSeven"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionSevenExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              :rules="[
                completeApplication.qualifyingQuestions.questionEight !== null,
              ]"
              v-model="completeApplication.qualifyingQuestions.questionEight"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionEight"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionEightExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionNine"
              :rules="[
                completeApplication.qualifyingQuestions.questionNine !== null,
              ]"
              row
            >
              <v-radio
                :label="$t('YES')"
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionNine"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionNineExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTen"
              :rules="[
                completeApplication.qualifyingQuestions.questionTen !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionTen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="completeApplication.qualifyingQuestions.questionTenExp"
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionEleven"
              :rules="[
                completeApplication.qualifyingQuestions.questionEleven !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionEleven"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionElevenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionTwelve"
              :rules="[
                completeApplication.qualifyingQuestions.questionTwelve !== null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionTwelve"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionTwelveExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-THIRTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionThirteen"
              :rules="[
                completeApplication.qualifyingQuestions.questionThirteen !==
                  null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionThirteen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionThirteenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FOURTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFourteen"
              row
              :rules="[
                completeApplication.qualifyingQuestions.questionFourteen !==
                  null,
              ]"
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionFourteen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionFourteenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-FIFTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="completeApplication.qualifyingQuestions.questionFifteen"
              :rules="[
                completeApplication.qualifyingQuestions.questionFifteen !==
                  null,
              ]"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionFifteen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionFifteenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            class="text-left"
            cols="12"
            lg="6"
          >
            {{ $t('QUESTION-SIXTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              :rules="[
                completeApplication.qualifyingQuestions.questionSixteen !==
                  null,
              ]"
              v-model="completeApplication.qualifyingQuestions.questionSixteen"
              row
            >
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionSixteen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionSixteenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                completeApplication.qualifyingQuestions.questionSeventeen
              "
              :rules="[
                completeApplication.qualifyingQuestions.questionSeventeen !==
                  null,
              ]"
              row
            >
              <v-radio
                :label="$t('YES')"
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :value="true"
              />
              <v-radio
                :color="$vuetify.theme.dark ? 'info' : 'primary'"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
          <v-col
            cols="12"
            lg="6"
            v-if="completeApplication.qualifyingQuestions.questionSeventeen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                completeApplication.qualifyingQuestions.questionSeventeenExp
              "
              :rules="[
                v =>
                  (v && v.length <= 1000) ||
                  $t('Maximum 1000 characters are allowed'),
              ]"
            >
            </v-textarea>
          </v-col>
        </v-row>
      </v-form>
      <FormButtonContainer
        :valid="valid"
        @submit="updateMutation.mutate"
        @save="saveMutation.mutate"
        @back="goBackMutation.mutate"
        @cancel="router.push('/')"
      />
    </v-sheet>
    <v-snackbar
      :value="snackbar"
      :timeout="3000"
      bottom
      color="error"
      outlined
    >
      {{ $t('Section update unsuccessful please try again.') }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue';
import Routes from '@core-public/router/routes';
import { ref } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRouter } from 'vue-router/composables';

const snackbar = ref(false);
const valid = ref(false);
const applicationStore = useCompleteApplicationStore();
const completeApplication = applicationStore.completeApplication.application;
const router = useRouter();

const updateMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push(Routes.FINALIZE_ROUTE_PATH);
  },
  onError: () => {
    snackbar.value = true;
  },
});

const goBackMutation = useMutation({
  mutationFn: () => {
    applicationStore.completeApplication.application.currentStep = 9;

    return applicationStore.updateApplication('');
  },
  onSuccess: () => {
    router.push(Routes.FORM_ROUTE_PATH);
  },
  onError: () => {
    snackbar.value = true;
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
    snackbar.value = true;
  },
});
</script>
