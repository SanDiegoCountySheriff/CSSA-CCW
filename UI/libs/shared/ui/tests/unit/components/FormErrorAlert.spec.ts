import Vue from 'vue';
import FormErrorAlert from '@shared-ui/components/alerts/FormErrorAlert.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

Vue.use(Vuetify);
const localVue = createLocalVue();

describe('FormErrorAlert', () => {
  let vuetify;

  beforeEach(() => {
    vuetify = new Vuetify();
  });

  it('Should render the passed in error', () => {
    const wrapper = mount(FormErrorAlert, {
      localVue,
      vuetify,
      propsData: {
        errors: ['error1'],
      },
      mocks: {
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        $t: () => {},
      },
    });
    expect(wrapper.text()).toContain('error1');
  });

  it('should render multiple errors', () => {
    const wrapper = mount(FormErrorAlert, {
      localVue,
      vuetify,
      propsData: {
        errors: ['error1', 'error2'],
      },
      mocks: {
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        $t: () => {},
      },
    });
    expect(wrapper.text()).toContain('error1');
    expect(wrapper.text()).toContain('error2');
  });
});
