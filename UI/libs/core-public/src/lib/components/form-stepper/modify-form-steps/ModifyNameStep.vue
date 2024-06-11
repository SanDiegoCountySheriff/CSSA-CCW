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
              :rules="nameRules"
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
              :rules="nameRules"
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
        v-on="$listeners"
      />
    </v-form>
  </div>
</template>

<script lang="ts" setup>
import { CompleteApplication } from '@shared-utils/types/defaultTypes'
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import { useVuetify } from '@shared-ui/composables/useVuetify'
import { computed, onMounted, reactive, ref, watch } from 'vue'

interface ModifyNameProps {
  value: boolean
  application: CompleteApplication
}

const props = defineProps<ModifyNameProps>()

const emit = defineEmits([
  'handle-continue',
  'handle-save',
  'input',
  'update-step-two-valid',
])

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

onMounted(() => {
  updateModificationStatus()
})

const nameRules = computed(() => {
  return [(v: string) => Boolean(v) || 'Name is required.']
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

function updateModificationStatus() {
  if (
    props.application.application.personalInfo.modifiedFirstName !== '' ||
    props.application.application.personalInfo.modifiedLastName !== '' ||
    props.application.application.personalInfo.modifiedMiddleName !== ''
  ) {
    updatedName.firstName =
      props.application.application.personalInfo.modifiedFirstName
    updatedName.lastName =
      props.application.application.personalInfo.modifiedLastName
    updatedName.middleName =
      props.application.application.personalInfo.modifiedMiddleName
    modify.value = true
  }
}

watch(valid, (newValue, oldValue) => {
  if (newValue !== oldValue) {
    emit('update-step-two-valid', newValue)
  }
})

watch(modify, newValue => {
  if (!newValue) {
    updatedName.firstName = ''
    updatedName.lastName = ''
    updatedName.middleName = ''
  }
})

watch(props.application, () => {
  updateModificationStatus()
})
</script>
