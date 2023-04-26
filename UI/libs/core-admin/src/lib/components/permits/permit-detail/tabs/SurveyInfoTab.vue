<template>
  <div>
    <v-card elevation="0">
      <v-card-title>
        {{ $t('Qualifying Questions') }}
        <v-spacer></v-spacer>
        <SaveButton
          :disabled="false"
          @on-save="handleSave"
        />
      </v-card-title>

      <v-card-text>
        <v-row>
          <v-col>
            {{ $t('QUESTION-ONE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOne
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row
          v-if="
            permitStore.getPermitDetail.application.qualifyingQuestions
              .questionOne
          "
        >
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneAgency
              "
              :label="$t('Agency')"
              :rules="[v => !!v || $t('An Agency is required.')]"
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneIssueDate
              "
              :label="$t('Issue Date')"
              :rules="[v => !!v || $t('An Issue Date is required.')]"
              type="date"
              append-icon="mdi-calendar"
              clearable
            ></v-text-field>
          </v-col>
          <v-col>
            <v-text-field
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionOneNumber
              "
              :label="$t('Number')"
              :rules="[v => !!v || $t('An Issue Number is required.')]"
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row>
          <v-col>
            {{ $t('QUESTION-TWO') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwo
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwo
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwoExp
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

        <v-row>
          <v-col>
            {{ $t('QUESTION-THREE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThree
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionThree
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col>
            {{ $t('QUESTION-FOUR') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFour
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFour
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-FIVE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFive
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFive
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-SIX') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSix
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSix
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSixExp
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-SEVEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSeven
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSeven
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-EIGHT') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEight
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionEight
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col>
            {{ $t('QUESTION-NINE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionNine
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionNine
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-TEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTenExp
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-ELEVEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionEleven
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionEleven
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionTwelve
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionTwelve
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-THIRTEEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionThirteen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionThirteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-FOURTEEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFourteen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFourteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-FIFTEEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionFifteen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionFifteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col>
            {{ $t('QUESTION-SIXTEEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSixteen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSixteen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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

        <v-row>
          <v-col class="text-left">
            {{ $t('QUESTION-SEVENTEEN') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
                  .questionSeventeen
              "
              row
            >
              <v-radio
                color="primary"
                :label="$t('YES')"
                :value="true"
              />
              <v-radio
                color="primary"
                :label="$t('NO')"
                :value="false"
              />
            </v-radio-group>
          </v-col>
        </v-row>
        <v-row>
          <v-col
            v-if="
              permitStore.getPermitDetail.application.qualifyingQuestions
                .questionSeventeen
            "
          >
            <v-textarea
              outlined
              :label="$t('Please explain')"
              v-model="
                permitStore.getPermitDetail.application.qualifyingQuestions
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
      </v-card-text>
    </v-card>
  </div>
</template>

<script setup lang="ts">
import SaveButton from './SaveButton.vue'
import { usePermitsStore } from '@core-admin/stores/permitsStore'

const emit = defineEmits(['on-save'])
const permitStore = usePermitsStore()

function handleSave() {
  emit('on-save', 'Qualifying Questions')
}
</script>

<style>
::-webkit-calendar-picker-indicator {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  width: auto;
  height: auto;
  color: transparent;
  background: transparent;
}
input::-webkit-date-and-time-value {
  text-align: left;
}
</style>
