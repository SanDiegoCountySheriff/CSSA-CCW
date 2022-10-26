import BrandForm from '@core-admin/components/brand/BrandForm.vue';
import { createLocalVue, mount } from '@vue/test-utils';
import Vuetify from 'vuetify';
import { createTestingPinia } from '@pinia/testing';
import { useQuery } from '@tanstack/vue-query';

const localVue = createLocalVue();
const pinia = createTestingPinia();
const tMock = {
  $t: text => text,
};

jest.mock('@tanstack/vue-query', () => ({
  useQuery: jest.fn(),
}));

describe('BrandForm', () => {
  let vuetify;
  let result;

  beforeEach(() => {
    vuetify = new Vuetify();
    result = {
      data: null,
      isLoading: true,
    };
    (useQuery as jest.Mock).mockImplementationOnce(() => result);
  });

  it('Should render logo label', () => {
    result = {
      isLoading: false,
    };
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
