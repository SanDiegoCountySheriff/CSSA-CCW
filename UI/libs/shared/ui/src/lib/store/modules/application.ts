export default {
  state: {
    applicationType: '',
  },
  getters: {
    applicationType: state => state.applicationType,
  },
  mutations: {
    GET_APPLICATION_TYPE(state, applicationType) {
      state.applicationType = applicationType;
    },
    ADD_APPLICATION_TYPE(state, applicationType) {
      state.applicationType = applicationType;
    },
    UPDATE_APPLICATION_TYPE(state, payload) {
      state.applicationType = payload;
    },
    DELETE_APPLICATION_TYPE(state) {
      state.applicationType = '';
    },
  },
  actions: {
    /**
     * Used to get the currently existing application type from local storage or an api and
     * put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param applicationType application type object to be put into the store
     */
    getApplicationTypeInfo({ commit }, applicationType) {
      commit('GET_APPLICATION_TYPE', applicationType);
    },
    /**
     * Used to create a new application type object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param applicationType application type object to be put into the store
     */
    addApplicationTypeInfo({ commit }, applicationType) {
      commit('ADD_APPLICATION_TYPE', applicationType);
    },
    /**
     * Used to update currently existing application type object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing application type object
     */
    updateApplicationTypeInfo({ commit }, payload) {
      commit('UPDATE_APPLICATION_TYPE', payload);
    },
    /**
     * Used to delete a currently existing application type object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deleteApplicationTypeInfo({ commit }) {
      commit('DELETE_APPLICATION_TYPE');
    },
  },
};
