<template>
  <div>
    <v-form
      ref="form"
      v-model="valid"
    >
      <v-card-title v-if="!isMobile">Modify Address</v-card-title>

      <v-card-subtitle v-else>Modify Address</v-card-subtitle>

      <v-card-text>
        <v-row>
          <v-col
            md="3"
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
            md="3"
            cols="12"
          >
            <v-text-field
              :value="application.application.currentAddress.streetAddress"
              :dense="isMobile"
              label="Current Street Address"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              :value="application.application.currentAddress.city"
              :dense="isMobile"
              label="Current City"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              :value="application.application.currentAddress.county"
              :dense="isMobile"
              label="Current County"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              :value="application.application.currentAddress.zip"
              :dense="isMobile"
              label="Current Zip Code"
              color="primary"
              outlined
              readonly
            ></v-text-field>
          </v-col>
        </v-row>

        <v-row v-if="modify">
          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              v-model="updatedAddress.streetAddress"
              :dense="isMobile"
              :rules="addressRules"
              label="Updated Street Address"
              outlined
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              v-model="updatedAddress.city"
              :dense="isMobile"
              label="Updated City"
              outlined
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              v-model="updatedAddress.county"
              :dense="isMobile"
              :rules="addressRules"
              label="Updated County"
              outlined
            ></v-text-field>
          </v-col>

          <v-col
            md="3"
            cols="12"
          >
            <v-text-field
              v-model="updatedAddress.zip"
              :dense="isMobile"
              :rules="addressRules"
              label="Updated Zip Code"
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
import { computed, onMounted, reactive, ref } from 'vue'

interface ModifyNameProps {
  value: boolean
  application: CompleteApplication
}

const props = defineProps<ModifyNameProps>()

const emit = defineEmits(['handle-continue', 'handle-save', 'input'])

const vuetify = useVuetify()
const updatedAddress = reactive({
  streetAddress: '',
  city: '',
  county: '',
  zip: '',
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
  if (
    props.application.application.modifiedAddress.streetAddress !== '' ||
    props.application.application.modifiedAddress.city !== '' ||
    props.application.application.modifiedAddress.county !== '' ||
    props.application.application.modifiedAddress.zip
  ) {
    updatedAddress.streetAddress =
      props.application.application.modifiedAddress.streetAddress
    updatedAddress.city = props.application.application.modifiedAddress.city
    updatedAddress.county = props.application.application.modifiedAddress.county
    updatedAddress.zip = props.application.application.modifiedAddress.zip
    modify.value = true
  }
})

const addressRules = computed(() => {
  return [(v: string) => Boolean(v) || 'Address is required.']
})

const isMobile = computed(
  () => vuetify?.breakpoint.name === 'sm' || vuetify?.breakpoint.name === 'xs'
)

function handleContinue() {
  emit('handle-continue', updatedAddress)
}

function handleSave() {
  emit('handle-save', updatedAddress)
}
</script>
