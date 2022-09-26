// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
//**Todo: Add Typescript support to vuex later*/
import Vue from 'vue';
import Vuex from 'vuex';
import aliasModule from '@shared-ui/store/modules/alias';
import applicationModule from '@shared-ui/store/modules/application';
import citizenshipModule from '@shared-ui/store/modules/citizenship';
import contactModule from '@shared-ui/store/modules/contact';
import currentAddressModule from '@shared-ui/store/modules/currentAddress';
import differentMailingModule from '@shared-ui/store/modules/differentMailing';
import DOBModule from '@shared-ui/store/modules/DOB';
import employmentModule from '@shared-ui/store/modules/employment';
import idModule from '@shared-ui/store/modules/id';
import personalInfoModule from '@shared-ui/store/modules/personalInfo';
import previousAddressModule from '@shared-ui/store/modules/previousAddress';
import userModule from '@shared-ui/store/modules/user';
import weaponsModule from '@shared-ui/store/modules/weapons';
import localeModule from '@shared-ui/store/modules/locale';

Vue.use(Vuex);

/**
 * Below are the default state the both the admin and pulic site will need.
 * To add application specific modules create the modules in the shared store and then bring in the modules
 * in the store/index in the core- <name>/store
 * @type {Store<unknown>}
 */

export const store = new Vuex.Store({
  modules: {
    alias: aliasModule,
    application: applicationModule,
    citizenship: citizenshipModule,
    contact: contactModule,
    currentAddress: currentAddressModule,
    differentMailing: differentMailingModule,
    DOB: DOBModule,
    employment: employmentModule,
    id: idModule,
    personalInfo: personalInfoModule,
    previousAddress: previousAddressModule,
    user: userModule,
    weapons: weaponsModule,
    locale: localeModule,
  },
});
