import { defineStore } from 'pinia';
import { computed, reactive } from 'vue';

export const useCurrentInfoSection = defineStore('currentInfoSection', () => {
  const state = reactive({
    selection: 0,
  });

  const getCurrentInfoSection = computed(() => state);

  function setCurrentInfoSection(payload: number) {
    state.selection = payload;
  }

  return {
    state,
    getCurrentInfoSection,
    setCurrentInfoSection,
  };
});
