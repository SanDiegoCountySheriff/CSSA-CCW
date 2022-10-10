import { AliasType } from '@shared-utils/types/defaultTypes';
import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

export const useAliasStore = defineStore('alias', () => {
  const aliases = ref<Array<AliasType>>([]);
  const getAliases = computed(() => aliases.value);

  function addAlias(payload: Array<AliasType>) {
    aliases.value = payload;
  }

  return { aliases, getAliases, addAlias };
});
