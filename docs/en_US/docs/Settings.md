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
|Do Not Add Variant To The End Of Prefab Name|Do not add "Variant" to the end of the name when creating a Prefab Variant.|
|Add Unitypackage Directory Selection Menu|Add a button to change the import directory in the unitypackage import window.|

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

## Animator Controller Editor

|Name|Description|
|-|-|
|Default Layer Weight 1|Change the default Layer Weight value to 1 when creating a new layer.|
|Default Write Defaults Off|Change the default value of Write Defaults when creating a new state to off.|
|Default Has Exit Time Off|Change the default value of Has Exit Time when creating a new transition to off.|
|Default Exit Time 1|Change the default value of Exit Time when creating a new transition to 1.|
|Default Duration 0|Change the default value of Duration when creating a new transition to 0.|
|Default Can Transition Self Off|Change the default value of Can Transition Self when creating a new transition to off.|
|Extend Animator Controller Parameter GUI|The Animator Controller parameter type has been expanded to allow it to be changed, and a decimal point will be displayed if the type is Float, making it easier to distinguish from Int types.|
|Add Copy And Paste Layer Menu|Add copy and paste to the right-click menu of the Animator Controller layer.|
|Add Copy Transition Settings Menu|Add copy and paste settings to the transition right-click menu.|
|Add Select In Out Transitions Menu|Add a bulk selection menu for entering and exiting transitions to the state right-click menu.|
|Add Parameter References Menu|Add a menu to the right-click menu of parameters that displays the referenced transition state.|
|Make Transition With Double Click|Make transition when you double-click a state.|
|Fix Copy Interruption Settings|Fix a Unity bug where transition Interruption settings were not copied.|
|Fix Transition Condition GUI|When changing a parameter in the transition condition settings, if the destination is an Int type, the default value will be Equals.|
|Fix State Multiple Edit|Fix a Unity bug that caused some properties to be overwritten when editing multiple states at the same time.|

## Hierarchy Extension

You can display objects, components, tags, layers, etc. on the Hierarchy. You can also add your own extensions by implementing `IHierarchyExtensionConponent`. Please refer to the scripts under `Editor/HierarchyExtension/Components` for how to write them.

|Name|Description|
|-|-|
|Hierarchy Spacer Width|The width of the margin to avoid interfering with other Hierarchy extensions.|
|Hierarchy Spacer Priority|This is the timing to insert margins so as not to interfere with other Hierarchy extensions.|
|Hierarchy Mouse Button|This is the mouse button that the hierarchy window extension that supports this property will respond to. If you make many erroneous operations, please change the button.|
|Hierarchy Layer And Tag Side By Side|Displays Layer and Tag side by side.|
|Hierarchy Spacer|Add margins so as not to interfere with other Hierarchy extensions.|
|Active Toggle|A checkbox that turns an object on and off.|
|Alternating Background|Alternates the background color of the Hierarchy.|
|Object Marker Background|Applies a background color to the ObjectMarker.|
|Children Drawer|Displays the child components of an object.|
|Components Drawer|Displays the object's components. You can turn components on or off by clicking their icons.|
|Editor Only Label|Shows the icon if the object is EditorOnly.|
|Hierarchy Line|Displays lines representing the parent-child relationships of objects in the Hierarchy.|
|Layer And Tag|Displays the layer and tag of an object.|

## Project Extension

You can display extensions and prefab information on the project. You can also add your own extensions by implementing `IProjectExtensionConponent`. Please refer to the script under `Editor/ProjectExtension/Components` for how to write it.

|Name|Description|
|-|-|
|Overlay File In Folder|Overlays the folder icon with the files inside.|
|Icon Overlay|Overlay any image on the icon.|
|Alternating Project Background|Alternates the background color of the Project window.|
|Unitypackage Hilighter|Highlight the assets you imported in each Unitypackage.|
|Extension Drawer|Displays the file extension.|
|Prefab Info|Displays Prefab information.|
|Material Variant|Displays the parent material if the material is a variant.|
|Material Queue|Displays the Render Queue for materials.|
|Material Shader|Displays the material's shader.|
|Project Line|Displays lines that represent the hierarchy of files.|
|Asset Marker|Mark any asset to make it easier to find.|

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

