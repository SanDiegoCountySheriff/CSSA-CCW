import { i18n } from '@shared-ui/plugins'

export const ssnRuleSet = [
  v => Boolean(v) || i18n.t('Social Security Number cannot be blank'),
  v => Boolean(/^\d+$/.test(v)) || i18n.t('Must only contain numbers'),
  v =>
    Boolean(!/^(\d)\1{8,}/.test(v)) ||
    i18n.t('Cannot contain repeating numbers'),
  v => v.length >= 9 || i18n.t('SSN to short must be 9 characters'),
  v => v.length <= 9 || i18n.t('SSN to long must be 9 characters'),
]

export const phoneRuleSet = [
  v => Boolean(v) || i18n.t('Primary phone number cannot be blank'),
  v => Boolean(/^\d+$/.test(v)) || i18n.t('Must only contain numbers'),
  v => v.length === 10 || i18n.t('Must be 10 numbers in length'),
]

export const notRequiredPhoneRuleSet = [
  v => v.length === 10 || v === '' || i18n.t('Must be 10 numbers in length'),
  v =>
    v === '' || Boolean(/^\d+$/.test(v)) || i18n.t('Must only contain numbers'),
]
export const requireNameRuleSet = [
  v => Boolean(!/\s/.test(v)) || i18n.t('Cannot contain spaces'),
  v => Boolean(v) || i18n.t('Field is required'),
  v =>
    Boolean(/[a-zA-Z\s]+$/.test(v)) || i18n.t('Names may only contain letters'),
]

export const notRequiredNameRuleSet = [
  v =>
    v === '' ||
    Boolean(/[a-zA-Z\s]+$/.test(v)) ||
    i18n.t('Names may only contain letters and spaces'),
]

export const permitRuleSet = [
  v => Boolean(/^\d+$/.test(v)) || i18n.t('Must only contain numbers'),
  v => v.length === 9 || i18n.t('Must be 9 digits in length'),
]

export const zipRuleSet = [
  v => Boolean(v) || i18n.t('Field is required'),
  v =>
    Boolean(/^\d+$/.test(v)) ||
    v === 'N/A' ||
    v === 'n/a' ||
    i18n.t('Must contain only numbers'),
]
