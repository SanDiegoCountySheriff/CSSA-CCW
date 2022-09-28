import PreviousAddressDialog from '@core-public/components/dialogs/PreviousAddressDialog.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('PreviousAddressDialog', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(PreviousAddressDialog, {
      localVue,
      vuetify,
      mocks: tMock,
    });
  });
  afterEach(() => {
    wrapper.destroy();
  });

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('should render the add button', () => {
    expect(wrapper.find('button').exists()).toBeTruthy();
    expect(wrapper.find('form').exists()).toBeFalsy();
  });

  it('Should render the form', async () => {
    await wrapper.setData({ dialog: true });
    expect(wrapper.find('form').exists()).toBeTruthy();
  });

  it('Should not allow submission', async () => {
    await wrapper.setData({ dialog: true });
    expect(
      wrapper.find('#pre-address-submit-btn').attributes().disabled
    ).toBeTruthy();
  });

  it('should allow for sumbission', async () => {
    const newWrapper = mount(PreviousAddressDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
          valid: true,
        };
      },
    });
    expect(
      newWrapper.find('#pre-address-submit-btn').attributes().disabled
    ).toBeFalsy();
    newWrapper.destroy();
  });

  it('should change to valid on required inputs', () => {
    const newWrapper = mount(PreviousAddressDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
        };
      },
    });

    newWrapper.find('#address-line-1').setValue('t');
    newWrapper.find('#city').setValue('t');
    newWrapper.find('#state').setValue('t');
    newWrapper.find('#county').setValue('t');
    newWrapper.find('#zip').setValue('t');
    newWrapper.find('#country').setValue('t');

    expect(newWrapper.vm.$data.valid).toBeTruthy();
  });
});
