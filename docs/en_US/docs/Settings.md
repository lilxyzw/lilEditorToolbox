# Settings

Settings related to lilEditorToolbox. You can open it from `Edit/Preference/lilEditorToolbox` on the menu bar, where you can change the language and enable various functions.

[[toc]]

## Language

|Name|Description|
|-|-|
|Language|The language setting for lilEditorToolbox. The language file exists in `jp.lilxyzw.editortoolbox/Editor/Localization`, and you can support other languages by creating a language file.|

## Asset Import

|Name|Description|
|-|-|
|Drag And Drop Overwrite|When importing assets via D&D, if there is a file with the same name at the same level, it will be overwritten by the import.|
|Cancel Unitypackage Overwrite In Packages|Prevents unitypackage from overwriting assets under Packages.|

## Texture Import

|Name|Description|
|-|-|
|Turn Off Crunch Compression|Automatically turn off Crunch Compression when importing textures to speed up imports.|
|Turn On Streaming Mipmaps|Automatically turn on `Streaming Mipmaps` when importing textures.|
|Change To Kaiser Mipmaps|Automatically change to `Kaiser` when importing texture.|

## Model Import

|Name|Description|
|-|-|
|Turn On Readable|Automatically turn on `Readable` when importing a model.|
|Fix Blendshapes|Turn off `Legacy Blend Shape Normals` when importing a model to turn off automatic recalculation of BlendShape normals.|
|Remove Jaw|When importing a model, if a bone that does not contain `jaw` (case insensitive) in the bone name is assigned to the Humanoid Jaw, it will be automatically unassigned.|

## Hierarchy Extension

You can display objects, components, tags, layers, etc. on the Hierarchy. You can also add your own extensions by implementing `IHierarchyExtensionConponent`. Please refer to the scripts under `Editor/HierarchyExtension/Components` for how to write them.

|Name|Description|
|-|-|
|Hierarchy Spacer Width|The width of the margin to avoid interfering with other Hierarchy extensions.|
|Hierarchy Spacer Priority|This is the timing to insert margins so as not to interfere with other Hierarchy extensions.|
|Hierarchy Spacer|Add margins so as not to interfere with other Hierarchy extensions.|
|Active Toggle|A checkbox that turns an object on and off.|
|Alternating Background|Alternates the background color of the Hierarchy.|
|Children Drawer|Displays the child components of an object.|
|Components Drawer|Displays the object's components. You can turn components on or off by clicking their icons.|
|Editor Only Label|Shows the icon if the object is EditorOnly.|
|Hierarchy Line|Displays lines representing the parent-child relationships of objects in the Hierarchy.|
|Layer And Tag|Displays the layer and tag of an object.|

## Project Extension

You can display extensions and prefab information on the project. You can also add your own extensions by implementing `IProjectExtensionConponent`. Please refer to the script under `Editor/ProjectExtension/Components` for how to write it.

|Name|Description|
|-|-|
|Alternating Project Background|Alternates the background color of the Project window.|
|Unitypackage Hilighter|Highlight the assets you imported in each Unitypackage.|
|Extension Drawer|Displays the file extension.|
|Prefab Info|Displays Prefab information.|
|Material Variant|Displays the parent material if the material is a variant.|
|Material Queue|Displays the Render Queue for materials.|
|Material Shader|Displays the material's shader.|

## Toolbar Extension

You can display an assembly lock button or an extension inspector tab on the Toolbar. You can also add your own extension by implementing `IToolbarExtensionComponent`. Please refer to the script under `Editor/ToolbarExtension/Components` for how to write it.

|Name|Description|
|-|-|
|Lock Reload Assemblies Button|Locks the assembly to reduce the wait time for script compilation. This is useful if you frequently rewrite scripts.|
|Add Inspector Tab Button|Displays a button to add a tab to the Inspector that is locked on the selected object.|

## Menu Directory Replaces

You can customize the menu bar by changing or deleting the menu hierarchy. You can also edit multiple menus at once by specifying `Tools/*`.

|Name|Description|
|-|-|
|Enable Menu Directory Replaces|When this check box is selected, changing and deleting the menu hierarchy is enabled.|
|Menu Directory Replaces|Add the menu to be changed here. If To is empty, the menu will be deleted, if it is not empty, it will be moved to that hierarchy.|

