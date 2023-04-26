/* eslint-disable @typescript-eslint/ban-ts-comment */
import FormButtonContainer from '@shared-ui/components/containers/FormButtonContainer.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const tMock = {
  $t: text => text,
}

describe('FormButtonContainer', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(FormButtonContainer, {
      localVue,
      vuetify,
      mocks: tMock,
      propsData: {
        valid: true,
      },
    })
  })
  afterEach(() => {
    wrapper.destroy()
  })

  it('should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })

  it('should render all three buttons', () => {
    expect(wrapper.findAll('button')).toHaveLength(4)
  })

  it('Should emit on button press', () => {
    const button = wrapper.findAll('button').at(0)

    button.trigger('click')
    expect(wrapper.emitted().submit).toHaveLength(1)
  })
})
