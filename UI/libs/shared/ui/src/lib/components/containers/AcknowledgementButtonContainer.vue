<template>
  <v-card-actions>
    <v-btn
      text
      absolute
      bottom
      color="error"
      @click="handleDecline"
    >
      {{ $t('Decline') }}
    </v-btn>
    <v-btn
      text
      absolute
      bottom
      right
      color="success"
      @click="handleAccept"
    >
      {{ $t('Accept') }}
    </v-btn>
  </v-card-actions>
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
