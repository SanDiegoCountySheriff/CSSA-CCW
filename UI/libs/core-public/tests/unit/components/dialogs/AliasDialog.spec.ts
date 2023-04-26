/* eslint-disable @typescript-eslint/ban-ts-comment */
import AliasDialog from '@shared-ui/components/dialogs/AliasDialog.vue'
import Vuetify from 'vuetify'
import { createLocalVue, mount } from '@vue/test-utils'

const localVue = createLocalVue()
const tMock = {
  $t: text => text,
}

describe('AliasDialog', () => {
  let vuetify
  let wrapper

  beforeEach(() => {
    vuetify = new Vuetify()
    //@ts-ignore
    wrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
    })
  })
  afterEach(() => {
    wrapper.destroy()
  })

  it('should match the snapshot', async () => {
    await wrapper.find('#add-alias-btn').trigger('click')
    expect(wrapper.html()).toMatchSnapshot()
  })

  it('should render the add button', () => {
    expect(wrapper.find('button').exists()).toBeTruthy()
    expect(wrapper.find('form').exists()).toBeFalsy()
  })

  it('should render the form on button click', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
        }
      },
    })

    await onWrapper.find('#add-alias-btn').trigger('click')
    expect(onWrapper.findAll('.v-input')).toHaveLength(6)
    onWrapper.destroy()
  })
  it('should not allow submission', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
      data() {
        return {
          dialog: true,
        }
      },
    })

    await onWrapper.find('#add-alias-btn').trigger('click')
    expect(onWrapper.find('#submit-btn').attributes().disabled).toBeTruthy()
    onWrapper.destroy()
  })

  it('should allow submission', async () => {
    //@ts-ignore
    const onWrapper = mount(AliasDialog, {
      localVue,
      vuetify,
      mocks: tMock,
    })

    await onWrapper.find('#add-alias-btn').trigger('click')
    const lastName = onWrapper.find('#last-name')

    await lastName.setValue('tdfg')
    const firstName = onWrapper.find('#first-name')

    await firstName.setValue('tsgsd')
    await wrapper.vm.$nextTick()
    expect(onWrapper.find('#submit-btn').attributes('disabled')).toBeUndefined()
    onWrapper.destroy()
  })
})
