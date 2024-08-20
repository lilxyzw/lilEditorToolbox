using UnityEditor;
using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal class PrefabInfo : IProjectExtensionComponent
    {
        public int Priority => 0;
        private static GUIStyle styleAssetLabel;
        private static GUIStyle StyleAssetLabel => styleAssetLabel ??= "AssetLabel";

        public void OnGUI(ref Rect currentRect, string guid, string path, string name, string extension, Rect fullRect)
        {
            if(extension != ".prefab") return;

            var prefab = AssetDatabase.LoadAssetAtPath<Object>(path);
            var type = PrefabUtility.GetPrefabAssetType(prefab);
            var label = type.ToString();
            if(type == PrefabAssetType.Variant)
            {
                var parent = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                if(parent) label = $"Variant: {parent.name}";
            }
            currentRect.xMin += 4;
            currentRect.width = StyleAssetLabel.CalcSize(new GUIContent(label)).x;
            GUI.Label(currentRect, label, StyleAssetLabel);
            currentRect.xMin += currentRect.width;
        }
    }
}
