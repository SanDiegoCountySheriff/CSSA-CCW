<template>
  <v-row>
    <v-col>
      <v-btn
        small
        color="success"
        @click="handleSubmit"
        :disabled="!props.valid"
        :loading="submitting"
      >
        {{ $t('Continue') }}
      </v-btn>

      <v-btn
        small
        color="accent"
        @click="handleSave"
        :disabled="!props.valid"
        :loading="submitting"
      >
        {{ $t('Save and Exit') }}
      </v-btn>
    </v-col>
    <v-col>
      <v-btn
        small
        :loading="submitting"
        color="warning"
        @click="handleBack"
      >
        {{ $t('Go back') }}
      </v-btn>
      <v-btn
        small
        :loading="submitting"
        color="error"
        @click="handleCancel"
      >
        {{ $t('Cancel') }}
      </v-btn>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean;
  submitting?: boolean;
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
});

const emit = defineEmits(['submit', 'save', 'back', 'cancel']);

function handleSubmit() {
  emit('submit');
}

function handleSave() {
  emit('save');
}

function handleBack() {
  emit('back');
}

function handleCancel() {
  emit('cancel');
}
</script>
