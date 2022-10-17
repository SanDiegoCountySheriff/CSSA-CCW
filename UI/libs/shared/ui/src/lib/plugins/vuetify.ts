import Vue from 'vue';
import Vuetify from 'vuetify';
import '@mdi/font/css/materialdesignicons.css';
import '@shared-ui/assets/vuetify.css';

Vue.use(Vuetify);

export const vuetify = new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
});
