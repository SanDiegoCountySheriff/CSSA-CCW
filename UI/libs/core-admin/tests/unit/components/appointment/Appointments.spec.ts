/* eslint-disable @typescript-eslint/ban-ts-comment */
import Appointments from '@core-admin/components/appointment/Appointments.vue'
import Vuetify from 'vuetify'
import { createTestingPinia } from '@pinia/testing'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const pinia = createTestingPinia()
const tMock = {
  $t: text => text,
}

describe('Appointments', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(Appointments, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
    })
  })
  afterEach(() => {
    wrapper.destroy()
  })

  it('should match the snapshot', async () => {
    expect(wrapper.html()).toMatchSnapshot()
  })
})
