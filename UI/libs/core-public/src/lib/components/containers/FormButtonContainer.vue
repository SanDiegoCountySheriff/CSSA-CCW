<template>
  <div class="form-btn-container">
    <v-btn
      color="success "
      @click="handleSubmit"
      :disabled="!valid"
    >
      {{ $t('Continue') }}
    </v-btn>
    <!-- TODO: this needs to save to save the current state to local storage or call the api to save in
      the db
       -->

    <v-btn color="secondary mx-2">
      {{ $t('Save and Exit') }}
    </v-btn>

    <!-- TODO: Make this return to the home page with out saving the form at all -->
    <v-btn color="error mr-2">
      {{ $t('Cancel') }}
    </v-btn>
  </div>
</template>

<script setup lang="ts">
interface FormButtonContainerProps {
  valid?: boolean;
}

const props = withDefaults(defineProps<FormButtonContainerProps>(), {
  valid: false,
});

const emit = defineEmits(['submit']);

function handleSubmit() {
  emit('submit');
}
</script>

<style lang="scss" scoped>
.form-btn-container {
  width: 90%;
  margin-top: 0.5rem;
  display: flex;
  justify-content: flex-end;
}
</style>
