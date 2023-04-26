import AcknowledgementPart from '@shared-ui/components/acknowledgement-section/AcknowledgementPart.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()

describe('AcknowledgemetnPart', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    //@ts-ignore
    wrapper = mount(AcknowledgementPart, {
      localVue,
      vuetify,
      mocks: {
        $t: text => {
          text
        },
      },
      propsData: {
        handleAccept: () => null,
        handleDecline: () => null,
        textBody: 'test body',
      },
    })
  })

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })
})
