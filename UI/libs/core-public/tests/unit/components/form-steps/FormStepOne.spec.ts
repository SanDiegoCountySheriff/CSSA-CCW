import FormStepOne from '@core-public/components/form-stepper/form-steps/FormStepOne.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormStepOne', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(FormStepOne, {
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
          personalInfo: {},
          aliases: [],
          errors: [],
          ssnConfirm: '',
        };
      },
    });
  });

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('Should render the correct amount of inputs', () => {
    const textFields = wrapper.findAllComponents('.v-text-field');
    expect(textFields.length).toEqual(7);
  });

  it('Should update the state on input change', async () => {
    const lastNameField = wrapper.find('#last-name-field');
    await lastNameField.setValue('last');
    expect(lastNameField.element.value).toBe('last');
  });
});
