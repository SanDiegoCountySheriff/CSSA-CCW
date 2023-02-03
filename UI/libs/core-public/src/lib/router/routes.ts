/**
 * Routes file
 * This is a helper file to reference page routes to be used in
 * the various JS files across the site.
 */
export default class Routes {
  static get HOME_ROUTE_PATH() {
    return `/`;
  }

  static get APPLICATION_ROUTE_PATH() {
    return `/application`;
  }

  static get APPLICATION_DETAIL_ROUTE() {
    return `/application-details`;
  }

  static get APPLICATION_STATUS_PATH() {
    return '/status';
  }

  static get FINALIZE_ROUTE_PATH() {
    return `/finalize`;
  }

  static get FORM_ROUTE_PATH() {
    return `/form`;
  }

  static get MORE_INFORMATION_ROUTE_PATH() {
    return `/moreinformation`;
  }

  static get PENAL_CODE_PATH() {
    return `/penalcode`;
  }

  static get RECEIPT_PATH() {
    return '/receipt';
  }

  static get RENEW_APPLICATION_ROUTE_PATH() {
    return `/renewapplication`;
  }

  static get RENEW_FORM_ROUTE_PATH() {
    return `/renewform`;
  }
}
