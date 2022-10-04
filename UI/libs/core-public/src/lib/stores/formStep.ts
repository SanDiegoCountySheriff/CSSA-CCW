import { computed, reactive } from 'vue';
import { defineStore } from 'pinia';

export const useFormStep = defineStore('formStep', () => {
  const formStep = reactive({
    step: 1,
  });
  const getFormStep = computed(() => formStep.step);
  const setFormStep = (payload: number) => {
    formStep.step = payload;
  };

  return { getFormStep, setFormStep };
});
