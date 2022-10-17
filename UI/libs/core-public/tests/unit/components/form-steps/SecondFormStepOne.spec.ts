import SecondFormStepOne from '@core-public/components/form-stepper/form-steps/SecondFormStepOne.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('SecondFormStepOne', () => {
  let vuetify;
  let wrapper;
  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(SecondFormStepOne, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSection: () => null,
      },
      data() {
        return {
          valid: false,
        };
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('Should render the header', () => {
    expect(wrapper.text()).toContain('Employment Status');
  });

  it('Should display the menu on click', async () => {
    await wrapper.find('#select').trigger('click');
    expect(wrapper.text()).toContain('Unemployed');
  });
});
