/* eslint-disable @typescript-eslint/ban-ts-comment */
import FormErrorAlert from '@shared-ui/components/alerts/FormErrorAlert.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();

describe('FormErrorAlert', () => {
  let vuetify;

  beforeEach(() => {
    vuetify = new Vuetify();
  });

  it('should match the snapshot', () => {
    //@ts-ignore
    const wrapper = mount(FormErrorAlert, {
      localVue,
      vuetify,
      mocks: {
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        $t: () => {},
      },
    });

    expect(wrapper.html()).toMatchSnapshot();
  });

  it('Should render the passed in error', () => {
    //@ts-ignore
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
    //@ts-ignore
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
