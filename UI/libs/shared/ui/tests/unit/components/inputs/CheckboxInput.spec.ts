/* eslint-disable @typescript-eslint/ban-ts-comment */
import CheckboxInput from '@shared-ui/components/inputs/CheckboxInput.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const tMock = {
  $t: text => text,
}

describe('CheckboxInput', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(CheckboxInput, {
      localVue,
      vuetify,
      mocks: tMock,
      propsData: {
        label: 'test',
        target: 'testTarget',
      },
    })
  })

  afterEach(() => {
    wrapper.destroy()
  })

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })

  it('Should render the Checkbox', () => {
    expect(wrapper.props().label).toBe('test')
    expect(wrapper.props().target).toBe('testTarget')
  })

  it('Should emit the correct information', async () => {
    const radioInput = wrapper.find('input[type="checkbox"]')

    await radioInput.trigger('click')
    expect(wrapper.emitted().input[0][1]).toBe('testTarget')
  })
})
