import { defineStore } from 'pinia';
import { reactive } from 'vue';

export const useAppInfoStep = defineStore('appInfoStep', () => {
  const state = reactive({
    step: 0,
  });

  return state;
});
