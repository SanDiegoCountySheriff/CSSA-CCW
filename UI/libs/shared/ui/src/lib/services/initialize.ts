/* Add App configuration module */
import axios from 'axios';
// import { store } from '@shared-ui/store';

export const initialize = async () => {
  const response = await axios.get('/config.json');
  //store.dispatch('setAppConfig', response.data);
  return response.data;
};
