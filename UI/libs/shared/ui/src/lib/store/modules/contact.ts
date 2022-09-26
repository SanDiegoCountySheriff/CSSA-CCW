export default {
  state: {
    contact: {
      primaryPhone: '',
      cellPhone: '',
      workPhone: '',
      faxNumber: '',
      textMessageUpdates: false,
    },
  },
  getters: {
    contactInfo: state => state.contact,
  },
  mutations: {
    GET_CONTACT(state, contact) {
      state.contact = contact;
    },
    ADD_CONTACT(state, contact) {
      state.contact = contact;
    },
    UPDATE_CONTACT(state, payload) {
      state.contact = payload;
    },
    DELETE_CONTACT(state) {
      state.contact = {
        primaryPhone: '',
        cellPhone: '',
        workPhone: '',
        faxNumber: '',
        textMessageUpdates: false,
      };
    },
  },
  actions: {
    /**
     * Used to get the currently existing contact information from local storage or an API and add it
     * back to the store .
     * @param commit
     * @param contact
     */
    getContactInfo({ commit }, contact) {
      commit('GET_CONTACT', contact);
    },
    /**
     * Used to create a new contact object and add it to the store.
     * @param commit
     * @param contact
     */
    addContactInfo({ commit }, contact) {
      commit('ADD_CONTACT', contact);
    },
    /**
     *  Used to update the currently existing contact Object in the store
     * @param commit
     * @param payload
     */
    updateContactInfo({ commit }, payload) {
      commit('UPDATE_CONTACT', payload);
    },
    /**
     *   Used to delete the current contact object in the store
     * @param commit
     */
    deleteContactInfo({ commit }) {
      commit('DELETE_CONTACT');
    },
  },
};
