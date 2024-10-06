# テクスチャチャンネルパッキングツール

メニューの場所 => `Tools/lilEditorToolbox/Texture Packer`

複数のテクスチャを1枚のテクスチャのRGBAチャンネルに格納します。Standard ShaderのPBRマテリアルなどに利用することを想定しています。

![TexturePacker](/images/ja_JP/EditorWindow/TexturePacker.png "TexturePacker")
## 使い方

各チャンネルにテクスチャをセットして出力するだけです。例えばStandardシェーダーで使いたいPBRテクスチャが複数に分かれている場合、MetallicをR、SmoothnessをA、OcclusionをGにセットしてそれぞれ`使うチャンネル`をRにセットして出力することで1枚にまとめることができます。

