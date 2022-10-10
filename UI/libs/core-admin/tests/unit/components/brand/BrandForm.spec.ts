import BrandForm from '@core-admin/components/brand/BrandForm.vue';
import { createLocalVue, mount } from '@vue/test-utils';
import Vuetify from 'vuetify';
import { createTestingPinia } from '@pinia/testing';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

describe('BrandForm', () => {
  let vuetify;

  beforeEach(() => {
    vuetify = new Vuetify();
  });

  it('Should render logo label', () => {
    const wrapper = mount(BrandForm, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
    });
    expect(wrapper.html()).toMatchSnapshot();
    expect(wrapper.find('.v-input').exists()).toBe(true);
  });
});
