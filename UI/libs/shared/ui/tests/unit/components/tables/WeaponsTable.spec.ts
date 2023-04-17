/* eslint-disable @typescript-eslint/ban-ts-comment */
import Vuetify from 'vuetify'
import WeaponsTable from '@shared-ui/components/tables/WeaponsTable.vue'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const weapon = {
  make: 'walther',
  model: 'pps',
  caliber: '9mm',
  serialNumber: '12345',
}

describe('WeaponsTable', () => {
  let vuetify
  let wrapper
  const tMock = {
    $t: text => text,
  }

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(WeaponsTable, {
      localVue,
      vuetify,
      mocks: tMock,
      propsData: {
        weapons: [weapon],
      },
    })
  })

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })

  it('Should render the weapon correctly', () => {
    const tableData = wrapper.findAll('td')

    expect(tableData.at(0).element.textContent).toContain('walther')
    expect(tableData.at(1).element.textContent).toContain('pps')
    expect(tableData.at(2).element.textContent).toContain('9mm')
    expect(tableData.at(3).element.textContent).toContain('12345')
  })
})
