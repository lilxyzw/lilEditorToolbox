# Missing Asset Identification Tool

This tool identifies tools and shaders that are missing from your project by looking at the [GUID database](https://github.com/lilxyzw/guid_database). If the missing assets are registered in the database, they will be displayed in the Inspector of the material or component that is causing the error. The database can be updated by `Tools/lilEditorToolbox/[AssetGrimoire] Update Database`.

## How to use

<u>Selecting the component or material in error</u> will automatically display the information, unless the missing asset is not in the database, in which case nothing will be displayed.

## Procedure for registering a tool that is not in the database

1. Select the target to register in the database in `Tools/lilEditorToolbox/[AssetGrimoire] Make Target (*)`  
   -> `Make Target (Folder)` specifies the folder selected in the Project window as the target  
   -> `Make Target (Unitypackage)` specifies a unitypackage outside the project as a direct target
2. The settings for the target specified in step 1 will be added to `Assets/GUIDList/`, so edit the settings as necessary.
3. Click `Tools/lilEditorToolbox/[AssetGrimoire] Output GUIDs` and select the database output folder in the dialog.
4. Submit an additional request via a pull request for [guid_database](https://github.com/lilxyzw/guid_database)

