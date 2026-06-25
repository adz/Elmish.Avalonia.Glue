const path = require('path');

const repoRoot = __dirname.replace(/\\/g, '/');
const sourceBaseUrl = 'https://github.com/adz/Elmish.Avalonia.Glue/blob/main/';

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Elmish.Avalonia.Glue',
  tagline: 'Avalonia stays normal; authored UI shape moves into F#.',
  favicon: 'img/favicon.svg',
  url: 'https://adz.github.io',
  baseUrl: '/Elmish.Avalonia.Glue/',
  organizationName: 'adz',
  projectName: 'Elmish.Avalonia.Glue',
  onBrokenLinks: 'throw',
  customFields: {
    sourceBaseUrl,
  },
  markdown: {
    hooks: {
      onBrokenMarkdownLinks: 'warn',
    },
  },
  presets: [
    [
      'classic',
      {
        docs: {
          path: 'docs',
          routeBasePath: 'docs',
          sidebarPath: require.resolve('./sidebars.js'),
          showLastUpdateAuthor: false,
          showLastUpdateTime: false
        },
        blog: false,
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      },
    ],
  ],
  themeConfig: {
    navbar: {
      title: 'Elmish.Avalonia.Glue',
      logo: {
        alt: 'Elmish.Avalonia.Glue',
        src: 'img/logo.svg',
      },
      items: [
        { to: '/docs/intro', label: 'Docs', position: 'left' },
        { to: '/docs/guides/start', label: 'Start', position: 'left' },
        { to: '/docs/api', label: 'API', position: 'left' },
        { to: '/docs/examples', label: 'Examples', position: 'left' },
        { href: sourceBaseUrl, label: 'Source', position: 'right' },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            { label: 'Start', to: '/docs/guides/start' },
            { label: 'Integrations', to: '/docs/integrations' },
            { label: 'API', to: '/docs/api' },
          ],
        },
        {
          title: 'Families',
          items: [
            { label: 'Projection', to: '/docs/api/projection' },
            { label: 'ElmView', to: '/docs/api/elmview' },
            { label: 'Core', to: '/docs/api/core' },
          ],
        },
        {
          title: 'Examples',
          items: [
            { label: 'Executable examples', to: '/docs/examples' },
            { label: 'Sample matrix', to: '/docs/guides/model' },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Elmish.Avalonia.Glue`,
    },
    prism: {
      additionalLanguages: ['fsharp', 'csharp'],
    },
  },
};

module.exports = config;
