import Vue from 'vue';
import FormButtonContainer from '@core-public/components/containers/FormButtonContainer.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

Vue.use(Vuetify);
const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('FormButtonContainer', () => {
  let vuetify;
  let wrapper;

  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(FormButtonContainer, {
      localVue,
      vuetify,
      mocks: tMock,
    });
  });
  afterEach(() => {
    wrapper.destroy();
  });

  it('should render all three buttons', () => {
    expect(wrapper.findAll('button').length).toBe(3);
  });

  it('Should emit on button press', async () => {
    await wrapper.setData({ valid: true });
    const button = wrapper.findAll('button').at(0);
    button.trigger('click');
    expect(wrapper.emitted().submit).toHaveLength(1);
  });
});
