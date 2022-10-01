<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <p>
      {{
        $t(
          'For a guide and recipes on how to configure / customize this project check out the'
        )
      }}
      <a
        href="https://cli.vuejs.org"
        target="_blank"
        rel="noopener"
        >{{ $t('vue-cli documentation.') }}</a
      >
    </p>
    <h3>{{ $t('Installed CLI Plugins') }}</h3>
    <ul>
      <li>
        <a
          href="https://github.com/vuejs/vue-cli/tree/dev/packages/%40vue/cli-plugin-typescript"
          target="_blank"
          rel="noopener"
          >{{ $t('typescript') }}</a
        >
      </li>
    </ul>
    <h3>{{ $t('Essential Links') }}</h3>
    <ul>
      <li>
        <a
          href="https://vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('Core Docs') }}</a
        >
      </li>
      <li>
        <a
          href="https://forum.vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('Forum') }}</a
        >
      </li>
      <li>
        <a
          href="https://chat.vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('Community Chat') }}</a
        >
      </li>
      <li>
        <a
          href="https://twitter.com/vuejs"
          target="_blank"
          rel="noopener"
          >{{ $t('Twitter') }}</a
        >
      </li>
      <li>
        <a
          href="https://news.vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('News') }}</a
        >
      </li>
    </ul>
    <h3>{{ $t('Ecosystem') }}</h3>
    <ul>
      <li>
        <a
          href="https://router.vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('vue-router') }}</a
        >
      </li>
      <li>
        <a
          href="https://github.com/vuejs/vue-devtools#vue-devtools"
          target="_blank"
          rel="noopener"
          >{{ $t('vue-devtools') }}</a
        >
      </li>
      <li>
        <a
          href="https://vue-loader.vuejs.org"
          target="_blank"
          rel="noopener"
          >{{ $t('vue-loader') }}</a
        >
      </li>
      <li>
        <a
          href="https://github.com/vuejs/awesome-vue"
          target="_blank"
          rel="noopener"
          >{{ $t('awesome-vue') }}</a
        >
      </li>
    </ul>
  </div>
</template>

<script setup lang="ts">
export interface HelloWorldProps {
  msg: string;
}

const props = withDefaults(defineProps<HelloWorldProps>(), {
  msg: '',
});
</script>

<!-- Add 'scoped' attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}

ul {
  padding: 0;
  list-style-type: none;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
