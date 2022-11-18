<template>
  <div class="form-btn-container">
    <div class="form-btn-inner">
      <div>
        <v-btn
          small
          color="success "
          @click="handleSubmit"
          :disabled="!valid"
          class="mt-3"
        >
          {{ $t('Continue') }}
        </v-btn>

        <v-btn
          small
          color="accent mx-2"
          @click="handleSave"
          class="mt-3"
        >
          {{ $t('Save and Exit') }}
        </v-btn>
      </div>
      <div>
        <v-btn
          small
          color="warning mx-2"
          @click="handleBack"
          class="mt-3"
        >
          {{ $t('Go back') }}
        </v-btn>

        <v-btn
          small
          color="error mr-2"
          @click="handleCancel"
          class="mt-3"
        >
          {{ $t('Cancel') }}
        </v-btn>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean;
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
</style>
