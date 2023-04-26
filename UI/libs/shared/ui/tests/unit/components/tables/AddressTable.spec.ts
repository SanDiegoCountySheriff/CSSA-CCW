/* eslint-disable @typescript-eslint/ban-ts-comment */
import AddressTable from '@shared-ui/components/tables/AddressTable.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()

describe('AddressTable', () => {
  let vuetify

  beforeEach(() => {
    vuetify = new Vuetify()
  })

  it('should match the snapshot', () => {
    //@ts-ignore
    const wrapper = mount(AddressTable, {
      localVue,
      vuetify,
      mocks: {
        $t: text => {
          text
        },
      },
    })

    expect(wrapper.html()).toMatchSnapshot()
  })

  it('Should render the address table in the correct order', () => {
    //@ts-ignore
    const wrapper = mount(AddressTable, {
      localVue,
      vuetify,
      mocks: {
        $t: text => text,
      },
      propsData: {
        addresses: [
          {
            addressLine1: 'line1',
            addressLine2: 'line2',
            city: 'city',
          },
        ],
      },
    })

    expect(wrapper.findAll('td').at(0).element.textContent).toContain('line1')
    expect(wrapper.findAll('td').at(1).element.textContent).toContain('line2')
    expect(wrapper.findAll('td').at(2).element.textContent).toContain('city')
  })
})
