import { computed, ref } from 'vue';
import { defineStore } from 'pinia';

export const useWeaponsStore = defineStore('weapons', () => {
  const weapons = ref([]);
  const getWeapons = computed(() => weapons.value);

  function setWeapons(payload) {
    weapons.value = payload;
  }

  function addWeapons(weapon) {
    weapons.value.unshift(weapon);
  }

  function updateWeapons(weapon) {
    const index = weapons.value.find(weapon);
    weapons.value.splice(index, 1, weapons.value);
  }

  function deleteWeapons(weapon) {
    const index = weapons.value.find(weapon);
    weapons.value.splice(index, 1);
  }

  return {
    weapons,
    getWeapons,
    addWeapons,
    setWeapons,
    updateWeapons,
    deleteWeapons,
  };
});
