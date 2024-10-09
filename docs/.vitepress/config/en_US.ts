import { defineConfig } from 'vitepress'

const langName = '/en_US';

export const en_US = defineConfig({
  lang: 'en_US',
  description: "This package contains various useful editor extensions.",
  themeConfig: {
    logo: '/images/logo.svg',
    nav: [
      { text: 'Home', link: langName + '/' },
      { text: 'Document', link: langName + '/docs/', activeMatch: '/docs/' }
    ],
    sidebar: [
      {
        text: 'Document',
        link: langName + '/docs/',
        collapsed: false,
        items: [
          { text: 'Settings', link: langName + '/docs/Settings' },
          { text: 'Missing Asset Identification Tool', link: langName + '/docs/Grimoire' },
          { text: 'Scene View Extensions', link: langName + '/docs/SceneExtension' },
          {
            text: 'EditorWindow',
            items: [
              { text: 'Tool to open Unity-related folders', link: langName + '/docs/EditorWindow/FolderOpener' },
              { text: 'A tool to view and edit any object in json format', link: langName + '/docs/EditorWindow/JsonObjectViewer' },
              { text: 'Missing reference finder tool', link: langName + '/docs/EditorWindow/MissingFinder' },
              { text: 'Batch replacement tool for all object references', link: langName + '/docs/EditorWindow/ReferenceReplacer' },
              { text: 'High-resolution capture tool for Scene View', link: langName + '/docs/EditorWindow/SceneCapture' },
              { text: 'Shader keyword check tool', link: langName + '/docs/EditorWindow/ShaderKeywordViewer' },
              { text: 'Extended Inspector', link: langName + '/docs/EditorWindow/TabInspector' },
              { text: 'Texture Channel Packing Tool', link: langName + '/docs/EditorWindow/TexturePacker' },
              { text: 'Transform Initialization/Copy Tool', link: langName + '/docs/EditorWindow/TransformResetter' },
            ]
          },
          {
            text: 'Components',
            items: [
              { text: 'ObjectMarker', link: langName + '/docs/Components/ObjectMarker' },
              { text: 'SceneMSAA', link: langName + '/docs/Components/SceneMSAA' },
            ]
          },
        ]
      },
    ],
    search: {
      provider: 'local',
      options: {
        locales: {
          en_US: {
            translations: {
              button: {
                buttonText: 'Search',
                buttonAriaLabel: 'Search'
              },
              modal: {
                displayDetails: 'Display detailed list',
                resetButtonTitle: 'Reset search',
                backButtonTitle: 'Close search',
                noResultsText: 'No results for',
                footer: {
                  selectText: 'to select',
                  selectKeyAriaLabel: 'enter',
                  navigateText: 'to navigate',
                  navigateUpKeyAriaLabel: 'up arrow',
                  navigateDownKeyAriaLabel: 'down arrow',
                  closeText: 'to close',
                  closeKeyAriaLabel: 'escape'
                }
              }
            }
          }
        }
      }
    },
    lastUpdated: {
      text: 'Updated at',
      formatOptions: {
        dateStyle: 'full',
        timeStyle: 'medium'
      }
    }
  }
})
