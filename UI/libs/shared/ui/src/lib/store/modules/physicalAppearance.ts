export default {
  state: {
    physicalAppearance: {
      gender: '',
      heightFeet: 0,
      heightInches: 0,
      weight: 0,
      hairColor: '',
      eyeColor: '',
      physicalDesc: '',
    },
  },
  getters: {
    physicalAppearanceInfo: state => state.physicalAppearance,
  },
  mutations: {
    GET_PHYSICAL_APPEARANCE(state, physicalApp) {
      state.physicalAppearance = physicalApp;
    },
    ADD_PHYSICAL_APPEARANCE(state, physicalApp) {
      state.physicalAppearance = physicalApp;
    },
    UPDATE_PHYSICAL_APPEARANCE(state, payload) {
      state.physicalAppearance = payload;
    },
    DELETE_PHYSICAL_APPEARANCE(state) {
      state.physicalAppearance = {
        gender: '',
        heightFeet: 0,
        heightInches: 0,
        weight: 0,
        hairColor: '',
        eyeColor: '',
        physicalDesc: '',
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing Appearance object from local storage or an API
     * @param commit internal prop do not pass anything in for this
     * @param physicalApp current existing object
     */
    getPhysicalAppearance({ commit }, physicalApp) {
      commit('GET_PHYSICAL_APPEARANCE', physicalApp);
    },
    /**
     * Used to create a new Appearance object from the form
     * @param commit internal prop do not pass anything in for this
     * @param physicalApp new appearance object
     */
    addPhysicalAppearance({ commit }, physicalApp) {
      commit('ADD_PHYSICAL_APPEARANCE', physicalApp);
    },
    /**
     * Used to update the current existing Appearance object in the store
     * @param commit internal prop do not pass anything in for this
     * @param payload updated existing Appearance object
     */
    updatePhysicalAppearance({ commit }, payload) {
      commit('UPDATE_PHYSICAL_APPEARANCE', payload);
    },
    /**
     * Used to delete the curren Appearance object from the store
     * @param commit internal prop do not pass anything in for this
     */
    deletePhysicalAppearance({ commit }) {
      commit('DELETE_PHYSICAL_APPEARANCE');
    },
  },
};
