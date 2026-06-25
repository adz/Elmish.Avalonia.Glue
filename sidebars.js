/** @type {import('@docusaurus/plugin-content-docs').SidebarsConfig} */
const sidebars = {
  docsSidebar: [
    'intro',
    {
      type: 'category',
      label: 'Guides',
      items: [
        {
          type: 'category',
          label: 'Start',
          items: ['guides/start', 'guides/start/preview-and-design'],
        },
        {
          type: 'category',
          label: 'Integrate',
          items: ['guides/integrate'],
        },
        {
          type: 'category',
          label: 'Understand',
          items: [
            'guides/understand',
            'guides/understand/shared-substrate',
            'guides/understand/projection-family',
            'guides/understand/elmview-family',
          ],
        },
        {
          type: 'category',
          label: 'Model',
          items: ['guides/model'],
        },
        {
          type: 'category',
          label: 'Measure',
          items: ['guides/measure', 'guides/measure/comparison'],
        },
      ],
    },
    {
      type: 'category',
      label: 'Integrations',
      items: ['integrations/index', 'integrations/avalonia', 'integrations/elmish'],
    },
    {
      type: 'category',
      label: 'API',
      items: [
        'api/index',
        {
          type: 'category',
          label: 'Core',
          items: ['api/core/index', 'api/core/host-lifetime', 'api/core/snapshots', 'api/core/collections', 'api/core/projections'],
        },
        {
          type: 'category',
          label: 'Avalonia',
          items: ['api/avalonia/index', 'api/avalonia/dispatcher', 'api/avalonia/aliases'],
        },
        {
          type: 'category',
          label: 'Projection',
          items: ['api/projection/index', 'api/projection/snapshot-host', 'api/projection/keyed-collections'],
        },
        {
          type: 'category',
          label: 'ElmView',
          items: ['api/elmview/index', 'api/elmview/hosts', 'api/elmview/write-back', 'api/elmview/nodes'],
        },
      ],
    },
    {
      type: 'category',
      label: 'Examples',
      items: ['examples/index', 'examples/projection-snapshot-host', 'examples/elmview-write-back', 'examples/keyed-collection-sync'],
    },
  ],
};

module.exports = sidebars;
