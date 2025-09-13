# Transform 初始化/复制工具

菜单位置 => `Tools/lilEditorToolbox/Transform Resetter`

可将任意对象的 Transform 恢复为预制体状态，或从其他对象复制。适用于角色卡在蹲姿无法复原等情况。

![TransformResetter](/images/zh_Hans/EditorWindow/TransformResetter.png "TransformResetter")
## 使用方法

只需将卡住的角色或服装拖入「编辑目标」并点击按钮。通常建议「恢复到预制体」；若预制体解包后引用丢失，则改用「恢复到 Animator」。非 Humanoid 服装无法使用时，可将原预制体设为「从其他对象复制」的源，点击按钮即可复制状态。

## 属性

|名称|描述|
|-|-|
|编辑目标|要编辑 Transform 的目标。|
|恢复到预制体|将预制体恢复到初始状态，仅限未解包时使用。|
|恢复到 Animator|将 Animator 恢复到初始状态，适用于已解包预制体的情况。|
|从其他对象复制|从选中的其他对象复制 Transform。上述两项不可用时使用。|
|所有 Transform|对所有 Transform 执行操作。|
|仅 Humanoid|仅对 Humanoid 骨骼执行操作。|
|复制来源|复制 Transform 的源对象，其状态将被复制到编辑目标。|

