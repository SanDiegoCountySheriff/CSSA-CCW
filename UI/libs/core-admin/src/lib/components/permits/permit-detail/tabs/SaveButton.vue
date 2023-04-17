<template>
  <v-btn
    small
    color="primary"
    :disabled="props.disabled"
    @click="handleSave"
  >
    <v-icon left> mdi-content-save </v-icon>
    Save
  </v-btn>
</template>

<script setup lang="ts">
const props = defineProps({
  disabled: Boolean,
})

const emit = defineEmits(['on-save'])

function handleSave() {
  emit('on-save')
}
</script>
