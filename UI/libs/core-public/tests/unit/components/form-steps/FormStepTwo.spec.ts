import FormStepTwo from '@core-public/components/form-stepper/form-steps/FormStepTwo.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormStepTwo', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(FormStepTwo, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSection: () => null,
      },
      data() {
        return {
          DOBInfo: {
            DOB: '',
          },
        };
      },
    });
  });

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('should render all the text inputs', () => {
    const textFields = wrapper.findAllComponents('.v-text-field');

    expect(textFields).toHaveLength(8);
  });

  it('should render the alert if no DOB is present', () => {
    const alerts = wrapper.findAllComponents('.v-alert');

    expect(alerts.at(0).element.textContent).toContain('Date of birth');
  });
});
