# 纹理通道合并工具

菜单位置 => `Tools/lilEditorToolbox/Texture Packer`

将多张纹理合并到单张纹理的 RGBA 通道，适用于 Standard Shader PBR 材质。

![TexturePacker](/images/zh_Hans/EditorWindow/TexturePacker.png "TexturePacker")
## 使用方法

只需为各通道指定纹理并输出即可。例如，将 Metallic 贴到 R、Smoothness 贴到 A、Occlusion 贴到 G，再把「使用通道」都设为 R，即可合并成一张图。

