import { i18n } from '@shared-ui/plugins';

export const ssnRuleSet = [
  v => Boolean(v) || i18n.t('Social Security Number cannot be blank'),
  v =>
    Boolean(v.match(/^[0-9]+$/)) ||
    i18n.t('Social Security can only contain numbers'),
  v => v.length >= 10 || i18n.t('SSN to short must be 10 characters'),
  v => v.length <= 10 || i18n.t('SSN to long must be 10 characters'),
];

export const phoneRuleSet = [
  v => Boolean(v) || i18n.t('Primary phone number cannot be blank'),
  v => Boolean(v.match(/^[0-9]+$/)) || i18n.t('Must only contain numbers'),
  v => v.length >= 10 || i18n.t('Must be 10 numbers in length'),
];
