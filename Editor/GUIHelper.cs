using UnityEngine;

namespace jp.lilxyzw.editortoolbox
{
    internal static class GUIHelper
    {
        private static GUIStyle styleAssetLabel;
        private static GUIStyle StyleAssetLabel => styleAssetLabel ??= "AssetLabel";

        internal static void DrawLabel(ref Rect currentRect, string label)
        {
            currentRect.xMin += 4;
            currentRect.width = StyleAssetLabel.CalcSize(new GUIContent(label)).x;
            GUI.Label(currentRect, label, StyleAssetLabel);
            currentRect.xMin += currentRect.width;
        }
    }
}
