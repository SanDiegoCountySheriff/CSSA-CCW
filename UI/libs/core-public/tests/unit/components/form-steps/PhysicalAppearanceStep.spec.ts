// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import PhysicalAppearanceStep from '@core-public/components/form-stepper/form-steps/PhysicalAppearanceStep.vue';
import Vuetify from 'vuetify';
import { createTestingPinia } from '@pinia/testing';
import { createLocalVue, mount } from '@vue/test-utils';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormStepFour', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(PhysicalAppearanceStep, {
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

  it('should render all the inputs', () => {
    expect(wrapper.findAllComponents('.v-text-field')).toHaveLength(6);
  });
});
