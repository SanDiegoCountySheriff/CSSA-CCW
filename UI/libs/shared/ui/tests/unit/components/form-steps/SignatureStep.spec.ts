// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import SignatureStep from '@shared-ui/components/form-stepper/form-steps/SignatureStep.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('SignatureStep', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(SignatureStep, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSection: () => null,
      },
    });
  });
  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
