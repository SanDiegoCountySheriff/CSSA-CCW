export default {
  state: {
    user: {
      email: '',
      // Need a way to secure this
      password: '',
    },
  },
  getters: {
    userInfo: state => state.user,
  },
  mutations: {
    GET_USER(state, user) {
      state.user = user;
    },
    ADD_USER(state, user) {
      state.user = user;
    },
    UPDATE_USER(state, payload) {
      state.user = payload;
    },
    DELETE_USER(state) {
      state.user = {
        email: '',
        password: '',
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing user information from local storage or an api and put it
     * into the store
     * @param commit internal prop do not pass anything in here.
     * @param user user information object to be put into the store
     */
    getUserInfo({ commit }, user) {
      commit('GET_USER', user);
    },
    /**
     * Used to create a new user information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param user user information object to be put into the store
     */
    addUserInfo({ commit }, user) {
      commit('ADD_USER', user);
    },
    /**
     * Used to update currently existing user information object in the store.
     * @param commit internal prop do not pass anything in here.
     * @param payload updated currently existing user information object
     */
    updateUserInfo({ commit }, payload) {
      commit('UPDATE_USER', payload);
    },
    /**
     * Used to delete a currently existing user information object in the store.
     * @param commit internal prop do not pass anything in here.
     */
    deleteUserInfo({ commit }) {
      commit('DELETE_USER');
    },
  },
};
