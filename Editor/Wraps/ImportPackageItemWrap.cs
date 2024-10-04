using System;
using System.Reflection;
using UnityEditor;

namespace jp.lilxyzw.editortoolbox
{
    internal class ImportPackageItemWrap
    {
        private static readonly Type TYPE = typeof(Editor).Assembly.GetType("UnityEditor.ImportPackageItem");
        private static readonly FieldInfo FI_existingAssetPath = TYPE.GetField("existingAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_destinationAssetPath = TYPE.GetField("destinationAssetPath", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_isFolder = TYPE.GetField("isFolder", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_assetChanged = TYPE.GetField("assetChanged", BindingFlags.Public | BindingFlags.Instance);
        private static readonly FieldInfo FI_guid = TYPE.GetField("guid", BindingFlags.Public | BindingFlags.Instance);

        public object instance;
        public ImportPackageItemWrap(object i) => instance = i;

        public string existingAssetPath => FI_existingAssetPath.GetValue(instance) as string;
        public bool isFolder => (bool)FI_isFolder.GetValue(instance);
        public string guid => FI_guid.GetValue(instance) as string;
        public string destinationAssetPath
        {
            get => FI_destinationAssetPath.GetValue(instance) as string;
            set => FI_destinationAssetPath.SetValue(instance, value);
        }
        public bool assetChanged
        {
            get => (bool)FI_assetChanged.GetValue(instance);
            set => FI_assetChanged.SetValue(instance, value);
        }

    }
}
