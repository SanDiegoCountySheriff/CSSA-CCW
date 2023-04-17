/* eslint-disable @typescript-eslint/ban-ts-comment */
import ContactInfoSection from '@shared-ui/components/info-sections/ContactInfoSection.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()

describe('ContactInfoSection', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(ContactInfoSection, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        contactInfo: {
          primaryPhoneNumber: 'primary',
          cellPhoneNumber: 'cell',
          workPhoneNumber: 'work',
          faxPhoneNumber: 'fax',
          textMessageUpdates: true,
        },
      },
    })
  })

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })
})
