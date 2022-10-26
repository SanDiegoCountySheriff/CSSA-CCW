/* eslint-disable @typescript-eslint/ban-ts-comment */
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

  beforeEach(async () => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(PreviousAddressDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return { dialog: true };
      },
    });
    await wrapper.find('#add-previous-address-btn').trigger('click');
  });
  afterEach(() => {
    wrapper.destroy();
  });

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });

  it('should render the add button', async () => {
    expect(wrapper.find('button').exists()).toBeTruthy();
    expect(wrapper.find('form').exists()).toBeTruthy();
  });

  it('Should render the form', () => {
    expect(wrapper.find('form').exists()).toBeTruthy();
  });

  it('Should not allow submission', () => {
    expect(
      wrapper.find('#pre-address-submit-btn').attributes().disabled
    ).toBeTruthy();
  });

  it('should allow for submission', async () => {
    const newWrapper = mount(PreviousAddressDialog, {
      localVue,
      vuetify,
      mocks: tMock,
    });

    await newWrapper.find('#add-previous-address-btn').trigger('click');
    const line1 = newWrapper.find('#address-line-1');

    await line1.setValue('t');
    const city = newWrapper.find('#city');

    await city.setValue('t');
    const state = newWrapper.find('#state');

    await state.setValue('t');
    const county = newWrapper.find('#county');

    await county.setValue('t');
    const zip = newWrapper.find('#zip');

    await zip.setValue('t');
    const country = newWrapper.find('#country');

    await country.setValue('t');
    await newWrapper.vm.$nextTick();
    expect(
      newWrapper.find('#pre-address-submit-btn').attributes('disabled')
    ).toBeUndefined();
    newWrapper.destroy();
  });
});
