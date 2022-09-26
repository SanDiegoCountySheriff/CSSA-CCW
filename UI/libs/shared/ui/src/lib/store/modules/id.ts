export default {
  state: {
    id: {
      idNumber: '',
      issuingState: '',
    },
  },
  getters: {
    id: state => state.id,
  },
  mutations: {
    GET_ID(state, id) {
      state.id = id;
    },
    ADD_ID(state, id) {
      state.id = id;
    },
    UPDATE_ID(state, payload) {
      state.id = payload;
    },
    DELETE_ID(state) {
      state.id = {
        idNumber: '',
        issuingState: '',
      };
    },
  },
  actions: {
    /**
     * Used to get currently existing id information from local storage or an API and put it into the
     * store
     * @param commit Internal prop do not pass anything in for this.
     * @param id Id object to be added to the store.
     */
    getId({ commit }, id) {
      commit('GET_ID', id);
    },
    /**
     * Used to create a new Id object and add it to the store.
     * @param commit Internal prop do not pass anything in for this.
     * @param id Id object to be added to the store.
     */
    addId({ commit }, id) {
      commit('ADD_ID', id);
    },
    /**
     * Used to update a currently existing id in the store.
     * @param commit Internal prop do not pass anything in for this.
     * @param payload Id object with the changes.
     */
    updateId({ commit }, payload) {
      commit('UPDATE_ID', payload);
    },
    /**
     * Used to delete the information inside the currently existing Id object
     * @param commit Internal prop do not pass anything in for this.
     */
    deleteId({ commit }) {
      commit('DELETE_ID');
    },
  },
};
