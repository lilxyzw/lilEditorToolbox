# 缺失资源识别工具

此工具通过查看 [GUID 数据库](https://github.com/lilxyzw/guid_database) 来识别项目中缺失的工具和着色器。如果缺失的资源已在数据库中注册，它们将显示在出错材质或组件的 Inspector 中。可以通过 `Tools/lilEditorToolbox/[AssetGrimoire] Update Database` 更新数据库。

## 使用方法

<u>选择出错的组件或材质</u> 后将自动显示信息。但如果缺失的资源不在数据库中，则不会显示任何内容。

## 注册数据库中不存在的工具的步骤

1. 在 `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)` 中选择要注册到数据库中的目标
   -> `Make Target (Folder)` 会将 Project 窗口中选中的文件夹作为目标
   -> `Make Target (Unitypackage)` 会将项目外的 unitypackage 直接作为目标
2. 第一步中指定目标的设置将被添加到 `Assets/GUIDList/` 中，可根据需要进行编辑。
3. 点击 `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs`，在对话框中选择数据库输出文件夹。
4. 通过 Pull Request 向 [guid_database](https://github.com/lilxyzw/guid_database) 提交添加请求

