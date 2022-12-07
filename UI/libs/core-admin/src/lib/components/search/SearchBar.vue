<!-- eslint-disable vue/singleline-html-element-content-newline -->
<template>
  <div>
    <v-autocomplete
      v-model="state.model"
      class="search"
      :items="items"
      :loading="state.isLoading"
      :search-input.sync="search"
      hide-no-data
      hide-selected
      item-text="Name"
      item-value="orderID"
      placeholder="Start typing to search"
      prepend-icon="mdi-magnify"
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
              {{ item.Name }}
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
    const Name = entry?.name || '';

    return { ...entry, Name };
  });
});

watch(search, () => {
  // Items have already been loaded
  if (items.value.length > 0) return;

  // Items have already been requested
  if (state.isLoading) return;

  state.isLoading = true;

  // Lazily load input items
  state.entries = permitStore.getPermits;

  state.isLoading = false;
});
</script>
<style lang="scss" scoped>
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
