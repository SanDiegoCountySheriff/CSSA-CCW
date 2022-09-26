export default {
  state: {
    adminAppConfig: {},
  },
  getters: {
    adminAppConfig: state => state.adminAppConfig,
  },
  mutations: {
    GET_ADMIN_APP_CONFIG(state, adminAppConfig) {
      state.adminAppConfig = adminAppConfig;
    },
  },
  actions: {
    /**
     * Used to get app configuration information from local storage or an API and put it into the
     * store
     * @param commit Internal prop do not pass anything in for this.
     * @param adminAppConfig Id object to be added to the store.
     */
    getAppConfig({ commit }, adminAppConfig) {
      commit('GET_ADMIN_APP_CONFIG', adminAppConfig);
    },
  },
};
