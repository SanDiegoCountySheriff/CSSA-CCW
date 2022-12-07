// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import IdBirthInfoStep from '@shared-ui/components/form-stepper/form-steps/IdBirthInfoStep.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';

const pinia = createTestingPinia();
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('IdBirthInfoStep', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    wrapper = mount(IdBirthInfoStep, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSection: () => null,
        handlePreviousSection: () => null,
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

    expect(textFields).toHaveLength(7);
  });
});
