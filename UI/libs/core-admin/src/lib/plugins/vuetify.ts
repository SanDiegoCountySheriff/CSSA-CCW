import Vue from 'vue';
import Vuetify from 'vuetify';
import colors from 'vuetify/lib/util/colors';
import '@mdi/font/css/materialdesignicons.css';
import '@shared-ui/assets/vuetify.css';

Vue.use(Vuetify);

export const vuetify = new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
  theme: {
    themes: {
      light: {
        primary: '#1D2939',
        accent: '#1849A9',
        secondary: '293056',
        warning: '93370D',
        error: '912018',
        text: colors.grey.darken4,
        info: colors.blue.darken2,
      },
      dark: {
        primary: colors.blue.darken3,
        secondary: colors.green,
        error: colors.red.accent4,
        anchor: colors.blueGrey.darken4,
      },
    },
  },
});
