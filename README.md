lilEditorToolbox
====

りるさんが適当につくったエディタ拡張をまとめたやつです。

## SceneMSAA

Main Cameraに付けるとScene画面でもアンチエイリアスが適用されるようになるコンポーネントです。

## カメラモード拡張

`Vertex Attribute`がカメラモードに追加されます。このモードでは、シーン画面左上から確認対象を切り替えられます。

> [!WARNING]
> ジオメトリシェーダーを使用しているため`Metal`環境では一部機能が動作しません。

## インポート拡張

アセットをD&Dした階層に同名のファイルがある場合上書きインポートするようになります。この機能のオンオフは`Preferences`の`lilEditorToolbox`タブから切り替えられます。

## Hierarchy拡張

Hierarchy上にオブジェクトのオンオフ、コンポーネント、タグ、レイヤーなどを表示できます。この機能のオンオフは`Preferences`の`lilEditorToolbox`タブから切り替えられます。`IHierarchyExtentionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/HierarchyExtention/Components`配下のスクリプトを参考にしてください。

## アセット特定ツール

[GUIDのデータベース](https://github.com/lilxyzw/guid_database)を見てプロジェクトに不足しているツールやシェーダーを特定するツールです。不足しているアセットがデータベースに登録されている場合、エラーになっているマテリアルやコンポーネントのInspectorに不足アセットが表示されます。データベースは`Tools/lilEditorToolbox/[AssetGrimoire] Update Database`で更新できます。

### データベースにないツールを登録する手順

1. `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)`でデータベースに登録するターゲットを選択
  - `Make Target (Folder)`はProjectウィンドウで選択中のフォルダをターゲットとして指定します
  - `Make Target (Unitypackage)`はプロジェクト外のunitypackageを直接ターゲットとして指定します
2. `Assets/GUIDList/`内に手順1で指定したターゲットの設定が追加されるので必要に応じて設定を編集
3. `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs`をクリックし、ダイアログでデータベース出力先フォルダを選択
4. [guid_database](https://github.com/lilxyzw/guid_database)のPull Requestで追加リクエストを送信
