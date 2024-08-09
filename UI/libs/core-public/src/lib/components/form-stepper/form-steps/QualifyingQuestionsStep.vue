<template>
  <div>
    <FormButtonContainer
      v-if="$vuetify.breakpoint.lgAndUp"
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />

    <v-container v-if="model.application.qualifyingQuestions">
      <v-row
        v-if="isRenew"
        justify="center"
        align="center"
      >
        <v-col
          cols="12"
          md="6"
        >
          <v-alert
            :class="{ 'mt-5': isMobile }"
            type="info"
            color="primary"
            dark
            outlined
            elevation="2"
          >
            Please review the qualifying questions and ensure each answer is up
            to date before proceeding
          </v-alert>
        </v-col>
      </v-row>

      <v-card-title>
        {{ $t('Qualifying Questions') }}
      </v-card-title>

      <v-form
        ref="form"
        v-model="valid"
      >
        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
          >
            {{ $t('QUESTION-ONE') }}
          </v-col>
          <v-col>
            <v-radio-group
              v-model="
                model.application.qualifyingQuestions.questionOne.selected
              "
              :rules="[
                model.application.qualifyingQuestions.questionOne.selected !==
                  null,
              ]"
              row
              :disabled="
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions &&
                !model.application.qualifyingQuestions.questionOne
                  .updateInformation
              "
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
            v-if="isRenew && !model.application.isRenewingWithLegacyQuestions"
          >
            <v-btn
              color="primary"
              @click="toggleUpdateInformation('questionOne')"
              :disabled="
                model.application.qualifyingQuestions.questionOne
                  .updateInformation
              "
            >
              Update Question 1
            </v-btn>
          </v-col>
        </v-row>

        <v-row
          v-if="model.application.qualifyingQuestions.questionOne.selected"
          class="mx-5"
        >
          <v-col
            cols="12"
            md="3"
          >
            <v-text-field
              v-model="model.application.qualifyingQuestions.questionOne.agency"
              :label="$t('Issuing Agency')"
              :rules="[v => !!v || $t('Field cannot be blank')]"
              :disabled="
                !model.application.qualifyingQuestions.questionOne
                  .updateInformation &&
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions
              "
              color="primary"
              maxlength="50"
              outlined
              dense
            >
            </v-text-field>
          </v-col>

          <v-col
            cols="12"
            md="3"
          >
            <v-text-field
              outlined
              dense
              color="primary"
              maxlength="50"
              :label="$t('Issuing State')"
              v-model="
                model.application.qualifyingQuestions.questionOne.issuingState
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
              :disabled="
                !model.application.qualifyingQuestions.questionOne
                  .updateInformation &&
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions
              "
            >
            </v-text-field>
          </v-col>

          <v-col
            cols="12"
            md="3"
          >
            <v-menu
              :v-model="state.menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  outlined
                  dense
                  readonly
                  v-model="
                    model.application.qualifyingQuestions.questionOne.issueDate
                  "
                  :label="$t('Issue Date')"
                  :rules="[v => !!v || $t('Date is required')]"
                  :disabled="
                    !model.application.qualifyingQuestions.questionOne
                      .updateInformation &&
                    isRenew &&
                    !model.application.isRenewingWithLegacyQuestions
                  "
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="
                  model.application.qualifyingQuestions.questionOne.issueDate
                "
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col
            cols="12"
            md="3"
          >
            <v-text-field
              v-model="model.application.qualifyingQuestions.questionOne.number"
              :rules="[v => !!v || $t('Field cannot be blank')]"
              :disabled="
                !model.application.qualifyingQuestions.questionOne
                  .updateInformation &&
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions
              "
              :label="$t('CCW number')"
              color="primary"
              maxlength="50"
              outlined
              dense
            >
            </v-text-field>
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
          <v-col>
            <v-radio-group
              v-model="
                model.application.qualifyingQuestions.questionTwo.selected
              "
              :rules="[
                model.application.qualifyingQuestions.questionTwo.selected !==
                  null,
              ]"
              row
              :disabled="
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions &&
                !model.application.qualifyingQuestions.questionTwo
                  .updateInformation
              "
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
          <v-col>
            <v-btn
              v-if="isRenew && !model.application.isRenewingWithLegacyQuestions"
              color="primary"
              @click="toggleUpdateInformation('questionTwo')"
              :disabled="
                model.application.qualifyingQuestions.questionTwo
                  .updateInformation
              "
            >
              Update Question 2
            </v-btn>
          </v-col>
        </v-row>

        <v-row
          cols="12"
          class="mx-5"
          v-if="model.application.qualifyingQuestions.questionTwo.selected"
        >
          <v-col
            cols="12"
            md="4"
          >
            <v-text-field
              outlined
              dense
              color="primary"
              maxlength="50"
              :label="$t('Agency Name')"
              v-model="model.application.qualifyingQuestions.questionTwo.agency"
              :rules="[v => !!v || $t('Field cannot be blank')]"
              :disabled="
                !model.application.qualifyingQuestions.questionTwo
                  .updateInformation &&
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions
              "
            >
            </v-text-field>
          </v-col>
          <v-col
            cols="12"
            md="4"
          >
            <v-menu
              :v-model="state.menu"
              :close-on-content-click="false"
              transition="scale-transition"
              offset-y
              min-width="auto"
            >
              <template #activator="{ on, attrs }">
                <v-text-field
                  outlined
                  dense
                  readonly
                  v-model="
                    model.application.qualifyingQuestions.questionTwo.denialDate
                  "
                  :label="$t('Denial Date')"
                  :rules="[v => !!v || $t('Date is required')]"
                  :disabled="
                    !model.application.qualifyingQuestions.questionTwo
                      .updateInformation &&
                    isRenew &&
                    !model.application.isRenewingWithLegacyQuestions
                  "
                  prepend-inner-icon="mdi-calendar"
                  v-bind="attrs"
                  v-on="on"
                ></v-text-field>
              </template>
              <v-date-picker
                v-model="
                  model.application.qualifyingQuestions.questionTwo.denialDate
                "
                no-title
                scrollable
              >
              </v-date-picker>
            </v-menu>
          </v-col>
          <v-col
            cols="12"
            md="4"
          >
            <v-text-field
              outlined
              dense
              color="primary"
              maxlength="50"
              :label="$t('Reason for denial')"
              v-model="
                model.application.qualifyingQuestions.questionTwo.denialReason
              "
              :rules="[v => !!v || $t('Field cannot be blank')]"
              :disabled="
                !model.application.qualifyingQuestions.questionTwo
                  .updateInformation &&
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions
              "
            >
            </v-text-field>
          </v-col>
        </v-row>

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionThree"
          :max-length="config.appConfig.questions.three"
          :title="$t('QUESTION-THREE')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="3"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionFour"
          :max-length="config.appConfig.questions.four"
          :title="$t('QUESTION-FOUR')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="4"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionFive"
          :max-length="config.appConfig.questions.five"
          :title="$t('QUESTION-FIVE')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="5"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionSix"
          :max-length="config.appConfig.questions.six"
          :title="$t('QUESTION-SIX')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="6"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionSeven"
          :max-length="config.appConfig.questions.seven"
          :title="$t('QUESTION-SEVEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="7"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionEight"
          :max-length="config.appConfig.questions.eight"
          :title="$t('QUESTION-EIGHT')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="8"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionNine"
          :max-length="config.appConfig.questions.nine"
          :title="$t('QUESTION-NINE')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="9"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionTen"
          :max-length="config.appConfig.questions.ten"
          :title="$t('QUESTION-TEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="10"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionEleven"
          :max-length="config.appConfig.questions.eleven"
          :title="$t('QUESTION-ELEVEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="11"
        />

        <v-row class="ml-5">
          <v-col
            cols="12"
            lg="6"
            class="text-left"
          >
            {{ $t('QUESTION-TWELVE') }}
          </v-col>
          <v-col>
            <v-radio-group
              :rules="[
                model.application.qualifyingQuestions.questionTwelve
                  .selected !== null,
              ]"
              v-model="
                model.application.qualifyingQuestions.questionTwelve.selected
              "
              @change="handleChangeQuestionTwelve"
              row
              :disabled="
                isRenew &&
                !model.application.isRenewingWithLegacyQuestions &&
                !model.application.qualifyingQuestions.questionTwelve
                  .updateInformation
              "
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
          <v-col>
            <v-btn
              v-if="isRenew && !model.application.isRenewingWithLegacyQuestions"
              color="primary"
              @click="toggleUpdateInformation('questionTwelve')"
              :disabled="
                model.application.qualifyingQuestions.questionTwelve
                  .updateInformation
              "
            >
              Update Question 12
            </v-btn>
          </v-col>
        </v-row>

        <template
          v-if="model.application.qualifyingQuestions.questionTwelve.selected"
        >
          <v-row
            class="mx-5"
            v-for="index of model.application.qualifyingQuestions.questionTwelve
              .trafficViolations?.length"
            :key="index"
          >
            <v-col
              cols="12"
              md="3"
            >
              <v-menu
                v-model="menu[index]"
                :close-on-content-click="false"
                transition="scale-transition"
                offset-y
                min-width="auto"
              >
                <template #activator="{ on, attrs }">
                  <v-text-field
                    v-model="
                      model.application.qualifyingQuestions.questionTwelve
                        .trafficViolations[index - 1].date
                    "
                    :label="$t('Date')"
                    :rules="[v => !!v || $t('Date is required')]"
                    dense
                    outlined
                    hint="YYYY-MM-DD format"
                    prepend-inner-icon="mdi-calendar"
                    v-bind="attrs"
                    v-on="on"
                    :disabled="
                      isRenew &&
                      !model.application.qualifyingQuestions.questionTwelve
                        .updateInformation &&
                      !model.application.isRenewingWithLegacyQuestions
                    "
                  ></v-text-field>
                </template>
                <v-date-picker
                  v-model="
                    model.application.qualifyingQuestions.questionTwelve
                      .trafficViolations[index - 1].date
                  "
                  color="primary"
                  no-title
                  scrollable
                >
                </v-date-picker>
              </v-menu>
            </v-col>
            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="
                  model.application.qualifyingQuestions.questionTwelve
                    .trafficViolations[index - 1].violation
                "
                dense
                outlined
                label="Violation/Accident"
                :rules="[v => !!v || $t('Violation is required')]"
                :disabled="
                  isRenew &&
                  !model.application.qualifyingQuestions.questionTwelve
                    .updateInformation &&
                  !model.application.isRenewingWithLegacyQuestions
                "
              ></v-text-field>
            </v-col>
            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="
                  model.application.qualifyingQuestions.questionTwelve
                    .trafficViolations[index - 1].agency
                "
                :rules="[v => !!v || $t('Agency is required')]"
                :disabled="
                  isRenew &&
                  !model.application.qualifyingQuestions.questionTwelve
                    .updateInformation &&
                  !model.application.isRenewingWithLegacyQuestions
                "
                dense
                outlined
                label="Agency"
              ></v-text-field>
            </v-col>
            <v-col
              cols="12"
              md="3"
            >
              <v-text-field
                v-model="
                  model.application.qualifyingQuestions.questionTwelve
                    .trafficViolations[index - 1].citationNumber
                "
                :rules="[v => !!v || $t('Citation number is required')]"
                :disabled="
                  isRenew &&
                  !model.application.qualifyingQuestions.questionTwelve
                    .updateInformation &&
                  !model.application.isRenewingWithLegacyQuestions
                "
                dense
                outlined
                label="Citation Number"
                hint="If unknown please enter unknown"
              ></v-text-field>
            </v-col>
          </v-row>
        </template>

        <v-row
          v-if="model.application.qualifyingQuestions.questionTwelve.selected"
        >
          <v-col>
            <v-btn
              @click="addTrafficViolation"
              color="primary"
              class="mr-3 ml-5"
            >
              <v-icon left>mdi-plus</v-icon>Add
            </v-btn>
            <v-btn
              @click="removeTrafficViolation"
              color="primary"
              :disabled="
                model.application.qualifyingQuestions.questionTwelve
                  .trafficViolations?.length < 2
              "
            >
              <v-icon left>mdi-minus</v-icon>Remove
            </v-btn>
          </v-col>
        </v-row>

        <v-row
          v-if="
            model.application.qualifyingQuestions.questionTwelve
              .trafficViolationsExplanation
          "
        >
          <v-col class="mx-8">
            <v-textarea
              v-model="
                model.application.qualifyingQuestions.questionTwelve
                  .trafficViolationsExplanation
              "
              color="primary"
              dense
              outlined
              label="Traffic Violations Explanation"
              hint="Please transcribe these violations into the form above.  Click 'Add' to add a traffic violation."
              persistent-hint
            >
            </v-textarea>
          </v-col>
        </v-row>

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionThirteen"
          :max-length="config.appConfig.questions.thirteen"
          :title="$t('QUESTION-THIRTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="13"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionFourteen"
          :max-length="config.appConfig.questions.fourteen"
          :title="$t('QUESTION-FOURTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="14"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionFifteen"
          :max-length="config.appConfig.questions.fifteen"
          :title="$t('QUESTION-FIFTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="15"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionSixteen"
          :max-length="config.appConfig.questions.sixteen"
          :title="$t('QUESTION-SIXTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="16"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionSeventeen"
          :max-length="config.appConfig.questions.seventeen"
          :title="$t('QUESTION-SEVENTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="17"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionEighteen"
          :max-length="config.appConfig.questions.eighteen"
          :title="$t('QUESTION-EIGHTEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="18"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionNineteen"
          :max-length="config.appConfig.questions.nineteen"
          :title="$t('QUESTION-NINETEEN')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="19"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionTwenty"
          :max-length="config.appConfig.questions.twenty"
          :title="$t('QUESTION-TWENTY')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="20"
        />

        <StandardQualifyingQuestion
          v-model="model.application.qualifyingQuestions.questionTwentyOne"
          :max-length="config.appConfig.questions.twentyone"
          :title="$t('QUESTION-TWENTYONE')"
          :is-renew="isRenew"
          :is-renewing-with-legacy-questions="
            model.application.isRenewingWithLegacyQuestions
          "
          @toggle-update-information="toggleUpdateInformation"
          question-number="21"
        />
      </v-form>
    </v-container>

    <FormButtonContainer
      :valid="valid"
      @continue="handleContinue"
      @save="handleSave"
      v-on="$listeners"
    />
  </div>
