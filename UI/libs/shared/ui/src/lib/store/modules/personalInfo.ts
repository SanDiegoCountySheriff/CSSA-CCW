export default {
  state: {
    personalInfo: {
      lastName: '',
      firstName: '',
      middleName: '',
      maidenName: '',
      suffix: '',
      ssn: '',
      maritalStatus: '',
    },
  },
  getters: {
    personalInfo: state => state.personalInfo,
  },
  mutations: {
    GET_PERSONAL_INFO(state, PI) {
      state.personalInfo = PI;
    },
    ADD_PERSONAL_INFO(state, PI) {
      state.personalInfo = PI;
    },
    DELETE_PERSONAL_INFO(state) {
      state.personalInfo = {
        lastName: '',
        firstName: '',
        middleName: '',
        maidenName: '',
        suffix: '',
        ssn: '',
        maritalStatus: '',
      };
    },
    UPDATE_PERSONAL_INFO(state, payload) {
      state.personalInfo = payload;
    },
  },
  actions: {
    /**
     * Used to get the currently existing personal information from local storage or an api and put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param PI personal information object to be put into the store
     */
    getPersonalInfo({ commit }, PI) {
      commit('GET_PERSONAL_INFO', PI);
    },
    /**
     * Used to create a new personal information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param PI personal information object to be put into the store
     */
    addPersonalInfo({ commit }, PI) {
      commit('ADD_PERSONAL_INFO', PI);
    },
    /**
     * Used to update currently existing personal information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing personal information object
     */
    updatePersonalInfo({ commit }, payload) {
      commit('UPDATE_PERSONAL_INFO', payload);
    },
    /**
     * Used to delete a currently existing personal information object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deletePersonalInfo({ commit }) {
      commit('DELETE_PERSONAL_INFO');
    },
  },
};
