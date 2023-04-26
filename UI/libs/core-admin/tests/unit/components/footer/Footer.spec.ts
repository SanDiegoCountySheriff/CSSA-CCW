/* eslint-disable @typescript-eslint/ban-ts-comment */
import Footer from '@shared-ui/components/footer/Footer.vue'
import Vuetify from 'vuetify'
import { createTestingPinia } from '@pinia/testing'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const pinia = createTestingPinia()
const tMock = {
  $t: text => text,
}

describe('Footer', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(Footer, {
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
