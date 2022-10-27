import Vue from 'vue';
import VueI18n from 'vue-i18n';
import en from '@shared-ui/locales/en.json';

Vue.use(VueI18n);

const defaultLocale = 'en';
const languages = {
  en,
};
const messages = Object.assign(languages);

export const i18n = new VueI18n({
  locale: defaultLocale,
  fallbackLocale: 'en',
  silentTranslationWarn: true,
  messages,
});
