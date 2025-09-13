import { defineConfig } from 'vitepress'

const langName = '/ja_JP';

export const ja_JP = defineConfig({
  lang: 'ja_JP',
  description: "様々な便利系エディタ拡張が入ったパッケージです。",
  themeConfig: {
    logo: '/images/logo.svg',
    nav: [
      { text: 'ホーム', link: langName + '/' },
      { text: 'ドキュメント', link: langName + '/docs/', activeMatch: '/docs/' }
    ],
    sidebar: [
      {
        text: 'ドキュメント',
        link: langName + '/docs/',
        collapsed: false,
        items: [
          { text: '設定', link: langName + '/docs/Settings' },
          { text: 'AnimatorController関係の拡張メニュー', link: langName + '/docs/AnimatorControllerEditorMenu' },
          { text: '不足アセット特定ツール', link: langName + '/docs/Grimoire' },
          { text: 'PlayModeでの変更を保存', link: langName + '/docs/PlayModeSaver' },
          { text: 'Scene View拡張', link: langName + '/docs/SceneExtension' },
          {
            text: 'EditorWindow',
            items: [
              { text: 'Unity関連フォルダを開くツール', link: langName + '/docs/EditorWindow/FolderOpener' },
              { text: '任意のObjectをjson形式で確認＆編集するツール', link: langName + '/docs/EditorWindow/JsonObjectViewer' },
              { text: 'Missing参照発見ツール', link: langName + '/docs/EditorWindow/MissingFinder' },
              { text: 'オブジェクト参照を何でも一括置き換えツール', link: langName + '/docs/EditorWindow/ReferenceReplacer' },
              { text: 'Scene Viewの高解像度キャプチャツール', link: langName + '/docs/EditorWindow/SceneCapture' },
              { text: 'オブジェクトのインベントリー', link: langName + '/docs/EditorWindow/SelectionInventory' },
              { text: 'シェーダー本体のキーワード確認ツール', link: langName + '/docs/EditorWindow/ShaderKeywordViewer' },
              { text: '拡張Inspector', link: langName + '/docs/EditorWindow/TabInspector' },
              { text: 'テクスチャチャンネルパッキングツール', link: langName + '/docs/EditorWindow/TexturePacker' },
              { text: 'Transform初期化・コピーツール', link: langName + '/docs/EditorWindow/TransformResetter' },
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
          ja_JP: {
            translations: {
              button: {
                buttonText: '検索',
                buttonAriaLabel: '検索'
              },
              modal: {
                displayDetails: '詳細リストを表示',
                resetButtonTitle: '検索条件を削除',
                backButtonTitle: '検索を閉じる',
                noResultsText: '見つかりませんでした。',
                footer: {
                  selectText: '選択',
                  selectKeyAriaLabel: 'エンター',
                  navigateText: '切り替え',
                  navigateUpKeyAriaLabel: '上矢印',
                  navigateDownKeyAriaLabel: '下矢印',
                  closeText: '閉じる',
                  closeKeyAriaLabel: 'エスケープ'
                }
              }
            }
          }
        }
      }
    },
    lastUpdated: {
      text: '最終更新',
      formatOptions: {
        dateStyle: 'full',
        timeStyle: 'medium'
      }
    }
  }
})
