import { i18n } from '@shared-ui/plugins';

export const ssnRuleSet = [
  v => !!v || i18n.t('Social Security Number cannot be blank'),
  v =>
    !!v.match(/^[0-9]+$/) || i18n.t('Social Security can only contain numbers'),
  v => v.length >= 9 || i18n.t('SSN to short must be 9 characters'),
  v => v.length <= 9 || i18n.t('SSN to long must be 9 characters'),
];
