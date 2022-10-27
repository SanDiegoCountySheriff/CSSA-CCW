/* eslint-disable @typescript-eslint/ban-ts-comment */
import AcknowledgementButtonContainer from '@shared-ui/components/containers/AcknowledgementButtonContainer.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('AcknowledgementButtonContainer', () => {
  let vuetify;
  let wrapper;

  beforeAll(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AcknowledgementButtonContainer, {
      localVue,
      vuetify,
      mocks: tMock,
    });
  });

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot();
  });
});
