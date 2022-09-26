export default {
  state: {
    publicAppConfig: {},
  },
  getters: {
    publicAppConfig: state => state.publicAppConfig,
  },
  mutations: {
    GET_PUBLIC_APP_CONFIG(state, publicAppConfig) {
      state.publicAppConfig = publicAppConfig;
    },
  },
  actions: {
    /**
     * Used to get app configuration information from local storage or an API and put it into the
     * store
     * @param commit Internal prop do not pass anything in for this.
     * @param publicAppConfig Id object to be added to the store.
     */
    getAppConfig({ commit }, publicAppConfig) {
      commit('GET_PUBLIC_APP_CONFIG', publicAppConfig);
    },
  },
};
