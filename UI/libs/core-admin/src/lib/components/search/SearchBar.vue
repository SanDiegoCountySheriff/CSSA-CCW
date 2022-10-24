<template>
  <v-autocomplete
    v-model="model"
    :loading="isLoading"
    :search-input.sync="search"
    color="white"
    hide-no-data
    hide-selected
    placeholder="Search anything..."
    prepend-icon="mdi-magnify"
    return-object
  ></v-autocomplete>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const model = ref(null);
const search = ref(null);
const isLoading = ref(false);
</script>
