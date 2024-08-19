lilEditorToolbox
====

りるさんが適当につくったエディタ拡張をまとめたやつです。機能のオンオフは基本的に`Preferences`の`lilEditorToolbox`タブからできます。

## SceneMSAA

Main Cameraに付けるとScene画面でもアンチエイリアスが適用されるようになるコンポーネントです。

## カメラモード拡張

`Vertex Attribute`がカメラモードに追加されます。このモードでは、シーン画面左上から確認対象を切り替えられます。

> [!WARNING]
> ジオメトリシェーダーを使用しているため`Metal`環境では一部機能が動作しません。

## インポート拡張

アセットをD&Dした階層に同名のファイルがある場合上書きインポートするようになります。

## Hierarchy拡張

Hierarchy上にオブジェクトのオンオフ、コンポーネント、タグ、レイヤーなどを表示できます。`IHierarchyExtensionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/HierarchyExtension/Components`配下のスクリプトを参考にしてください。

## Project拡張

Project上に拡張子やprefabの情報などを表示できます。`IProjectExtensionConponent`を実装することで独自に拡張を追加することもできます。書き方は`Editor/ProjectExtension/Components`配下のスクリプトを参考にしてください。

## アセット特定ツール

[GUIDのデータベース](https://github.com/lilxyzw/guid_database)を見てプロジェクトに不足しているツールやシェーダーを特定するツールです。不足しているアセットがデータベースに登録されている場合、エラーになっているマテリアルやコンポーネントのInspectorに不足アセットが表示されます。データベースは`Tools/lilEditorToolbox/[AssetGrimoire] Update Database`で更新できます。

### データベースにないツールを登録する手順

1. `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)`でデータベースに登録するターゲットを選択
  - `Make Target (Folder)`はProjectウィンドウで選択中のフォルダをターゲットとして指定します
  - `Make Target (Unitypackage)`はプロジェクト外のunitypackageを直接ターゲットとして指定します
2. `Assets/GUIDList/`内に手順1で指定したターゲットの設定が追加されるので必要に応じて設定を編集
3. `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs`をクリックし、ダイアログでデータベース出力先フォルダを選択
4. [guid_database](https://github.com/lilxyzw/guid_database)のPull Requestで追加リクエストを送信

## テクスチャ変換ツール

テクスチャを右クリックして`_lil/TextureUtil/`内のメニューから変換できます。

`MetallicGlossMap`変換時にG・Bのチャンネルに1.0が設定されるため、多くの場合はそのまま`Mask Map`として使用できます。基本的に`Perceptual Roughness`として変換を行い、結果が滑らかすぎる場合は`Roughness`で変換を行ってください。

|Menu Name|Selection (Bold is required, others are optional)|Output|
|-|-|-|
|[Texture] Convert normal map (DirectX <-> OpenGL)|**Normal map**|Normal map|
|[Texture] Smoothness/Smoothness -> MetallicGlossMap|**Smoothness**|MetallicGlossMap|
|[Texture] Smoothness/Metallic, Smoothness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)|**Metallic**, **Smoothness**, Occlusion, Detail|MetallicGlossMap or Mask Map|
|[Texture] Perceptual Roughness/Roughness -> Smoothness|**Roughness**|Smoothness|
|[Texture] Perceptual Roughness/Roughness -> MetallicGlossMap|**Roughness**|MetallicGlossMap|
|[Texture] Perceptual Roughness/Metallic, Roughness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)|**Metallic**, **Roughness**, Occlusion, Detail|MetallicGlossMap or Mask Map|
|[Texture] Roughness/Roughness -> Smoothness|**Roughness**|Smoothness|
|[Texture] Roughness/Roughness -> MetallicGlossMap|**Roughness**|MetallicGlossMap|
|[Texture] Roughness/Metallic, Roughness (, Occlusion, Detail) -> MetallicGlossMap (MaskMap)|**Metallic**, **Roughness**, Occlusion, Detail|MetallicGlossMap or Mask Map|

## インポート設定最適化ツール

アセットの初回インポート時にインポート設定の最適化を行います。

- テクスチャの`Crunch Compression`のオフと`Streaming Mipmaps`のオン
- モデルの`Legacy Blend Shape Normals`のオフと`Blend Shape Normals`設定の最適化