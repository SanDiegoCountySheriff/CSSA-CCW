import FormStepThree from '@core-public/components/form-stepper/form-steps/FormStepThree.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormStepThree', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(FormStepThree, {
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

  it('Should render all the inital text inputs', () => {
    expect(wrapper.findAllComponents('.v-text-field').length).toBe(7);
  });

  it('Should render the next text inputs', async () => {
    await wrapper.find('#different-mailing').setChecked();
    expect(wrapper.findAllComponents('.v-text-field').length).toBe(14);
  });
});
