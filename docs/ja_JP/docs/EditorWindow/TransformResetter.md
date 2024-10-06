# Transform初期化・コピーツール

メニューの場所 => `Tools/lilEditorToolbox/Transform Resetter`

任意のオブジェクトのTransformをまとめてprefabの状態に戻したり、他のオブジェクトからコピーしたりできます。アバターが中腰のまま戻らなくなったときに使うことを想定しています。

![TransformResetter](/images/ja_JP/EditorWindow/TransformResetter.png "TransformResetter")
## 使い方

中腰になったアバター・衣装を`編集対象`にセットしてボタンを押すだけです。基本的に`Prefabの状態にリセット`がオススメですが、PrefabのUnpackで参照が破壊されている場合は`Animatorの状態にリセット`を使用します。Humanoid設定されていない衣装でそれすらもできない場合は`他オブジェクトからコピー`のコピー元に元のPrefabをセットしてボタンを押すことでPrefabから状態がコピーされます。

## プロパティ

|名前|説明|
|-|-|
|編集対象|Transformの編集を行う対象です。|
|Prefabの状態にリセット|Prefabの初期状態にリセットします。PrefabをUnpackしていないときのみ利用できます。|
|Animatorの状態にリセット|Animatorの初期状態にリセットします。PrefabをUnpackしている場合に使います。|
|他オブジェクトからコピー|選択した他オブジェクトからTransformをコピーします。上記メニューが2つとも使えない場合に使います。|
|全Transform|全てのTransformに対して処理を行います。|
|Humanoidのみ|Humanoid（体）のボーンに対して処理を行います。|
|コピー元|Transformをコピーする際のコピー元です。このオブジェクトから編集対象に対してコピーが行われます。|

