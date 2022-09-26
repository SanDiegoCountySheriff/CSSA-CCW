/**
 * To add store modules that are only specific to the public site well will register them here
 */

import { store } from '@shared-ui/store';
import publicAppConfigModule from './modules/publicAppConfig';
/**
 * Use the follow line as a guild to add a app specific module to the store
 * import aliasModule from "@shared-ui/store/modules/alias"
 * store.registerModule("alias", aliasModule)
 *
 */
store.registerModule('publicAppConfig', publicAppConfigModule);
export { store };
