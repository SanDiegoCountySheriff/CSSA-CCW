/**
 * Routes file
 * This is a helper file to reference page routes to be used in
 * the various JS files across the site.
 */
export default class Routes {
  static get HOME_ROUTE_PATH() {
    return `/`;
  }

  static get DASHBOARD_ROUTE_PATH() {
    return `/dashboard`;
  }

  static get APPOINTMENTS_ROUTE_PATH() {
    return `/appointments`;
  }

  static get PERMITS_ROUTE_PATH() {
    return `/applications`;
  }

  static get WORK_ROUTE_PATH() {
    return `/work`;
  }

  static get NUMBERS_ROUTE_PATH() {
    return `/numbers`;
  }

  static get SETTINGS_ROUTE_PATH() {
    return `/settings`;
  }
}
