/* eslint-disable @typescript-eslint/ban-ts-comment */
import RadioGroupInput from '@shared-ui/components/inputs/RadioGroupInput.vue';
import Vuetify from 'vuetify';
import { createLocalVue, shallowMount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('RadioGroupInput', () => {
  let vuetify;

  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = shallowMount(RadioGroupInput, {
      localVue,
      vuetify,
      mocks: tMock,
    });
  });
  afterEach(() => {
    wrapper.destroy();
  });

  it('Should match snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('Should Render the correct layout', async () => {
    await wrapper.setProps({ layout: 'row' });
    expect(wrapper.find('v-radio-group-stub').attributes().row).toBeTruthy();
  });

  it('Should render the correct layout non row ', () => {
    expect(wrapper.find('v-radio-group-stub').attributes().row).toBeFalsy();
  });

  it('Should render the correct first options', async () => {
    await wrapper.setProps({
      options: [
        {
          label: 'yes',
          value: 'true',
        },
        {
          label: 'no',
          value: 'false',
        },
      ],
    });
    const options = wrapper.findAll('v-radio-stub');

    expect(options.at(0).attributes().label).toEqual('yes');
    expect(options.at(0).attributes().value).toEqual('true');
  });

  it('Should render the correct second option', async () => {
    await wrapper.setProps({
      options: [
        {
          label: 'yes',
          value: 'true',
        },
        {
          label: 'no',
          value: 'false',
        },
      ],
    });
    const options = wrapper.findAll('v-radio-stub');

    expect(options.at(1).attributes().label).toEqual('no');
    expect(options.at(1).attributes().value).toEqual('false');
  });
});
