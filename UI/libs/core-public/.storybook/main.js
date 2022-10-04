/* eslint-disable @typescript-eslint/no-var-requires */
const rootMain = require('../../../.storybook/main');
const path = require('path');

module.exports = {
  ...rootMain,

  stories: [
    ...rootMain.stories,
    '../src/lib/**/*.stories.mdx',
    '../src/lib/**/*.stories.@(js|jsx|ts|tsx)',
  ],
  addons: [...rootMain.addons],
  webpackFinal: async (config, { configType }) => {
    // apply any global webpack configs that might have been specified in .storybook/main.js
    if (rootMain.webpackFinal) {
      config = await rootMain.webpackFinal(config, { configType });
    }
    config.resolve.modules = [path.resolve(__dirname, '..'), 'node_modules'];
    config.resolve.alias = {
      ...config.resolve.alias,
      '~storybook': path.resolve(__dirname),
      '@core-public': path.resolve(__dirname, '../src/lib'),
      '@shared-ui': path.resolve(__dirname, '../../shared/ui/src/lib'),
      '@shared-utils': path.resolve(__dirname, '../../shared/utils/src/lib'),
    };
    config.resolve.extensions.push(
      '.vue',
      '.css',
      '.less',
      '.scss',
      '.sass',
      '.html',
      '.svg',
      '.ts',
      '.js'
    );

    config.module.rules.push({
      test: /\.mjs$/,
      include: /node_modules/,
      type: 'javascript/auto',
    });

    config.module.rules.push({
      test: /\.scss$/,
      use: [
        require.resolve('vue-style-loader'),
        require.resolve('css-loader'),
        require.resolve('sass-loader'),
      ],
    });

    // add your own webpack tweaks if needed

    return config;
  },
};
