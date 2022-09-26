export default {
  state: {
    currentAddress: {
      addressLine1: '',
      addressLine2: '',
      city: '',
      county: '',
      state: '',
      // need to decide if this is a string or a number
      zip: '',
      country: '',
    },
  },
  getters: {
    currentAddress: state => state.currentAddress,
  },
  mutations: {
    GET_CURRENT_ADDRESS(state, currentAddress) {
      state.currentAddress = currentAddress;
    },
    ADD_CURRENT_ADDRESS(state, currentAddress) {
      state.currentAddress = currentAddress;
    },
    UPDATE_CURRENT_ADDRESS(state, payload) {
      state.currentAddress = payload;
    },
    DELETE_CURRENT_ADDRESS(state) {
      state.currentAddress = {
        addressLine1: '',
        addressLine2: '',
        city: '',
        county: '',
        state: '',
        // need to decide if this is a string or a number
        zip: '',
        country: '',
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing current address information from local storage or an api and
     * put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param currentAddress current address information object to be put into the store
     */
    getCurrentAddressInfo({ commit }, currentAddress) {
      commit('GET_CURRENT_ADDRESS', currentAddress);
    },
    /**
     * Used to create a new current address information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param currentAddress current address information object to be put into the store
     */
    addCurrentAddressInfo({ commit }, currentAddress) {
      commit('ADD_CURRENT_ADDRESS', currentAddress);
    },
    /**
     * Used to update currently existing current address information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing current address information object
     */
    updateCurrentAddressInfo({ commit }, payload) {
      commit('UPDATE_CURRENT_ADDRESS', payload);
    },
    /**
     * Used to delete a currently existing current address information object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deleteCurrentAddressInfo({ commit }) {
      commit('DELETE_CURRENT_ADDRESS');
    },
  },
};
