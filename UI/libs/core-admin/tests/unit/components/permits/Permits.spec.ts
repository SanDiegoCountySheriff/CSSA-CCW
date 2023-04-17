/* eslint-disable @typescript-eslint/ban-ts-comment */
import Permits from '@core-admin/components/permits/Permits.vue'
import Vuetify from 'vuetify'
import { createTestingPinia } from '@pinia/testing'
import { useQuery } from '@tanstack/vue-query'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const pinia = createTestingPinia()
const tMock = {
  $t: text => text,
}

jest.mock('@tanstack/vue-query', () => ({
  useQuery: jest.fn(),
}))

describe('Permits', () => {
  let vuetify
  let wrapper
  let result

  beforeEach(() => {
    vuetify = new Vuetify()
    result = {
      data: [],
      isLoading: false,
    }
    ;(useQuery as jest.Mock).mockImplementation(() => result)
    //@ts-ignore
    wrapper = mount(Permits, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
    })
  })

  it('should match the snapshot', async () => {
    result.isLoading = false
    expect(wrapper.html()).toMatchSnapshot()
  })
})
