// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-nocheck
import WorkInfoStep from '@core-public/components/form-stepper/form-steps/WorkInfoStep.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'
import { createTestingPinia } from '@pinia/testing'

const pinia = createTestingPinia()
const localVue = createLocalVue()
const tMock = {
  $t: text => text,
}

describe('WorkInfoStep', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    wrapper = mount(WorkInfoStep, {
      localVue,
      vuetify,
      pinia,
      mocks: tMock,
      propsData: {
        handleNextSection: () => null,
      },
      data() {
        return {
          valid: false,
        }
      },
    })
  })

  it('Should match the snapshot', () => {
    expect(wrapper.html()).toMatchSnapshot()
  })

  it('Should render the header', () => {
    expect(wrapper.text()).toContain('Employment Status')
  })

  it('Should display the menu on click', async () => {
    await wrapper.find('#select').trigger('click')
    expect(wrapper.text()).toContain('Unemployed')
  })
})
