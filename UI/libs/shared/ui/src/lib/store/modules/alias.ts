export default {
  state: {
    aliases: [],
  },
  getters: {
    aliases: state => state.aliases,
  },
  mutations: {
    GET_ALIAS(state, aliases) {
      state.aliases = aliases;
    },
    ADD_ALIAS(state, aliases) {
      state.aliases = aliases;
    },
    DELETE_ALIAS(state, alias) {
      const index = state.aliases.indexOf(alias);
      state.aliases.splice(index, 1);
    },
    UPDATE_ALIAS(state, payload, alias) {
      const index = state.aliases.indexOf(alias);
      state.aliases.splice(index, 1, payload);
    },
  },
  actions: {
    /**
     * Used to grab the aliases from either local storage or an api and put them in the store
     * @param commit calls the correct mutation -internal prop no need to pass anything in
     * @param aliases an array of aliases
     */
    getAlias({ commit }, aliases) {
      commit('GET_ALIAS', aliases);
    },
    /**
     * Used by the form to add another alias to the array of aliases.
     * @param commit calls the correct mutation -internal prop no need to pass anything in
     * @param aliases Array of aliases to save to add to the store.
     */
    addAlias({ commit }, aliases) {
      commit('ADD_ALIAS', aliases);
    },
    /**
     * Can be used to update a currently existing alias
     * @param commit calls the correct mutation -internal prop no need to pass anything in
     * @param payload alias body with the changes made to it.
     * @param alias current body with no changes used to find the correct index.
     */
    updateAlias({ commit }, payload, alias) {
      commit('UPDATE_ALIAS', payload, alias);
    },
    /**
     * Used to delete a currently existing alias
     * @param commit calls the correct mutation -internal prop no need to pass anything in
     * @param alias current alias used to find the correct index
     */
    deleteAlias({ commit }, alias) {
      commit('DELETE_ALIAS', alias);
    },
  },
};
