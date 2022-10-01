import { AliasType } from '@shared-ui/types/defaultTypes';
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAliasStore = defineStore('alias', () => {
  const aliases = ref<AliasType>([]);
  const getAliases = computed(() => aliases.value);

  function addAlias(payload: addAlias) {
    aliases.value.push(payload);
  }

  return { aliases, getAliases, addAlias };
});