</template>

<script setup lang="ts">
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import StandardQualifyingQuestion from '@core-public/components/qualifying-questions/StandardQualifyingQuestion.vue'
import { useAppConfigStore } from '@shared-ui/stores/configStore'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import {
  ApplicationType,
  CompleteApplication,
} from '@shared-utils/types/defaultTypes'
import { computed, onMounted, reactive, ref, watch } from 'vue'

interface IProps {
  value: CompleteApplication
}

const props = defineProps<IProps>()
const emit = defineEmits([
  'input',
  'handle-continue',
  'handle-save',
  'update-step-seven-valid',
])

const model = computed({
  get: () => props.value,
  set: (value: CompleteApplication) => emit('input', value),
})

const menu = ref([false])
const form = ref()
const valid = ref(false)
const config = useAppConfigStore()
const vuetify = useVuetify()
const state = reactive({
  menu: false,
})

const isRenew = computed(() => {
  const applicationType = model.value.application.applicationType

  return (
    applicationType === ApplicationType['Renew Standard'] ||
    applicationType === ApplicationType['Renew Reserve'] ||
    applicationType === ApplicationType['Renew Judicial'] ||
    applicationType === ApplicationType['Renew Employment']
  )
})

const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

onMounted(() => {
  if (form.value) {
    form.value.validate()
  }
})

