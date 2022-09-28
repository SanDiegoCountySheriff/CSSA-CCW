import AliasDialog from '@core-public/components/dialogs/AliasDialog.vue';
import Vuetify from 'vuetify';
import { createLocalVue, mount } from '@vue/test-utils';

const localVue = createLocalVue();
const tMock = {
  $t: text => text,
};

describe('AliasDialog', () => {
  let vuetify;
  let wrapper;
  beforeEach(() => {
    vuetify = new Vuetify();
    //@ts-ignore
    wrapper = mount(AliasDialog, {
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

  it('should render the form on button click', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
        };
      },
    });
    expect(onWrapper.findAll('.v-input').length).toBe(6);
    onWrapper.destroy();
  });
  it('should not allow submission', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
        };
      },
    });
    expect(onWrapper.find('#submit-btn').attributes().disabled).toBeTruthy();
    onWrapper.destroy();
  });

  it('should allow submission', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
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
    expect(onWrapper.find('#submit-btn').attributes().disabled).toBeFalsy();
    onWrapper.destroy();
  });
  it('should change valid to true on inputs', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
          valid: false,
        };
      },
    });
    onWrapper.find('#last-name').setValue('t');
    onWrapper.find('#first-name').setValue('t');
    expect(onWrapper.vm.$data.valid).toBeTruthy();
    onWrapper.destroy();
  });
});
