export default {
  state: {
    index: 1,
  },
  getters: {
    getIndex: state => state.index,
  },
  mutations: {
    GET_INDEX(state, index) {
      state.index = index;
    },
    UPDATE_INDEX(state, index) {
      state.index = index;
    },
  },
  actions: {
    getIndex({ commit }, index) {
      commit('GET_INDEX', index);
    },
    updateIndex({ commit }, index) {
      commit('UPDATE_INDEX', index);
    },
  },
};
