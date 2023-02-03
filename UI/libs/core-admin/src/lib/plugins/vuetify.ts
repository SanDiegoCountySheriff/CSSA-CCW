import Vue from 'vue';
import Vuetify from 'vuetify';
import colors from 'vuetify/lib/util/colors';
import '@shared-ui/assets/vuetify.css';

Vue.use(Vuetify);

export const vuetify = new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
  theme: {
    themes: {
      light: {
        accent: '#1D2939',
        primary: '#1849A9',
        secondary: '#293056',
        warning: '#93370D',
        error: '#912018',
        blue: '#1976D2',
        blue1: '#2e90fa',
        text: colors.grey.darken4,
        info: colors.blue.darken2,
      },
      dark: {
        primary: '#CCC',
        accent: "#363636",
        secondary: colors.green,
        error: colors.red.accent4,
        anchor: colors.blueGrey.darken4,
        text: '#FFF',
      },
    },
  },
});
