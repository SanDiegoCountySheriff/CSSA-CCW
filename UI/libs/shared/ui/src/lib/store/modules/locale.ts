export default {
  state: {
    locales: {
      en: {},
    },
  },
  getters: {
    enLocale: state => state.locales.en,
  },
  mutations: {
    GET_EN_CONFIG(state, en) {
      state.locales.en = en;
    },
    SET_EN_CONFIG(state, payload) {
      state.locales.en = payload;
    },
  },
  actions: {
    /**
     * Used to get english locale information from local storage or an API and put it into the
     * store
     * @param commit Internal prop do not pass anything in for this.
     * @param en Id object to be added to the store.
     */
    getEnConfig({ commit }, en) {
      commit('GET_EN_CONFIG', en);
    },
    /**
     * Used to update a currently existing en locale configuration in the store.
     * @param commit Internal prop do not pass anything in for this.
     * @param payload Id object with the changes.
     */
    setEnLocale({ commit }, payload) {
      commit('SET_EN_CONFIG', payload);
    },
  },
};
