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
          v-model="viewModel.selected"
          :rules="[viewModel.selected !== null]"
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

    <v-row
      cols="12"
      class="mx-5"
      v-if="viewModel.selected"
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
          v-model="viewModel.agency"
          :rules="[v => !!v || $t('Field cannot be blank')]"
        >
        </v-text-field>
      </v-col>

      <v-col
        cols="12"
        md="4"
      >
        <v-menu
          :v-model="menu"
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
              v-model="viewModel.denialDate"
              :label="$t('Denial Date')"
              :rules="[v => !!v || $t('Date is required')]"
              prepend-inner-icon="mdi-calendar"
              v-bind="attrs"
              v-on="on"
            ></v-text-field>
          </template>

          <v-date-picker
            v-model="viewModel.denialDate"
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
          v-model="viewModel.denialReason"
          :rules="[v => !!v || $t('Field cannot be blank')]"
        >
        </v-text-field>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { QualifyingQuestionTwo } from '@shared-utils/types/defaultTypes'
import { TranslateResult } from 'vue-i18n'
import { computed, ref } from 'vue'

const emit = defineEmits(['input'])

interface TrafficQualifyingQuestionProps {
  value: QualifyingQuestionTwo
  title: TranslateResult
}

const props = defineProps<TrafficQualifyingQuestionProps>()

const menu = ref(false)

const viewModel = computed({
  get() {
    return props.value
  },
  set(newVal) {
    emit('input', newVal)
  },
})
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
