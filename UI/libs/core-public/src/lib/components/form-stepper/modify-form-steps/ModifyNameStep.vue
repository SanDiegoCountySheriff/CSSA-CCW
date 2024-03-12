<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title v-if="!isMobile">Modify Name</v-card-title>

      <v-card-subtitle v-else>Modify Name</v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="4"
            cols="12"
          >
            <v-switch
              v-model="modify"
              label="Are you modifying this information?"
            ></v-switch>
          </v-col>
        </v-row>

        <v-row>
          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              :value="application.application.personalInfo.firstName"
              :dense="isMobile"
              label="Current First Name"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>

          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              :value="application.application.personalInfo.middleName"
              :dense="isMobile"
              label="Current Middle Name"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>

          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              :value="application.application.personalInfo.lastName"
              :dense="isMobile"
              label="Current Last Name"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row v-if="modify">
          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              v-model="updatedName.firstName"
              :dense="isMobile"
              label="Updated First Name"
              outlined
            ></v-text-field>
          </v-col>

          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              v-model="updatedName.middleName"
              :dense="isMobile"
              label="Updated Middle Name"
              outlined
            ></v-text-field>
          </v-col>

          <v-col
            md="4"
            cols="12"
          >
            <v-text-field
              v-model="updatedName.lastName"
              :dense="isMobile"
              label="Updated Last Name"
              outlined
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <FormButtonContainer
        :valid="valid"
        @continue="handleContinue"
        @save="handleSave"
      />
    </v-form>
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, reactive, ref } from 'vue'

interface ModifyNameProps {
  value: boolean
  application: CompleteApplication
}

const props = defineProps<ModifyNameProps>()

const emit = defineEmits(['handle-continue', 'handle-save', 'input'])

const vuetify = useVuetify()
const updatedName = reactive({
  firstName: '',
  middleName: '',
  lastName: '',
})
const form = ref()
const valid = ref(false)
const modify = computed({
  get() {
    return props.value
  },
  set(value) {
    emit('input', value)
  },
})

const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

function handleContinue() {
  emit('handle-continue', updatedName)
}

function handleSave() {
  emit('handle-save', updatedName)
}
</script>
