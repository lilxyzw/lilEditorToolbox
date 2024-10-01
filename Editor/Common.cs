using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static class Common
    {
        internal const string MENU_HEAD = "Tools/lilEditorToolbox/";

        internal static bool SkipScan(Object obj)
        {
            return obj is GameObject ||
                // Skip - Component
                obj is Transform ||
                // Skip - Asset
                obj is Mesh ||
                obj is Texture ||
                obj is Shader ||
                obj is TextAsset ||
                obj.GetType() == typeof(Object);
        }
    }
}
