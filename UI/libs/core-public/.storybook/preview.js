import Vue from 'vue';
import Vuetify from 'vuetify';
import { addDecorator, addParameters } from '@storybook/vue';
import '@core-public/plugins/storybook';
import '@mdi/font/css/materialdesignicons.css';
import { INITIAL_VIEWPORTS } from '@storybook/addon-viewport';
import { i18n } from '../../core-public/src/lib/plugins';
import { createPinia, PiniaVuePlugin } from 'pinia';

Vue.use(PiniaVuePlugin);

const pinia = createPinia();

Vue.prototype.$t = function (...args) {
  return i18n.t(...args);
};

const customViewports = {
  panasonicCf33: {
    name: 'Panasonic CF-33',
    styles: {
      width: '2160px',
      height: '1440px',
    },
  },
  dellLat7202: {
    name: 'Dell Lat 7202',
    styles: {
      width: '1366px',
      height: '768px',
    },
  },
  dellLat7212: {
    name: 'Dell Lat 7212',
    styles: {
      width: '1920px',
      height: '1080px',
    },
  },
  dellLat7220: {
    name: 'Dell Lat 7220',
    styles: {
      width: '1920px',
      height: '1080px',
    },
  },
};

addParameters({
  viewport: {
    viewports: { ...INITIAL_VIEWPORTS, ...customViewports },
  },
});

const vuetifyOptions = {
  icons: {
    iconfont: 'mdi',
  },
};

addDecorator(() => ({
  vuetify: new Vuetify(vuetifyOptions),
  i18n,
  pinia,
  template: '<v-app><v-main fluid><story/></v-main></v-app>',
}));

addParameters({
  a11y: {
    element: '#root',
    config: {},
    options: {},
    manual: true,
  },
});
