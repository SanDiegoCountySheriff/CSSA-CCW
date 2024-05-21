<template>
  <div>
    <v-row class="ml-5">
      <v-col
        cols="12"
        lg="6"
        class="text-left"
      >
        {{ props.title }}
      </v-col>

      <v-col>
        <v-radio-group
          :rules="[viewModel.selected !== null]"
          v-model="viewModel.selected"
          @change="handleChangeQuestionTwelve"
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
    </v-row>

    <template v-if="viewModel.selected">
      <v-row
        class="mx-5"
        v-for="index of viewModel.trafficViolations?.length"
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
                v-model="viewModel.trafficViolations[index - 1].date"
                :label="$t('Date')"
                :rules="[v => !!v || $t('Date is required')]"
                dense
                outlined
                hint="YYYY-MM-DD format"
                prepend-inner-icon="mdi-calendar"
                v-bind="attrs"
                v-on="on"
              ></v-text-field>
            </template>

            <v-date-picker
              v-model="viewModel.trafficViolations[index - 1].date"
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
            v-model="viewModel.trafficViolations[index - 1].violation"
            dense
            outlined
            label="Violation/Accident"
            :rules="[v => !!v || $t('Violation is required')]"
          ></v-text-field>
        </v-col>

        <v-col
          cols="12"
          md="3"
        >
          <v-text-field
            v-model="viewModel.trafficViolations[index - 1].agency"
            :rules="[v => !!v || $t('Agency is required')]"
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
            v-model="viewModel.trafficViolations[index - 1].citationNumber"
            :rules="[v => !!v || $t('Citation number is required')]"
            dense
            outlined
            label="Citation Number"
            hint="If unknown please enter unknown"
          ></v-text-field>
        </v-col>
      </v-row>
    </template>

    <v-row v-if="viewModel.selected">
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
          :disabled="viewModel.trafficViolations?.length < 2"
        >
          <v-icon left>mdi-minus</v-icon>Remove
        </v-btn>
      </v-col>
    </v-row>

    <v-row v-if="viewModel.trafficViolationsExplanation">
      <v-col class="mx-8">
        <v-textarea
          v-model="viewModel.trafficViolationsExplanation"
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
  </div>
</template>

<script setup lang="ts">
import { QualifyingQuestionTwelve } from '@shared-utils/types/defaultTypes'
import { TranslateResult } from 'vue-i18n'
import { computed, ref } from 'vue'

const emit = defineEmits(['input'])

interface TrafficQualifyingQuestionProps {
  value: QualifyingQuestionTwelve
  title: TranslateResult
}

const props = defineProps<TrafficQualifyingQuestionProps>()

const menu = ref([false])

const viewModel = computed({
  get() {
    return props.value
  },
  set(newVal) {
    emit('input', newVal)
  },
})

function addTrafficViolation() {
  menu.value.push(false)
  viewModel.value.trafficViolations.push({
    date: '',
    violation: '',
    agency: '',
    citationNumber: '',
  })
}

function removeTrafficViolation() {
  menu.value.pop()
  viewModel.value.trafficViolations.pop()
}

function handleChangeQuestionTwelve() {
  window.console.log('selected', viewModel.value.selected)

  if (viewModel.value.selected) {
    viewModel.value.trafficViolations.push({
      date: '',
      violation: '',
      agency: '',
      citationNumber: '',
    })
  } else {
    viewModel.value.trafficViolations = []
  }
}
</script>

<style scoped>
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
