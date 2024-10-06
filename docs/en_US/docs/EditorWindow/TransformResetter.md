# Transform Initialization/Copy Tool

Menu Location => `Tools/lilEditorToolbox/Transform Resetter`

You can restore the Transform of any object to its prefab state or copy it from another object. It is intended to be used when an avatar is stuck in a crouching position and cannot return to its original position.

![TransformResetter](/images/en_US/EditorWindow/TransformResetter.png "TransformResetter")
## How to use

Just set the crouching avatar and costume to `Editing target` and press the button. Generally, `Reset to prefab` is recommended, but if the reference is broken when unpacking the prefab, use `Reset to animator`. If you can't do that with a costume that is not set to Humanoid, set the original prefab to the copy source of `Copy from other object` and press the button to copy the state from the prefab.

## Property

|Name|Description|
|-|-|
|Edit target|This is the target for Transform editing.|
|Reset to prefab|Resets the prefab to its initial state. This is only available when the prefab has not been unpacked.|
|Reset to animator|Resets the Animator to its initial state. Use this if you have unpacked the Prefab.|
|Copy from other object|Copies the Transform from another selected object. Use this when neither of the above menus can be used.|
|All transforms|The process is performed on all Transforms.|
|Humanoid transforms|The process is performed on humanoid bones.|
|Copy from|This is the source of the Transform copy. The object will be copied from this object to the one being edited.|

