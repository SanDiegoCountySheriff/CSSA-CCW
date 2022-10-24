import SecondFormStepFour from '@core-public/components/form-stepper/form-steps/SecondFormStepFour.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('SecondFormStepFour', () => {
  let vuetify;
  let wrapper;
  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(SecondFormStepFour, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSerction: () => null,
      },
    });
  });
  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
