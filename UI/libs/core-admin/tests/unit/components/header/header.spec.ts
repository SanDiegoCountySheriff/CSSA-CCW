/* eslint-disable @typescript-eslint/ban-ts-comment */
import Header from '@core-admin/components/header/Header.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

describe('Header', () => {
  let vuetify;
  let wrapper;
  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(Header, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
    });
  });
  afterEach(() => {
    wrapper.destroy();
  });

  it('should match the snapshot', async () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
