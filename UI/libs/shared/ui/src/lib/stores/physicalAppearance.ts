import { computed, ref } from 'vue';
import { defineStore } from 'pinia';
import { AppearanceInfoType } from '@shared-utils/types/defaultTypes';

export const usePhysicalAppearanceStore = defineStore(
  'physicalAppearance',
  () => {
    const physicalAppearance = ref<AppearanceInfoType>({
      gender: '',
      heightFeet: 0,
      heightInches: 0,
      weight: 0,
      hairColor: '',
      eyeColor: '',
      physicalDesc: '',
    });
    const getPhysicalAppearance = computed(() => physicalAppearance.value);

    function setPhysicalAppearance(payload: AppearanceInfoType) {
      physicalAppearance.value = payload;
    }

    function deletePhysicalAppearance() {
      physicalAppearance.value = {
        gender: '',
        heightFeet: 0,
        heightInch: 0,
        weight: 0,
        hairColor: '',
        eyeColor: '',
        physicalDesc: '',
      };
    }

    return {
      physicalAppearance,
      getPhysicalAppearance,
      setPhysicalAppearance,
      deletePhysicalAppearance,
    };
  }
);
