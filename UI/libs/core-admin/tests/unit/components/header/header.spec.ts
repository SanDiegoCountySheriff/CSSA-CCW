/* eslint-disable @typescript-eslint/ban-ts-comment */
import Header from '@core-admin/components/header/Header.vue'
import VueRouter from 'vue-router'
import Vuetify from 'vuetify'
import { createTestingPinia } from '@pinia/testing'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const pinia = createTestingPinia()
const tMock = {
  $t: text => text,
}

localVue.use(VueRouter)
const router = new VueRouter()

describe('Header', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(Header, {
      localVue,
      vuetify,
      pinia,
      router,
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
