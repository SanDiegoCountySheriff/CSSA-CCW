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
            :label="$t('YES')"
            :value="true"
            color="primary"
          />

          <v-radio
            :label="$t('NO')"
            :value="false"
            color="primary"
          />
        </v-radio-group>
      </v-col>
    </v-row>

    <v-row v-if="viewModel.selected">
      <v-col class="mx-8">
        <v-textarea
          v-model="viewModel.explanation"
          :rules="[v => !!v || $t('Field cannot be blank')]"
          :label="$t('Please explain')"
          :maxlength="maxLength"
          outlined
        >
        </v-textarea>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { QualifyingQuestionStandard } from '@shared-utils/types/defaultTypes'
import { TranslateResult } from 'vue-i18n'
import { computed } from 'vue'

const emit = defineEmits(['input'])

interface StandardQualifyingQuestionsProps {
  value: QualifyingQuestionStandard
  maxLength: number
  title: TranslateResult
}

const props = defineProps<StandardQualifyingQuestionsProps>()

const viewModel = computed({
  get() {
    return props.value
  },
  set(newVal) {
    emit('input', newVal)
  },
})
</script>
