## What is Composition API? [Documentation](https://vuejs.org/guide/extras/composition-api-faq.html#what-is-composition-api)

Composition API is a set of APIs that allows us to author Vue components using imported functions instead of declaring options. It is an umbrella term that covers the following APIs:

[Reactivity API](https://vuejs.org/api/reactivity-core.html), e.g. ref() and reactive(), that allows us to directly create reactive state, computed state, and watchers.

[Lifecycle Hooks](https://vuejs.org/api/composition-api-lifecycle.html), e.g. onMounted() and onUnmounted(), that allow us to programmatically hook into the component lifecycle.

[Dependency Injection](https://vuejs.org/api/composition-api-dependency-injection.html), i.e. provide() and inject(), that allow us to leverage Vue's dependency injection system while using Reactivity APIs.

Composition API is a built-in feature of Vue 3 and [Vue 2.7](https://blog.vuejs.org/posts/vue-2-7-naruto.html). It is also primarily used together with the [script setup](https://vuejs.org/api/sfc-script-setup.html) syntax in Single-File Components. Here's a basic example of a component using Composition API:

```ts
<template>
  <input ref='el' />
</template>

<script setup lang='ts'>
import { ref, onMounted } from 'vue';

export interface Props {
  msg?: string
  labels?: string[]
};

const props = withDefaults(defineProps<Props>(), {
  msg: 'hello',
  labels: () => ['one', 'two']
});

const emit = defineEmits<{
  (e: 'change', id: number): void
  (e: 'update', value: string): void
}>();

const year = ref(2020);
const book = reactive({ title: 'Vue 3 Guide' });

function handleChange(event: Event) {
  console.log((event.target as HTMLInputElement).value)
};

const el = ref<HTMLInputElement | null>(null)

onMounted(() => {
  el.value?.focus()
})
</script>

<style lang='ts'>
// scss BEM based style system
</style>

```