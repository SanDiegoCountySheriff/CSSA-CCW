import Vue from 'vue';
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue';
import Vuetify from 'vuetify';
import { createLocalVue, shallowMount } from '@vue/test-utils';

Vue.use(Vuetify);
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('CheckboxInput', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = shallowMount(CheckboxInput, {
      localVue,
      vuetify,
      mocks: tMock,
      propsData: {
        label: 'test',
        target: 'testTarget',
      },
    });
  });

  afterEach(() => {
    wrapper.destroy();
  });

  it('Should render the Checkbox', () => {
    expect(wrapper.props().label).toEqual('test');
    expect(wrapper.props().target).toEqual('testTarget');
  });

  it('Should emit the correct information', async () => {
    await wrapper.vm.$nextTick(() => {
      wrapper.vm.handleChange();
    });
    // false is the default value for the component.
    expect(wrapper.emitted().input[0][0]).toEqual('false');
    expect(wrapper.emitted().input[0][1]).toEqual('testTarget');
  });
});
