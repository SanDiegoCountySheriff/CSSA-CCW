<template>
  <div class="acknowledgement-button-container">
    <v-btn
      color="success mr-3"
      @click="handleAccept"
    >
      {{ $t('Accept') }}
    </v-btn>
    <v-btn
      color="error mr-3"
      @click="handleDecline"
    >
      {{ $t('Decline') }}
    </v-btn>
  </div>
</template>

<script setup lang="ts">
const emit = defineEmits(['accept', 'decline'])

function handleAccept() {
  emit('accept')
}

function handleDecline() {
  emit('decline')
}
</script>

<style lang="scss" scoped>
.acknowledgement-button-container {
  display: flex;
  flex-direction: row;
  width: 100%;
  justify-content: flex-end;
}
</style>
