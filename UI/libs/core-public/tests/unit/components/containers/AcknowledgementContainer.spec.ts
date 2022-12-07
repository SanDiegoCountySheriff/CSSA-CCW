/* eslint-disable @typescript-eslint/ban-ts-comment */
import AcknowledgementContainer from '@core-public/components/containers/AcknowledgementContainer.vue';
import Vuetify from 'vuetify';
import { createTestingPinia } from '@pinia/testing';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

describe('AcknowledgementContainer', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AcknowledgementContainer, {
      localVue,
      pinia,
      vuetify,
      mocks: tMock,
      propsData: {
        nextRoute: 'test',
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
