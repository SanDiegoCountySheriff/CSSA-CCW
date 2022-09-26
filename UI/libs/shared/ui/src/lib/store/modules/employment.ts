export default {
  state: {
    employment: '',
  },
  getters: {
    employment: state => state.employment,
  },
  mutations: {
    GET_EMPLOYMENT(state, employment) {
      state.employment = employment;
    },
    ADD_EMPLOYMENT(state, employment) {
      state.employment = employment;
    },
    UPDATE_EMPLOYMENT(state, payload) {
      state.employment = payload;
    },
    DELETE_EMPLOYMENT(state) {
      state.employment = '';
    },
  },
  actions: {
    /**
     * Used to get the currently existing employment information from local storage or an api and put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param employment employment information object to be put into the store
     */
    getEmploymentInfo({ commit }, employment) {
      commit('GET_EMPLOYMENT', employment);
    },
    /**
     * Used to create a new employment information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param employment employment information object to be put into the store
     */
    addEmploymentInfo({ commit }, employment) {
      commit('ADD_EMPLOYMENT', employment);
    },
    /**
     * Used to update currently existing employment information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing employment information object
     */
    updateEmploymentInfo({ commit }, payload) {
      commit('UPDATE_EMPLOYMENT', payload);
    },
    /**
     * Used to delete a currently existing employment information object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deleteEmploymentInfo({ commit }) {
      commit('DELETE_EMPLOYMENT');
    },
  },
};
