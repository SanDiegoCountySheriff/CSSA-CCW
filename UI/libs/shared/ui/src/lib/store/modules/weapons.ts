export default {
  state: {
    weapons: [],
  },
  getters: {
    weapons: state => state.weapons,
  },
  mutations: {
    GET_WEAPONS(state, weapons) {
      state.weapons = weapons;
    },
    ADD_WEAPONS(state, weapon) {
      state.weapons.unshift(weapon);
    },
    UPDATE_WEAPONS(state, weapon, payload) {
      const index = state.weapons.find(weapon);
      state.weapons.splice(index, 1, payload);
    },
    DELETE_WEAPONS(state, weapon) {
      const index = state.state.weapons.find(weapon);
      state.weapons.splice(index, 1);
    },
  },
  actions: {
    /**
     * Used to get the currently existing weapon array from local storage or an api and put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param weapon weapon array object to be put into the store
     */
    getWeaponInfo({ commit }, weapon) {
      commit('GET_WEAPONS', weapon);
    },
    /**
     * Used to create and add a new weapon information array in the store.
     * @param commit internal prop do not pass anything in here.
     * @param weapon weapon information object to be put into weapons array the store
     */
    addWeaponInfo({ commit }, weapon) {
      commit('ADD_WEAPONS', weapon);
    },
    /**
     * Used to update currently existing weapon information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param weapon Used to get the correct index to be updated.
     * @param payload updated currently existing weapon information object
     */
    updateWeaponInfo({ commit }, weapon, payload) {
      commit('UPDATE_WEAPONS', weapon, payload);
    },
    /**
     * Used to delete a currently existing weapon information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param weapon Used to get the correct index.
     */
    deleteWeaponInfo({ commit }, weapon) {
      commit('DELETE_WEAPONS', weapon);
    },
  },
};
