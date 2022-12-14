<!-- eslint-disable vue/singleline-html-element-content-newline -->
<template>
  <div>
    <v-autocomplete
      v-model="state.model"
      class="search"
      :items="items"
      :loading="state.isLoading"
      :search-input.sync="search"
      item-text="Name"
      item-value="orderID"
      placeholder="Start typing to search"
      prepend-icon="mdi-magnify"
      no-data-text="No results found..."
      clearable
      append-icon=""
      return-object
    >
      <template #item="{ item }">
        <router-link
          :to="{
            name: 'PermitDetail',
            params: { orderId: item.orderID },
          }"
          tag="a"
          target="_self"
          style="text-decoration: none; color: inherit"
          replace
        >
          <v-row>
            <v-col>
              {{ item.Name }}, {{ item.dob.birthDate }},
              <v-chip
                medium
                label
                color="primary lighten-2"
                class="ml-4"
              >
                {{ item.orderID }}
              </v-chip>
            </v-col>
          </v-row>
        </router-link>
      </template>
    </v-autocomplete>
  </div>
</template>
<script setup lang="ts">
import { usePermitsStore } from '@core-admin/stores/permitsStore';
import { computed, reactive, ref, watch } from 'vue';

const permitStore = usePermitsStore();

const state = reactive({
  entries: [],
  isLoading: false,
  model: null,
});

const search = ref(null);

const items = computed(() => {
  return state.entries.map(entry => {
    const Name = entry?.firstName || '';

    return { ...entry, Name };
  });
});

watch(search, async () => {
  // Items have already been loaded
  if (search.value && search.value.length > 3) {
    // Items have already been requested
    if (state.isLoading) return;

    state.isLoading = true;

    // Lazily load input items
    await permitStore.getPermitSearchApi(search.value);
    state.entries = permitStore.getSearchResults;

    state.isLoading = false;
  } else {
    permitStore.setSearchResults([]);
    state.entries = [];
  }
});
</script>
<style lang="scss" scoped>
.v-menu__content {
  z-index: 99999;
}
.v-list-item {
  position: absolute;
  bottom: 20px;

  &__icon {
    margin: 0;
  }

  &__content {
    padding: 0;
    margin-left: 9px;
  }

  &__title {
    text-align: left;
  }

  &__subtitle {
    text-align: left;
    text-overflow: ellipsis;
    font-size: 0.76rem;
  }
}
</style>
