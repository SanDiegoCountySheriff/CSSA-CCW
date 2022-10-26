/* eslint-disable @typescript-eslint/ban-ts-comment */
import AppointmentScheduler from '@core-admin/components/appointment/AppointmentScheduler.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

describe('AppointmentScheduler', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AppointmentScheduler, {
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
