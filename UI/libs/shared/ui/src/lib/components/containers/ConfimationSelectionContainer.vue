<template>
  <v-container fluid>
    <div class="confirm-button-container">
      <v-btn
        color="success mr-3"
        @click="handleConfirm"
      >
        {{ $t('confirm') }}
      </v-btn>
      <v-btn
        color="error mr-5"
        @click="handleCancel"
      >
        {{ $t('cancel') }}
      </v-btn>
    </div>
  </v-container>
</template>

<script setup lang="ts">
const emit = defineEmits(['confirm', 'cancel']);

function handleConfirm() {
  emit('confirm');
}

function handleCancel() {
  emit('cancel');
}
</script>

<style lang="scss" scoped>
.confirm-button-container {
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: flex-end;
}
</style>
