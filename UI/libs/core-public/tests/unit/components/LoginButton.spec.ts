/* eslint-disable @typescript-eslint/ban-ts-comment */
import LoginButton from '@core-public/components/LoginButton.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
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
    expect(wrapper.find('button').exists()).toBeTruthy();
  });
});
