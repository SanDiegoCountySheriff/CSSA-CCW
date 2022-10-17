import SecondFormStepTwo from '@core-public/components/form-stepper/form-steps/SecondFormStepTwo.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('SecondFormStepTwo', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(SecondFormStepTwo, {
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
