export default {
  state: {
    previousAddresses: [],
  },
  getters: {
    previousAddresses: state => state.previousAddresses,
  },
  mutations: {
    GET_PREVIOUS_ADDRESS(state, previousAddresses) {
      state.previousAddresses = previousAddresses;
    },
    ADD_PREVIOUS_ADDRESS(state, previousAddress) {
      state.previousAddresses.unshift(previousAddress);
    },
    UPDATE_PREVIOUS_ADDRESS(state, previousAddress, payload) {
      const index = state.previousAddresses.find(previousAddress);
      state.previousAddresses.splice(index, 1, payload);
    },
    DELETE_PREVIOUS_ADDRESS(state, previousAddress) {
      const index = state.previousAddresses.find(previousAddress);
      state.previousAddresses.splice(index, 1);
    },
  },
  actions: {
    /**
     * Used to get the Array of previous Addresses from either local storage or an api and add it to
     * the store.
     * @param commit internal prop do not pass anything in here.
     * @param previousAddresses and array of previous address objects.
     */
    getPreviousAddresses({ commit }, previousAddresses) {
      commit('GET_PREVIOUS_ADDRESS', previousAddresses);
    },
    /**
     * Used to add a new previous Address to the array in the store.
     * @param commit internal prop do not pass anything in here.
     * @param previousAddress a new previous Address create from the form
     */
    addPreviousAddress({ commit }, previousAddress) {
      commit('ADD_PREVIOUS_ADDRESS', previousAddress);
    },
    /**
     * Used to update a previous existing to the array of addresses in the store.
     * @param commit internal prop do not pass anything in here.
     * @param previousAddress the address to be updated. Used to get the correct index.
     * @param payload the updated address object.
     */
    updatePreviousAddress({ commit }, previousAddress, payload) {
      commit('UPDATE_PREVIOUS_ADDRESS', previousAddress, payload);
    },
    /**
     * Used to delete a currently existing address from the array in the store
     * @param commit internal prop do not pass anything in here.
     * @param previousAddress The address to be deleted out of the array. Used to get the correct index.
     */
    deletePreviousAddress({ commit }, previousAddress) {
      commit('DELETE_PREVIOUS_ADDRESS', previousAddress);
    },
  },
};
