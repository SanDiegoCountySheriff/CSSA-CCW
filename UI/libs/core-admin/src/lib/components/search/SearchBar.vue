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
      :filter="filterData"
      placeholder="Start typing to search"
      prepend-icon="mdi-magnify"
      no-data-text="No results found..."
      clearable
      hide-selected
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
                :color="
                  $vuetify.theme.dark ? '' : item.isComplete ? 'blue' : 'error'
                "
                class="ml-4 white--text"
              >
                {{ item.isComplete ? 'Ready for review' : 'Incomplete' }}
              </v-chip>

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
      <template #selection="{}"> </template>
    </v-autocomplete>
    <v-expand-transition>
      <v-list
        v-if="state.model"
        class="red lighten-3"
      >
        <v-list-item
          v-for="(field, i) in fields"
          :key="i"
        >
          <v-list-item-content>
            <v-list-item-title v-text="field.value"></v-list-item-title>
            <v-list-item-subtitle v-text="field.key"></v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-expand-transition>
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

const fields = computed(() => {
  if (!state.model) return [];

  return Object.keys(state.model).map(key => {
    return {
      key,
      value: state.model[key] || 'n/a',
    };
  });
});

const items = computed(() => {
  return state.entries.map(entry => {
    const Name = entry?.firstName || '';

    return { ...entry, Name };
  });
});

function filterData(item, queryText) {
  return (
    item.dob.birthDate.toLowerCase().includes(queryText.toLowerCase()) ||
    item.address.addressLine1.toLowerCase().includes(queryText.toLowerCase()) ||
    item.firstName.toLowerCase().includes(queryText.toLowerCase()) ||
    item.lastName.toLowerCase().includes(queryText.toLowerCase()) ||
    item.email.toLowerCase().includes(queryText.toLowerCase()) ||
    item.address.zip.toLowerCase().includes(queryText.toLowerCase())
  );
}

watch(search, async () => {
  // Items have already been loaded
  if (search.value && search.value.length > 2) {
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
