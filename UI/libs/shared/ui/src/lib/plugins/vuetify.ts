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
        primary: colors.lightBlue.darken1,
        accent: colors.blueGrey.lighten5,
        secondary: colors.blue.darken3,
        error: colors.red.accent3,
      },
      dark: {
        primary: colors.blue.darken3,
        secondary: colors.green,
        error: colors.red.accent3,
        anchor: colors.blueGrey.darken4,
      },
    },
  },
});
