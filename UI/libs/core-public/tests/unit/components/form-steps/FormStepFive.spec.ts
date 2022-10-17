import FormStepFive from '@core-public/components/form-stepper/form-steps/FormStepFive.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormStepFive', () => {
  let vuetify = new Vuetify();
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(FormStepFive, {
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
