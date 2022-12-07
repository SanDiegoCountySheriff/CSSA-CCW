/* eslint-disable @typescript-eslint/ban-ts-comment */
import PreviousAddressDialog from '@shared-ui/components/dialogs/PreviousAddressDialog.vue';
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
});
