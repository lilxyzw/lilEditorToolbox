# 不足アセット特定ツール

[GUIDのデータベース](https://github.com/lilxyzw/guid_database)を見てプロジェクトに不足しているツールやシェーダーを特定するツールです。不足しているアセットがデータベースに登録されている場合、エラーになっているマテリアルやコンポーネントのInspectorに不足アセットが表示されます。データベースは`Tools/lilEditorToolbox/[AssetGrimoire] Update Database`で更新できます。

## 使い方

<u>エラーになっているコンポーネントやマテリアルを選択する</u>と自動で情報が表示されます。ただし、不足アセットがデータベースにない場合は何も表示されません。

## データベースにないツールを登録する手順

1. `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)`でデータベースに登録するターゲットを選択  
   -> `Make Target (Folder)`はProjectウィンドウで選択中のフォルダをターゲットとして指定します  
   -> `Make Target (Unitypackage)`はプロジェクト外のunitypackageを直接ターゲットとして指定します
2. `Assets/GUIDList/`内に手順1で指定したターゲットの設定が追加されるので必要に応じて設定を編集
3. `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs`をクリックし、ダイアログでデータベース出力先フォルダを選択
4. [guid_database](https://github.com/lilxyzw/guid_database)のPull Requestで追加リクエストを送信

