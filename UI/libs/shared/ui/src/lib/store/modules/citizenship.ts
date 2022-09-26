export default {
  state: {
    citizenshipInfo: {
      citizen: false,
      militaryStatus: '',
    },
  },
  getters: {
    citizenshipInfo: state => state.citizenshipInfo,
  },
  mutations: {
    GET_CITIZEN(state, citizenship) {
      state.citizenshipInfo = citizenship;
    },
    ADD_CITIZEN(state, citizenship) {
      state.citizenshipInfo = citizenship;
    },
    UPDATE_CITIZEN(state, payload) {
      state.citizenshipInfo = payload;
    },
    DELETE_CITIZEN(state) {
      state.citizenshipInfo = {
        citizen: false,
        militaryStatus: '',
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing citizenship information from local storage or an API and put
     * it into the store
     * @param commit internal prop do not pass anything here
     * @param citizenship citenzenshipInfo object obtained from local storage or api
     */
    getCitizenshipInfo({ commit }, citizenship) {
      commit('GET_CITIZEN', citizenship);
    },
    /**
     * Used to create a new citizenship object and put it into the store
     * @param commit internal prop do not pass anything here
     * @param citizenship new citizenship Object created from the form
     */
    addCitizenshipInfo({ commit }, citizenship) {
      commit('ADD_CITIZEN', citizenship);
    },
    /**
     * Used to update the currently existing citizen object in the store
     * @param commit internal prop do not pass anything here
     * @param payload updated citizenship Object
     */
    updateCitizenshipInfo({ commit }, payload) {
      commit('GET_CITIZEN', payload);
    },

    /**
     * Used to delete the currently existing citizen object in the store
     * @param commit internal prop do not pass anything here
     */
    deleteCitizenshipInfo({ commit }) {
      commit('GET_CITIZEN');
    },
  },
};
