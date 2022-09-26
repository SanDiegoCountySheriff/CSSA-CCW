export default {
  state: {
    differentMailingAddress: false,
  },
  getters: {
    differentMailingAddress: state => state.differentMailingAddress,
  },
  mutations: {
    GET_DIFFERENT_MAILING(state, differentMailing) {
      state.differentMailingAddress = differentMailing;
    },
    ADD_DIFFERENT_MAILING(state, differentMailing) {
      state.differentMailingAddress = differentMailing;
    },
    UPDATE_DIFFERENT_MAILING(state, payload) {
      state.differentMailingAddress = payload;
    },
    DELETE_DIFFERENT_MAILING(state) {
      state.differentMailingAddress = false;
    },
  },
  actions: {
    /**
     * Used to get the currently existing different Mailing information from local storage or an api and
     * put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param differentMailing different Mailing information object to be put into the store
     */
    getDifferentMailingInfo({ commit }, differentMailing) {
      commit('GET_DIFFERENT_MAILING', differentMailing);
    },
    /**
     * Used to create a new different Mailing information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param differentMailing different Mailing information object to be put into the store
     */
    addDifferentMailingInfo({ commit }, differentMailing) {
      commit('ADD_DIFFERENT_MAILING', differentMailing);
    },
    /**
     * Used to update currently existing different Mailing information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing different Mailing information object
     */
    updateDifferentMailingInfo({ commit }, payload) {
      commit('UPDATE_DIFFERENT_MAILING', payload);
    },
    /**
     * Used to delete a currently existing different Mailing information object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deleteDifferentMailingInfo({ commit }) {
      commit('DELETE_DIFFERENT_MAILING');
    },
  },
};
