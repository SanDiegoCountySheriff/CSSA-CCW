<template>
  <div class="form-btn-container">
    <v-row>
      <v-col>
        <v-btn
          small
          color="success "
          @click="handleSubmit"
          :disabled="!valid"
          :loading="submitting"
          :class="!valid ? 'mr-3' : ''"
          class="mt-3"
        >
          {{ $t('Continue') }}
        </v-btn>

        <v-btn
          small
          color="accent mx-2"
          @click="handleSave"
          :disabled="!valid"
          :loading="submitting"
          class="mt-3"
        >
          {{ $t('Save and Exit') }}
        </v-btn>
      </v-col>
      <v-col :class="$vuetify.breakpoint.smAndDown ? '' : 'cancel-buttons'">
        <v-btn
          small
          :loading="submitting"
          color="warning"
          @click="handleBack"
          class="mt-3"
        >
          {{ $t('Go back') }}
        </v-btn>
        <v-btn
          small
          :loading="submitting"
          color="error"
          @click="handleCancel"
          :class="$vuetify.breakpoint.xs ? 'mt-3' : 'mt-3 ml-3'"
        >
          {{ $t('Cancel') }}
        </v-btn>
      </v-col>
    </v-row>
  </div>
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

<style lang="scss" scoped>
.form-btn {
  &-container {
    display: flex;
    justify-content: center;
  }
  &-inner {
    width: 90%;
    margin-top: 0.5rem;
    margin-bottom: 1rem;
    display: flex;
    justify-content: space-between;
  }
}

.cancel-buttons {
  text-align: end;
}
</style>
