import Vue from 'vue';
import Vuetify from 'vuetify';
import '@shared-ui/assets/vuetify.css';
import '@mdi/font/css/materialdesignicons.css';

Vue.use(Vuetify);

export const vuetify = new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
});
