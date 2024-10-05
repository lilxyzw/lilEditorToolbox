lilEditorToolbox
====

単体で出すほどでもないエディタ拡張をまとめたものです。機能のオンオフは基本的に`Preferences`の`lilEditorToolbox`タブからできます。

## インポート拡張

- アセットをD&Dした階層に同名のファイルがある場合上書きインポート
- unitypackageをD&Dした階層にインポート
- unitypackageでProjects配下のアセットの上書きを防止
- unitypackageでインポートしたアセットのログを保存して後から確認できる機能
- テクスチャの`Crunch Compression`のオフと`Streaming Mipmaps`のオンと`Kaiser`ミップマップ化
- モデルの`Legacy Blend Shape Normals`のオフと`Blend Shape Normals`設定の最適化、Humanoidから`Jaw`の削除

## Hierarchy拡張

Hierarchy上にオブジェクトのオンオフ、コンポーネント、タグ、レイヤーなどを表示できます。`IHierarchyExtensionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/HierarchyExtension/Components`配下のスクリプトを参考にしてください。

## Project Window拡張

Project上に拡張子やprefabの情報などを表示できます。`IProjectExtensionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/ProjectExtension/Components`配下のスクリプトを参考にしてください。

## Inspector拡張

Toolbar上にある+ボタンを押すと他のオブジェクトを選択しても表示対象が変わらないInspectorが開かれます。Unity標準の`Properties...`でも同様のことができますが、こちらはオブジェクトフィールドから編集対象のオブジェクトに飛べたり、開発者モード専用Inspectorに切り替えたりなどができます。

## Toolbar拡張

Toolbar上にアセンブリのロックボタンや拡張インスペクターのタブを開くボタンを表示できます。`IToolbarExtensionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/ToolbarExtension/Components`配下のスクリプトを参考にしてください。

## Scene View拡張

- Scene Viewを高解像度キャプチャ（`Tools/lilEditorToolbox/Scene Capture`）
- アンチエイリアス適用（Main Cameraに`SceneMSAA`コンポーネントを付ける）
- モデルデータを確認するカメラモード追加（`Vertex Attribute`）

> [!WARNING]
> カメラモード拡張でジオメトリシェーダーを使用しているため`Metal`環境では一部機能が動作しません。

## メニューの階層変更・削除

メニューバーにあるメニューの階層を変更したり削除したりして任意にカスタマイズできます。`Preferences`の`lilEditorToolbox`タブに設定があります。`Tools/*`のように指定して複数まとめて編集することもできます。

## その他ツール

- 不足アセット特定ツール（詳細は後述）
- シェーダー本体のキーワード確認（`Tools/lilEditorToolbox/Shader Keyword Viewer`）
- Missing参照発見ツール（`Tools/lilEditorToolbox/Missing Finder`）
- テクスチャチャンネルパッキングツール（`Tools/lilEditorToolbox/Texture Packer`）
- オブジェクト参照を何でも一括置き換え（`Tools/lilEditorToolbox/Reference Replacer`）
- スクリプトのコンパイルを抑制（`Tools/lilEditorToolbox/Lock Reload Assemblies`）
- 任意のObjectをjson形式で確認＆編集（`Tools/lilEditorToolbox/Json Object Viewer`）
- Unity関連フォルダを開く（`Tools/lilEditorToolbox/Folder Opener`）
- Transform初期化・コピーツール（`Tools/lilEditorToolbox/Transform Resetter`）
- Stencil確認用prefab（`lilEditorToolbox/Editor/Shaders/StencilChecker.prefab`）

## 不足アセット特定ツール

[GUIDのデータベース](https://github.com/lilxyzw/guid_database)を見てプロジェクトに不足しているツールやシェーダーを特定するツールです。不足しているアセットがデータベースに登録されている場合、エラーになっているマテリアルやコンポーネントのInspectorに不足アセットが表示されます。データベースは`Tools/lilEditorToolbox/[AssetGrimoire] Update Database`で更新できます。

### データベースにないツールを登録する手順

1. `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)`でデータベースに登録するターゲットを選択
  - `Make Target (Folder)`はProjectウィンドウで選択中のフォルダをターゲットとして指定します
  - `Make Target (Unitypackage)`はプロジェクト外のunitypackageを直接ターゲットとして指定します
2. `Assets/GUIDList/`内に手順1で指定したターゲットの設定が追加されるので必要に応じて設定を編集
3. `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs`をクリックし、ダイアログでデータベース出力先フォルダを選択
4. [guid_database](https://github.com/lilxyzw/guid_database)のPull Requestで追加リクエストを送信