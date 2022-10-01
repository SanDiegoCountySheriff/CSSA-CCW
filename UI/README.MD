# CSSA-CCW User Interface | Admin + Public

# Nx Plus Vue

> First class support for [Vue](https://vuejs.org/) in your [Nx](https://nx.dev/) workspace.

<div align="center">
  <img src="https://raw.githubusercontent.com/ZachJW34/nx-plus/master/libs/vue/nx-plus-vue.png">
</div>

## Intro to Nx
Nx is a smart, fast and extensible build system with first class integrated repo support and powerful integrations.

##### Philosophy
Nx has a similar design philosophy to Visual Studio Code. VSCode is a powerful text editor, and you can be very productive with it even if you don't install any extensions. The ecosystem of VSCode's extensions though is what can really level up your productivity.

Nx is similar. The core of Nx is generic, simple, and unobtrusive. Nx plugins, although very useful for many projects, are completely optional.

Please spend few minutes reading through this concepts to understand the [Mental Modal](https://nx.dev/concepts/mental-model) of Nx.

## Contents

- [Prerequisite](#prerequisite)
- [Getting Started](#getting-started)
- [Nx Dependency Graph Support](#nx-dependency-graph-support)
- [Core Coding Principals](#core-coding-principals)
- [Schematics (i.e. code generation)](#schematics-ie-code-generation)
- [Pinia State management + Library)](#pinia)
- [Vuetify Material Design Framework)](#vuetify)
- [PWA (Progressive Web App)](#PWA)
- [Builders (i.e. task runners)](#builders-ie-task-runners)
- [Enforced Project Boundaries](#enforced-project-boundaries)
- [Linting](#linting)
- [Unit Testing](#unit-testing)
- [Storybook](#storybook)
- [Git Hooks](#git-hooks)
- [Modify the Webpack Configuration](#modify-the-webpack-configuration)
- [Updating Nx Plus Vue](#updating-nx-plus-vue)

## Prerequisite

###### System Requirements

```
- NODE - v14.15.5
- NPM  - v6.14.11
```

### Nx Installation

If you have not already, with the following:

```
npm install -g nx
npm install
```

## Getting Started

### Serve Your Apps

```
nx serve admin
```
```
nx serve public
```
### Nx Dependency Graph Support

This will show you how your apps and libraries are dependent on each other in a graphical format

```
nx dep-graph
```

###### Example:
Admin application: `admin —> core-admin —> [shared-ui, shared-utils]`
Public application: `public —> core-public —> [shared-ui, shared-utils]`

### Core Coding Principals

1. Please keep 95% of the code and units under `/libs` folder (code-admin, core-public, etc). `/apps` folder should only have build and configuration related files like main.js, App.vue etc. 

2. Implement sharable features under `shared-ui` and `shared-utils` libraries which will be consumed by admin and public applications respectively.

3. Avoid importing anything between admin and public applications/libraries to eliminate the risk of bloating. Although Eslint rule is being enforced to avoid accidental imports.

4. `shared-ui` and `shared-utils` are application independent libraries, meaning it should only contain modular and reusable components without effecting one's design or layout.

5. Try to adopt Composition API vs Options API since this project is already on Vue 2.7, doing so will make Vue3 migration simple and easier.

6. Please use our automated GIT branching and PR strategy script located at root folder [git.sh](../git.sh) for creating a new branch, rebasing, and rasing PR urls.

7. Make sure to lint, format, unit test and most importantly rebase/squash your code before raising a PR and delete the stale branches once PR is merged.

>[Vue - Patterns](https://learn-vuejs.github.io/vue-patterns/patterns)
>[Vue - Composition API](https://vuejs.org/guide/typescript/composition-api.html)
>[Vue - Useful links and Performance](https://learn-vuejs.github.io/vue-patterns/patterns)

`HAPPY CODING! 🎉`

## Schematics (i.e. code generation)

### Generate Nx Application

`nx g @nx-plus/vue:app <name> [options]`

| Arguments | Description           |
| --------- | --------------------- |
| `<name>`  | The name of your app. |

| Options            | Default   | Description                                                    |
| ------------------ | --------- | -------------------------------------------------------------- |
| `--tags`           | -         | Tags to use for linting (comma-delimited).                     |
| `--directory`      | `apps`    | A directory where the project is placed.                       |
| `--style`          | `css`     | The file extension to be used for style files.                 |
| `--unitTestRunner` | `jest`    | Test runner to use for unit tests.                             |
| `--e2eTestRunner`  | `cypress` | Test runner to use for end to end (e2e) tests.                 |
| `--routing`        | `false`   | Generate routing configuration.                                |
| `--vueVersion`     | `2`       | The version of Vue.js that you want to start the project with. |
| `--skipFormat`     | `false`   | Skip formatting files.                                         |
| `--babel`          | `false`   | Add Babel support. 


### Generate Nx Library

`nx g @nx-plus/vue:lib <name> [options]`

| Arguments | Description               |
| --------- | ------------------------- |
| `<name>`  | The name of your library. |

| Options            | Default | Description                                                    |
| ------------------ | ------- | -------------------------------------------------------------- |
| `--tags`           | -       | Tags to use for linting (comma-delimited).                     |
| `--directory`      | `libs`  | A directory where the project is placed.                       |
| `--unitTestRunner` | `jest`  | Test runner to use for unit tests.                             |
| `--skipFormat`     | `false` | Skip formatting files.                                         |
| `--skipTsConfig`   | `false` | Do not update tsconfig.json for development experience.        |
| `--vueVersion`     | `2`     | The version of Vue.js that you want to start the project with. |
| `--publishable`    | `false` | Create a buildable library.                                    |
| `--babel`          | `false` | Add Babel support.                                             |

### Generate Vue Component

`nx g @nx-plus/vue:component <name> [options]`

| Arguments | Description                 |
| --------- | --------------------------- |
| `<name>`  | The name of your component. |

| Options       | Default | Description                                    |
| ------------- | ------- | ---------------------------------------------- |
| `--project`   | -       | Tags to use for linting (comma-delimited).     |
| `--directory` | -       | A directory where the component is placed.     |
| `--style`     | `css`   | The file extension to be used for style files. |                                            |

## Pinia

Pinia is the new recommended store library for Vue, it allows you to share a state across components/pages similar to Vue's Composition API.

- Devtools support
- A timeline to track actions, mutations
- Stores appear in components where they are used
- Time travel and easier debugging
- Hot module replacement
- Modify your stores without reloading your page
- Keep any existing state while developing
- Plugins: extend Pinia features with plugins
- Proper TypeScript support or autocompletion for JS users
- Server Side Rendering Support

You can see the different between Vuex and Pinia below

![Pinia logo](https://miro.medium.com/max/1400/1*5pz0ih6TUClcPyS9yF-Wmg.png)

Unlike Vuex, mutations are no longer needed with Pinia and Vue 2.7 or higher. Below is a basic setup to define pinia store with composition API. You can also use Options API if you want to.

> UI/libs/shared/ui/src/lib/stores/index.ts
``` ts
import Vue from 'vue';
import { createPinia, PiniaVuePlugin } from 'pinia';

Vue.use(PiniaVuePlugin);

export const pinia = createPinia();
```
And initialize the store to respective app under `/apps` (admin, public).
> UI/apps/admin/src/main.ts

```ts
import Vue from 'vue';
import AdminApp from './AdminApp.vue';
import { pinia, router } from '@cssa-ccw/core-admin';

Vue.config.productionTip = false;

new Vue({
  pinia,
  router,
  render: h => h(AdminApp),
}).$mount('#app');

```

Defining the store

```ts
import { AliasType } from '@shared-ui/types/defaultTypes';
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAliasStore = defineStore('alias', () => {
  const aliases = ref<AliasType>([]);
  const getAliases = computed(() => aliases.value);

  function addAlias(payload: addAlias) {
    aliases.value.push(payload);
  }

  return { aliases, getAliases, addAlias };
});
```

Access the store in vue components like below:

```ts
<script setup lang="ts">

import { useAliasStore } from 'stores';

const store = useAliasStore();
store.addAlias(payload);

</script>
```


## Vuetify

Vuetify is a complete UI framework built on top of Vue.js. The goal of vuetify is to provide developers with the tools they need to build rich and engaging user experiences. Unlike other frameworks, Vuetify is designed from the ground up to be easy to learn and rewarding to master with hundreds of carefully crafted components from the [Material Design specification](https://material.io/).

Vuetify is developed exactly according to [Material Design specification](https://material.io/) with every component meticulously crafted to be modular, responsive, and performant. Customize your application with unique and dynamic [Layouts](https://vuetifyjs.com/en/features/layouts/) and customize the styles of your components using [SASS variables](https://vuetifyjs.com/en/features/sass-variables/).

```ts
import Vue from 'vue'
import Vuetify from 'vuetify'
import '@mdi/font/css/materialdesignicons.css'
import '@shared-ui/assets/vuetify.css'

Vue.use(Vuetify)

export const vuetify = new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
})

```

## Internationalization (I18n)

[Vue-I18n](https://kazupon.github.io/vue-i18n/started.html) is internationalization plugin for Vue.js

language messages are loading through a JSON file (ex: en.json) and vue-i18n module will be initialize it for each app.

```ts
export const i18n = new VueI18n({
  locale: defaultLocale,
  fallbackLocale: 'en',
  silentTranslationWarn: true,
  messages,
});
```
```json
{
  "hello": "Welcome to Your Vue.js + TypeScript {msg} App"
}
```

You will have to make any hardcoded labels into i18n translatable string, `$t` will map any keys you loaded through JSON file.

```jsx
  <HelloWorld :msg="$t('hello', { msg: 'Admin' })" />
```

We also have a eslint plugin enabled to make sure we don't keep any stale hardcoded labels around the repo during the development process.

```ts
"plugin:@intlify/vue-i18n/recommended"
```

## PWA

Progressive Web Apps (PWAs) are web apps that use service workers, manifests, and other web-platform features in combination with progressive enhancement to give users an experience on par with native apps.

Admin and Public facing applications support PWA.

> webpack.config.js under each app's root folder

```js
const { GenerateSW } = require("workbox-webpack-plugin");
// Note: vue-plugin-pwa is not being used to create PWA boiler plate due to technical reasons.

module.exports = {
  
  configureWebpack: {
    plugins: [new GenerateSW()]
  }
};
```

Making sure the file manifest.json is linked in public/index.html.

```html
<link rel="manifest" href="<%= BASE_URL %>manifest.json">
```

Followed by adding service worker into main.ts

###### Test PWA Locally

1. Perform Production build
  `npm run build-admin`
2. Go to dist folder
  ` cd dist/apps`
3. Run http-server
  `npx http-server admin` and verify 'Add to home' button + PWA features through Lighthouse

## Builders (i.e. task runners)

### Run Local Development Server

`nx serve <name> [options]`

| Arguments | Description           |
| --------- | --------------------- |
| `<name>`  | The name of your app. |

| Options                      | Default       | Description                                                                                                                                                                                                                          |
| ---------------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `--open`                     | `false`       | Open browser on server start.                                                                                                                                                                                                        |
| `--copy`                     | `false`       | Copy url to clipboard on server start.                                                                                                                                                                                               |
| `--stdin`                    | `false`       | Close when stdin ends.                                                                                                                                                                                                               |
| `--mode`                     | `development` | Specify env mode (default: development).                                                                                                                                                                                             |
| `--host`                     | `0.0.0.0`     | Specify host (default: 0.0.0.0).                                                                                                                                                                                                     |
| `--port`                     | `8080`        | Specify port (default: 8080).                                                                                                                                                                                                        |
| `--https`                    | `false`       | Use https (default: false).                                                                                                                                                                                                          |
| `--public`                   | -             | Specify the public network URL for the HMR client.                                                                                                                                                                                   |
| `--skipPlugins`              | -             | Comma-separated list of plugin names to skip for this run.                                                                                                                                                                           |
| `--browserTarget`            | -             | Target to serve.                                                                                                                                                                                                                     |
| `--watch`                    | `true`        | Watch for changes.                                                                                                                                                                                                                   |
| `--publicPath`               | `/`           | The base URL your application bundle will be deployed at.                                                                                                                                                                            |
| `--transpileDependencies`    | []            | By default babel-loader ignores all files inside node_modules. If you want to explicitly transpile a dependency with Babel, you can list it in this option.                                                                          |
| `css.requireModuleExtension` | `true`        | By default, only files that end in `*.module.[ext]` are treated as CSS modules. Setting this to `false` will allow you to drop `.module` in the filenames and treat all `*.(css\|scss\|sass\|less\|styl(us)?)` files as CSS modules. |
| `css.extract`                | `false`       | Whether to extract CSS in your components into a standalone CSS file (instead of inlined in JavaScript and injected dynamically).                                                                                                    |
| `css.sourceMap`              | `false`       | Whether to enable source maps for CSS. Setting this to `true` may affect build performance.                                                                                                                                          |
| `css.loaderOptions`          | `{}`          | Pass options to CSS-related loaders.                                                                                                                                                                                                 |
| `devServer`                  | `{}`          | All options for `webpack-dev-server` are supported.                                                                                                                                                                                  |

### Run Production Build

`nx build <name> [options]`

| Arguments | Description           |
| --------- | --------------------- |
| `<name>`  | The name of your app. |

| Options                      | Default       | Description                                                                                                                                                                                                                          |
| ---------------------------- | ------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `--mode`                     | `development` | Specify env mode (default: development).                                                                                                                                                                                             |
| `--dest`                     | -             | Specify output directory.                                                                                                                                                                                                            |
| `--clean`                    | `true`        | Remove the dist directory before building the project.                                                                                                                                                                               |
| `--report`                   | `false`       | Generate report.html to help analyze bundle content.                                                                                                                                                                                 |
| `--reportJson`               | `false`       | Generate report.json to help analyze bundle content.                                                                                                                                                                                 |
| `--skipPlugins`              | -             | Comma-separated list of plugin names to skip for this run.                                                                                                                                                                           |
| `--watch`                    | `false`       | Watch for changes.                                                                                                                                                                                                                   |
| `--index`                    | -             | The path of a file to use for the application's HTML index. The filename of the specified path will be used for the generated file and will be created in the root of the application's configured output path.                      |
| `--main`                     | -             | The full path for the main entry point to the app, relative to the current workspace.                                                                                                                                                |
| `--tsConfig`                 | -             | The full path for the TypeScript configuration file, relative to the current workspace.                                                                                                                                              |
| `--publicPath`               | `/`           | The base URL your application bundle will be deployed at.                                                                                                                                                                            |
| `--filenameHashing`          | `false`       | Generated static assets contain hashes in their filenames for better caching control.                                                                                                                                                |
| `--productionSourceMap`      | `false`       | Setting this to `false` can speed up production builds if you don't need source maps for production.                                                                                                                                 |
| `--transpileDependencies`    | []            | By default babel-loader ignores all files inside node_modules. If you want to explicitly transpile a dependency with Babel, you can list it in this option.                                                                          |
| `css.requireModuleExtension` | `true`        | By default, only files that end in `*.module.[ext]` are treated as CSS modules. Setting this to `false` will allow you to drop `.module` in the filenames and treat all `*.(css\|scss\|sass\|less\|styl(us)?)` files as CSS modules. |
| `css.extract`                | `false`       | Whether to extract CSS in your components into a standalone CSS file (instead of inlined in JavaScript and injected dynamically).                                                                                                    |
| `css.sourceMap`              | `false`       | Whether to enable source maps for CSS. Setting this to `true` may affect build performance.                                                                                                                                          |
| `css.loaderOptions`          | `{}`          | Pass options to CSS-related loaders.                                                                                                                                                                                                 |
| `--stdin`                    | `false`       | Close when stdin ends.                                                                                                                                                                                                               |

### Run Nx Library Production Build

`nx build <name> [options]`

| Arguments | Description               |
| --------- | ------------------------- |
| `<name>`  | The name of your library. |

| Options                      | Default                | Description                                                                                                                                                                                                                          |
| ---------------------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `--dest`                     | -                      | Specify output directory.                                                                                                                                                                                                            |
| `--clean`                    | `true`                 | Remove the dist directory before building the project.                                                                                                                                                                               |
| `--report`                   | `false`                | Generate report.html to help analyze bundle content.                                                                                                                                                                                 |
| `--reportJson`               | `false`                | Generate report.json to help analyze bundle content.                                                                                                                                                                                 |
| `--skipPlugins`              | -                      | Comma-separated list of plugin names to skip for this run.                                                                                                                                                                           |
| `--watch`                    | `false`                | Watch for changes.                                                                                                                                                                                                                   |
| `--entry`                    | -                      | The full path for the main entry point to your library, relative to the current workspace.                                                                                                                                           |
| `--tsConfig`                 | -                      | The full path for the TypeScript configuration file, relative to the current workspace.                                                                                                                                              |
| `--inlineVue`                | `false`                | Include the Vue module in the final bundle of library.                                                                                                                                                                               |
| `--formats`                  | `commonjs,umd,umd-min` | List of output formats for library builds.                                                                                                                                                                                           |
| `--name`                     | -                      | Name for library.                                                                                                                                                                                                                    |
| `--filename`                 | -                      | File name for output.                                                                                                                                                                                                                |
| `--transpileDependencies`    | []                     | By default babel-loader ignores all files inside node_modules. If you want to explicitly transpile a dependency with Babel, you can list it in this option.                                                                          |
| `css.requireModuleExtension` | `true`                 | By default, only files that end in `*.module.[ext]` are treated as CSS modules. Setting this to `false` will allow you to drop `.module` in the filenames and treat all `*.(css\|scss\|sass\|less\|styl(us)?)` files as CSS modules. |
| `css.extract`                | `true`                 | Whether to extract CSS in your components into a standalone CSS file (instead of inlined in JavaScript and injected dynamically).                                                                                                    |
| `css.sourceMap`              | `false`                | Whether to enable source maps for CSS. Setting this to `true` may affect build performance.                                                                                                                                          |
| `css.loaderOptions`          | `{}`                   | Pass options to CSS-related loaders.                                                                                                                                                                                                 |

## Enforced Project Boundaries

If you partition your code into well-defined cohesive units, even a small organization will end up with a dozen apps and dozens or hundreds of libs. If all of them can depend on each other freely, chaos will ensue, and the workspace will become unmanageable.

To help with that Nx uses code analysis to make sure projects can only depend on each other's well-defined public API. It also allows you to declaratively impose constraints on how projects can depend on each other.

Nx provides an ‘enforce-module-boundaries’ eslint rule that enforces the public API of projects in the repo. Each project defines its public API in an index.ts (or index.js) file. If another project tries to import a variable from a file deep within a different project, an error will be thrown during linting.

```json
      "rules": {
        "@nrwl/nx/enforce-module-boundaries": [
          "error",
          {
            "enforceBuildableLibDependency": true,
            "allowCircularSelfDependency": true,
            "allow": [],
            "depConstraints": [
              {
                "sourceTag": "@public",
                "onlyDependOnLibsWithTags": ["@public", "@core-public", "@shared-ui", "@shared-utils"]
              },
              {
                "sourceTag": "@admin",
                "onlyDependOnLibsWithTags": ["@admin", "@core-admin", "@shared-ui", "@shared-utils"]
              },
              {
                "sourceTag": "@core-public",
                "onlyDependOnLibsWithTags": ["@core-public", "@shared-ui", "@shared-utils"]
              },
              {
                "sourceTag": "@core-admin",
                "onlyDependOnLibsWithTags": ["@core-admin", "@shared-ui", "@shared-utils"]
              },
              {
                "sourceTag": "@shared-ui",
                "onlyDependOnLibsWithTags": ["@shared-ui", "@shared-utils"]
              },
              {
                "sourceTag": "@shared-utils",
                "onlyDependOnLibsWithTags": ["@shared-utils", "@shared-ui"]
              }
            ]
          }
        ]
      }

```

> Located in project.json
```json
 "tags": ["@admin"]

```

## Linting

`nx lint <name> [options]`

We use `@nrwl/linter` for linting, so the options are as documented [here](https://github.com/nrwl/nx/tree/master/packages/linter).

## Unit Testing

`nx test <name> [options]`

We use `@nrwl/jest` for unit testing, so the options are as documented [here](https://github.com/nrwl/nx/tree/master/packages/jest).

## Storybook

Storybook is an open source tool for building UI components and pages in isolation. It streamlines UI development, testing, and documentation.

To serve [Storybook](https://storybook.js.org) using this command

```bash
nx storybook core-public
nx storybook core-admin
nx storybook shared-ui
```

To build [Storybook](https://storybook.js.org) using this command

```bash
nx build-storybook <name> [project]
```

## Git Hooks

[Husky](https://typicode.github.io/husky/#/) and [Lint-staged](https://github.com/okonet/lint-staged#readme) are used to maintain the quality of this project by checking js/ts lint, formatting, style lint and running test cases before your code is pushed over to origin for a PR or CI/CD.

- `git commit -m "msg"` will trigger pre-commit hook to run linting & formatting on only effected uncommitted **'staged'** changes before you commit your changes. Therefore, If an issue is recognized effected files will be un-staged, please fix the issues and stage it before u try to commit it again.

```json
{
  "*.{js,jsx,ts,tsx,css,scss,md,vue}": [
    "nx format:write --uncommitted"
  ],
  "*.{js,jsx,ts,tsx,vue}": [
    "nx affected --target=lint  --uncommitted"
  ],
  "*.{css,scss,vue}": [
    "nx affected --target=stylelint --uncommitted"
  ]
}

```
- `git push origin` will trigger pre-push hook to run unit tests on effected files to make sure test cases are passing before you push to origin.

```
nx affected:test --uncommitted
```

## Modify the Webpack Configuration

Modify the webpack config by exporting an Object or Function from your project's `configure-webpack.js` file.

> If your project does not have a `configure-webpack.js` file, then simply add it at the root of your project.

If the value is an Object, it will be merged into the final config using [`webpack-merge`](https://github.com/survivejs/webpack-merge).

If the value is a function, it will receive the resolved config as the argument. The function can either mutate the config and return nothing, OR return a cloned or merged version of the config.

For more information see the [Vue CLI documentation](https://cli.vuejs.org/config/#configurewebpack).

## Updating Nx Plus Vue

Nx Plus Vue provides migrations which help you stay up to date with the latest version of Nx Plus Vue.

Not only do we migrate the version of Nx Plus Vue, but we also update the versions of dependencies which we install such as `vue` and `vue-router`.

We recommend waiting for Nx Plus Vue to update these dependencies for you as we verify that these versions work together.

## How to Migrate

#### Generate migrations.json

All you have to do to update Nx Plus Vue to the latest version is run the following:

```
nx migrate @nx-plus/vue
nx migrate @nx-plus/vue@version # you can also specify version
```

This will fetch the specified version of `@nx-plus/vue`, analyze the dependencies and fetch all the dependent packages. The process will keep going until the whole tree of dependencies is resolved. This will result in:

- `package.json` being updated
- `migrations.json` being generated

At this point, no packages have been installed, and no other files have been touched.

Now, you can inspect `package.json` to see if the changes make sense and install the packages by running `npm install` or `yarn`.

#### Run Migrations

`migrations.json` contains the transformations that must run to prepare the workspace to the newly installed versions of packages. To run all the migrations, invoke:

```
nx migrate --run-migrations=migrations.json
```

#### Cheat Sheet

For any new developers who are adopting Nx workspaces, following cheat sheet should help speed up the development process
<br>

  ![CCW-UI-Cheat-sheet](./README_CHEETSHEET.png?raw=true "Nx Cheat sheet")