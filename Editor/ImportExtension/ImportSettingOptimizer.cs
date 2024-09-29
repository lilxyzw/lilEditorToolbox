using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class ImportSettingOptimizer : AssetPostprocessor
    {
        private static string[] platforms;
        private static string[] Platforms => platforms ??= typeof(NamedBuildTarget).GetField("k_ValidNames", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as string[];
        private static bool isImportingPackage = false;
        private bool IsFirstTime => assetImporter.importSettingsMissing || isImportingPackage;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            AssetDatabase.importPackageStarted += (_) => isImportingPackage = true;
            AssetDatabase.importPackageCompleted += (_) => isImportingPackage = false;
            AssetDatabase.importPackageCancelled += (_) => isImportingPackage = false;
            AssetDatabase.importPackageFailed += (_,_) => isImportingPackage = false;
        }

        private void OnPreprocessAsset()
        {
            if(assetImporter is TextureImporter) OnPreprocessTextureInternal();
            if(assetImporter is ModelImporter) OnPreprocessModelInternal();
        }

        private void OnPreprocessTextureInternal()
        {
            if(assetPath.StartsWith("Packages") || !IsFirstTime) return;

            var importer = assetImporter as TextureImporter;

            if(EditorToolboxSettings.instance.turnOnStreamingMipmaps) importer.streamingMipmaps = true;
            if(EditorToolboxSettings.instance.changeToKaiserMipmaps) importer.mipmapFilter = TextureImporterMipFilter.KaiserFilter;
            if(EditorToolboxSettings.instance.turnOffCrunchCompression)
            {
                importer.crunchedCompression = false;
                FixImportSettings(importer, importer.GetDefaultPlatformTextureSettings());

                foreach(var setting in Platforms.Select(p => importer.GetPlatformTextureSettings(p)).Where(s => s.overridden))
                    FixImportSettings(importer, setting);
            }
        }

        private static void FixImportSettings(TextureImporter importer, TextureImporterPlatformSettings settings)
        {
            settings.crunchedCompression = false;
            switch(settings.format)
            {
                case TextureImporterFormat.DXT1Crunched: settings.format = TextureImporterFormat.DXT1; break;
                case TextureImporterFormat.DXT5Crunched: settings.format = TextureImporterFormat.DXT5; break;
                case TextureImporterFormat.ETC_RGB4Crunched: settings.format = TextureImporterFormat.ETC2_RGB4; break;
                case TextureImporterFormat.ETC2_RGBA8Crunched: settings.format = TextureImporterFormat.ETC2_RGBA8; break;
            }
            importer.SetPlatformTextureSettings(settings);
        }

        private void OnPreprocessModelInternal()
        {
            if(assetPath.StartsWith("Packages") || !IsFirstTime) return;

            var importer = assetImporter as ModelImporter;
            if(EditorToolboxSettings.instance.turnOnReadable) importer.isReadable = true;
            if(EditorToolboxSettings.instance.fixBlendshapes)
            {
                if(importer.importBlendShapeNormals == ModelImporterNormals.Calculate) importer.importBlendShapeNormals = ModelImporterNormals.None;
                typeof(ModelImporter).GetProperty("legacyComputeAllNormalsFromSmoothingGroupsWhenMeshHasBlendShapes", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(importer, false);
            }
        }

        private void OnPostprocessModel(GameObject obj)
        {
            if(assetPath.StartsWith("Packages")) return;

            var importer = assetImporter as ModelImporter;
            if(EditorToolboxSettings.instance.removeJaw)
            {
                var humanDescription = importer.humanDescription;
                humanDescription.human = humanDescription.human.Where(b => b.humanName != "Jaw" || b.boneName.Contains("jaw", StringComparison.InvariantCultureIgnoreCase)).ToArray();
                importer.humanDescription = humanDescription;
            }
        }
    }
}
