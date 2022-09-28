import TextInput from '@shared-ui/components/inputs/TextInput.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount, shallowMount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('TextInput', () => {
  let vuetify;

  beforeEach(() => {
    vuetify = new Vuetify();
  });

  it('Should render the correct label', () => {
    //@ts-ignore
    const wrapper = shallowMount(TextInput, {
      localVue,
      vuetify,
      propsData: {
        label: 'testLabel',
      },
      mocks: tMock,
    });
    expect(wrapper.find('v-text-field-stub').attributes().label).toEqual(
      'testLabel'
    );
  });
  it('Should match snapshot', () => {
    //@ts-ignore
    const wrapper = mount(TextInput, {
      localVue,
      vuetify,
      mocks: tMock,
    });
    expect(wrapper.html()).toMatchSnapshot();
  });
});