function handleContinue() {
  if (isRenew) {
    appendRenewalExplanation()
  }

  emit('update-step-seven-valid', true)
  emit('handle-continue')
}

function handleSave() {
  if (isRenew) {
    appendRenewalExplanation()
  }

  emit('update-step-seven-valid', true)
  emit('handle-save')
}

function toggleUpdateInformation(questionKey: string) {
  if (model.value.application.qualifyingQuestions) {
    const question = model.value.application.qualifyingQuestions[questionKey]

    question.updateInformation = true
    question.selected = true
  }
}

function appendRenewalExplanation() {
  const questions = model.value.application.qualifyingQuestions

  for (const key in questions) {
    if (Object.prototype.hasOwnProperty.call(questions, key)) {
      const question = questions[key]

      if (question.updateInformation) {
        if (question.explanation && question.renewalExplanation) {
          question.explanation += `\n${question.renewalExplanation}`
        } else if (question.renewalExplanation) {
          question.explanation = question.renewalExplanation
        }

        question.renewalExplanation = ''
        question.updateInformation = false
      }
    }
  }
}

function addTrafficViolation() {
  menu.value.push(false)
  model.value.application.qualifyingQuestions?.questionTwelve.trafficViolations.push(
    {
      date: '',
      violation: '',
      agency: '',
      citationNumber: '',
    }
  )
}

function removeTrafficViolation() {
  menu.value.pop()
  model.value.application.qualifyingQuestions?.questionTwelve.trafficViolations.pop()
}

function handleChangeQuestionTwelve() {
  if (model.value.application.qualifyingQuestions?.questionTwelve.selected) {
    model.value.application.qualifyingQuestions?.questionTwelve.trafficViolations.push(
      {
        date: '',
        violation: '',
        agency: '',
        citationNumber: '',
      }
    )
  } else if (model.value.application.qualifyingQuestions) {
    model.value.application.qualifyingQuestions.questionTwelve.trafficViolations =
      []
  }
}

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-seven-valid', newValue)
  }
})
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
