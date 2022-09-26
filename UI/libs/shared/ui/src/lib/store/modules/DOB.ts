export default {
  state: {
    DOBInfo: {
      DOB: '',
      currentAge: 0,
      birthCity: '',
      birthState: '',
      birthCountry: '',
    },
  },
  getters: {
    DOBInfo: state => state.DOBInfo,
  },
  mutations: {
    GET_DOB(state, DOB) {
      state.DOBInfo = DOB;
    },
    ADD_DOB(state, DOB) {
      state.DOBInfo = DOB;
    },
    UPDATE_DOB(state, payload) {
      state.DOBInfo = payload;
    },
    DELETE_DOB(state) {
      state.DOBInfo = {
        DOB: '',
        currentAge: 0,
        birthCity: '',
        birthState: '',
        birthCountry: '',
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing DOB from local storage or an API and put it into the store
     * @param commit internal prop do not pass anything in here
     * @param DOB currently existing DOB object
     */
    getDOB({ commit }, DOB) {
      commit('GET_DOB', DOB);
    },
    /**
     * Used to create a new DOB object and put it in the store
     * @param commit internal prop do not pass anything in here
     * @param DOB new DOB object
     */
    addDOB({ commit }, DOB) {
      commit('ADD_DOB', DOB);
    },

    /**
     * Used to update a currently existing DOB object in the store
     * @param commit internal prop do not pass anything in here
     * @param payload updated currently existing DOB object
     */
    updateDOB({ commit }, payload) {
      commit('UPDATE_DOB', payload);
    },

    /**
     * Used to delete a currently existing DOB object from the store
     * @param commit internal prop do not pass anything in here
     */
    deleteDOB({ commit }) {
      commit('DELETE_DOB');
    },
  },
};
