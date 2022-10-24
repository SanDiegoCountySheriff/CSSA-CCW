/* eslint-disable @typescript-eslint/ban-ts-comment */
import LoginButton from '@core-admin/components/login/LoginButton.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

describe('LoginButton', () => {
  let vuetify;
  let wrapper;
  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(LoginButton, {
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

  it('should render the Login button', () => {
    expect(wrapper.find('.v-list-item__title').text()).toBe('Login');
  });
});
