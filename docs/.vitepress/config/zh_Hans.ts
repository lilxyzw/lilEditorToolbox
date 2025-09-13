import { defineConfig } from 'vitepress'

const langName = '/zh_Hans';

export const zh_Hans = defineConfig({
  lang: 'zh_Hans',
  description: "此包包含多种实用的编辑器扩展功能。",
  themeConfig: {
    logo: '/images/logo.svg',
    nav: [
      { text: '主页', link: langName + '/' },
      { text: '文档', link: langName + '/docs/', activeMatch: '/docs/' }
    ],
    sidebar: [
      {
        text: '文档',
        link: langName + '/docs/',
        collapsed: false,
        items: [
          { text: '设置', link: langName + '/docs/Settings' },
          { text: 'AnimatorController 相关扩展菜单', link: langName + '/docs/AnimatorControllerEditorMenu' },
          { text: '缺失资源识别工具', link: langName + '/docs/Grimoire' },
          { text: '保存 PlayMode 下的修改', link: langName + '/docs/PlayModeSaver' },
          { text: '场景视图扩展', link: langName + '/docs/SceneExtension' },
          {
            text: 'EditorWindow',
            items: [
              { text: '打开 Unity 相关文件夹的工具', link: langName + '/docs/EditorWindow/FolderOpener' },
              { text: '以 JSON 格式查看并编辑任意对象的工具', link: langName + '/docs/EditorWindow/JsonObjectViewer' },
              { text: '缺失引用查找工具', link: langName + '/docs/EditorWindow/MissingFinder' },
              { text: '批量替换所有对象引用工具', link: langName + '/docs/EditorWindow/ReferenceReplacer' },
              { text: 'Scene 视图高分辨率截图工具', link: langName + '/docs/EditorWindow/SceneCapture' },
              { text: '任意对象收纳箱', link: langName + '/docs/EditorWindow/SelectionInventory' },
              { text: 'Shader 关键字查看工具', link: langName + '/docs/EditorWindow/ShaderKeywordViewer' },
              { text: '扩展 Inspector', link: langName + '/docs/EditorWindow/TabInspector' },
              { text: '纹理通道合并工具', link: langName + '/docs/EditorWindow/TexturePacker' },
              { text: 'Transform 初始化/复制工具', link: langName + '/docs/EditorWindow/TransformResetter' },
            ]
          },
          {
            text: 'Components',
            items: [
              { text: 'CameraMover', link: langName + '/docs/Components/CameraMover' },
              { text: 'CustomLightmapping', link: langName + '/docs/Components/CustomLightmapping' },
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
          zh_Hans: {
            translations: {
              button: {
                buttonText: '搜索',
                buttonAriaLabel: '搜索'
              },
              modal: {
                displayDetails: '显示详细列表',
                resetButtonTitle: '重置搜索',
                backButtonTitle: '关闭搜索',
                noResultsText: '未找到结果',
                footer: {
                  selectText: '选择',
                  selectKeyAriaLabel: '回车',
                  navigateText: '切换',
                  navigateUpKeyAriaLabel: '上箭头',
                  navigateDownKeyAriaLabel: '下箭头',
                  closeText: '关闭',
                  closeKeyAriaLabel: 'ESC'
                }
              }
            }
          }
        }
      }
    },
    lastUpdated: {
      text: '更新时间',
      formatOptions: {
        dateStyle: 'full',
        timeStyle: 'medium'
      }
    }
  }
})
