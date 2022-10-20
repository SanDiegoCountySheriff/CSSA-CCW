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
        primary: colors.blue.darken3,
        accent: colors.blueGrey.darken3,
        secondary: colors.grey.darken3,
        warning: colors.amber.darken4,
        error: colors.red.darken4,
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
