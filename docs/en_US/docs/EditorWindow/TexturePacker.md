# Texture Channel Packing Tool

Menu Location => `Tools/lilEditorToolbox/Texture Packer`

Stores multiple textures in the RGBA channels of a single texture. Intended for use with Standard Shader PBR materials.

![TexturePacker](/images/en_US/EditorWindow/TexturePacker.png "TexturePacker")
## How to use

Simply set a texture for each channel and output it. For example, if you have multiple PBR textures you want to use with the Standard shader, you can combine them into a single image by setting Metallic to R, Smoothness to A, and Occlusion to G, and then setting the `Channel to be used` for each to R and outputting them.

