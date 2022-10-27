<template>
  <v-autocomplete
    v-model="model"
    :loading="isLoading"
    :search-input.sync="search"
    hide-no-data
    hide-selected
    item-text="Description"
    item-value="API"
    label="Search"
    placeholder="Start typing to Search"
    prepend-icon="mdi-magnify"
  ></v-autocomplete>
</template>
<script setup lang="ts">
import { ref } from 'vue';

const model = ref(null);
const search = ref(null);
const isLoading = ref(false);
</script>
