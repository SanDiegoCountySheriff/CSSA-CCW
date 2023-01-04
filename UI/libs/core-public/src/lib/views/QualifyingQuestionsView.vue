<template>
  <v-container class="mb-10">
    <v-container
      v-if="state.isLoading && !state.isError"
      fluid
    >
      <v-skeleton-loader
        fluid
        class="fill-height"
        type="list-item,
        divider, list-item-three-line,
        card-heading, image, image, image,
        image, actions"
      >
      </v-skeleton-loader>
    </v-container>
    <v-sheet
      v-else
      class="rounded p-4"
    >
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
              v-model="
                state.application.application.qualifyingQuestions.questionOne
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionOne !== null,
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
            v-if="state.application.application.qualifyingQuestions.questionOne"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="
                $t(
                  'Provide issuing agency name, issue date and CCW license number.'
                )
              "
              v-model="
                state.application.application.qualifyingQuestions.questionOneExp
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
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionTwo
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTwo !== null,
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
            v-if="state.application.application.qualifyingQuestions.questionTwo"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="
                $t(
                  'Provide issuing agency name, issue date and CCW license number.'
                )
              "
              v-model="
                state.application.application.qualifyingQuestions.questionTwoExp
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
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionThree
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionThree !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionThree
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThreeExp
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
            {{ $t('QUESTION-FOUR') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionFour
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFour !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionFour
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourExp
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
            {{ $t('QUESTION-FIVE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionFive
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFive !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionFive
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFiveExp
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
            {{ $t('QUESTION-SIX') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionSix
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSix !== null,
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
            v-if="state.application.application.qualifyingQuestions.questionSix"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions.questionSixExp
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
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionSeven
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSeven !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionSeven
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSevenExp
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
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionEight !== null,
              ]"
              v-model="
                state.application.application.qualifyingQuestions.questionEight
              "
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
            v-if="
              state.application.application.qualifyingQuestions.questionEight
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="
                $t('Provide date, violation/accident, Agency, Citation No. ')
              "
              v-model="
                state.application.application.qualifyingQuestions
                  .questionEightExp
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
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionNine
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionNine !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionNine
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="
                $t(
                  'Please explain including the date, agency, charges and disposition.'
                )
              "
              v-model="
                state.application.application.qualifyingQuestions
                  .questionNineExp
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
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionTen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTen !== null,
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
            v-if="state.application.application.qualifyingQuestions.questionTen"
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions.questionTenExp
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
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col
            cols="12"
            lg="6"
          >
            <v-radio-group
              v-model="
                state.application.application.qualifyingQuestions.questionEleven
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionEleven !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionEleven
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionElevenExp
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
              v-model="
                state.application.application.qualifyingQuestions.questionTwelve
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionTwelve !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionTwelve
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionTwelveExp
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThirteen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionThirteen !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionThirteen
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionThirteenExp
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourteen
              "
              row
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFourteen !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionFourteen
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFourteenExp
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
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFifteen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionFifteen !== null,
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
            v-if="
              state.application.application.qualifyingQuestions.questionFifteen
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionFifteenExp
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
                state.application.application.qualifyingQuestions
                  .questionSixteen !== null,
              ]"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSixteen
              "
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
            v-if="
              state.application.application.qualifyingQuestions.questionSixteen
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSixteenExp
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
                state.application.application.qualifyingQuestions
                  .questionSeventeen
              "
              :rules="[
                state.application.application.qualifyingQuestions
                  .questionSeventeen !== null,
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
            v-if="
              state.application.application.qualifyingQuestions
                .questionSeventeen
            "
          >
            <v-textarea
              outlined
              counter
              maxlength="1000"
              :label="$t('Please explain')"
              v-model="
                state.application.application.qualifyingQuestions
                  .questionSeventeenExp
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
import { onMounted, reactive, ref } from 'vue';
import { useCompleteApplicationStore } from '@shared-ui/stores/completeApplication';
import { useMutation } from '@tanstack/vue-query';
import { useRoute, useRouter } from 'vue-router/composables';
import { CompleteApplication } from '@shared-utils/types/defaultTypes';

const snackbar = ref(false);
const valid = ref(false);
const applicationStore = useCompleteApplicationStore();
const router = useRouter();
const route = useRoute();
const state = reactive({
  application: applicationStore.getCompleteApplication as CompleteApplication,
  isLoading: true,
  isError: false,
});

onMounted(() => {
  if (!applicationStore.completeApplication.application.orderId) {
    applicationStore
      .getCompleteApplicationFromApi(
        route.query.orderId,
        route.query.isComplete
      )
      .then(res => {
        applicationStore.setCompleteApplication(res);
        state.application = res;
        state.isLoading = false;
      })
      .catch(() => {
        state.isError = true;
      });
  } else {
    state.isLoading = false;
  }
});

const updateMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push({
      path: Routes.FINALIZE_ROUTE_PATH,
      query: {
        orderId: applicationStore.completeApplication.application.orderId,
        isComplete: applicationStore.completeApplication.application.isComplete,
      },
    });
  },
  onError: () => {
    snackbar.value = true;
  },
});

const goBackMutation = useMutation({
  mutationFn: () => {
    applicationStore.completeApplication.application.currentStep = 9;

    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push({
      path: Routes.FORM_ROUTE_PATH,
      query: {
        orderId: applicationStore.completeApplication.application.orderId,
        isComplete: applicationStore.completeApplication.application.isComplete,
      },
    });
  },
  onError: () => {
    snackbar.value = true;
  },
});

const saveMutation = useMutation({
  mutationFn: () => {
    return applicationStore.updateApplication();
  },
  onSuccess: () => {
    router.push('/');
  },
  onError: () => {
    snackbar.value = true;
  },
});
</script>
