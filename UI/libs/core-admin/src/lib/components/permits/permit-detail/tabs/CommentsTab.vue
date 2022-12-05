<template>
  <v-textarea
    fluid
    fill-height
    auto-grow
    clearable
    :rules="[
      v => (v && v.length <= 1000) || $t('Maximum 1000 characters are allowed'),
    ]"
    label="Interview questions / Work comments"
  />
</template>
